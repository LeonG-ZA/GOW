using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class GlossyBluePigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1151910; } }

        [Constructable]
        public GlossyBluePigment(int Uses)
            : base(0x077C, Uses)
        {
        }
        [Constructable]
        public GlossyBluePigment()
            : this(5)
        {

        }
        public GlossyBluePigment(Serial serial)
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