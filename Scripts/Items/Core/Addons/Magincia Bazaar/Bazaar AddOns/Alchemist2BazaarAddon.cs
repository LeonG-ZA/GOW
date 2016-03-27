using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class Alchemist2BazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {8511, -1, -1, 0}, {8511, 2, 2, 0}, {8511, -1, 2, 0}// 1	2	3	
			, {8511, 2, -1, 0}, {8512, 0, 2, 0}, {8512, 1, -1, 0}// 4	5	6	
			, {8513, -1, 1, 0}, {8513, 2, 0, 0}, {8514, 1, 2, 0}// 7	8	9	
			, {8514, 0, -1, 0}, {8515, -1, 0, 0}, {8515, 2, 1, 0}// 10	11	12	
			, {8516, 0, 1, 0}, {8516, 1, 0, 0}, {8511, 0, 0, 0}// 13	14	15	
			, {8511, 1, 1, 0}, {6422, 0, 2, 0}, {6422, 0, -1, 0}// 16	17	18	
			, {6423, 1, 2, 0}, {6423, 1, -1, 0}, {6192, 1, -1, 7}// 19	20	23	
			, {6214, 0, -1, 3}, {6195, 1, -1, 1}, {6238, 1, 2, 6}// 24	25	26	
			, {6226, 0, 2, 4}, {3659, 2, 2, 0}, {6185, 1, -1, 1}// 27	28	29	
					};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new Alchemist2BazaarAddonDeed();
			}
		}

		[ Constructable ]
		public Alchemist2BazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 3633, -1, -1, 0, 0, 1, "", 1);// 21
			AddComplexComponent( (BaseAddon) this, 3633, 2, -1, 0, 0, 1, "", 1);// 22

		}

		public Alchemist2BazaarAddon( Serial serial ) : base( serial )
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

	public class Alchemist2BazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new Alchemist2BazaarAddon();
			}
		}

		[Constructable]
		public Alchemist2BazaarAddonDeed()
		{
			Name = "Alchemist2Bazaar";
		}

		public Alchemist2BazaarAddonDeed( Serial serial ) : base( serial )
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