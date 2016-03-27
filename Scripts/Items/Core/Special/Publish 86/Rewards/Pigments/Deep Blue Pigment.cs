using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class DeepBluePigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1152348; } }

        [Constructable]
        public DeepBluePigment(int Uses)
            : base(0x0793, Uses)
        {
        }
        [Constructable]
        public DeepBluePigment()
            : this(5)
        {

        }
        public DeepBluePigment(Serial serial)
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
