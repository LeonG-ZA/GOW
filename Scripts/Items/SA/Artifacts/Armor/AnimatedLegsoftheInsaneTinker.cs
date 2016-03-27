using System;

namespace Server.Items
{
    public class AnimatedLegsoftheInsaneTinker : PlateLegs
    {
        public override int LabelNumber { get { return 1113760; } } // Animated Legs of the Insane Tinker

        public override int BasePhysicalResistance { get { return 12; } }
        public override int BaseFireResistance { get { return 12; } }
        public override int BaseColdResistance { get { return 5; } }
        public override int BasePoisonResistance { get { return 12; } }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public AnimatedLegsoftheInsaneTinker()
            : base()
        {
            Hue = 0x420;

            Attributes.BonusDex = 5;
            Attributes.RegenStam = 2;
            Attributes.WeaponSpeed = 10;
            Attributes.WeaponDamage = 10;
            ArmorAttributes.LowerStatReq = 50;
        }

        public AnimatedLegsoftheInsaneTinker(Serial serial)
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
            writer.Write((int)0); //version
        }
    }
}