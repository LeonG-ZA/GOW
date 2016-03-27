using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class VibrantOcherPigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1154736; } }

        [Constructable]
        public VibrantOcherPigment(int Uses)
            : base(0x0AA5, Uses)
        {
        }
        [Constructable]
        public VibrantOcherPigment()
            : this(5)
        {

        }
        public VibrantOcherPigment(Serial serial)
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