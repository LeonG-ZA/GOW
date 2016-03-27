using System;

namespace Server.Items
{
    public class Slither : BaseTalisman, ITokunoDyable
    {
        [Constructable]
        public Slither()
            : base(0x2F5B)
        {
            Hue = 1415;
				
            this.Blessed = RandomTalisman.GetRandomBlessed();

            Weight = 1.0;
            Hue = 1415;
            Attributes.BonusHits = 10;
            Attributes.RegenHits = 2;
            Attributes.DefendChance = 10;
            Charges = -1;
        }

        public Slither(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 1114782; } } // Slither

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); //version
        }
    }
}