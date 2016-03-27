using System;

namespace Server.Items
{
    public class ArcaneCircleScroll : BaseSpellScroll
    {
        [Constructable]
        public ArcaneCircleScroll()
            : this(1)
        {
        }

        [Constructable]
        public ArcaneCircleScroll(int amount)
            : base(600, 0x2D51, amount)
        {
            this.Hue = 0x8FD;
        }

        public ArcaneCircleScroll(Serial serial)
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

    public class GiftOfRenewalScroll : BaseSpellScroll
    {
        [Constructable]
        public GiftOfRenewalScroll()
            : this(1)
        {
        }

        [Constructable]
        public GiftOfRenewalScroll(int amount)
            : base(601, 0x2D52, amount)
        {
            this.Hue = 0x8FD;
        }

        public GiftOfRenewalScroll(Serial serial)
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

    public class ImmolatingWeaponScroll : BaseSpellScroll
    {
        [Constructable]
        public ImmolatingWeaponScroll()
            : this(1)
        {
        }

        [Constructable]
        public ImmolatingWeaponScroll(int amount)
            : base(602, 0x2D53, amount)
        {
            this.Hue = 0x8FD;
        }

        public ImmolatingWeaponScroll(Serial serial)
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

    public class AttuneWeaponScroll : BaseSpellScroll
    {
        [Constructable]
        public AttuneWeaponScroll()
            : this(1)
        {
        }

        [Constructable]
        public AttuneWeaponScroll(int amount)
            : base(603, 0x2D54, amount)
        {
            this.Hue = 0x8FD;
        }

        public AttuneWeaponScroll(Serial serial)
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

    public class ThunderstormScroll : BaseSpellScroll
    {
        [Constructable]
        public ThunderstormScroll()
            : this(1)
        {
        }

        [Constructable]
        public ThunderstormScroll(int amount)
            : base(604, 0x2D55, amount)
        {
            this.Hue = 0x8FD;
        }

        public ThunderstormScroll(Serial serial)
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

    public class NatureFuryScroll : BaseSpellScroll
    {
        [Constructable]
        public NatureFuryScroll()
            : this(1)
        {
        }

        [Constructable]
        public NatureFuryScroll(int amount)
            : base(605, 0x2D56, amount)
        {
            this.Hue = 0x8FD;
        }

        public NatureFuryScroll(Serial serial)
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

    public class SummonFeyScroll : BaseSpellScroll
    {
        [Constructable]
        public SummonFeyScroll()
            : this(1)
        {
        }

        [Constructable]
        public SummonFeyScroll(int amount)
            : base(606, 0x2D57, amount)
        {
            this.Hue = 0x8FD;
        }

        public SummonFeyScroll(Serial serial)
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

    public class SummonFiendScroll : BaseSpellScroll
    {
        [Constructable]
        public SummonFiendScroll()
            : this(1)
        {
        }

        [Constructable]
        public SummonFiendScroll(int amount)
            : base(607, 0x2D58, amount)
        {
            this.Hue = 0x8FD;
        }

        public SummonFiendScroll(Serial serial)
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

    public class ReaperFormScroll : BaseSpellScroll
    {
        [Constructable]
        public ReaperFormScroll()
            : this(1)
        {
        }

        [Constructable]
        public ReaperFormScroll(int amount)
            : base(608, 0x2D59, amount)
        {
            this.Hue = 0x8FD;
        }

        public ReaperFormScroll(Serial serial)
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

    public class WildfireScroll : BaseSpellScroll
    {
        [Constructable]
        public WildfireScroll()
            : this(1)
        {
        }

        [Constructable]
        public WildfireScroll(int amount)
            : base(609, 0x2D5A, amount)
        {
            this.Hue = 0x8FD;
        }

        public WildfireScroll(Serial serial)
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

    public class EssenceOfWindScroll : BaseSpellScroll
    {
        [Constructable]
        public EssenceOfWindScroll()
            : this(1)
        {
        }

        [Constructable]
        public EssenceOfWindScroll(int amount)
            : base(610, 0x2D5B, amount)
        {
            this.Hue = 0x8FD;
        }

        public EssenceOfWindScroll(Serial serial)
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

    public class DryadAllureScroll : BaseSpellScroll
    {
        [Constructable]
        public DryadAllureScroll()
            : this(1)
        {
        }

        [Constructable]
        public DryadAllureScroll(int amount)
            : base(611, 0x2D5C, amount)
        {
            this.Hue = 0x8FD;
        }

        public DryadAllureScroll(Serial serial)
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

    public class EtherealVoyageScroll : BaseSpellScroll
    {
        [Constructable]
        public EtherealVoyageScroll()
            : this(1)
        {
        }

        [Constructable]
        public EtherealVoyageScroll(int amount)
            : base(612, 0x2D5D, amount)
        {
            this.Hue = 0x8FD;
        }

        public EtherealVoyageScroll(Serial serial)
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

    public class WordOfDeathScroll : BaseSpellScroll
    {
        [Constructable]
        public WordOfDeathScroll()
            : this(1)
        {
        }

        [Constructable]
        public WordOfDeathScroll(int amount)
            : base(613, 0x2D5E, amount)
        {
            this.Hue = 0x8FD;
        }

        public WordOfDeathScroll(Serial serial)
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

    public class GiftOfLifeScroll : BaseSpellScroll
    {
        [Constructable]
        public GiftOfLifeScroll()
            : this(1)
        {
        }

        [Constructable]
        public GiftOfLifeScroll(int amount)
            : base(614, 0x2D5F, amount)
        {
            this.Hue = 0x8FD;
        }

        public GiftOfLifeScroll(Serial serial)
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

    public class ArcaneEmpowermentScroll : BaseSpellScroll
    {
        [Constructable]
        public ArcaneEmpowermentScroll()
            : this(1)
        {
        }

        [Constructable]
        public ArcaneEmpowermentScroll(int amount)
            : base(615, 0x2D60, amount)
        {
            this.Hue = 0x8FD;
        }

        public ArcaneEmpowermentScroll(Serial serial)
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