using System;
using Server.Gumps;

namespace Server.Mobiles
{
    [CorpseName("a fallen warrior corpse")]
    public class FallenWarrior : BaseCreature
    {
        [Constructable]
        public FallenWarrior()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = NameList.RandomName("ethereal warrior");
            Body = 123;

            SetStr(1000);
            SetDex(1000);
            SetInt(1000);

            SetHits(1000);

            SetDamage(17, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 50.1, 75.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 99.1, 100.0);
            SetSkill(SkillName.Meditation, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 97.6, 100.0);

            Fame = 7000;
            Karma = 7000;

            VirtualArmor = 120;

            m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(2, 5));
        }

        public override int TreasureMapLevel { get { return Core.AOS ? 5 : 0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 3);  //Make sure to add 1000 - 2000 gold
            AddLoot(LootPack.Gems);

            /*
                        if ( 0.02 > Utility.RandomDouble() )
                        {
                            switch ( Utility.Random( 2 ) )
                            {
                                case 0:	PackItem( new PlaneSword() );	break;
                                case 1:	PackItem( new PlaneShield() );	break;

                            }
                        }
            */
        }

        public override OppositionGroup OppositionGroup
        {
            get { return OppositionGroup.FeyAndUndead; }
        }

        public override int Feathers { get { return 100; } }

        public override int GetAngerSound()
        {
            return 0x2F8;
        }

        public override int GetIdleSound()
        {
            return 0x2F8;
        }

        public override int GetAttackSound()
        {
            return Utility.Random(0x2F5, 2);
        }

        public override int GetHurtSound()
        {
            return 0x2F9;
        }

        public override int GetDeathSound()
        {
            return 0x2F7;
        }

        //Drain System

        private DateTime m_NextAbilityTime;

        //HP Drain

        private void DoFocusedLeech(Mobile combatant, string message)
        {
            Say(true, message);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerStateCallback(DoFocusedLeech_Stage1), combatant);
        }

        private void DoFocusedLeech_Stage1(object state)
        {
            Mobile combatant = (Mobile)state;

            if (CanBeHarmful(combatant))
            {
                MovingParticles(combatant, 0x36FA, 1, 0, false, false, 1108, 0, 9533, 1, 0, (EffectLayer)255, 0x100);
                MovingParticles(combatant, 0x0001, 1, 0, false, true, 1108, 0, 9533, 9534, 0, (EffectLayer)255, 0);
                PlaySound(0x1FB);

                Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(DoFocusedLeech_Stage2), combatant);
            }
        }

        private void DoFocusedLeech_Stage2(object state)
        {
            Mobile combatant = (Mobile)state;

            if (CanBeHarmful(combatant))
            {
                combatant.MovingParticles(this, 0x36F4, 1, 0, false, false, 32, 0, 9535, 1, 0, (EffectLayer)255, 0x100);
                combatant.MovingParticles(this, 0x0001, 1, 0, false, true, 32, 0, 9535, 9536, 0, (EffectLayer)255, 0);

                PlaySound(0x209);
                DoHarmful(combatant);
                Hits += ItemAttributes.Damage(combatant, this, Utility.RandomMinMax(40, 80) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0);
            }
        }

        //HP Drain End

        //Mana Drain

        private void DoFocusedLeechMana(Mobile combatant, string message)
        {
            Say(true, message);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerStateCallback(DoFocusedLeechMana_Stage2), combatant);
        }

        private void DoFocusedLeechMana_Stage1(object state)
        {
            Mobile combatant = (Mobile)state;

            if (CanBeHarmful(combatant))
            {
                MovingParticles(combatant, 0x36FA, 1, 0, false, false, 1108, 0, 9533, 1, 0, (EffectLayer)255, 0x100);
                MovingParticles(combatant, 0x0001, 1, 0, false, true, 1108, 0, 9533, 9534, 0, (EffectLayer)255, 0);
                PlaySound(0x1FB);

                Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(DoFocusedLeechMana_Stage2), combatant);
            }
        }

        private void DoFocusedLeechMana_Stage2(object state)
        {
            Mobile combatant = (Mobile)state;

            if (CanBeHarmful(combatant))
            {
                combatant.MovingParticles(this, 0x36F4, 1, 0, false, false, 88, 0, 9535, 1, 0, (EffectLayer)255, 0x100);
                combatant.MovingParticles(this, 0x0001, 1, 0, false, true, 88, 0, 9535, 9536, 0, (EffectLayer)255, 0);

                PlaySound(0x209);
                int manaLeech1 = (int)(combatant.ManaMax * .50);
                int manaLeech2 = (int)(combatant.ManaMax * .90);
                int manaLeech3 = Utility.Random(manaLeech1, manaLeech2);
                combatant.Mana -= manaLeech3;
                Mana += manaLeech3;
            }
        }

        //Mana Drain End

        //Stamina Drain

        private void DoFocusedLeechStamina(Mobile combatant, string message)
        {
            Say(true, message);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerStateCallback(DoFocusedLeechStamina_Stage2), combatant);
        }

        private void DoFocusedLeechStamina_Stage1(object state)
        {
            Mobile combatant = (Mobile)state;

            if (CanBeHarmful(combatant))
            {
                MovingParticles(combatant, 0x36FA, 1, 0, false, false, 1108, 0, 9533, 1, 0, (EffectLayer)255, 0x100);
                MovingParticles(combatant, 0x0001, 1, 0, false, true, 1108, 0, 9533, 9534, 0, (EffectLayer)255, 0);
                PlaySound(0x1FB);

                Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(DoFocusedLeechStamina_Stage2), combatant);
            }
        }

        private void DoFocusedLeechStamina_Stage2(object state)
        {
            Mobile combatant = (Mobile)state;

            if (CanBeHarmful(combatant))
            {
                combatant.MovingParticles(this, 0x36F4, 1, 0, false, false, 900, 0, 9535, 1, 0, (EffectLayer)255, 0x100);
                combatant.MovingParticles(this, 0x0001, 1, 0, false, true, 900, 0, 9535, 9536, 0, (EffectLayer)255, 0);

                PlaySound(0x209);
                int stamLeech1 = (int)(combatant.StamMax * .50);
                int stamLeech2 = (int)(combatant.StamMax * .90);
                int stamLeech3 = Utility.Random(stamLeech1, stamLeech2);
                combatant.Stam -= stamLeech3;
                Stam += stamLeech3;
            }
        }

        //Stamina Drain End


        public override void OnThink()
        {
            if (DateTime.UtcNow >= m_NextAbilityTime)
            {
                Mobile combatant = Combatant;

                if (combatant != null && combatant.Map == Map && combatant.InRange(this, 12))
                {
                    m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));

                    int ability = Utility.Random(3);

                    switch (ability)
                    {
                        case 0: DoFocusedLeech(combatant, "I will suck your life and add it to my own"); break;
                        case 1: DoFocusedLeechMana(combatant, "Your power is mine to use as I wish"); break;
                        case 2: DoFocusedLeechStamina(combatant, "You shalt go nowhere unless I deem it be so"); break;
                    }
                }
            }

            base.OnThink();
        }

        //Drain System End


        //Resurrection
        private DateTime m_NextResurrect;
        private static TimeSpan ResurrectDelay = TimeSpan.FromSeconds(2.0);

        public virtual bool CheckResurrect(Mobile m)
        {
            if (m.Criminal || m.Kills <= 5 || m.Karma >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual void OfferResurrection(Mobile m)
        {
            Direction = GetDirectionTo(m);

            m.PlaySound(0x214);
            m.FixedEffect(0x376A, 10, 16);

            m.CloseGump(typeof(ResurrectGump));
            m.SendGump(new ResurrectGump(m, ResurrectMessage.Healer));
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (!m.Frozen && DateTime.UtcNow >= m_NextResurrect && InRange(m, 4) && !InRange(oldLocation, 4) && InLOS(m))
            {
                if (!m.Alive)
                {
                    m_NextResurrect = DateTime.UtcNow + ResurrectDelay;

                    if (m.Map == null || !m.Map.CanFit(m.Location, 16, false, false))
                    {
                        m.SendLocalizedMessage(502391); // Thou can not be resurrected there!
                    }
                    else if (CheckResurrect(m))
                    {
                        OfferResurrection(m);
                    }
                }
            }
        }

        //Resurrection End

        public FallenWarrior(Serial serial)
            : base(serial)
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
        }
    }
}