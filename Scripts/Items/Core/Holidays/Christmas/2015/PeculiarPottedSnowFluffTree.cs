using System;

namespace Server.Items
{
	public class PeculiarPottedSnowFluffTree : Item
	{
		[Constructable]
		public PeculiarPottedSnowFluffTree( ) : base(0x9B0F)
		{
            Name = ("Peculiar Potted Snowfluff Tree");
            LootType = LootType.Blessed;
            Hue = Utility.RandomList(1166, 1618, 1910, 1914, 1968, 1287, 2598, 2729, 0);
			Weight = 1.0;
		}

        public PeculiarPottedSnowFluffTree(Serial serial)
            : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
