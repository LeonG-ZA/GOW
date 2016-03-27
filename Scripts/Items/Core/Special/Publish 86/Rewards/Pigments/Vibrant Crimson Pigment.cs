using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class VibrantCrimsonPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1153386; } }

        [Constructable]
        public VibrantCrimsonPigment(int Uses)
            : base(0x07AC, Uses)
        {
        }
        [Constructable]
        public VibrantCrimsonPigment()
            : this(5)
        {

        }
        public VibrantCrimsonPigment(Serial serial)
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
