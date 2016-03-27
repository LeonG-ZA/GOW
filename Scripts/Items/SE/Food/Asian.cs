using System;

namespace Server.Items
{
    public class Wasabi : Item
    {
        [Constructable]
        public Wasabi()
            : base(0x24E8)
        {
            this.Weight = 1.0;
        }

        public Wasabi(Serial serial)
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

    public class WasabiClumps : BaseFood
    {
        [Constructable]
        public WasabiClumps()
            : base(0x24EB)
        {
            this.Stackable = false;
            this.Weight = 1.0;
            this.FillFactor = 2;
        }

        public WasabiClumps(Serial serial)
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

    public class EmptyBentoBox : Item
    {
        [Constructable]
        public EmptyBentoBox()
            : base(0x2834)
        {
            this.Weight = 5.0;
        }

        public EmptyBentoBox(Serial serial)
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

    public class BentoBox : BaseFood
    {
        [Constructable]
        public BentoBox()
            : base(0x2836)
        {
            this.Stackable = false;
            this.Weight = 5.0;
            this.FillFactor = 2;
        }

        public BentoBox(Serial serial)
            : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (!base.Eat(from))
                return false;

            from.AddToBackpack(new EmptyBentoBox());
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

    public class SushiRolls : BaseFood
    {
        [Constructable]
        public SushiRolls()
            : base(0x283E)
        {
            this.Stackable = false;
            this.Weight = 3.0;
            this.FillFactor = 2;
        }

        public SushiRolls(Serial serial)
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

    public class SushiPlatter : BaseFood
    {
        [Constructable]
        public SushiPlatter()
            : base(0x2840)
        {
            this.Stackable = Core.ML;
            this.Weight = 3.0;
            this.FillFactor = 2;
        }

        public SushiPlatter(Serial serial)
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

    public class GreenTeaBasket : Item
    {
        [Constructable]
        public GreenTeaBasket()
            : base(0x284B)
        {
            this.Weight = 10.0;
        }

        public GreenTeaBasket(Serial serial)
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

    public class GreenTea : BaseFood
    {
        [Constructable]
        public GreenTea()
            : base(0x284C)
        {
            this.Stackable = false;
            this.Weight = 4.0;
            this.FillFactor = 2;
        }

        public GreenTea(Serial serial)
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

    public class MisoSoup : BaseFood
    {
        [Constructable]
        public MisoSoup()
            : base(0x284D)
        {
            this.Stackable = false;
            this.Weight = 4.0;
            this.FillFactor = 2;
        }

        public MisoSoup(Serial serial)
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

    public class WhiteMisoSoup : BaseFood
    {
        [Constructable]
        public WhiteMisoSoup()
            : base(0x284E)
        {
            this.Stackable = false;
            this.Weight = 4.0;
            this.FillFactor = 2;
        }

        public WhiteMisoSoup(Serial serial)
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

    public class RedMisoSoup : BaseFood
    {
        [Constructable]
        public RedMisoSoup()
            : base(0x284F)
        {
            this.Stackable = false;
            this.Weight = 4.0;
            this.FillFactor = 2;
        }

        public RedMisoSoup(Serial serial)
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

    public class AwaseMisoSoup : BaseFood
    {
        [Constructable]
        public AwaseMisoSoup()
            : base(0x2850)
        {
            this.Stackable = false;
            this.Weight = 4.0;
            this.FillFactor = 2;
        }

        public AwaseMisoSoup(Serial serial)
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