using System;

namespace Server.Items
{
    public class MashedSweetPotatoes : BaseFood
    {
        [Constructable]
        public MashedSweetPotatoes()
            : base(1, 0x4C05)
        {
            this.Name = "mashed sweet potatoes";
            this.Weight = 1.0;
            this.FillFactor = 2;
            this.Stackable = false;
            this.Hue = 42;
        }

        public MashedSweetPotatoes(Serial serial)
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