using System;

namespace Server.Items
{
    public class DragonHideShield : GargishKiteShield
    {
        public override int LabelNumber { get { return 1113532; } } // Dragon Hide Shield

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }
        public override int BaseFireResistance { get { return 15; } }
        public override int BaseEnergyResistance { get { return -5; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public DragonHideShield()
        {
            Hue = 1359;

            AbsorptionAttributes.EaterFire = 20;
            Attributes.RegenHits = 2;
            Attributes.DefendChance = 10;
        }

        public DragonHideShield(Serial serial)
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