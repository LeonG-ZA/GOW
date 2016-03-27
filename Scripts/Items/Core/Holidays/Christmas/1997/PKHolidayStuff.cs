using System;

namespace Server.Items
{
    public class NaughtyBag : Bag
    {
        [Constructable]
        public NaughtyBag()
            : this(1)
        {
            Movable = true;
            Hue = 0x3d0;
        }

        [Constructable]
        public NaughtyBag(int amount)
        {
            DropItem(new Coal());
            DropItem(new BadCard());
            DropItem(new Spam());
            DropItem(new Switches());
        }

        public NaughtyBag(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041052;
            }
        }// You were naughty this year!

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

    public class Coal : Item
    {
        [Constructable]
        public Coal()
            : base(0x19b9)
        {
            Stackable = false;
            LootType = LootType.Blessed;
            Hue = 0x965;
        }

        public Coal(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "Coal";
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

    public class BadCard : Item
    {
        [Constructable]
        public BadCard()
            : base(0x14ef)
        {
            int[] BadCardHues = new int[] { 0x45, 0x27, 0x3d0 };
            Hue = BadCardHues[Utility.Random(BadCardHues.Length)];
            Stackable = false;
            LootType = LootType.Blessed;
            Movable = true;
        }

        public BadCard(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041428;
            }
        }// Maybe next year youll get a better...
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

    public class Spam : BaseFood
    {
        [Constructable]
        public Spam()
            : base(0x1044)
        {
            Stackable = false;
            LootType = LootType.Blessed;
        }

        public Spam(Serial serial)
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