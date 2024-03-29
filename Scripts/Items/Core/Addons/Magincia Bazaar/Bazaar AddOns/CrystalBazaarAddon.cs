using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class CrystalBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {10944, 2, -1, 0}, {1174, -1, -1, 0}, {1174, -1, 0, 0}// 1	2	3	
			, {1174, -1, 1, 0}, {1174, -1, 2, 0}, {1174, 0, -1, 0}// 4	5	6	
			, {1174, 0, 0, 0}, {1174, 0, 1, 0}, {1174, 0, 2, 0}// 7	8	9	
			, {1174, 1, -1, 0}, {1174, 1, 0, 0}, {1174, 1, 1, 0}// 10	11	12	
			, {1174, 1, 2, 0}, {1174, 2, -1, 0}, {1174, 2, 0, 0}// 13	14	15	
			, {1174, 2, 1, 0}, {1174, 2, 2, 0}, {12253, -1, -1, 0}// 16	17	18	
			, {12244, -1, 2, 0}, {11596, 2, 2, 0}, {11743, 1, -1, 0}// 19	20	21	
			, {11744, -1, 1, 0}, {8767, 0, 0, 0}, {8777, 2, 0, 0}// 22	23	24	
			, {8765, 1, 1, 0}, {8767, 0, 2, 0}// 25	26	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new CrystalBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public CrystalBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


		}

		public CrystalBazaarAddon( Serial serial ) : base( serial )
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

	public class CrystalBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new CrystalBazaarAddon();
			}
		}

		[Constructable]
		public CrystalBazaarAddonDeed()
		{
			Name = "CrystalBazaar";
		}

		public CrystalBazaarAddonDeed( Serial serial ) : base( serial )
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