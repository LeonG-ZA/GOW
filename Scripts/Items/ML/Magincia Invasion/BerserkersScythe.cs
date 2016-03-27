using System;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Engines.Harvest;
using Server.Mobiles;

namespace Server.Items
{
    [FlipableAttribute(0x26C4, 0x26BA)]
    public class BerserkersScythe : Scythe
    {
        public override int LabelNumber { get { return 1079837; } } // Berserkers Scythe

        public override int ArtifactRarity { get { return 12; } }
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public BerserkersScythe()
        {
            Hue = 1157;
            Weight = 5.0;
            Slayer = SlayerName.Exorcism;
            WeaponAttributes.HitLeechHits = 35;
            Attributes.WeaponDamage = 50;
            AosElementDamages[AosElementAttribute.Physical] = 100;
        }

        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            phys = 100;
            fire = 0;
            cold = 0;
            nrgy = 0;
            pois = 0;
            chaos = 0;
            direct = 0;
        }

        public BerserkersScythe(Serial serial)
            : base(serial)
        {
        }

        public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            base.OnHit(attacker, defender, damageBonus);

            if (attacker == null || defender == null)
                return;


            if (defender is BaseCreature)
            {
                BaseCreature c = (BaseCreature)defender;
                if (c.Controlled == true)
                {
                    return;
                }
                else if (c.IsBlackRock == true)
                {
                    return;
                }
                else
                {
                    if (0.02 >= Utility.RandomDouble())
                    {
                        c.IsBlackRock = true;
                    }
                }

            }
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

            if (Weight == 15.0)
                Weight = 5.0;
        }
    }
}