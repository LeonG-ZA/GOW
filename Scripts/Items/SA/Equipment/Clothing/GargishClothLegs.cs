using System;

namespace Server.Items
{
    public class GargishClothLegs : BaseClothing
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
        public GargishClothLegs()
            : this(0)
        {
        }

        [Constructable]
        public GargishClothLegs(int hue)
            : base(0x040A, Layer.Pants, hue)
        {
            this.Weight = 2.0;
        }

        public override void OnAdded(IEntity parent)
        {
            if (parent is Mobile)
            {
                if (((Mobile)parent).Female)
                    this.ItemID = 0x0409;
                else
                    this.ItemID = 0x040A;
            }
        }

        public GargishClothLegs(Serial serial)
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