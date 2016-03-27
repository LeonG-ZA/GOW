using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class MotherofPearlPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1154120; } }

        [Constructable]
        public MotherofPearlPigment(int Uses)
            : base(0x0AA0, Uses)
        {
        }
        [Constructable]
        public MotherofPearlPigment()
            : this(5)
        {

        }
        public MotherofPearlPigment(Serial serial)
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