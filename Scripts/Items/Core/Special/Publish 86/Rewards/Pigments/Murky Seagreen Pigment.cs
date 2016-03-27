using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class MurkySeagreenPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1152309; } }

        [Constructable]
        public MurkySeagreenPigment(int Uses)
            : base(0x07C8, Uses)
        {
        }
        [Constructable]
        public MurkySeagreenPigment()
            : this(5)
        {

        }
        public MurkySeagreenPigment(Serial serial)
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