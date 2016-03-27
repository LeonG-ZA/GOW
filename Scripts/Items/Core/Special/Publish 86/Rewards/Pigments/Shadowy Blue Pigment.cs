using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class ShadowyBluePigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1152350; } }

        [Constructable]
        public ShadowyBluePigment(int Uses)
            : base(0x07A8, Uses)
        {
        }
        [Constructable]
        public ShadowyBluePigment()
            : this(5)
        {

        }
        public ShadowyBluePigment(Serial serial)
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