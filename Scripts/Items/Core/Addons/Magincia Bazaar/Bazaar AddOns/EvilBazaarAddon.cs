using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EvilBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {7773, 1, 0, 7}, {12295, 2, 2, 0}, {12295, -1, 2, 0}// 2	3	4	
			, {12295, 2, -1, 0}, {12295, 1, 1, 0}, {12295, 1, 0, 0}// 7	9	10	
			, {12295, -1, -1, 0}, {12295, 2, 0, 0}, {12295, -1, 0, 0}// 11	12	13	
			, {12295, 0, 0, 0}, {12295, 0, 1, 0}, {12295, 1, 2, 0}// 14	15	16	
			, {12295, 2, 1, 0}, {4683, -1, 0, 0}, {12295, 0, -1, 0}// 17	18	19	
			, {12295, -1, 1, 0}, {10840, 1, 1, 0}, {12295, 1, -1, 0}// 20	21	22	
			, {12295, 0, 2, 0}, {7772, 2, 0, 7}// 23	24	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new EvilBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public EvilBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 3682, -1, -1, 0, 0, 1, "", 1);// 1
			AddComplexComponent( (BaseAddon) this, 3685, -1, 2, 0, 0, 1, "", 1);// 5
			AddComplexComponent( (BaseAddon) this, 3688, 2, 2, 0, 0, 1, "", 1);// 6
			AddComplexComponent( (BaseAddon) this, 3679, 2, -1, 0, 0, 1, "", 1);// 8

		}

		public EvilBazaarAddon( Serial serial ) : base( serial )
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

	public class EvilBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new EvilBazaarAddon();
			}
		}

		[Constructable]
		public EvilBazaarAddonDeed()
		{
			Name = "EvilBazaar";
		}

		public EvilBazaarAddonDeed( Serial serial ) : base( serial )
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