using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LeatherArmourBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {1292, -1, -1, 0}, {1292, -1, 0, 0}, {1292, -1, 1, 0}// 1	2	3	
			, {1292, -1, 2, 0}, {1292, 0, -1, 0}, {1292, 0, 0, 0}// 4	5	6	
			, {1292, 0, 1, 0}, {1292, 0, 2, 0}, {1292, 1, -1, 0}// 7	8	9	
			, {1292, 1, 0, 0}, {1292, 1, 1, 0}, {1292, 1, 2, 0}// 10	11	12	
			, {1292, 2, -1, 0}, {1292, 2, 0, 0}, {1292, 2, 1, 0}// 13	14	15	
			, {1292, 2, 2, 0}, {4206, -1, 0, 0}, {4207, -1, 1, 0}// 16	17	18	
			, {4217, -1, 2, 0}, {4271, -2, -1, 0}, {4271, 2, -1, 0}// 19	23	24	
			, {4272, -2, -1, 0}, {4272, -1, -1, 0}, {4272, 0, -1, 0}// 25	26	27	
			, {4272, 1, -1, 0}, {9, -2, -2, 0}, {9, 2, -2, 0}// 28	29	30	
			, {4600, 2, 1, 0}, {3997, -1, -1, 0}// 31	32	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new LeatherArmourBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public LeatherArmourBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 5066, -1, -1, 1, 2117, -1, "", 1);// 20
			AddComplexComponent( (BaseAddon) this, 5065, 0, -1, 0, 2117, -1, "", 1);// 21
			AddComplexComponent( (BaseAddon) this, 5064, 1, -1, 0, 2117, -1, "", 1);// 22

		}

		public LeatherArmourBazaarAddon( Serial serial ) : base( serial )
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

	public class LeatherArmourBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new LeatherArmourBazaarAddon();
			}
		}

		[Constructable]
		public LeatherArmourBazaarAddonDeed()
		{
			Name = "LeatherArmourBazaar";
		}

		public LeatherArmourBazaarAddonDeed( Serial serial ) : base( serial )
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