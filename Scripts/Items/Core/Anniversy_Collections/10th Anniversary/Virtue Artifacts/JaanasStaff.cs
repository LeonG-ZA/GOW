using System;

namespace Server.Items
{
    public class JaanasStaff : GnarledStaff
    {
        [Constructable]
        public JaanasStaff()
            : base()
        {
            Hue = 0x58C;
            WeaponAttributes.MageWeapon = 20;
            Attributes.SpellChanneling = 1;
            Attributes.Luck = 220;
            Attributes.DefendChance = 15;
        }

        public JaanasStaff(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1079790;
            }
        }// Jaana's Staff
        public override int ArtifactRarity
        {
            get
            {
                return 11;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 120;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 120;
            }
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