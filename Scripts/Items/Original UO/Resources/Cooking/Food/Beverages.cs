using System;

namespace Server.Items
{
    public class Jug : BaseBeverage
    {
        public override int BaseLabelNumber
        {
            get
            {
                return 1042965;
            }
        }// a jug of Ale
        public override int MaxQuantity
        {
            get
            {
                return 10;
            }
        }
        public override bool Fillable
        {
            get
            {
                return false;
            }
        }

        public override int ComputeItemID()
        {
            if (!this.IsEmpty)
                return 0x9C8;

            return 0;
        }

        [Constructable]
        public Jug(BeverageType type)
            : base(type)
        {
            this.Weight = 1.0;
        }

        public Jug(Serial serial)
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
        }
    }

    public class CeramicMug : BaseBeverage
    {
        public override int BaseLabelNumber
        {
            get
            {
                return 1042982;
            }
        }// a ceramic mug of Ale
        public override int MaxQuantity
        {
            get
            {
                return 1;
            }
        }

        public override int ComputeItemID()
        {
            if (this.ItemID >= 0x995 && this.ItemID <= 0x999)
                return this.ItemID;
            else if (this.ItemID == 0x9CA)
                return this.ItemID;

            return 0x995;
        }

        [Constructable]
        public CeramicMug()
        {
            this.Weight = 1.0;
        }

        [Constructable]
        public CeramicMug(BeverageType type)
            : base(type)
        {
            this.Weight = 1.0;
        }

        public CeramicMug(Serial serial)
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
        }
    }

    public class PewterMug : BaseBeverage
    {
        public override int BaseLabelNumber
        {
            get
            {
                return 1042994;
            }
        }// a pewter mug with Ale
        public override int MaxQuantity
        {
            get
            {
                return 1;
            }
        }

        public override int ComputeItemID()
        {
            if (this.ItemID >= 0xFFF && this.ItemID <= 0x1002)
                return this.ItemID;

            return 0xFFF;
        }

        [Constructable]
        public PewterMug()
        {
            this.Weight = 1.0;
        }

        [Constructable]
        public PewterMug(BeverageType type)
            : base(type)
        {
            this.Weight = 1.0;
        }

        public PewterMug(Serial serial)
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
        }
    }

    public class Goblet : BaseBeverage
    {
        public override int BaseLabelNumber
        {
            get
            {
                return 1043000;
            }
        }// a goblet of Ale
        public override int MaxQuantity
        {
            get
            {
                return 1;
            }
        }

        public override int ComputeItemID()
        {
            if (this.ItemID == 0x99A || this.ItemID == 0x9B3 || this.ItemID == 0x9BF || this.ItemID == 0x9CB)
                return this.ItemID;

            return 0x99A;
        }

        [Constructable]
        public Goblet()
        {
            this.Weight = 1.0;
        }

        [Constructable]
        public Goblet(BeverageType type)
            : base(type)
        {
            this.Weight = 1.0;
        }

        public Goblet(Serial serial)
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
        }
    }

    [TypeAlias("Server.Items.MugAle", "Server.Items.GlassCider", "Server.Items.GlassLiquor",
        "Server.Items.GlassMilk", "Server.Items.GlassWine", "Server.Items.GlassWater")]
    public class GlassMug : BaseBeverage
    {
        public override int EmptyLabelNumber
        {
            get
            {
                return 1022456;
            }
        }// mug
        public override int BaseLabelNumber
        {
            get
            {
                return 1042976;
            }
        }// a mug of Ale
        public override int MaxQuantity
        {
            get
            {
                return 5;
            }
        }

        public override int ComputeItemID()
        {
            if (this.IsEmpty)
                return (this.ItemID >= 0x1F81 && this.ItemID <= 0x1F84 ? this.ItemID : 0x1F81);

            switch( this.Content )
            {
                case BeverageType.Ale:
                    return (this.ItemID == 0x9EF ? 0x9EF : 0x9EE);
                case BeverageType.Cider:
                    return (this.ItemID >= 0x1F7D && this.ItemID <= 0x1F80 ? this.ItemID : 0x1F7D);
                case BeverageType.Liquor:
                    return (this.ItemID >= 0x1F85 && this.ItemID <= 0x1F88 ? this.ItemID : 0x1F85);
                case BeverageType.Milk:
                    return (this.ItemID >= 0x1F89 && this.ItemID <= 0x1F8C ? this.ItemID : 0x1F89);
                case BeverageType.Wine:
                    return (this.ItemID >= 0x1F8D && this.ItemID <= 0x1F90 ? this.ItemID : 0x1F8D);
                case BeverageType.Water:
                    return (this.ItemID >= 0x1F91 && this.ItemID <= 0x1F94 ? this.ItemID : 0x1F91);
            }

            return 0;
        }

        [Constructable]
        public GlassMug()
        {
            this.Weight = 1.0;
        }

        [Constructable]
        public GlassMug(BeverageType type)
            : base(type)
        {
            this.Weight = 1.0;
        }

        public GlassMug(Serial serial)
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

            switch( version )
            {
                case 0:
                    {
                        if (this.CheckType("MugAle"))
                        {
                            this.Quantity = this.MaxQuantity;
                            this.Content = BeverageType.Ale;
                        }
                        else if (this.CheckType("GlassCider"))
                        {
                            this.Quantity = this.MaxQuantity;
                            this.Content = BeverageType.Cider;
                        }
                        else if (this.CheckType("GlassLiquor"))
                        {
                            this.Quantity = this.MaxQuantity;
                            this.Content = BeverageType.Liquor;
                        }
                        else if (this.CheckType("GlassMilk"))
                        {
                            this.Quantity = this.MaxQuantity;
                            this.Content = BeverageType.Milk;
                        }
                        else if (this.CheckType("GlassWine"))
                        {
                            this.Quantity = this.MaxQuantity;
                            this.Content = BeverageType.Wine;
                        }
                        else if (this.CheckType("GlassWater"))
                        {
                            this.Quantity = this.MaxQuantity;
                            this.Content = BeverageType.Water;
                        }
                        else
                        {
                            throw new Exception(World.LoadingType);
                        }

                        break;
                    }
            }
        }
    }

    [TypeAlias("Server.Items.PitcherAle", "Server.Items.PitcherCider", "Server.Items.PitcherLiquor",
        "Server.Items.PitcherMilk", "Server.Items.PitcherWine", "Server.Items.PitcherWater",
        "Server.Items.GlassPitcher")]
    public class Pitcher : BaseBeverage
    {
        public override int BaseLabelNumber
        {
            get
            {
                return 1048128;
            }
        }// a Pitcher of Ale
        public override int MaxQuantity
        {
            get
            {
                return 5;
            }
        }

        public override int ComputeItemID()
        {
            if (this.IsEmpty)
            {
                if (this.ItemID == 0x9A7 || this.ItemID == 0xFF7)
                    return this.ItemID;

                return 0xFF6;
            }

            switch( this.Content )
            {
                case BeverageType.Ale:
                    {
                        if (this.ItemID == 0x1F96)
                            return this.ItemID;

                        return 0x1F95;
                    }
                case BeverageType.Cider:
                    {
                        if (this.ItemID == 0x1F98)
                            return this.ItemID;

                        return 0x1F97;
                    }
                case BeverageType.Liquor:
                    {
                        if (this.ItemID == 0x1F9A)
                            return this.ItemID;

                        return 0x1F99;
                    }
                case BeverageType.Milk:
                    {
                        if (this.ItemID == 0x9AD)
                            return this.ItemID;

                        return 0x9F0;
                    }
                case BeverageType.Wine:
                    {
                        if (this.ItemID == 0x1F9C)
                            return this.ItemID;

                        return 0x1F9B;
                    }
                case BeverageType.Water:
                    {
                        if (this.ItemID == 0xFF8 || this.ItemID == 0xFF9 || this.ItemID == 0x1F9E)
                            return this.ItemID;

                        return 0x1F9D;
                    }
            }

            return 0;
        }

        [Constructable]
        public Pitcher()
        {
            this.Weight = 2.0;
        }

        [Constructable]
        public Pitcher(BeverageType type)
            : base(type)
        {
            this.Weight = 2.0;
        }

        public Pitcher(Serial serial)
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
            if (this.CheckType("PitcherWater") || this.CheckType("GlassPitcher"))
                base.InternalDeserialize(reader, false);
            else
                base.InternalDeserialize(reader, true);

            int version = reader.ReadInt();

            switch( version )
            {
                case 0:
                    {
                        if (this.CheckType("PitcherAle"))
                        {
                            this.Quantity = this.MaxQuantity;
                            this.Content = BeverageType.Ale;
                        }
                        else if (this.CheckType("PitcherCider"))
                        {
                            this.Quantity = this.MaxQuantity;
                            this.Content = BeverageType.Cider;
                        }
                        else if (this.CheckType("PitcherLiquor"))
                        {
                            this.Quantity = this.MaxQuantity;
                            this.Content = BeverageType.Liquor;
                        }
                        else if (this.CheckType("PitcherMilk"))
                        {
                            this.Quantity = this.MaxQuantity;
                            this.Content = BeverageType.Milk;
                        }
                        else if (this.CheckType("PitcherWine"))
                        {
                            this.Quantity = this.MaxQuantity;
                            this.Content = BeverageType.Wine;
                        }
                        else if (this.CheckType("PitcherWater"))
                        {
                            this.Quantity = this.MaxQuantity;
                            this.Content = BeverageType.Water;
                        }
                        else if (this.CheckType("GlassPitcher"))
                        {
                            this.Quantity = 0;
                            this.Content = BeverageType.Water;
                        }
                        else
                        {
                            throw new Exception(World.LoadingType);
                        }

                        break;
                    }
            }
        }
    }
}