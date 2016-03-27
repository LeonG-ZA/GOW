using System;

namespace Server.Items
{
    public class BraceletOfProtection : GoldBracelet
    {
        public override int LabelNumber { get { return 1152730; } } // Bracelet of Protection
        [Constructable]
        public BraceletOfProtection()
        {
			Hue = 437;
			AbsorptionAttributes.EaterDamage = 15;
			Attributes.BonusHits = 5; 
			Attributes.RegenHits = 10;			
            Attributes.DefendChance = 5;
        }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public BraceletOfProtection(Serial serial)
            : base(serial)
        {
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}