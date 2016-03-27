using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class BlackrockDetectorAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {98, -2, -3, 0}, {95, -1, -3, 0}, {95, 2, -2, 0}// 1	2	3	
			, {96, -3, 2, 0}, {96, 2, -1, 0}, {1876, 1, 0, 0}// 5	6	9	
			, {1892, 1, -1, 0}, {96, -2, 0, 0}, {96, 1, 3, 0}// 10	11	12	
			, {1299, -2, 2, 13}, {1891, -1, 1, 0}, {95, 1, 2, 0}// 13	14	15	
			, {97, -1, -2, 0}, {95, -1, 2, 0}, {95, 3, -2, 0}// 16	17	18	
			, {96, 2, 1, 0}, {1299, -1, -2, 13}, {97, 3, -1, 0}// 19	22	23	
			, {1890, -1, -1, 0}, {98, -3, 1, 0}, {96, 2, 0, 0}// 24	25	26	
			, {96, -2, -2, 0}, {1299, 2, 3, 13}, {97, 2, 2, 0}// 28	29	30	
			, {97, -2, 2, 0}, {95, 0, 2, 0}, {96, -2, -1, 0}// 31	32	33	
			, {1889, 1, 1, 0}, {95, 0, -2, 0}, {97, 2, 2, 0}// 34	35	36	
			, {97, -2, 1, 0}, {97, 2, 3, 0}, {1299, 3, -1, 13}// 38	40	46	
			, {98, 2, -2, 0}, {95, 1, -2, 0}, {1875, 0, 1, 0}// 47	48	49	
			, {1873, 0, -1, 0}, {1874, -1, 0, 0}// 50	51	
		};
      
		public override BaseAddonDeed Deed
		{
			get
			{
				return new BlackrockDetectorAddonDeed();
			}
		}

		[ Constructable ]
		public BlackrockDetectorAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 7386, 0, 1, 5, 591, -1, "", 1);// 4
			AddComplexComponent( (BaseAddon) this, 3679, -1, -2, 13, 0, 1, "", 1);// 7
			AddComplexComponent( (BaseAddon) this, 7386, 1, 0, 5, 591, -1, "", 1);// 8
			AddComplexComponent( (BaseAddon) this, 3688, 2, 3, 13, 0, 1, "", 1);// 20
			AddComplexComponent( (BaseAddon) this, 3682, 3, -1, 13, 0, 1, "", 1);// 21
			AddComplexComponent( (BaseAddon) this, 8705, 0, 0, 0, 1175, -1, "", 1);// 27
			AddComplexComponent( (BaseAddon) this, 7386, 1, 1, 5, 591, -1, "", 1);// 37
			AddComplexComponent( (BaseAddon) this, 3676, -2, 2, 13, 0, 1, "", 1);// 39
			AddComplexComponent( (BaseAddon) this, 7386, -1, 0, 5, 591, -1, "", 1);// 41
			AddComplexComponent( (BaseAddon) this, 7386, 0, -1, 5, 591, -1, "", 1);// 42
			AddComplexComponent( (BaseAddon) this, 7386, -1, -1, 5, 591, -1, "", 1);// 43
			AddComplexComponent( (BaseAddon) this, 7386, -1, 1, 5, 591, -1, "", 1);// 44
			AddComplexComponent( (BaseAddon) this, 7386, 1, -1, 5, 591, -1, "", 1);// 45

		}

		public BlackrockDetectorAddon( Serial serial ) : base( serial )
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

	public class BlackrockDetectorAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new BlackrockDetectorAddon();
			}
		}

		[Constructable]
		public BlackrockDetectorAddonDeed()
		{
			Name = "Blackrock Detector";
		}

		public BlackrockDetectorAddonDeed( Serial serial ) : base( serial )
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