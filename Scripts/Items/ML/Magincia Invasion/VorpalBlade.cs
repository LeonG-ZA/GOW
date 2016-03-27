using System;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Engines.Harvest;
using Server.Mobiles;

namespace Server.Items
{
    [FlipableAttribute(0x1400, 0x1401)]
    public class VorpalBlade : Kryss
    {
        public override int LabelNumber { get { return 1079836; } } // Vorpal Blade

        public override int ArtifactRarity { get { return 12; } }
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }


        [Constructable]
        public VorpalBlade()
        {
            Hue = 1175;
            Weight = 2.0;
            Slayer = SlayerName.Repond;
            Attributes.BonusDex = 5;
            Attributes.AttackChance = 15;
            Attributes.WeaponDamage = 40;
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

                else
                {
                    if (.01 >= Utility.RandomDouble())
                    {
                        c.Kill();
                    }
                }

            }
        }

        public VorpalBlade(Serial serial)
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

            if (Weight == 1.0)
                Weight = 2.0;
        }
    }
}