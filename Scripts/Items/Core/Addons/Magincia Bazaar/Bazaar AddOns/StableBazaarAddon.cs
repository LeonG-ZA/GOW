using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class StableBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {2149, -2, -2, 0}, {6954, 1, 2, 0}, {4090, 0, -1, 3}// 1	2	3	
			, {3894, -1, -1, 0}, {6952, -1, 1, 0}, {6958, 2, -1, 0}// 4	5	6	
			, {6951, 0, -1, 0}, {6954, 0, 2, 0}, {6951, 1, -1, 0}// 7	8	9	
			, {6953, 2, 0, 0}, {6953, 2, 1, 0}, {2146, 2, 2, 0}// 10	11	12	
			, {4150, 0, 0, 0}, {2148, 2, -1, 0}, {3900, -1, 1, 0}// 13	14	15	
			, {3899, 1, -1, 0}, {5193, 1, 0, 0}, {2147, -1, 2, 0}// 16	17	18	
			, {2148, -2, 2, 0}, {2148, -2, 1, 0}, {6956, 2, 2, 0}// 19	20	21	
			, {6955, -1, -1, 0}, {3896, -1, 1, 0}, {6952, -1, 0, 0}// 22	23	24	
			, {2147, 1, 2, 0}, {2148, -2, -1, 0}, {2148, 2, 1, 0}// 25	26	27	
			, {3893, 1, 1, 0}, {2158, 2, 0, 0}, {4977, 1, -1, 5}// 28	29	30	
			, {2148, -2, 0, 0}, {6957, -1, 2, 0}, {2147, 0, 2, 0}// 31	32	33	
			, {2147, 2, -2, 0}, {5193, 0, 1, 0}, {2147, -1, -2, 0}// 34	35	36	
			, {2147, 1, -2, 0}, {2147, 0, -2, 0}// 37	38	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new StableBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public StableBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


		}

		public StableBazaarAddon( Serial serial ) : base( serial )
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

	public class StableBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new StableBazaarAddon();
			}
		}

		[Constructable]
		public StableBazaarAddonDeed()
		{
			Name = "StableBazaar";
		}

		public StableBazaarAddonDeed( Serial serial ) : base( serial )
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