using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class VibrantSeagreenPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1152349; } }

        [Constructable]
        public VibrantSeagreenPigment(int Uses)
            : base(0x07B2, Uses)
        {
        }
        [Constructable]
        public VibrantSeagreenPigment()
            : this(5)
        {

        }
        public VibrantSeagreenPigment(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }

    }
}