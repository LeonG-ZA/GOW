using System;

namespace Server.Items
{
    public class RawRibs : BaseCookableFood
    {
        [Constructable]
        public RawRibs()
            : this(1)
        {
        }

        [Constructable]
        public RawRibs(int amount)
            : base(0x9F1, 10)
        {
            this.Weight = 1.0;
            this.Stackable = true;
            this.Amount = amount;
        }

        public RawRibs(Serial serial)
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

        public override BaseFood Cook()
        {
            return new Ribs();
        }
    }

    // ********** RawLambLeg **********
    public class RawLambLeg : BaseCookableFood
    {
        [Constructable]
        public RawLambLeg()
            : this(1)
        {
        }

        [Constructable]
        public RawLambLeg(int amount)
            : base(0x1609, 10)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public RawLambLeg(Serial serial)
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

            if (version == 0 && this.Weight == 1)
                this.Weight = -1;
        }

        public override BaseFood Cook()
        {
            return new LambLeg();
        }
    }

    // ********** RawChickenLeg **********
    public class RawChickenLeg : BaseCookableFood
    {
        [Constructable]
        public RawChickenLeg()
            : base(0x1607, 10)
        {
            this.Weight = 1.0;
            this.Stackable = true;
        }

        public RawChickenLeg(Serial serial)
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

        public override BaseFood Cook()
        {
            return new ChickenLeg();
        }
    }

    // ********** RawBird **********
    public class RawBird : BaseCookableFood
    {
        [Constructable]
        public RawBird()
            : this(1)
        {
        }

        [Constructable]
        public RawBird(int amount)
            : base(0x9B9, 10)
        {
            this.Weight = 1.0;
            this.Stackable = true;
            this.Amount = amount;
        }

        public RawBird(Serial serial)
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

        public override BaseFood Cook()
        {
            return new CookedBird();
        }
    }

    // ********** UnbakedPeachCobbler **********
    public class UnbakedPeachCobbler : BaseCookableFood
    {
        public override int LabelNumber
        {
            get
            {
                return 1041335;
            }
        }// unbaked peach cobbler

        [Constructable]
        public UnbakedPeachCobbler()
            : base(0x1042, 25)
        {
            this.Weight = 1.0;
        }

        public UnbakedPeachCobbler(Serial serial)
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

        public override BaseFood Cook()
        {
            return new PeachCobbler();
        }
    }

    // ********** UnbakedFruitPie **********
    public class UnbakedFruitPie : BaseCookableFood
    {
        public override int LabelNumber
        {
            get
            {
                return 1041334;
            }
        }// unbaked fruit pie

        [Constructable]
        public UnbakedFruitPie()
            : base(0x1042, 25)
        {
            this.Weight = 1.0;
        }

        public UnbakedFruitPie(Serial serial)
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

        public override BaseFood Cook()
        {
            return new FruitPie();
        }
    }

    // ********** UnbakedMeatPie **********
    public class UnbakedMeatPie : BaseCookableFood
    {
        public override int LabelNumber
        {
            get
            {
                return 1041338;
            }
        }// unbaked meat pie

        [Constructable]
        public UnbakedMeatPie()
            : base(0x1042, 25)
        {
            this.Weight = 1.0;
        }

        public UnbakedMeatPie(Serial serial)
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

        public override BaseFood Cook()
        {
            return new MeatPie();
        }
    }

    // ********** UnbakedPumpkinPie **********
    public class UnbakedPumpkinPie : BaseCookableFood
    {
        public override int LabelNumber
        {
            get
            {
                return 1041342;
            }
        }// unbaked pumpkin pie

        [Constructable]
        public UnbakedPumpkinPie()
            : base(0x1042, 25)
        {
            this.Weight = 1.0;
        }

        public UnbakedPumpkinPie(Serial serial)
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

        public override BaseFood Cook()
        {
            return new PumpkinPie();
        }
    }

    // ********** UnbakedApplePie **********
    public class UnbakedApplePie : BaseCookableFood
    {
        public override int LabelNumber
        {
            get
            {
                return 1041336;
            }
        }// unbaked apple pie

        [Constructable]
        public UnbakedApplePie()
            : base(0x1042, 25)
        {
            this.Weight = 1.0;
        }

        public UnbakedApplePie(Serial serial)
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

        public override BaseFood Cook()
        {
            return new ApplePie();
        }
    }

    // ********** UncookedCheesePizza **********
    [TypeAlias("Server.Items.UncookedPizza")]
    public class UncookedCheesePizza : BaseCookableFood
    {
        public override int LabelNumber
        {
            get
            {
                return 1041341;
            }
        }// uncooked cheese pizza

        [Constructable]
        public UncookedCheesePizza()
            : base(0x1083, 20)
        {
            this.Weight = 1.0;
        }

        public UncookedCheesePizza(Serial serial)
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

            if (this.ItemID == 0x1040)
                this.ItemID = 0x1083;

            if (this.Hue == 51)
                this.Hue = 0;
        }

        public override BaseFood Cook()
        {
            return new CheesePizza();
        }
    }

    // ********** UncookedSausagePizza **********
    public class UncookedSausagePizza : BaseCookableFood
    {
        public override int LabelNumber
        {
            get
            {
                return 1041337;
            }
        }// uncooked sausage pizza

        [Constructable]
        public UncookedSausagePizza()
            : base(0x1083, 20)
        {
            this.Weight = 1.0;
        }

        public UncookedSausagePizza(Serial serial)
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

        public override BaseFood Cook()
        {
            return new SausagePizza();
        }
    }

    #if false
	// ********** UncookedPizza **********
	public class UncookedPizza : CookableFood
	{
		[Constructable]
		public UncookedPizza() : base( 0x1083, 20 )
		{
			Weight = 1.0;
		}

		public UncookedPizza( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( ItemID == 0x1040 )
				ItemID = 0x1083;

			if ( Hue == 51 )
				Hue = 0;
		}

		public override BaseFood Cook()
		{
			return new Pizza();
		}
	}
    #endif

    // ********** UnbakedQuiche **********
    public class UnbakedQuiche : BaseCookableFood
    {
        public override int LabelNumber
        {
            get
            {
                return 1041339;
            }
        }// unbaked quiche

        [Constructable]
        public UnbakedQuiche()
            : base(0x1042, 25)
        {
            this.Weight = 1.0;
        }

        public UnbakedQuiche(Serial serial)
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

        public override BaseFood Cook()
        {
            return new Quiche();
        }
    }

    // ********** Eggs **********
    public class Eggs : BaseCookableFood
    {
        [Constructable]
        public Eggs()
            : this(1)
        {
        }

        [Constructable]
        public Eggs(int amount)
            : base(0x9B5, 15)
        {
            this.Weight = 1.0;
            this.Stackable = true;
            this.Amount = amount;
        }

        public Eggs(Serial serial)
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

            if (version < 1)
            {
                this.Stackable = true;

                if (this.Weight == 0.5)
                    this.Weight = 1.0;
            }
        }

        public override BaseFood Cook()
        {
            return new FriedEggs();
        }
    }

    // ********** BrightlyColoredEggs **********
    public class BrightlyColoredEggs : BaseCookableFood
    {
        public override string DefaultName
        {
            get
            {
                return "brightly colored eggs";
            }
        }

        [Constructable]
        public BrightlyColoredEggs()
            : base(0x9B5, 15)
        {
            this.Weight = 0.5;
            this.Hue = 3 + (Utility.Random(20) * 5);
        }

        public BrightlyColoredEggs(Serial serial)
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

        public override BaseFood Cook()
        {
            return new FriedEggs();
        }
    }

    // ********** EasterEggs **********
    public class EasterEggs : BaseCookableFood
    {
        public override int LabelNumber
        {
            get
            {
                return 1016105;
            }
        }// Easter Eggs

        [Constructable]
        public EasterEggs()
            : base(0x9B5, 15)
        {
            this.Weight = 0.5;
            this.Hue = 3 + (Utility.Random(20) * 5);
        }

        public EasterEggs(Serial serial)
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

        public override BaseFood Cook()
        {
            return new FriedEggs();
        }
    }

    // ********** CookieMix **********
    public class CookieMix : BaseCookableFood
    {
        [Constructable]
        public CookieMix()
            : base(0x103F, 20)
        {
            this.Weight = 1.0;
        }

        public CookieMix(Serial serial)
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

        public override BaseFood Cook()
        {
            return new Cookies();
        }
    }

    // ********** CakeMix **********
    public class CakeMix : BaseCookableFood
    {
        public override int LabelNumber
        {
            get
            {
                return 1041002;
            }
        }// cake mix

        [Constructable]
        public CakeMix()
            : base(0x103F, 40)
        {
            this.Weight = 1.0;
        }

        public CakeMix(Serial serial)
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

        public override BaseFood Cook()
        {
            return new Cake();
        }
    }

    public class RawFishSteak : BaseCookableFood
    {
        public override double DefaultWeight
        {
            get
            {
                return 0.1;
            }
        }

        [Constructable]
        public RawFishSteak()
            : this(1)
        {
        }

        [Constructable]
        public RawFishSteak(int amount)
            : base(0x097A, 10)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public RawFishSteak(Serial serial)
            : base(serial)
        {
        }

        public override BaseFood Cook()
        {
            return new FishSteak();
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

    public class RawRotwormMeat : BaseCookableFood
    {
        [Constructable]
        public RawRotwormMeat()
            : this(1)
        {
        }

        [Constructable]
        public RawRotwormMeat(int amount)
            : base(0x2DB9, 10)
        {
            Stackable = true;
            Weight = 0.1;
            Amount = amount;
        }

        public RawRotwormMeat(Serial serial)
            : base(serial)
        {
        }

        public override BaseFood Cook()
        {
            return null;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            /*int version = */
            reader.ReadInt();
        }
    }
}