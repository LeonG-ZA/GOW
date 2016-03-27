using System;

namespace Server.Items
{
    public class StormCaller : Boomerang
    {
        public override int LabelNumber { get { return 1113530; } } // Storm Caller

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public StormCaller()
        {
            Hue = 1159;

            WeaponAttributes.HitLightning = 40;
            WeaponAttributes.HitLowerDefend = 30;
            AbsorptionAttributes.BattleLust = 1;
            Attributes.BonusStr = 5;
            Attributes.WeaponSpeed = 30;
            Attributes.WeaponDamage = 40;
        }

        public StormCaller(Serial serial)
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