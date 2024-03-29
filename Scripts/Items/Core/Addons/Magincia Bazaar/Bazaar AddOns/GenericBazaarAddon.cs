using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class GenericBazaarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {16534, 2, 1, 0}, {16534, 2, 0, 0}, {16534, -1, 1, 0}// 1	2	3	
			, {16534, -1, 0, 0}, {16532, 1, 2, 0}, {16532, 0, 2, 0}// 4	5	6	
			, {16532, 0, -1, 0}, {16532, 1, -1, 0}, {16536, -1, 2, 0}// 7	8	9	
			, {16538, 2, 2, 0}, {16533, 2, -1, 0}, {16531, -1, -1, 0}// 10	11	12	
			, {16530, 1, 1, 0}, {16530, 0, 1, 0}, {16530, 0, 0, 0}// 13	14	15	
			, {16530, 1, 0, 0}, {4643, -1, 2, 0}, {4643, -1, -1, 0}// 16	17	18	
			, {9009, -1, 2, 9}, {9009, -1, -1, 9}, {9241, 2, 2, 0}// 19	20	21	
			, {9241, 2, -1, 0}// 22	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new GenericBazaarAddonDeed();
			}
		}

		[ Constructable ]
		public GenericBazaarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


		}

		public GenericBazaarAddon( Serial serial ) : base( serial )
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

	public class GenericBazaarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new GenericBazaarAddon();
			}
		}

		[Constructable]
		public GenericBazaarAddonDeed()
		{
			Name = "GenericBazaar";
		}

		public GenericBazaarAddonDeed( Serial serial ) : base( serial )
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