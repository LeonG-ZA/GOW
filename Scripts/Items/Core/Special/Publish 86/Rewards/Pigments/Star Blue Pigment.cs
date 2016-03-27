using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Server.Items
{
    public class StarBluePigment : BaseCustomPigment
    {
        public override int LabelNumber { get { return 1154121; } }

        [Constructable]
        public StarBluePigment(int Uses)
            : base(0x0AA3, Uses)
        {
        }
        [Constructable]
        public StarBluePigment()
            : this(5)
        {

        }
        public StarBluePigment(Serial serial)
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