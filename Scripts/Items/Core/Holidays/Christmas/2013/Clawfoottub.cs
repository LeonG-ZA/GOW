using System;

namespace Server.Items
{
    public class ClawFootTubComponent : AddonComponent
    {
        public ClawFootTubComponent(int itemID)
            : base(itemID)
        {
        }

        public ClawFootTubComponent(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }

    [FlipableAddon(Direction.South, Direction.East)]
    public class ClawFootTubAddon : BaseAddon
    {
        [Constructable]
        public ClawFootTubAddon()
            : base()
        {
            this.Direction = Direction.South;
            this.AddComponent(new ClawFootTubComponent(0x996B), 0, 0, 0);
            this.AddComponent(new ClawFootTubComponent(0x996C), 0, -1, 0);
            this.AddComponent(new ClawFootTubComponent(0x996D), 0, -2, 0);
        }

        public ClawFootTubAddon(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddonDeed Deed
        {
            get
            {
                return new ClawFootTubDeed();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }

        public virtual void Flip(Mobile from, Direction direction)
        {
            switch (direction)
            {
                case Direction.East:
                    this.AddComponent(new ClawFootTubComponent(0x9976), 0, 0, 0);
                    this.AddComponent(new ClawFootTubComponent(0x9977), -1, 0, 0);
                    this.AddComponent(new ClawFootTubComponent(0x9978), -2, 0, 0);
                    break;
                case Direction.South:
                    this.AddComponent(new ClawFootTubComponent(0x996B), 0, 0, 0);
                    this.AddComponent(new ClawFootTubComponent(0x996C), 0, -1, 0);
                    this.AddComponent(new ClawFootTubComponent(0x996D), 0, -2, 0);
                    break;
            }
        }
    }
    public class ClawFootTubDeed : BaseAddonDeed
    {
        [Constructable]
        public ClawFootTubDeed()
            : base()
        {
            this.Name = "Claw Foot Tub";
            this.LootType = LootType.Blessed;
        }

        public ClawFootTubDeed(Serial serial)
            : base(serial)
        {
        }

        public override BaseAddon Addon
        {
            get
            {
                return new ClawFootTubAddon();
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}