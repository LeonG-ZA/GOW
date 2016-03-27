using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class BlackandGreenPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1151911; } }

        [Constructable]
        public BlackandGreenPigment(int Uses)
            : base(0x07BB, Uses)
        {
        }
        [Constructable]
        public BlackandGreenPigment()
            : this(5)
        {

        }
        public BlackandGreenPigment(Serial serial)
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