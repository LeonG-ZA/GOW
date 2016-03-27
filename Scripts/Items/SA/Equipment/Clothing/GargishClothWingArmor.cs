using System;

namespace Server.Items
{
    [FlipableAttribute(0x45A4, 0x45A5)]
    public class GargishClothWingArmor : BaseClothing
    {
        public override Race RequiredRace
        {
            get
            {
                return Race.Gargoyle;
            }
        }
        public override bool CanBeWornByGargoyles
        {
            get
            {
                return true;
            }
        }

        [Constructable]
        public GargishClothWingArmor()
            : this(0)
        {
        }

        [Constructable]
        public GargishClothWingArmor(int hue)
            : base(0x45A4, Layer.Cloak, hue)
        {
            this.Weight = 2.0;
        }

        public GargishClothWingArmor(Serial serial)
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