using System;

namespace Server.Items
{
    public class Backpack : BaseContainer, IDyable, IEngravable
    {
        [Constructable]
        public Backpack()
            : base(0xE75)
        {
            Layer = Layer.Backpack;
            Weight = 3.0;
        }

        public Backpack(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultMaxWeight
        {
            get
            {
                if (Core.ML)
                {
                    //Mobile m = ParentEntity as Mobile;
                    Mobile m = Parent as Mobile;
                    if (m != null && m.Player && m.Backpack == this)
                    {
                        return 550;
                    }
                    else
                    {
                        return base.DefaultMaxWeight;
                    }
                }
                else
                {
                    return base.DefaultMaxWeight;
                }
            }
        }
        public bool Dye(Mobile from, IDyeTub sender)
        {
            if (Deleted)
                return false;

            Hue = sender.DyedHue;

            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && ItemID == 0x9B2)
                ItemID = 0xE75;
        }
    }

    public class Pouch : TrapableContainer
    {
        [Constructable]
        public Pouch()
            : base(0xE79)
        {
            Weight = 1.0;
        }

        public Pouch(Serial serial)
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

    public class Bag : BaseContainer, IDyable, IEngravable
    {
        [Constructable]
        public Bag()
            : base(0xE76)
        {
            Weight = 2.0;
        }

        public Bag(Serial serial)
            : base(serial)
        {
        }

        public bool Dye(Mobile from, IDyeTub sender)
        {
            if (Deleted)
                return false;

            Hue = sender.DyedHue;

            return true;
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

    public class Barrel : BaseContainer
    {
        [Constructable]
        public Barrel()
            : base(0xE77)
        {
            Weight = 25.0;
        }

        public Barrel(Serial serial)
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

            if (Weight == 0.0)
                Weight = 25.0;
        }
    }

    public class Keg : BaseContainer
    {
        [Constructable]
        public Keg()
            : base(0xE7F)
        {
            Weight = 15.0;
        }

        public Keg(Serial serial)
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

    public class PicnicBasket : BaseContainer
    {
        [Constructable]
        public PicnicBasket()
            : base(0xE7A)
        {
            Weight = 2.0; // Stratics doesn't know weight
        }

        public PicnicBasket(Serial serial)
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

    public class Basket : BaseContainer
    {
        [Constructable]
        public Basket()
            : base(0x990)
        {
            Weight = 1.0; // Stratics doesn't know weight
        }

        public Basket(Serial serial)
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

    [Furniture]
    [Flipable(0x9AA, 0xE7D)]
    public class WoodenBox : LockableContainer, IEngravable
    {
        [Constructable]
        public WoodenBox()
            : base(0x9AA)
        {
            Weight = 4.0;
        }

        public WoodenBox(Serial serial)
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

    [Furniture]
    [Flipable(0x9A9, 0xE7E)]
    public class SmallCrate : LockableContainer, IEngravable
    {
        [Constructable]
        public SmallCrate()
            : base(0x9A9)
        {
            Weight = 2.0;
        }

        public SmallCrate(Serial serial)
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

            if (Weight == 4.0)
                Weight = 2.0;
        }
    }

    [Furniture]
    [Flipable(0xE3F, 0xE3E)]
    public class MediumCrate : LockableContainer, IEngravable
    {
        [Constructable]
        public MediumCrate()
            : base(0xE3F)
        {
            Weight = 2.0;
        }

        public MediumCrate(Serial serial)
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

            if (Weight == 6.0)
                Weight = 2.0;
        }
    }

    [Furniture]
    [Flipable(0xE3D, 0xE3C)]
    public class LargeCrate : LockableContainer, IEngravable
    {
        [Constructable]
        public LargeCrate()
            : base(0xE3D)
        {
            Weight = 1.0;
        }

        public LargeCrate(Serial serial)
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

            if (Weight == 8.0)
                Weight = 1.0;
        }
    }

