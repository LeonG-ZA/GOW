using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Items;
using Server.Engines.Quests;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
    [CorpseName("a desert rotworm corpse")]
    public class DesertRotworm : RotWorm
    {
        [Constructable]
        public DesertRotworm()
            : base()
        {
            Name = "a desert rotworm";
            Hue = 2708;
        }

        public DesertRotworm(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}