using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ScribeBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {1308, 2, 2, 20}, {1308, 2, 1, 20}, {1308, 2, 0, 20}// 1	2	3	
			, {1308, 2, -1, 20}, {1308, 1, 2, 20}, {1308, 1, 1, 20}// 4	5	7	
			, {1308, 1, 0, 20}, {1308, 1, -1, 20}, {1308, 0, 2, 20}// 8	9	10	
			, {1308, 0, 1, 20}, {7713, 2, 0, 20}, {6160, 1, -1, 23}// 11	12	13	
			, {7957, 1, 2, 24}, {1308, 0, 0, 20}, {7716, 0, -1, 20}// 14	15	16	
			, {8901, 1, 2, 28}, {1308, -1, -1, 20}, {1308, -1, 0, 0}// 17	18	19	
			, {1308, -1, -1, 0}, {1308, -1, 0, 20}, {1308, -1, 1, 20}// 20	23	24	
			, {1308, -1, 2, 20}, {1308, 0, -1, 20}, {8787, 2, 2, 29}// 25	26	27	
			, {3642, 0, 2, 26}, {3834, 2, 2, 23}, {4031, 2, -1, 29}// 28	29	31	
			, {2890, 2, -1, 20}, {4030, 2, -1, 24}// 32	33	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new ScribeBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public ScribeBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 6570, -1, -1, 20, 0, 1, "", 1);// 6
			AddComplexComponent( (BaseAddon) this, 4614, 1, 2, 20, 6, -1, "", 1);// 21
			AddComplexComponent( (BaseAddon) this, 4613, 0, 2, 20, 6, -1, "", 1);// 22
			AddComplexComponent( (BaseAddon) this, 4612, 2, 2, 20, 6, -1, "", 1);// 30

		}

		public ScribeBazaarAddon( Serial serial ) : base( serial )
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

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class ScribeBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new ScribeBazaarAddon();
			}
		}

		[Constructable]
		public ScribeBazaarAddonDeed()
		{
			Name = "ScribeBazaar";
		}

		public ScribeBazaarAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}