    [DynamicFliping]
    [Flipable(0x9A8, 0xE80)]
    public class MetalBox : LockableContainer
    {
        [Constructable]
        public MetalBox()
            : base(0x9A8)
        {
        }

        public MetalBox(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && Weight == 3)
                Weight = -1;
        }
    }

    [DynamicFliping]
    [Flipable(0x9AB, 0xE7C)]
    public class MetalChest : LockableContainer, IEngravable
    {
        [Constructable]
        public MetalChest()
            : base(0x9AB)
        {
        }

        public MetalChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && Weight == 25)
                Weight = -1;
        }
    }

    [DynamicFliping]
    [Flipable(0xE41, 0xE40)]
    public class MetalGoldenChest : LockableContainer
    {
        [Constructable]
        public MetalGoldenChest()
            : base(0xE41)
        {
        }

        public MetalGoldenChest(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0 && Weight == 25)
                Weight = -1;
        }
    }

    [Furniture]
    [Flipable(0xe43, 0xe42)]
    public class WoodenChest : LockableContainer, IEngravable
    {
        [Constructable]
        public WoodenChest()
            : base(0xe43)
        {
            Weight = 2.0;
        }

        public WoodenChest(Serial serial)
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

            if (Weight == 15.0)
                Weight = 2.0;
        }
    }

    [Furniture]
    [Flipable(0x2857, 0x2858)]
    public class RedArmoire : BaseContainer, IEngravable
    {
        [Constructable]
        public RedArmoire()
            : base(0x2857)
        {
            Weight = 1.0;
        }

        public RedArmoire(Serial serial)
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

    [Furniture]
    [Flipable(0xa97, 0xa99, 0xa98, 0xa9a, 0xa9b, 0xa9c)]
    public class FullBookcase : BaseContainer
    {
        [Constructable]
        public FullBookcase()
            : base(0xA97)
        {
            Weight = 1.0;
        }

        public FullBookcase(Serial serial)
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

    [Furniture]
    [Flipable(0xa9d, 0xa9e)]
    public class EmptyBookcase : BaseContainer
    {
        [Constructable]
        public EmptyBookcase()
            : base(0xA9D)
        {
        }

        public EmptyBookcase(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version == 0 && Weight == 1.0)
                Weight = -1;
        }
    }

    [Furniture]
    [Flipable(0xa2c, 0xa34)]
    public class Drawer : BaseContainer
    {
        [Constructable]
        public Drawer()
            : base(0xA2C)
        {
            Weight = 1.0;
        }

        public Drawer(Serial serial)
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

    [Furniture]
    [Flipable(0xa30, 0xa38)]
    public class FancyDrawer : BaseContainer
    {
        [Constructable]
        public FancyDrawer()
            : base(0xA30)
        {
            Weight = 1.0;
        }

        public FancyDrawer(Serial serial)
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

    [Furniture]
    [Flipable(0xa4f, 0xa53)]
    public class Armoire : BaseContainer, IEngravable
    {
        [Constructable]
        public Armoire()
            : base(0xA4F)
        {
            Weight = 1.0;
        }

        public Armoire(Serial serial)
            : base(serial)
        {
        }

        public override void DisplayTo(Mobile m)
        {
            if (DynamicFurniture.Open(this, m))
                base.DisplayTo(m);
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

            DynamicFurniture.Close(this);
        }
    }

    [Furniture]
    [Flipable(0xa4d, 0xa51)]
    public class FancyArmoire : BaseContainer, IEngravable
    {
        [Constructable]
        public FancyArmoire()
            : base(0xA4D)
        {
            Weight = 1.0;
        }

        public FancyArmoire(Serial serial)
            : base(serial)
        {
        }

        public override void DisplayTo(Mobile m)
        {
            if (DynamicFurniture.Open(this, m))
                base.DisplayTo(m);
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

            DynamicFurniture.Close(this);
        }
    }
}