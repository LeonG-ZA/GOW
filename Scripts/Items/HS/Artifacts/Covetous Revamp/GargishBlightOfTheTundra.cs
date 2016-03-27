using System;

namespace Server.Items
{
    public class GargishBlightOfTheTundra : SoulGlaive
    {
        [Constructable]
        public GargishBlightOfTheTundra()
            : base()
        {
            Hue = 0x48D;
            Slayer2 = (SlayerName)Utility.RandomMinMax(1, 27);
            Attributes.RegenStam = 10;
            Attributes.AttackChance = 15;
            Attributes.WeaponSpeed = 45;
            Attributes.WeaponDamage = 50;
            WeaponAttributes.ResistColdBonus = 15;
        }

        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            phys = fire = nrgy = pois = chaos = direct = 0;
            cold = 100;
        }

        public GargishBlightOfTheTundra(Serial serial)
            : base(serial)
        {
        }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }


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