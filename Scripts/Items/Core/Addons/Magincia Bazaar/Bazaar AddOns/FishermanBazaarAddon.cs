using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class FishermanBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {5374, -1, 2, 6}, {1217, -1, -1, 0}, {1217, -1, 0, 0}// 1	2	3	
			, {1217, -1, 1, 0}, {1217, -1, 2, 0}, {1217, 0, -1, 0}// 4	5	6	
			, {1217, 0, 0, 0}, {1217, 0, 1, 0}, {1217, 0, 2, 0}// 7	8	9	
			, {1217, 1, -1, 0}, {1217, 1, 0, 0}, {1217, 1, 1, 0}// 10	11	12	
			, {1217, 1, 2, 0}, {1217, 2, -1, 0}, {1217, 2, 0, 0}// 13	14	15	
			, {1217, 2, 1, 0}, {1217, 2, 2, 0}, {2880, -1, 2, 0}// 16	17	18	
			, {2880, 2, 2, 0}, {2878, 0, 2, 0}, {7703, 2, 2, 6}// 19	20	22	
			, {2878, -1, 2, 0}, {2878, 1, 2, 0}, {5365, 1, 2, 6}// 23	24	25	
			, {4167, 2, -1, 4}, {5369, -1, -1, 0}, {5370, 1, -1, 0}// 26	27	28	
			, {5368, 0, -1, 0}, {2463, 1, 2, 9}// 29	30	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new FishermanBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public FishermanBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 5355, 0, 2, 6, 0, -1, "Treasure Map", 1);// 21

		}

		public FishermanBazaarAddon( Serial serial ) : base( serial )
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

	public class FishermanBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new FishermanBazaarAddon();
			}
		}

		[Constructable]
		public FishermanBazaarAddonDeed()
		{
			Name = "FishermanBazaar";
		}

		public FishermanBazaarAddonDeed( Serial serial ) : base( serial )
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