using System;

namespace Server.Items
{
    public class FemaleGargishClothArms : BaseClothing
    {
        [Constructable]
        public FemaleGargishClothArms()
            : this(0)
        {
        }

        [Constructable]
        public FemaleGargishClothArms(int hue)
            : base(0x0403, Layer.Arms, hue)
        {
            this.Weight = 2.0;
        }

        public FemaleGargishClothArms(Serial serial)
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