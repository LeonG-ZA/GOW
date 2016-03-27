using System;

namespace Server.Items
{
    public class Lavaliere : GoldNecklace
    {
        public override int LabelNumber { get { return 1114843; } } // Lavaliere

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public Lavaliere()
        {
            Hue = 1194;
			
            AbsorptionAttributes.EaterKinetic = 20;
            Attributes.DefendChance = 10;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 20;
            Resistances.Physical = 15;
        }

        public Lavaliere(Serial serial)
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