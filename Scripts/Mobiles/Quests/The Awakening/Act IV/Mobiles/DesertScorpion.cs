using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a desert scorpion corpse")]
    public class DesertScorpion : Scorpion
    {
        [Constructable]
        public DesertScorpion()
            : base()
        {
            Name = "a desert scorpion";
            Hue = 2499;
        }

        public DesertScorpion(Serial serial)
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