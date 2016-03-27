using System;

namespace Server.Items
{
    public class NightEyes : ElvenGlasses
    {
        public override int LabelNumber { get { return 1114785; } } // Night Eyes

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }
        public override int BasePhysicalResistance { get { return 8; } }
        public override int BaseFireResistance { get { return 6; } }
        public override int BaseColdResistance { get { return 7; } }
        public override int BasePoisonResistance { get { return 7; } }
        public override int BaseEnergyResistance { get { return 7; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public NightEyes()
        {
            Hue = 1233;

            Attributes.NightSight = 1;
            Attributes.DefendChance = 10;
            Attributes.CastRecovery = 3;	
        }

        public NightEyes(Serial serial)
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