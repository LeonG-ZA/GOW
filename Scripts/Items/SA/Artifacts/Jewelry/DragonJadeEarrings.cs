using System;

namespace Server.Items
{
    [TypeAlias("Server.Items.DragonJadeEarringsArmor")]
    public class DragonJadeEarrings : GargishEarrings
    {
        public override int LabelNumber { get { return 1113720; } } // Dragon Jade Earrings

        public override int BasePhysicalResistance { get { return 8; } }
        public override int BaseFireResistance { get { return 14; } }
        public override int BaseColdResistance { get { return 2; } }
        public override int BasePoisonResistance { get { return 11; } }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public DragonJadeEarrings()
        {
            Hue = 683;

            Attributes.BonusStr = 5;
            Attributes.BonusDex = 5;
            Attributes.RegenHits = 2;
            Attributes.RegenStam = 3;
            Attributes.LowerManaCost = 5;
            AbsorptionAttributes.EaterFire = 10;
        }

        public DragonJadeEarrings(Serial serial)
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