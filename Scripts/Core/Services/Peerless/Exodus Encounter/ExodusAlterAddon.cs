using System;
using System.Collections.Generic;
using Server;
using Server.Multis;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
	public class ExodusAlterAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {1806, -1, -1, 0}, {1807, 2, 2, 0}, {1808, 2, -1, 0}// 1	2	3	
			, {1809, -1, 2, 0}, {1802, 0, 2, 0}, {1802, 1, 2, 0}// 4	5	6	
			, {1805, -1, 0, 0}, {1805, -1, 1, 0}, {1804, 0, -1, 0}// 7	8	9	
			, {1804, 1, -1, 0}, {1803, 2, 0, 0}, {1803, 2, 1, 0}// 10	11	12	
			, {1168, 1, 1, 0}// 18	
		};


		[ Constructable ]
		public ExodusAlterAddon()
		{
			BeginDecay( m_DefaultDecayTime );
			
			AddComplexComponent( (BaseAddon) this, 1922, 0, 1, 0, 0, -1, "", 1);// 13
			AddComplexComponent( (BaseAddon) this, 1923, 1, 0, 0, 0, -1, "", 1);// 14
			AddComplexComponent( (BaseAddon) this, 1870, 0, 0, 0, 0, -1, "", 1);// 15
			AddComplexComponent( (BaseAddon) this, 1871, 1, 1, 0, 0, -1, "", 1);// 16
		}
		
		//decaytimer
		private Timer m_DecayTimer;
		private DateTime m_DecayTime;

		private static TimeSpan m_DefaultDecayTime = TimeSpan.FromMinutes( 60 );

		public void BeginDecay( TimeSpan delay )
		{
            if (m_DecayTimer != null)
            {
                m_DecayTimer.Stop();
            }

			m_DecayTime = DateTime.UtcNow + delay;

			m_DecayTimer = new InternalTimer( this, delay );
			m_DecayTimer.Start();
		}
		
		public override void OnAfterDelete()
		{
			if ( m_DecayTimer != null )
				m_DecayTimer.Stop();

			m_DecayTimer = null;
		}
		
		private class InternalTimer : Timer
		{
			private ExodusAlterAddon m_ExodusAlterAddon;

			public InternalTimer( ExodusAlterAddon a, TimeSpan delay ) : base( delay )
			{
				m_ExodusAlterAddon = a;
				Priority = TimerPriority.FiveSeconds;
			}

			protected override void OnTick()
			{
				m_ExodusAlterAddon.Remove();
			}
		}
		
		public void Remove()
		{
            if (Deleted)
            {
                return;
            }
			//after 60 min item is deleted
			Delete();
		}
		
		//decaytimer

		public ExodusAlterAddon( Serial serial ) : base( serial )
		{
		}

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource)
        {
            AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1);
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource, string name, int amount)
        {
            AddonComponent ac;
            ac = new AddonComponent(item);
            if (name != null && name.Length > 0)
                ac.Name = name;
            if (hue != 0)
                ac.Hue = hue;
            if (amount > 1)
            {
                ac.Stackable = true;
                ac.Amount = amount;
            }
            if (lightsource != -1)
                ac.Light = (LightType) lightsource;
            addon.AddComponent(ac, xoffset, yoffset, zoffset);
        }

		public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
			
			writer.WriteDeltaTime( m_DecayTime );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
			
			m_DecayTime = reader.ReadDeltaTime();
        }
	}
	
   [Flipable( 0x14F0, 0x14EF )]
    public class ExodusAlterAddonDeed : Item
    {		
		private BaseAddon m_Deed;
		
		public BaseAddon ExodusAlterAddon
        {
            get
            {
                return this.m_Deed;
            }
            set
            {
                m_Deed = value;
            }
        }
		
		[Constructable]
        public ExodusAlterAddonDeed()
            : base(0x14F0)
        {
            Weight = 1.0;
			Name = "Exodus Summoning Altar";
        }

        public ExodusAlterAddonDeed( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            if ( Weight == 0.0 )
                Weight = 1.0;
        }

        public override void OnDoubleClick( Mobile from )
        {
            if ( IsChildOf( from.Backpack ) )
                from.Target = new InternalTarget( this );
            else
                from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
        }

        private class InternalTarget : Target
        {
            private ExodusAlterAddonDeed m_Deed;

            public InternalTarget( ExodusAlterAddonDeed deed ) : base( -1, true, TargetFlags.None )
            {
                m_Deed = deed;

                CheckLOS = false;
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
				
                IPoint3D p = targeted as IPoint3D;
				
                Map map = from.Map;
				
                if ( p == null || map == null || m_Deed.Deleted || m_Deed == null )
				return;
				
                if ( m_Deed.IsChildOf( from.Backpack ) )
                {
					
					BaseAddon addon = new ExodusAlterAddon();
					//PeerlessAltar altar = new ExodusTomeAltar();
					
                    Server.Spells.SpellHelper.GetSurfaceTop( ref p );

					PlayerMobile mobile = (PlayerMobile) from;	
					if (mobile == null)
						return;
						
						//START RITUAL PLACEMENT SCRIPT
						if (mobile.Map == Map.Trammel || mobile.Map == Map.Felucca)
						{
							if (mobile.Location.X >= 1853 && mobile.Location.X <= 1862 && mobile.Location.Y >= 870 && mobile.Location.Y <= 878)
							{
								//location compassion
								addon.MoveToWorld( new Point3D( 1857, 874, 0 ), map );
								//altar.MoveToWorld( new Point3D( 1858, 875, 12 ), map );
								m_Deed.Delete();
                                mobile.AddToBackpack(new ExodusSummoningAlter()); 
								return;
							}
							else if (mobile.Location.X >= 4209 && mobile.Location.X <= 4215 && mobile.Location.Y >= 561 && mobile.Location.Y <= 566)
							{
								//location honesty
								//from.SendMessage("debug pass");
								addon.MoveToWorld( new Point3D( 4208, 563, 47), map );
								//altar.MoveToWorld( new Point3D( 4209, 564, 59 ), map );
                                mobile.AddToBackpack(new ExodusSummoningAlter());
								m_Deed.Delete();
								
								return;
							}
							else if (mobile.Location.X >= 1729 && mobile.Location.X <= 1732 && mobile.Location.Y >= 3526 && mobile.Location.Y <= 3529)
							{
								//location honor
								addon.MoveToWorld( new Point3D( 1726, 3527, 3), map );
								//altar.MoveToWorld( new Point3D( 1727, 3528, 15 ), map );
                                mobile.AddToBackpack(new ExodusSummoningAlter());
								m_Deed.Delete();
								return;
							}
							else if (mobile.Location.X >= 4273 && mobile.Location.X <= 4276 && mobile.Location.Y >= 3693 && mobile.Location.Y <= 3700)
							{
								//location humility
								addon.MoveToWorld( new Point3D( 4273, 3696, 0), map );
								//altar.MoveToWorld( new Point3D( 4274, 3697, 12), map );
                                mobile.AddToBackpack(new ExodusSummoningAlter());
								m_Deed.Delete();
								return;
							}
							else if (mobile.Location.X >= 1297 && mobile.Location.X <= 1304 && mobile.Location.Y >= 629 && mobile.Location.Y <= 636)
							{
								//location justice
								addon.MoveToWorld( new Point3D( 1300, 633, 16), map );
								//altar.MoveToWorld( new Point3D( 1301, 634, 28), map );
                                mobile.AddToBackpack(new ExodusSummoningAlter());
								m_Deed.Delete();
								return;
							}
							else if (mobile.Location.X >= 3352 && mobile.Location.X <= 3357 && mobile.Location.Y >= 287 && mobile.Location.Y <= 295)
							{
								//location sacrifice
								addon.MoveToWorld( new Point3D( 3354, 289, 4), map );
								//altar.MoveToWorld( new Point3D( 3355, 290, 16), map );
                                mobile.AddToBackpack(new ExodusSummoningAlter());
								m_Deed.Delete();
								return;
							}
							else if (mobile.Location.X >= 1593 && mobile.Location.X <= 1607 && mobile.Location.Y >= 2488 && mobile.Location.Y <= 2493)
							{
								//location spirituality
								addon.MoveToWorld( new Point3D( 1605, 2489, 8 ), map );
								//altar.MoveToWorld( new Point3D( 1606, 2490, 20 ), map );
                                mobile.AddToBackpack(new ExodusSummoningAlter());
								m_Deed.Delete();
								return;
							}
							else if (mobile.Location.X >= 2486 && mobile.Location.X <= 2495 && mobile.Location.Y >= 3926 && mobile.Location.Y <= 3936)
							{
								//location valor
								addon.MoveToWorld( new Point3D( 2491, 3930, 5 ), map );
								//altar.MoveToWorld( new Point3D( 2492, 3931, 17 ), map );
                                mobile.AddToBackpack(new ExodusSummoningAlter());
								m_Deed.Delete();
								return;
							}
						}
						mobile.SendMessage("That is not the right place to permorm thy ritual.");
                }
                else
                {
                    from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
                }
            }
        }
    }
}