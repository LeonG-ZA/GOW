using System;

namespace Server.Items
{
    public class BansheesCall : Cyclone
    {
        public override int LabelNumber { get { return 1113529; } } // Banshee's Call

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override bool CanImbue { get { return false; } }

        [Constructable]
        public BansheesCall() 
        {
            Hue = 1266;
            WeaponAttributes.HitHarm = 40;
            Attributes.BonusStr = 5;
            WeaponAttributes.HitLeechHits = 45;
            Attributes.WeaponSpeed = 30;
            Attributes.WeaponDamage = 50;
            WeaponAttributes.Velocity = 35;		
            AosElementDamages.Cold = 100;
        }

        public BansheesCall(Serial serial)
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