using System;

namespace Server.Items
{
    public class ProtectoroftheBattleMage : LeatherChest
    {
        public override int LabelNumber { get { return 1113761; } } // Protector of the Battle Mage

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override int BasePhysicalResistance { get { return 8; } }
        public override int BaseFireResistance { get { return 12; } }
        public override int BaseColdResistance { get { return 7; } }
        public override int BasePoisonResistance { get { return 5; } }
        public override int BaseEnergyResistance { get { return 5; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public ProtectoroftheBattleMage()
        {
            Hue = 532;

            Attributes.RegenMana = 2;
            Attributes.SpellDamage = 5;
            Attributes.LowerManaCost = 8;
            Attributes.LowerRegCost = 10;
            AbsorptionAttributes.CastingFocus = 3;
        }

        public ProtectoroftheBattleMage(Serial serial)
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