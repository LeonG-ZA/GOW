using System;
using Server;

namespace Server.Items
{
	public class VEEarrings : SilverEarrings
	{
		public override int LabelNumber{ get{ return 1150259; } } // Earrings of Virtuous Epiphany
		public override int ArtifactRarity{ get{ return 10; } }

		[Constructable]
		public VEEarrings()
		{
			Hue = 0xA34;

            Attributes.Luck = 50;
			Attributes.RegenMana = 1;
            Resistances.Fire = 1;
			Resistances.Cold = 2;
			Resistances.Poison = 2;
            Resistances.Physical = 2;
            Resistances.Energy = 3;
		}

		public VEEarrings( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
