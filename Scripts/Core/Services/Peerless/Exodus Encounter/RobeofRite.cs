using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class RobeofRite : TransientItem
    {
        [Constructable]
        public RobeofRite()
            : base(0x1F04, TimeSpan.FromSeconds(21600.0))
        {
            Hue = 2062;
        }

        public RobeofRite(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 1153510; } }// Robe of Rite

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

 