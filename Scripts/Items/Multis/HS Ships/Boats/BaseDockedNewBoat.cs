using System;
using Server;
using Server.Regions;
using Server.Targeting;
using Server.Engines.ChampionSpawns;

namespace Server.Multis
{
	public abstract class BaseDockedNewBoat : Item
	{
        private int m_MultiID, m_MultiID_South, m_MultiID_East, m_MultiID_West, m_MultiID_North;
		private Point3D m_Offset;
		private string m_ShipName;
		private Direction m_ChosenDirection;	

		[CommandProperty( AccessLevel.GameMaster )]
		public int MultiID{ get{ return m_MultiID; } set{ m_MultiID = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Offset{ get{ return m_Offset; } set{ m_Offset = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public string ShipName{ get{ return m_ShipName; } set{ m_ShipName = value; InvalidateProperties(); } }
		
		[CommandProperty(AccessLevel.GameMaster)]
		public abstract NewBaseBoat Boat{ get; }

		public BaseDockedNewBoat( int id, Point3D offset, NewBaseBoat boat ) : base( 0x14F4 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;

            m_MultiID = id;
            m_MultiID_North = id;
            m_MultiID_South = id + 2;
            m_MultiID_East = id + 1;
            m_MultiID_West = id + 3;

            m_Offset = offset;

            m_ShipName = boat.ShipName;

            m_ChosenDirection = Direction.North;		
		}

		public BaseDockedNewBoat( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

            writer.Write((int)2); // version

            writer.Write(m_MultiID_North); //version 2
            writer.Write(m_MultiID_East);
            writer.Write(m_MultiID_South);
            writer.Write(m_MultiID_West);

            writer.Write(m_MultiID);
            writer.Write(m_Offset);
            writer.Write(m_ShipName);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        m_MultiID_North = reader.ReadInt();
                        m_MultiID_East = reader.ReadInt();
                        m_MultiID_South = reader.ReadInt();
                        m_MultiID_West = reader.ReadInt();

                        goto case 1;
                    }

                case 1:
                    {
                        // Empty case ??
                        goto case 0;
                    }

                case 0:
                    {
                        m_MultiID = reader.ReadInt();
                        m_Offset = reader.ReadPoint3D();
                        m_ShipName = reader.ReadString();

                        if (version == 0)
                            reader.ReadUInt();

                        break;
                    }
            }

            if (LootType == LootType.Newbied)
                LootType = LootType.Blessed;

            if (Weight == 0.0)
                Weight = 1.0;
		}

		public override void OnDoubleClick( Mobile from )
        {            
            from.CloseGump(typeof(BoatPlacementGump));
			from.SendGump( new BoatPlacementGump( from, null, this ) );
		}		
		
		public void PlacementDirection( Mobile from, Direction chosenDirection)
		{
			m_ChosenDirection = chosenDirection;
		
			switch ( chosenDirection )
			{		
					case Direction.West:
					{
						m_MultiID = m_MultiID_West;	

						break;
					}	

					case Direction.South:
					{
						m_MultiID = m_MultiID_South;	

						break;					
					}

					case Direction.East:
					{
						m_MultiID = m_MultiID_East;
						
						break;
					}	

					case Direction.North:
					{
						m_MultiID = m_MultiID_North;
						
						break;
					}					
			}
			
			ShipPlacement(from);
		}
		
		public void ShipPlacement( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendLocalizedMessage( 502482 ); // Where do you wish to place the ship?

				from.Target = new InternalTarget( this );
			}
		}



		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( m_ShipName != null )
				list.Add( m_ShipName );
			else
				base.AddNameProperty( list );
		}

		public override void OnSingleClick( Mobile from )
		{
			if ( m_ShipName != null )
				LabelTo( from, m_ShipName );
			else
				base.OnSingleClick( from );
		}

		public void OnPlacement( Mobile from, Point3D p )
		{
			if ( Deleted )
			{
				return;
			}
			else if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				Map map = from.Map;

				if ( map == null )
					return;

				NewBaseBoat boat = Boat;

				if ( boat == null )
					return;

				p = new Point3D( p.X - m_Offset.X, p.Y - m_Offset.Y, p.Z - m_Offset.Z );

				if ( boat.CanFit( p, map, boat.ItemID ) && map != Map.Ilshenar && map != Map.Malas )
				{
					Delete();

					boat.Owner = from;
					boat.Anchored = false;
					boat.ShipName = m_ShipName;

					uint keyValue = boat.CreateKeys( from );

					if ( boat.PPlank != null )
						boat.PPlank.KeyValue = keyValue;						
					if ( boat.SPlank != null )
						boat.SPlank.KeyValue = keyValue;

					boat.MoveToWorld( p, map );
					
					boat.SetFacing(m_ChosenDirection);					
				}
				else
				{
					boat.Delete();
					from.SendLocalizedMessage( 1043284 ); // A ship can not be created here.
				}
			}
		}

		private class InternalTarget : MultiTarget
		{
			private BaseDockedNewBoat m_Model;

			public InternalTarget( BaseDockedNewBoat model ) : base( model.MultiID, model.Offset )
			{
				m_Model = model;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				IPoint3D ip = o as IPoint3D;

				if ( ip != null )
				{
					if ( ip is Item )
						ip = ((Item)ip).GetWorldTop();

					Point3D p = new Point3D( ip );

					Region region = Region.Find( p, from.Map );

					if ( region.IsPartOf( typeof( DungeonRegion ) ) )
						from.SendLocalizedMessage( 502488 ); // You can not place a ship inside a dungeon.
					else if ( region.IsPartOf( typeof( HouseRegion ) ) || region.IsPartOf( typeof( ChampionSpawnRegion ) ) )
						from.SendLocalizedMessage( 1042549 ); // A boat may not be placed in this area.
					else
						m_Model.OnPlacement( from, p );
				}
			}
		}
	}
}