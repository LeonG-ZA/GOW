using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ArtistBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {1327, 0, 2, 0}, {1327, 0, 1, 0}, {1327, 1, 0, 0}// 1	2	3	
			, {1327, 1, 1, 0}, {1327, 1, 2, 0}, {1327, 2, -1, 0}// 4	5	6	
			, {1327, 2, 0, 0}, {1327, 2, 2, 0}, {1327, 2, 1, 0}// 7	8	9	
			, {1327, 0, -1, 0}, {1327, 0, 0, 0}, {1327, 1, -1, 0}// 10	11	12	
			, {1327, -1, -1, 0}, {1327, -1, 0, 0}, {1327, -1, 1, 0}// 13	14	15	
			, {1327, -1, 2, 0}, {3955, -1, 0, 0}, {2927, 2, 1, 0}// 16	17	19	
			, {2926, 2, 2, 0}, {3953, 1, -1, 0}, {4033, 2, 2, 6}// 20	23	25	
					};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new ArtistBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public ArtistBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


			AddComplexComponent( (BaseAddon) this, 7570, 1, 0, 0, 488, -1, "Wet Paint", 1);// 18
			AddComplexComponent( (BaseAddon) this, 11617, 2, 1, 6, 0, -1, "Paint Can", 1);// 21
			AddComplexComponent( (BaseAddon) this, 4653, 0, 2, 0, 18, -1, "", 1);// 22
			AddComplexComponent( (BaseAddon) this, 7574, 0, 1, 3, 218, -1, "Fresh Paint", 1);// 24
			AddComplexComponent( (BaseAddon) this, 4654, 1, 0, 0, 19, -1, "", 1);// 26
			AddComplexComponent( (BaseAddon) this, 4651, 2, 1, 6, 5, -1, "", 1);// 27

		}

		public ArtistBazaarAddon( Serial serial ) : base( serial )
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

	public class ArtistBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new ArtistBazaarAddon();
			}
		}

		[Constructable]
		public ArtistBazaarAddonDeed()
		{
			Name = "ArtistBazaar";
		}

		public ArtistBazaarAddonDeed( Serial serial ) : base( serial )
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