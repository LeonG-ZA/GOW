using System;

namespace Server.Items
{
    public class FemaleGargishClothKilt : BaseClothing
    {
        [Constructable]
        public FemaleGargishClothKilt()
            : this(0)
        {
        }

        [Constructable]
        public FemaleGargishClothKilt(int hue)
            : base(0x0407, Layer.OuterLegs, hue)
        {
            this.Weight = 2.0;
        }

        public FemaleGargishClothKilt(Serial serial)
            : base(serial)
        {
        }

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