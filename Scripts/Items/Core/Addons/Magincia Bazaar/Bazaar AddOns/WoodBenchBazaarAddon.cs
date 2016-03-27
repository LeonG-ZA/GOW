using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class WoodBenchBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {9271, 1, 2, 0}, {9272, 1, 0, 0}, {9272, 0, 1, 0}// 1	2	3	
			, {9256, 2, 2, 0}, {9266, -1, 1, 0}, {9253, 2, -1, 0}// 4	5	6	
			, {9254, -1, 2, 0}, {9270, -1, 0, 0}, {9271, 0, -1, 0}// 7	8	9	
			, {9267, 1, -1, 0}, {9255, -1, -1, 0}, {9268, 2, 0, 0}// 10	11	12	
			, {9256, 1, 1, 0}, {9255, 0, 0, 0}, {9265, 0, 2, 0}// 13	14	15	
			, {9270, 2, 1, 0}, {2963, -1, 1, 0}, {2964, -1, 0, 0}// 16	17	18	
					};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new WoodBenchBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public WoodBenchBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


		}

		public WoodBenchBazaarAddon( Serial serial ) : base( serial )
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

	public class WoodBenchBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new WoodBenchBazaarAddon();
			}
		}

		[Constructable]
		public WoodBenchBazaarAddonDeed()
		{
			Name = "WoodBenchBazaar";
		}

		public WoodBenchBazaarAddonDeed( Serial serial ) : base( serial )
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