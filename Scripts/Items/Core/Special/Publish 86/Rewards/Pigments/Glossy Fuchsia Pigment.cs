using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class GlossyFuchsiaPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1152347; } }

        [Constructable]
        public GlossyFuchsiaPigment(int Uses)
            : base(0x077F, Uses)
        {
        }
        [Constructable]
        public GlossyFuchsiaPigment()
            : this(5)
        {

        }
        public GlossyFuchsiaPigment(Serial serial)
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