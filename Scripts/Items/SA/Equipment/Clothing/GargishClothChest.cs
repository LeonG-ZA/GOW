using System;

namespace Server.Items
{
    public class GargishClothChest : BaseClothing
    {
        [Constructable]
        public GargishClothChest()
            : this(0)
        {
        }

        [Constructable]
        public GargishClothChest(int hue)
            : base(0x0406, Layer.InnerTorso, hue)
        {
            this.Weight = 2.0;
        }

        public GargishClothChest(Serial serial)
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
                    this.ItemID = 0x0405;
                else
                    this.ItemID = 0x0406;
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