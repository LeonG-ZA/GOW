using System;

namespace Server.Items
{
    public class SwordOfShatteredHopes : GlassSword
    {
        public override int LabelNumber { get { return 1112770; } } // Sword of Shattered Hopes

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override int ArtifactRarity { get { return 10; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public SwordOfShatteredHopes()
        {
            AbsorptionAttributes.SplinteringWeapon = 20;
            WeaponAttributes.HitDispel = 25;
            Attributes.WeaponSpeed = 30;
            Attributes.WeaponDamage = 50;
        }

        public SwordOfShatteredHopes(Serial serial)
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