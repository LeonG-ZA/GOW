using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class DistilleryBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {1306, 1, -1, 0}, {1306, -1, -1, 0}, {1306, 0, -1, 0}// 1	2	3	
			, {1306, 2, -1, 0}, {3647, -1, -1, 0}, {3647, -1, -1, 3}// 4	5	6	
			, {4014, 0, -1, 0}, {2500, -1, -1, 6}, {17650, 2, -1, 0}// 8	9	10	
			, {2513, 2, -1, 1}, {2513, 2, -1, 1}, {2513, 2, -1, 3}// 11	12	13	
			, {2513, 2, -1, 2}, {2513, 2, -1, 4}, {2513, 2, -1, 1}// 14	15	16	
			, {2513, 2, -1, 0}, {1306, 1, 0, 0}, {1306, 1, 1, 0}// 17	18	19	
			, {15803, -1, 2, 0}, {1306, 1, 2, 0}, {1306, -1, 2, 0}// 20	21	22	
			, {1306, -1, 1, 0}, {1306, 2, 1, 0}, {1306, 2, 0, 0}// 23	24	25	
			, {1306, -1, 0, 0}, {1306, 0, 0, 0}, {1306, 0, 1, 0}// 26	27	28	
			, {1306, 0, 2, 0}, {1306, 2, 2, 0}, {15802, -1, 1, 0}// 29	30	32	
					};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new DistilleryBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public DistilleryBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 6464, 1, -1, 0, 0, -1, "a Keg of Wine", 1);// 7
			AddComplexComponent( (BaseAddon) this, 6871, -1, 0, 0, 644, -1, "", 1);// 31

		}

		public DistilleryBazaarAddon( Serial serial ) : base( serial )
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

	public class DistilleryBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new DistilleryBazaarAddon();
			}
		}

		[Constructable]
		public DistilleryBazaarAddonDeed()
		{
			Name = "DistilleryBazaar";
		}

		public DistilleryBazaarAddonDeed( Serial serial ) : base( serial )
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