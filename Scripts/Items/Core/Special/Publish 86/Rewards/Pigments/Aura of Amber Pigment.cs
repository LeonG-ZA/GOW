using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class AuraofAmberPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1152308; } }

        [Constructable]
        public AuraofAmberPigment(int Uses)
            : base(0x07AF, Uses)
        {
        }
        [Constructable]
        public AuraofAmberPigment()
            : this(5)
        {

        }
        public AuraofAmberPigment(Serial serial)
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
