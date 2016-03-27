using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class TalismanGrammarofOrchish : BaseTalisman, ICollectionItem
	{
		public override int LabelNumber { get { return 1074890; } } // Library Talisman - a Grammar of Orchish (Summoner)

		[Constructable]
		public TalismanGrammarofOrchish()
			: base( 0x2F59 )
		{
			Weight = 1.0;
            Protection = GetRandomProtection();
             Summoner = new TalismanAttribute(typeof(SummonedOrcBrute), 0, 1072414);
			SkillBonuses.SetValues( 0, SkillName.MagicResist, 5.0 );
			SkillBonuses.SetValues( 1, SkillName.Anatomy, 7.0 );
			Charges = -1;
		}

		public TalismanGrammarofOrchish( Serial serial )
			: base( serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}
