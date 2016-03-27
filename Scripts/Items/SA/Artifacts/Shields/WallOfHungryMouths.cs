using System;

namespace Server.Items
{
    public class WallofHungryMouths : HeaterShield
    {
        public override int LabelNumber { get { return 1113722; } } // Wall of Hungry Mouths

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }
        public override int BasePhysicalResistance { get { return 5; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public WallofHungryMouths()
        {
            Hue = 1034;

            AbsorptionAttributes.EaterEnergy = 20;
            AbsorptionAttributes.EaterPoison = 20;
            AbsorptionAttributes.EaterCold = 20;
            AbsorptionAttributes.EaterFire = 20;
        }

        public WallofHungryMouths(Serial serial)
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