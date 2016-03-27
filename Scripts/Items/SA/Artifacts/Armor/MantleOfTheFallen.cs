using System;

namespace Server.Items
{
    public class MantleOfTheFallen : GargishClothChest
    {
        public override int LabelNumber { get { return 1113819; } } // Mantle of the Fallen

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public MantleOfTheFallen() 
        {
            Hue = 1192;

            Attributes.BonusInt = 8;
            Attributes.BonusMana = 8;
            Attributes.RegenMana = 1;
            Attributes.SpellDamage = 5;
            Attributes.LowerRegCost = 25;
            Resistances.Fire = 1;
            Resistances.Cold = 5;
            Resistances.Poison = 6;
            Resistances.Energy = 2;
            AbsorptionAttributes.CastingFocus = 3;
        }

        public MantleOfTheFallen(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}