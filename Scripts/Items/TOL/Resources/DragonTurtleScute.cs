using System;

namespace Server.Items
{
    [FlipableAttribute(0x9BCE, 0x9BCF)]
    public class DragonTurtleScute : BaseLeather
    {
        [Constructable]
        public DragonTurtleScute()
            : this(1)
        {
        }

        [Constructable]
        public DragonTurtleScute(int amount)
            : base(CraftResource.RegularLeather, amount)
        {
        }

        public DragonTurtleScute(Serial serial)
            : base(serial)
        {
        }

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