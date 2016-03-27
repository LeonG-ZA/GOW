using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class DeepVioletPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1151912; } }

        [Constructable]
        public DeepVioletPigment(int Uses)
            : base(0x0789, Uses)
        {

        }
        [Constructable]
        public DeepVioletPigment()
            : this(5)
        {

        }
        public DeepVioletPigment(Serial serial)
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