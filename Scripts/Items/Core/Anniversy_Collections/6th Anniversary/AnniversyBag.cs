using System;

namespace Server.Items
{
        public class AnniversyBag6th : Bag
    {
        [Constructable]
        public AnniversyBag6th()
            : base()
        {
            Hue = Utility.RandomList(309, 399, 459, 469, 504, 509, 0);
        }

        public AnniversyBag6th(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 1062720; } }//Happy 6th Anniversary!

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