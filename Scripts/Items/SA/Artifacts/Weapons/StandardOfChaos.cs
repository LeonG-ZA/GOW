using System;

namespace Server.Items
{
    public class StandardOfChaos : DoubleBladedStaff 
    {
        public override int LabelNumber { get { return 1113522; } } // Standard of Chaos

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public StandardOfChaos()
        {
            Hue = 0x48C;

            WeaponAttributes.HitLightning = 10;
            WeaponAttributes.HitHarm = 30;
            WeaponAttributes.HitLowerDefend = 40;
            WeaponAttributes.HitFireball = 20;
            Attributes.CastSpeed = 1;
            Attributes.WeaponSpeed = 30;
            Attributes.WeaponDamage = -40;
            AosElementDamages.Chaos = 100;		
        }

        public StandardOfChaos(Serial serial)
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