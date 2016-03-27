using System;

namespace Server.Items
{
    public class DefenderOfTheMagus : MetalShield
    {
        public override int LabelNumber { get { return 1113851; } } // Defender of the Magus

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public DefenderOfTheMagus()
        {
            Hue = 0x495;

            Attributes.SpellChanneling = 1;
            Attributes.CastSpeed = 1;
            Attributes.DefendChance = 10;
            Attributes.CastRecovery = 1;

            switch (Utility.Random(5))
            {
                case 0:
                    AbsorptionAttributes.ResonanceKinetic = 10;
                    PhysicalBonus = 10;
                    break;
                case 1:
                    AbsorptionAttributes.ResonanceFire = 10;
                    FireBonus = 10;
                    break;
                case 2:
                    AbsorptionAttributes.ResonanceCold = 10;
                    ColdBonus = 10;
                    break;
                case 3:
                    AbsorptionAttributes.ResonancePoison = 10;
                    PoisonBonus = 10;
                    break;
                case 4:
                    AbsorptionAttributes.ResonanceEnergy = 10;
                    EnergyBonus = 10;
                    break;
            }
        }

        public DefenderOfTheMagus(Serial serial)
            : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);//version
        }
    }
}