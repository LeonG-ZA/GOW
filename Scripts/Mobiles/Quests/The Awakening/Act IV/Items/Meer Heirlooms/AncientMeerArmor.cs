using System;

namespace Server.Items
{
    public class AncientMeerArmor : StuddedChest
    {
        [Constructable]
        public AncientMeerArmor()
            : base()
        {
            Hue = 2708;
        }

        public override int LabelNumber { get { return 1153863; } }// Ancient Meer Armor

        public AncientMeerArmor(Serial serial)
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