using System;
using Server.Network;
using Server.Prompts;
using Server.Multis;
using Server.Regions;

namespace Server.Items
{
	public class DarkKnightRune : RecallRune
	{
		public override int LabelNumber { get { return 1077773; } } // The Darkness

		[Constructable]
		public DarkKnightRune()
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
		}

		public DarkKnightRune( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}
}