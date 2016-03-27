using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class CarpetVendor : BaseVendor
	{
		private List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos{ get { return m_SBInfos; } }

		[Constructable]
		public CarpetVendor() : base( "the weaver" )
		{
			SetSkill( SkillName.Tracking, 64.0, 100.0 );
			SetSkill( SkillName.ItemID, 60.0, 100.0 );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBCarpets() );

		}
		public override void InitBody()
		{
			Name = "Laifem";
			Hue = 1030;
			Body = 0x190;
						
		}

		public override VendorShoeType ShoeType { get { return VendorShoeType.Sandals; } }

		public override void InitOutfit()
		{
			//base.InitOutfit();

			AddItem( new Robe( Utility.RandomRedHue() ) );
			AddItem( new FeatheredHat( Utility.RandomRedHue() ) );
			
		}

		public CarpetVendor( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
