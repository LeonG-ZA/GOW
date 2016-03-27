using System;

namespace Server.Items
{
    public abstract class ShipCannonball : BaseShipProjectile
    {
        public ShipCannonball()
            : this(1)
        {
        }

        public ShipCannonball(int amount)
            : base(amount, 0xE74)
        {
        }

        public ShipCannonball(Serial serial)
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

    public class LightShipCannonball : ShipCannonball
    {
        [Constructable]
        public LightShipCannonball()
            : this(1)
        {
        }

        [Constructable]
        public LightShipCannonball(int amount)
            : base(amount)
        {
            this.Range = 17;
            this.Area = 0;
            this.AccuracyBonus = 0;
            this.PhysicalDamage = 1600;
            this.FireDamage = 0;
            this.FiringSpeed = 35;
        }

        public LightShipCannonball(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116266;
            }
        }// light cannonball

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

    public class HeavyShipCannonball : ShipCannonball
    {
        [Constructable]
        public HeavyShipCannonball()
            : this(1)
        {
        }

        [Constructable]
        public HeavyShipCannonball(int amount)
            : base(amount)
        {
            this.Range = 15;
            this.Area = 0;
            this.AccuracyBonus = 0;
            this.PhysicalDamage = 4500;
            this.FireDamage = 0;
            this.FiringSpeed = 25;
        }

        public HeavyShipCannonball(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116267;
            }
        }// heavy cannonball

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

    public class ExplodingShipCannonball : ShipCannonball
    {
        [Constructable]
        public ExplodingShipCannonball()
            : this(1)
        {
        }

        [Constructable]
        public ExplodingShipCannonball(int amount)
            : base(amount)
        {
            this.Range = 11;
            this.Area = 1;
            this.AccuracyBonus = -10;
            this.PhysicalDamage = 300;
            this.FireDamage = 1250;
            this.FiringSpeed = 20;
            this.Hue = 46;
            this.Name = "Exploding Ship Cannonball";
        }

        public ExplodingShipCannonball(Serial serial)
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

    public class FieryShipCannonball : ShipCannonball
    {
        [Constructable]
        public FieryShipCannonball()
            : this(1)
        {
        }

        [Constructable]
        public FieryShipCannonball(int amount)
            : base(amount)
        {
            this.Range = 8;
            this.Area = 2;
            this.AccuracyBonus = -20;
            this.PhysicalDamage = 0;
            this.FireDamage = 2500;
            this.FiringSpeed = 10;
            this.Hue = 33;
        }

        public FieryShipCannonball(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116759;
            }
        }// light flame cannonball

        // use a fireball animation when fired
        public override int AnimationID
        {
            get
            {
                return 0x36D4;
            }
        }
        public override int AnimationHue
        {
            get
            {
                return 0;
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

    public class LightShipGrapeShot : ShipCannonball
    {
        [Constructable]
        public LightShipGrapeShot()
            : this(1)
        {
        }

        [Constructable]
        public LightShipGrapeShot(int amount)
            : base(amount)
        {
            this.Range = 17;
            this.Area = 6;
            this.AccuracyBonus = 0;
            this.PhysicalDamage = 1800;
            this.FireDamage = 0;
            this.FiringSpeed = 35;
        }

        public LightShipGrapeShot(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116030;
            }
        }// light grapeshot

        // only does damage to mobiles
        public override double StructureDamageMultiplier
        {
            get
            {
                return 0.0;
            }
        }//  damage multiplier for structures
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

    public class HeavyShipGrapeShot : ShipCannonball
    {
        [Constructable]
        public HeavyShipGrapeShot()
            : this(1)
        {
        }

        [Constructable]
        public HeavyShipGrapeShot(int amount)
            : base(amount)
        {
            this.Range = 17;
            this.Area = 6;
            this.AccuracyBonus = 0;
            this.PhysicalDamage = 3600;
            this.FireDamage = 0;
            this.FiringSpeed = 25;
        }

        public HeavyShipGrapeShot(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116166;
            }
        }// heavy grapeshot

        // only does damage to mobiles
        public override double StructureDamageMultiplier
        {
            get
            {
                return 0.0;
            }
        }//  damage multiplier for structures
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