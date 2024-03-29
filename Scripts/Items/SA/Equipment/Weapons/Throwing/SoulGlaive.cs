using System;

namespace Server.Items
{
    public class SoulGlaive : BaseThrown
    {
        [Constructable]
        public SoulGlaive()
            : base(0x090A)
        {
            Weight = 6.0;
            Layer = Layer.OneHanded;
        }

        public SoulGlaive(Serial serial)
            : base(serial)
        {
        }

        public override int DefMaxRange { get { return 11; } }

        public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.ArmorIgnore;
            }
        }
        public override WeaponAbility SecondaryAbility
        {
            get
            {
                return WeaponAbility.MortalStrike;
            }
        }
        public override int AosStrengthReq
        {
            get
            {
                return 60;
            }
        }
        public override int AosMinDamage
        {
            get
            {
                return 18;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 22;
            }
        }
        public override float MlSpeed
        {
            get
            {
                return 4.00f;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 31;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 60;
            }
        }
        
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