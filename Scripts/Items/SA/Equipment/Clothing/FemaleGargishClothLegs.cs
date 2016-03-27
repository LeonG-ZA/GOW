using System;

namespace Server.Items
{
    public class FemaleGargishClothLegs : BaseClothing
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
        public FemaleGargishClothLegs()
            : this(0)
        {
        }

        [Constructable]
        public FemaleGargishClothLegs(int hue)
            : base(0x0409, Layer.Pants, hue)
        {
            this.Weight = 2.0;
        }

        public FemaleGargishClothLegs(Serial serial)
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