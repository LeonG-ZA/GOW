using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a toxic slimey corpse")]
    public class ToxicOoze : BaseCreature
    {
        private DateTime m_Delay = DateTime.UtcNow;

        [Constructable]
        public ToxicOoze() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a toxic ooze";
            Body = 0x33;
            Hue = 1175;
            BaseSoundID = 456;

            SetStr(185);
            SetDex(150);
            SetInt(300);

            SetStam(150);
            SetMana(300);

            SetHits(2127);

            SetDamage(18);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold, 30);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 55);
            SetResistance(ResistanceType.Cold, 52);
            SetResistance(ResistanceType.Poison, 51);
            SetResistance(ResistanceType.Energy, 40);

            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.MagicResist, 61.1);
            SetSkill(SkillName.Tactics, 76.2);
            SetSkill(SkillName.Wrestling, 77.5);
            SetSkill(SkillName.Meditation, 100.0);

            //Fame = 4500;
            //Karma = -4500;

            VirtualArmor = 74;

            switch (Utility.Random(1))
            {
                case 0: AddItem(new RandomBlackRock()); break;
            }
        }

        public override void GenerateLoot()
        {
        }

        private DateTime m_NextAbilityTime;

        private void DoAreaLeech()
        {
            m_NextAbilityTime += TimeSpan.FromSeconds(2.5);

            this.FixedParticles(0x376A, 10, 10, 9537, 33, 0, EffectLayer.Waist);

            Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerCallback(DoAreaLeech_Finish));
        }

        private void DoAreaLeech_Finish()
        {
            ArrayList list = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(6))
            {
                if (this.CanBeHarmful(m) && this.IsEnemy(m))
                    list.Add(m);
            }

            {
                double scalar;

                if (list.Count == 1)
                    scalar = 0.90;
                else if (list.Count == 2)
                    scalar = 0.50;
                else
                    scalar = 0.30;

                for (int i = 0; i < list.Count; ++i)
                {
                    Mobile m = (Mobile)list[i];

                    int damage = (int)(m.Hits * scalar);

                    damage += Utility.RandomMinMax(-5, 5);

                    if (damage < 1)
                        damage = 1;

                    m.MovingParticles(this, 0x36F4, 1, 0, false, false, 32, 0, 9535, 1, 0, (EffectLayer)255, 0x100);
                    m.MovingParticles(this, 0x0001, 1, 0, false, true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0);

                    this.DoHarmful(m);
                    this.Hits += ItemAttributes.Damage(m, this, damage, 100, 0, 0, 0, 0);
                }
            }
        }

        public override void OnThink()
        {
            if (DateTime.UtcNow >= m_NextAbilityTime)
            {
                Mobile combatant = this.Combatant;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 12))
                {
                    m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));

                    int ability = Utility.Random(4);

                    switch (ability)
                    {
                        case 1: DoAreaLeech(); break;
                    }
                }
            }

            if (DateTime.UtcNow > m_Delay)
            {
                Ability.Aura(this, 10, 20, 2, 3, 0, "The radiating energy emitted from the creature is damaging you!");
                m_Delay = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            }

            base.OnThink();
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 4; } }
        public override bool AlwaysMurderer { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from != null)
            {
                int hitback = damage;
                ItemAttributes.Damage(from, this, hitback, 100, 0, 0, 0, 0);
            }
        }

        public ToxicOoze(Serial serial) : base(serial)
        {
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

            if (BaseSoundID == 263)
                BaseSoundID = 655;
        }
    }
}