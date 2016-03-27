using System;

namespace Server.Items
{
    public class JadeWarAxe : WarAxe
    {
        public override int LabelNumber { get { return 1115445; } } // Jade War Axe

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public JadeWarAxe()
        {
            Hue = 0x48A;

            AbsorptionAttributes.EaterFire = 10;
            WeaponAttributes.HitFireball = 30;
            WeaponAttributes.HitLowerDefend = 60;
            Attributes.WeaponSpeed = 20;
            Attributes.WeaponDamage = 50;
            this.Slayer = SlayerName.ReptilianDeath;
        }

        public JadeWarAxe(Serial serial)
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