using System;

namespace Server.Items
{
    public class Boomerang : BaseThrown
    {
        [Constructable]
        public Boomerang()
            : base(0x8FF)
        {
            Weight = 6.0;
            Layer = Layer.OneHanded;
        }

        public Boomerang(Serial serial)
            : base(serial)
        {
        }

        public override int DefMaxRange { get { return 7; } }

        public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.MysticArc;
            }
        }
        public override WeaponAbility SecondaryAbility
        {
            get
            {
                return WeaponAbility.ConcussionBlow;
            }
        }

        public override int AosStrengthReq
        {
            get
            {
                return 25;
            }
        }
        public override int AosMinDamage
        {
            get
            {
                return 11;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 15;
            }
        }
        public override float MlSpeed
        {
            get
            {
                return 2.75f;
            }
        }

        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 70; } }

        public override Race RequiredRace { get { return Race.Gargoyle; } }
        public override bool CanBeWornByGargoyles { get { return true; } }

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