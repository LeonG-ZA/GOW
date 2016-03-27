using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class MurkyAmberPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1152350; } }

        [Constructable]
        public MurkyAmberPigment(int Uses)
            : base(0x07C5, Uses)
        {
        }
        [Constructable]
        public MurkyAmberPigment()
            : this(5)
        {

        }
        public MurkyAmberPigment(Serial serial)
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
