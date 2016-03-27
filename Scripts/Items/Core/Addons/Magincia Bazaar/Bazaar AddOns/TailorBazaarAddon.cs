using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class TailorBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {3997, 2, 2, 14}, {1277, 2, 0, 0}, {1277, 2, 1, 0}// 1	2	3	
			, {1277, 1, 2, 0}, {1277, 2, 2, 0}, {1277, 2, -1, 0}// 4	5	6	
			, {4000, 2, 1, 7}, {1277, 1, 1, 0}, {1277, 0, -1, 0}// 7	8	10	
			, {1277, -1, 2, 0}, {1277, -1, 1, 0}, {1277, -1, 0, 0}// 11	12	13	
			, {1277, -1, -1, 0}, {1277, 1, -1, 0}, {1277, 0, 2, 0}// 14	15	16	
			, {1277, 0, 1, 0}, {1277, 0, 0, 0}, {1277, 1, 0, 0}// 17	18	19	
			, {3994, 0, -1, 0}, {3999, 1, 1, 6}, {2923, -1, 1, 0}// 20	22	24	
			, {2925, -1, 0, 0}, {2924, -1, -1, 0}, {3574, 1, 2, 14}// 25	26	27	
			, {2943, 0, 1, 0}, {2942, 2, 1, 0}, {2944, 1, 1, 0}// 28	29	30	
			, {3577, -1, 0, 6}, {3989, 0, 0, 0}// 31	32	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new TailorBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public TailorBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 5983, -1, 1, 7, 588, -1, "", 1);// 9
			AddComplexComponent( (BaseAddon) this, 5983, -1, 1, 6, 323, -1, "", 1);// 21
			AddComplexComponent( (BaseAddon) this, 3615, -1, 1, 7, 1154, -1, "", 2);// 23

		}

		public TailorBazaarAddon( Serial serial ) : base( serial )
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

	public class TailorBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new TailorBazaarAddon();
			}
		}

		[Constructable]
		public TailorBazaarAddonDeed()
		{
			Name = "TailorBazaar";
		}

		public TailorBazaarAddonDeed( Serial serial ) : base( serial )
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