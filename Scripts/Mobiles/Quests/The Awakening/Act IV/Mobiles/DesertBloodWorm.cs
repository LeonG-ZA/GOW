using System;
using Server;
using Server.Network;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
    [CorpseName("a desert bloodworm corpse")]
    public class DesertBloodWorm : BloodWorm
    {
        [Constructable]
        public DesertBloodWorm()
            : base()
        {
            Name = "a desert bloodworm";
            Hue = 1956;
        }

        public DesertBloodWorm(Serial serial)
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