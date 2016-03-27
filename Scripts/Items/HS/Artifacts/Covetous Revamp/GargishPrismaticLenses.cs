using System;

namespace Server.Items
{
    [Flipable(0x4644, 0x4645)]
    public class GargishPrismaticLenses : GargishGlasses
    {
        [Constructable]
        public GargishPrismaticLenses()
            : base()
        {
			Hue = 0x455;
			HitLowerDefend = 30;	
			Attributes.RegenHits = 2; 
			Attributes.RegenStam = 3; 
			Attributes.WeaponDamage = 25;
        }

        public GargishPrismaticLenses(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1152716;
            }
        }// Gargish Glasses
        public override int BasePhysicalResistance
        {
            get
            {
                return 18;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 4;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 7;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 17;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 6;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 255;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 255;
            }
        }
        public override int AosStrReq
        {
            get
            {
                return 45;
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
}