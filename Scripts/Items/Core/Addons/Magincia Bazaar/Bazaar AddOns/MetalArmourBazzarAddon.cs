using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class MetalArmourBazzarAddon : BaseAddon
	{
        private static int[,] m_AddOnSimpleComponents = new int[,] {
			  {1184, -1, -1, 0}, {1184, -1, 0, 0}, {1184, -1, 1, 0}// 1	2	3	
			, {1184, -1, 2, 0}, {1184, 0, -1, 0}, {1184, 0, 0, 0}// 4	5	6	
			, {1184, 0, 1, 0}, {1184, 0, 2, 0}, {1184, 1, -1, 0}// 7	8	9	
			, {1184, 1, 0, 0}, {1184, 1, 1, 0}, {1184, 1, 2, 0}// 10	11	12	
			, {1184, 2, -1, 0}, {1184, 2, 0, 0}, {1184, 2, 1, 0}// 13	14	15	
			, {1184, 2, 2, 0}, {5093, -1, -1, 0}, {5097, 0, -1, 2}// 16	17	18	
			, {5095, 1, -1, 0}, {4271, -1, -1, 0}, {4271, 2, -1, 0}// 19	20	21	
			, {4272, -1, -1, 0}, {4272, 0, -1, 0}, {4272, 1, -1, 0}// 22	23	24	
			, {9, -1, -2, 0}, {9, 2, -2, 0}, {7111, 2, -1, 0}// 25	26	27	
			, {7620, 2, 2, 0}, {7621, 0, 2, 0}, {7622, 1, 2, 0}// 28	29	30	
			, {3936, 1, 2, 6}, {5049, 2, 2, 6}, {5120, 1, 2, 8}// 31	32	33	
			, {3779, 2, 2, 8}// 34	
		};

 
            
		public override BaseAddonDeed Deed
		{
			get
			{
				return new MetalArmourBazzarAddonDeed();
			}
		}

		[ Constructable ]
		public MetalArmourBazzarAddon()
		{

            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent( new AddonComponent( m_AddOnSimpleComponents[i,0] ), m_AddOnSimpleComponents[i,1], m_AddOnSimpleComponents[i,2], m_AddOnSimpleComponents[i,3] );


		}

		public MetalArmourBazzarAddon( Serial serial ) : base( serial )
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

	public class MetalArmourBazzarAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new MetalArmourBazzarAddon();
			}
		}

		[Constructable]
		public MetalArmourBazzarAddonDeed()
		{
			Name = "MetalArmourBazzar";
		}

		public MetalArmourBazzarAddonDeed( Serial serial ) : base( serial )
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