using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class MarbleStairBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {3225, -1, -1, 6}, {2868, 0, 0, 5}, {1802, 0, 2, 0}// 1	2	3	
			, {3223, 0, 0, 11}, {1806, -1, -1, 0}, {1805, -1, 0, 0}// 4	5	6	
			, {1803, 2, 1, 0}, {1805, -1, 1, 0}, {3227, -1, 2, 7}// 7	8	9	
			, {1809, -1, 2, 0}, {1808, 2, -1, 0}, {1804, 1, -1, 0}// 10	11	12	
			, {1802, 1, 2, 0}, {1804, 0, -1, 0}, {1803, 2, 0, 0}// 13	14	15	
			, {1807, 2, 2, 0}, {2867, 0, 1, 5}, {1801, 1, 0, 0}// 16	17	18	
			, {1801, 0, 0, 0}, {1801, 1, 1, 0}, {1801, 0, 1, 0}// 19	20	21	
					};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new MarbleStairBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public MarbleStairBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


		}

		public MarbleStairBazaarAddon( Serial serial ) : base( serial )
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

	public class MarbleStairBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new MarbleStairBazaarAddon();
			}
		}

		[Constructable]
		public MarbleStairBazaarAddonDeed()
		{
			Name = "MarbleStairBazaar";
		}

		public MarbleStairBazaarAddonDeed( Serial serial ) : base( serial )
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