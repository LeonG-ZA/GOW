using System;

namespace Server.Items
{
    public class VampiricEssence : Cutlass
    {
        public override int LabelNumber { get { return 1113873; } } // Vampiric Essence

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public VampiricEssence()
        {
            Hue = 0x14C;
            WeaponAttributes.HitLeechHits = 100;
            WeaponAttributes.HitHarm = 50;
            Attributes.WeaponSpeed = 20;
            Attributes.WeaponDamage = 50;
            AosElementDamages.Cold = 100;
            AbsorptionAttributes.BloodDrinker = 1;
        }

        public VampiricEssence(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (this.Attributes.AttackChance == 50)
                this.Attributes.AttackChance = 10;
        }
    }
}