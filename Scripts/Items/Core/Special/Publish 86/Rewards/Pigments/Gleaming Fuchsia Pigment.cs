using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class GleamingFuchsiaPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1152311; } }

        [Constructable]
        public GleamingFuchsiaPigment(int Uses)
            : base(0x078A, Uses)
        {
        }
        [Constructable]
        public GleamingFuchsiaPigment()
            : this(5)
        {

        }
        public GleamingFuchsiaPigment(Serial serial)
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