namespace Server.Items
{
    public class SmithingPressEastAddon : SpecialVeteranCraftAddon
    {
        [Constructable]
        public SmithingPressEastAddon()
            : this(0)
        { }

        [Constructable]
        public SmithingPressEastAddon(int uses)
        {
            UsesRemaining = uses;
            this.AddComponent(new SmithingPressComponent(0x9A89), 0, 0, 0); // 0x9AA9
            this.AddComponent(new SmithingPressBoxComponent(0x9A91), -1, 1, 0);
        }

        public SmithingPressEastAddon(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed
        {
            get
            {
                return new SmithingPressEastDeed(UsesRemaining);
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
    public class SmithingPressEastDeed : BaseAddonDeed
    {
        private int storedUses;
        [Constructable]
        public SmithingPressEastDeed()
            : this(0)
        {

        }
        public SmithingPressEastDeed(int uses)
        {
            storedUses = uses;
            Name = "smithing press (east)";
        }

        public SmithingPressEastDeed(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon
        {
            get
            {
                return new SmithingPressEastAddon(storedUses);
            }
        }
        public override int LabelNumber
        {
            get
            {
                return 1123577;
            }
        }// smithing press - in case the Name is reset or having the south there is not liked
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((int)storedUses);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            storedUses = reader.ReadInt();
        }
    }
}