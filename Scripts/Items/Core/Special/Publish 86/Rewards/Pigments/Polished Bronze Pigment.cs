using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class PolishedBronzePigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1151909; } }

        [Constructable]
        public PolishedBronzePigment(int Uses)
            : base(0x0798, Uses)
        {
        }
        [Constructable]
        public PolishedBronzePigment()
            : this(5)
        {

        }
        public PolishedBronzePigment(Serial serial)
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
