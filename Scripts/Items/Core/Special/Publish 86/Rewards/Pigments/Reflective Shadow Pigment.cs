using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class ReflectiveShadowPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1153387; } }

        [Constructable]
        public ReflectiveShadowPigment(int Uses)
            : base(0x0776, Uses)
        {
        }
        [Constructable]
        public ReflectiveShadowPigment()
            : this(5)
        {

        }
        public ReflectiveShadowPigment(Serial serial)
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