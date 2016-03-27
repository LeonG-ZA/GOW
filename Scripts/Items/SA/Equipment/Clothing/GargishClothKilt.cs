using System;

namespace Server.Items
{
    public class GargishClothKilt : BaseClothing
    {
        [Constructable]
        public GargishClothKilt()
            : this(0)
        {
        }

        [Constructable]
        public GargishClothKilt(int hue)
            : base(0x0408, Layer.OuterLegs, hue)
        {
            this.Weight = 2.0;
        }

        public GargishClothKilt(Serial serial)
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
        public override void OnAdded(IEntity parent)
        {
            if (parent is Mobile)
            {
                if (((Mobile)parent).Female)
                    this.ItemID = 0x0407;
                else
                    this.ItemID = 0x0408;
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