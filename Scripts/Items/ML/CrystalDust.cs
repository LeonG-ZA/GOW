using System;

namespace Server.Items
{
    public class CrystalDust : Item
    {
        [Constructable]
        public CrystalDust()
            : base(0x223B)
        {
        }

        public CrystalDust(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1112328;
            }
        }// Crystal Dust
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