using System.Collections;

namespace Server.Mobiles
{
    [CorpseName("a tormented minotaur corpse")]
    public class TormentedMinotaur : BaseCreature
    {
        [Constructable]
        public TormentedMinotaur()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Tormented Minotaur";
            Body = 262;

            SetStr(822, 930);
            SetDex(401, 415);
            SetInt(128, 138);

            SetHits(4000, 4200);

            SetDamage(16, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 62);
            SetResistance(ResistanceType.Fire, 74);
            SetResistance(ResistanceType.Cold, 54);
            SetResistance(ResistanceType.Poison, 56);
            SetResistance(ResistanceType.Energy, 54);

            SetSkill(SkillName.Wrestling, 110.1, 111.0);
            SetSkill(SkillName.Tactics, 100.7, 102.8);
            SetSkill(SkillName.MagicResist, 104.3, 116.3);

            Fame = 20000;
            Karma = -20000;
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.05 >= Utility.RandomDouble())
            {
                GroundSlap();
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 6);
            AddLoot(LootPack.FilthyRich, 2);
        }

        public override int Meat { get { return 2; } }
        public override int Hides { get { return 10; } }
        public override HideType HideType { get { return HideType.Regular; } }
        public override Poison PoisonImmune { get { return Poison.Regular; } }
        public override int TreasureMapLevel { get { return 5; } }

        public override int GetDeathSound()
        {
            return 0x596;
        }

        public override int GetAttackSound()
        {
            return 0x597;
        }

        public override int GetIdleSound()
        {
            return 0x598;
        }

        public override int GetAngerSound()
        {
            return 0x599;
        }

        public override int GetHurtSound()
        {
            return 0x59A;
        }

        public void GroundSlap()
        {
            Map map = Map;

            if (map == null)
            {
                return;
            }

            ArrayList targets = new ArrayList();

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m == this || !CanBeHarmful(m))
                {
                    continue;
                }

                if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != Team))
                {
                    targets.Add(m);
                }
                else if (m.Player)
                {
                    targets.Add(m);
                }
            }

            PlaySound(0x140);


            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                int distance = (int)m.GetDistanceToSqrt(this);

                if (distance == 0)
                    distance = 1;

                double damage = 75 / distance;
                damage = targets.Count * damage;

                damage = 10 * targets.Count;

                if (damage < 40.0)
                {
                    damage = 40.0;
                }
                else if (damage > 75.0)
                {
                    damage = 75.0;
                }

                DoHarmful(m);

                ItemAttributes.Damage(m, this, (int)damage, 100, 0, 0, 0, 0);

                m.FixedParticles(0x3728, 10, 15, 9955, EffectLayer.Waist);
                BaseMount.Dismount(m);

                if (m.Alive && m.Body.IsHuman && !m.Mounted)
                {
                    m.Animate(20, 7, 1, true, false, 0); // take hit
                }
            }
        }

        public TormentedMinotaur(Serial serial)
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