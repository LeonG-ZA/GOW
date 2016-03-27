using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{
	public class EventGravitySimulator : BaseToggler
	{		
		[Constructable]
		public EventGravitySimulator():this(0x1DB8)
		{	
		}
			
		public EventGravitySimulator(int ItemID):base(ItemID)
		{
			Name="EventGravitySimulator";
		}		
		
		public int GetStaticSurfaceTop(Mobile m)
		{
			object x = m.Map.GetTopSurface(m.Location);
            
			if(x is LandTile)
			{
                LandTile t = (LandTile)x;
				if(TileData.LandTable.Length> t.ID)
				{
                    LandData td = TileData.LandTable[t.ID];
					return t.Z;
				}
				
				return t.Z;
			}else if(x is StaticTile)
            {
                StaticTile t = (StaticTile)x;
				if(TileData.ItemTable.Length> t.ID)
				{
					ItemData td = TileData.ItemTable[t.ID];
					return t.Z+td.CalcHeight;
				}
				
				return t.Z;
            }
            else if (x is Item)
			{
				Item i = (Item)x;
				ItemData td = i.ItemData;
				return i.Z + td.CalcHeight;
			}
			else if(x is IPoint3D)
			{
				return ((IPoint3D)x).Z;
			}
			return m.Z;

		}
		public override bool Toggle(byte state, Mobile who, int sid)
		{
			if (sid==lsid)
				return false;
			lsid= sid;
			bool send = false;			
			
			if(who!=null)
			{
				if (state > 0)
				{
					who.Z = GetStaticSurfaceTop(who);
					
				}
				send = true;
			}
			bool ven = (Link == null || (Link as Item).Deleted)?true:Link.Toggle(state,who,sid);
			return send&ven;
		}
		
		
		public EventGravitySimulator( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write((int)0);//version
		}
		
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
