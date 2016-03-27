using System;

namespace Server.Items
{
    public class AncientJukanArmor : PlateHaidate
    {
        [Constructable]
        public AncientJukanArmor()
            : base()
        {
            Hue = 2708;
        }

        public override int LabelNumber { get { return 1153853; } }// Ancient Jukan Armor

        public AncientJukanArmor(Serial serial)
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