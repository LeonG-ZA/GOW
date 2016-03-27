using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class AncientPotteryFragments : Item
    {
        [Constructable]
        public AncientPotteryFragments()
            : this(1)
        {
        }

        [Constructable]
        public AncientPotteryFragments(int amount)
            : base(0x2F5F)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public AncientPotteryFragments(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1112990;
            }
        }// Ancient Pottery fragments
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

    public class CongealedSlugAcid : Item
    {
        [Constructable]
        public CongealedSlugAcid()
            : this(1)
        {
        }

        [Constructable]
        public CongealedSlugAcid(int amount)
            : base(0x5742)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public CongealedSlugAcid(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1112901;
            }
        }// Congealed Slug Acid
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

    public class FairyDragonWing : Item
    {
        [Constructable]
        public FairyDragonWing()
            : this(1)
        {
        }

        [Constructable]
        public FairyDragonWing(int amount)
            : base(0x5726)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public FairyDragonWing(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1112899;
            }
        }// Fairy Dragon Wing
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

    public class LeatherWolfSkin : Item
    {
        [Constructable]
        public LeatherWolfSkin()
            : this(1)
        {
        }

        [Constructable]
        public LeatherWolfSkin(int amount)
            : base(0x3189)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public LeatherWolfSkin(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1112906;
            }
        }// leather wolf skin
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

    public class PileInspectedIngots : Item
    {
        [Constructable]
        public PileInspectedIngots()
            : this(1)
        {
        }

        [Constructable]
        public PileInspectedIngots(int amount)
            : base(0x2F5F)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public PileInspectedIngots(Serial serial)
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

    public class TatteredAncientScroll : Item
    {
        [Constructable]
        public TatteredAncientScroll()
            : this(1)
        {
        }

        [Constructable]
        public TatteredAncientScroll(int amount)
            : base(0x2F5F)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public TatteredAncientScroll(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1112991;
            }
        }// Tattered Remnants of an Ancient Scroll
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

    public class UntransTome : Item
    {
        [Constructable]
        public UntransTome()
            : this(1)
        {
        }

        [Constructable]
        public UntransTome(int amount)
            : base(0x2F5F)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public UntransTome(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1112992;
            }
        }// Untranslated Ancient Tome
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