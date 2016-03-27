using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ProvisionerBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {11739, -1, -1, 0}, {2449, 1, 2, 15}, {11753, 1, 1, 0}// 1	2	3	
			, {1250, 2, 0, 0}, {1250, 2, -1, 0}, {3707, -1, 0, 2}// 4	5	6	
			, {1250, 1, 2, 0}, {1250, 2, 2, 0}, {1250, 2, 1, 0}// 7	8	9	
			, {7708, 1, 2, 17}, {1250, 1, -1, 0}, {1250, -1, 1, 0}// 10	11	12	
			, {1250, -1, 2, 0}, {1250, 0, -1, 0}, {1250, 1, 1, 0}// 13	14	15	
			, {1250, 1, 0, 0}, {1250, 0, 0, 0}, {1250, 0, 1, 0}// 16	17	18	
			, {1250, -1, 0, 0}, {1250, -1, -1, 0}, {1250, 0, 2, 0}// 19	20	21	
			, {11754, 2, 1, 0}// 22	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new ProvisionerBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public ProvisionerBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


		}

		public ProvisionerBazaarAddon( Serial serial ) : base( serial )
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

	public class ProvisionerBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new ProvisionerBazaarAddon();
			}
		}

		[Constructable]
		public ProvisionerBazaarAddonDeed()
		{
			Name = "ProvisionerBazaar";
		}

		public ProvisionerBazaarAddonDeed( Serial serial ) : base( serial )
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