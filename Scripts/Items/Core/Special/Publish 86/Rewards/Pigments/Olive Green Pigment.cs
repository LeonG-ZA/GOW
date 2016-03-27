using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class OliveGreenPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1154733; } }

        [Constructable]
        public OliveGreenPigment(int Uses)
            : base(0x0A95, Uses)
        {
        }
        [Constructable]
        public OliveGreenPigment()
            : this(5)
        {

        }
        public OliveGreenPigment(Serial serial)
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