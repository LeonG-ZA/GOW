using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class CarpetedStairBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {1980, 0, 1, 0}, {1980, 0, 0, 0}, {1305, 2, -1, 0}// 1	2	3	
			, {1305, 2, 0, 0}, {1305, 2, 1, 0}, {1298, 1, 1, 0}// 4	5	6	
			, {1305, 2, 2, 0}, {1305, 1, -1, 0}, {1305, -1, 2, 0}// 7	8	9	
			, {1298, 1, 0, 0}, {1113, -1, -1, 0}, {1305, 0, -1, 0}// 10	11	12	
			, {1305, 1, 2, 0}, {1305, 0, 2, 0}, {1305, -1, -1, 0}// 13	14	15	
			, {1978, -1, 0, 0}, {1978, -1, 1, 0}, {1113, -1, 2, 0}// 16	17	18	
					};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new CarpetedStairBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public CarpetedStairBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 2845, -1, 2, 7, 0, 0, "", 1);// 19
			AddComplexComponent( (BaseAddon) this, 2845, -1, -1, 7, 0, 0, "", 1);// 20

		}

		public CarpetedStairBazaarAddon( Serial serial ) : base( serial )
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

	public class CarpetedStairBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new CarpetedStairBazaarAddon();
			}
		}

		[Constructable]
		public CarpetedStairBazaarAddonDeed()
		{
			Name = "CarpetedStairBazaar";
		}

		public CarpetedStairBazaarAddonDeed( Serial serial ) : base( serial )
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