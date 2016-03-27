using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class ExodusSummoningRite : TransientItem
    {
        [Constructable]
        public ExodusSummoningRite()
            : base(0x2258, TimeSpan.FromSeconds(21600.0))
        {
            Hue = 2360;
        }

        public ExodusSummoningRite(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 1153498;} }// Exodus Summoning Rite

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
