using System;

namespace Server.Items
{
    public class AntiqueWeddingDress : PlainDress
    {
        [Constructable]
        public AntiqueWeddingDress()
        {
			this.Hue = 2961;
        }

        public AntiqueWeddingDress(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1149958;
            }
        }// An Antique Wedding Dress

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}