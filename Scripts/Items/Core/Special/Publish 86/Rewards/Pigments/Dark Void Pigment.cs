using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class DarkVoidPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1154214; } }

        [Constructable]
        public DarkVoidPigment(int Uses)
            : base(0x0814, Uses)
        {
        }
        [Constructable]
        public DarkVoidPigment()
            : this(5)
        {

        }
        public DarkVoidPigment(Serial serial)
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