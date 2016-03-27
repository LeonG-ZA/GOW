using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class LiquidSunshinePigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1154213; } }

        [Constructable]
        public LiquidSunshinePigment(int Uses)
            : base(0x0EFF, Uses)
        {
        }
        [Constructable]
        public LiquidSunshinePigment()
            : this(5)
        {

        }
        public LiquidSunshinePigment(Serial serial)
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