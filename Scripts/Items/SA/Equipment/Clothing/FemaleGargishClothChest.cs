using System;

namespace Server.Items
{
    public class FemaleGargishClothChest : BaseClothing
    {
        [Constructable]
        public FemaleGargishClothChest()
            : this(0)
        {
        }

        [Constructable]
        public FemaleGargishClothChest(int hue)
            : base(0x0405, Layer.InnerTorso, hue)
        {
            this.Weight = 2.0;
        }

        public FemaleGargishClothChest(Serial serial)
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