using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class PillowBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {2761, 2, 2, 2}, {2759, 1, 1, 2}, {2759, 0, 1, 2}// 1	2	3	
			, {2759, 0, 0, 2}, {2759, 1, 0, 2}, {2762, -1, -1, 2}// 4	5	6	
			, {2763, -1, 2, 2}, {2764, 2, -1, 2}, {2765, -1, 1, 2}// 7	8	9	
			, {2765, -1, 0, 2}, {2766, 0, -1, 2}, {2766, 1, -1, 2}// 10	11	12	
			, {2767, 2, 0, 2}, {2767, 2, 1, 2}, {2768, 0, 2, 2}// 13	14	15	
			, {2768, 1, 2, 2}, {9410, -1, -1, 0}// 16	17	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new PillowBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public PillowBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 5028, -1, -1, 1, 10, -1, "", 1);// 18
			AddComplexComponent( (BaseAddon) this, 5029, 1, -1, 2, 467, -1, "", 1);// 19
			AddComplexComponent( (BaseAddon) this, 5033, 0, -1, 2, 34, -1, "", 1);// 20
			AddComplexComponent( (BaseAddon) this, 5035, 0, 0, 8, 594, -1, "", 1);// 21
			AddComplexComponent( (BaseAddon) this, 5037, -1, 0, 2, 6, -1, "", 1);// 22
			AddComplexComponent( (BaseAddon) this, 5691, 1, 0, 6, 53, -1, "", 1);// 23

		}

		public PillowBazaarAddon( Serial serial ) : base( serial )
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

	public class PillowBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new PillowBazaarAddon();
			}
		}

		[Constructable]
		public PillowBazaarAddonDeed()
		{
			Name = "PillowBazaar";
		}

		public PillowBazaarAddonDeed( Serial serial ) : base( serial )
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