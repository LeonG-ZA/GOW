using System;

namespace Server.Items
{
    public class AncientJukanWaymasterBo : BlackStaff
    {
        [Constructable]
        public AncientJukanWaymasterBo()
            : base()
        {
            Hue = 2708;
        }

        public override int LabelNumber { get { return 1153848; } }// Ancient Jukan Waymaster Bo

        public AncientJukanWaymasterBo(Serial serial)
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