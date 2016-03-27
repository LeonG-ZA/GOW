using System;
using Server.Gumps;

namespace Server.Items
{
        public class AnniversyBag18th : Bag
    {
        [Constructable]
        public AnniversyBag18th()
            : base()
        {
            Hue = 1164;
        }

        public AnniversyBag18th(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 1156141; } }//18th Anniversary Gift Bag

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