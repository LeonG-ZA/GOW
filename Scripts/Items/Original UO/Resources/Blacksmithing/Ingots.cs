using System;

namespace Server.Items
{
    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class IronIngot : BaseIngot
    {
        [Constructable]
        public IronIngot()
            : this(1)
        {
        }

        [Constructable]
        public IronIngot(int amount)
            : base(CraftResource.Iron, amount)
        {
        }

        public IronIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class DullCopperIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.DullCopper; } }

        [Constructable]
        public DullCopperIngot()
            : this(1)
        {
        }

        [Constructable]
        public DullCopperIngot(int amount)
            : base(CraftResource.DullCopper, amount)
        {
        }

        public DullCopperIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class ShadowIronIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.ShadowIron; } }

        [Constructable]
        public ShadowIronIngot()
            : this(1)
        {
        }

        [Constructable]
        public ShadowIronIngot(int amount)
            : base(CraftResource.ShadowIron, amount)
        {
        }

        public ShadowIronIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class CopperIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Copper; } }

        [Constructable]
        public CopperIngot()
            : this(1)
        {
        }

        [Constructable]
        public CopperIngot(int amount)
            : base(CraftResource.Copper, amount)
        {
        }

        public CopperIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class BronzeIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Bronze; } }

        [Constructable]
        public BronzeIngot()
            : this(1)
        {
        }

        [Constructable]
        public BronzeIngot(int amount)
            : base(CraftResource.Bronze, amount)
        {
        }

        public BronzeIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class GoldIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Gold; } }

        [Constructable]
        public GoldIngot()
            : this(1)
        {
        }

        [Constructable]
        public GoldIngot(int amount)
            : base(CraftResource.Gold, amount)
        {
        }

        public GoldIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class AgapiteIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Agapite; } }

        [Constructable]
        public AgapiteIngot()
            : this(1)
        {
        }

        [Constructable]
        public AgapiteIngot(int amount)
            : base(CraftResource.Agapite, amount)
        {
        }

        public AgapiteIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class VeriteIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Verite; } }

        [Constructable]
        public VeriteIngot()
            : this(1)
        {
        }

        [Constructable]
        public VeriteIngot(int amount)
            : base(CraftResource.Verite, amount)
        {
        }

        public VeriteIngot(Serial serial)
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

    [FlipableAttribute(0x1BF2, 0x1BEF)]
    public class ValoriteIngot : BaseIngot
    {
        protected override CraftResource DefaultResource { get { return CraftResource.Valorite; } }

        [Constructable]
        public ValoriteIngot()
            : this(1)
        {
        }

        [Constructable]
        public ValoriteIngot(int amount)
            : base(CraftResource.Valorite, amount)
        {
        }

        public ValoriteIngot(Serial serial)
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