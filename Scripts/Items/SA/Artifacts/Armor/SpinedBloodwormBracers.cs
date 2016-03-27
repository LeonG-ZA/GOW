using System;

namespace Server.Items
{
    public class SpinedBloodwormBracers : GargishClothArms
    {
        public override int LabelNumber { get { return 1113865; } } // Spined Bloodworm Bracers

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public SpinedBloodwormBracers()
        {
            Hue = 438;

            Attributes.RegenHits = 2;
            Attributes.RegenHits = 2;
            Attributes.ReflectPhysical = 30;
            Attributes.WeaponDamage = 10;
            Resistances.Physical = 6;
            Resistances.Fire = 3;
            Resistances.Cold = 9;
            Resistances.Poison = 9;
            Resistances.Energy = 4;
            AbsorptionAttributes.EaterKinetic = 10;
        }

        public SpinedBloodwormBracers(Serial serial)
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
        }
    }
}