using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class Provisioner2BazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {3624, -1, 1, 5}, {1297, 2, 1, 0}, {1297, 0, 2, 0}// 1	2	3	
			, {1297, -1, 2, 0}, {1297, 1, -1, 0}, {1297, 1, 2, 0}// 4	5	6	
			, {1297, 0, -1, 0}, {18747, 2, -1, 0}, {1297, 2, -1, 0}// 7	8	9	
			, {1297, 2, 2, 0}, {1297, 0, 1, 0}, {1297, 2, 0, 0}// 10	11	12	
			, {1297, 1, 1, 0}, {1297, -1, 0, 0}, {1297, 0, 0, 0}// 13	14	15	
			, {1297, 1, 0, 0}, {1297, -1, 1, 0}, {18058, -1, 0, 5}// 16	17	18	
			, {3835, -1, 2, 5}, {2822, -1, 2, 0}, {2824, -1, 0, 0}// 19	20	21	
			, {2823, -1, 1, 0}, {2535, -1, 1, 3}, {1297, -1, -1, 0}// 22	23	24	
			, {17062, -1, -1, 0}// 25	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new Provisioner2BazaarAddonDeed();
			}
		}

		[ Constructable ]
		public Provisioner2BazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


		}

		public Provisioner2BazaarAddon( Serial serial ) : base( serial )
		{
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

	public class Provisioner2BazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new Provisioner2BazaarAddon();
			}
		}

		[Constructable]
		public Provisioner2BazaarAddonDeed()
		{
			Name = "Provisioner2Bazaar";
		}

		public Provisioner2BazaarAddonDeed( Serial serial ) : base( serial )
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