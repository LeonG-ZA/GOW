using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class JewelerBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {2779, 2, 2, 0}, {2778, 0, 0, 0}, {18406, 0, 0, 6}// 1	2	3	
			, {2781, -1, 2, 0}, {2780, -1, -1, 0}, {2783, -1, 1, 0}// 5	6	7	
			, {2782, 2, -1, 0}, {3876, 1, 0, 8}, {2785, 2, 1, 0}// 8	9	10	
			, {2785, 2, 0, 0}, {3887, 1, 0, 3}, {2783, -1, 0, 0}// 11	12	13	
			, {2778, 1, 1, 0}, {2778, 1, 0, 0}, {3885, 1, 0, 3}// 14	15	16	
			, {2826, 0, 0, 0}, {3873, 1, 0, 1}, {2786, 0, 2, 0}// 17	18	19	
			, {3883, 1, 0, 2}, {2784, 0, -1, 0}, {2784, 1, -1, 0}// 20	21	22	
			, {2786, 1, 2, 0}, {2778, 0, 1, 0}, {2826, 1, 0, 0}// 23	24	25	
					};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new JewelerBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public JewelerBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 2854, -1, 2, 0, 0, 1, "", 1);// 4

		}

		public JewelerBazaarAddon( Serial serial ) : base( serial )
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

	public class JewelerBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new JewelerBazaarAddon();
			}
		}

		[Constructable]
		public JewelerBazaarAddonDeed()
		{
			Name = "JewelerBazaar";
		}

		public JewelerBazaarAddonDeed( Serial serial ) : base( serial )
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