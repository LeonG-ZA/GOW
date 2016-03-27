using System;
using System.Collections;
using Server.Targeting;
using Server.Buff.Icons;
using Server.Spells.Bard;

namespace Server.Spells.Fourth
{
    public class CurseSpell : MagerySpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo("Curse", "Des Sanct", 227, 9031, Reagent.Nightshade, Reagent.Garlic, Reagent.SulfurousAsh);
        private static readonly Hashtable m_UnderEffect = new Hashtable();

        public CurseSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle
        {
            get
            {
                return SpellCircle.Fourth;
            }
        }
        public static void RemoveEffect(object state)
        {
            Mobile m = (Mobile)state;

            m_UnderEffect.Remove(m);

            m.UpdateResistances();
        }

        public static bool UnderEffect(Mobile m)
        {
            return m_UnderEffect.Contains(m);
        }

        #region Tainted Tree
        public static void AddTimer(Mobile m, Timer t)
        {
            m_UnderEffect[m] = t;
        }

        public static Timer GetTimer(Mobile m)
        {
            return (Timer)m_UnderEffect[m];
        }
        #endregion

        public override void OnCast()
        {
            Caster.Target = new CurseSpellTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckHSequence(m))
            {
                DoCurse(m);
            }

            FinishSequence();
        }

        public void DoCurse(Mobile m)
        {
                SpellHelper.Turn(this.Caster, m);

                SpellHelper.CheckReflect((int)this.Circle, this.Caster, ref m);

                SpellHelper.AddStatCurse(this.Caster, m, StatType.Str);
                SpellHelper.DisableSkillCheck = true;
                SpellHelper.AddStatCurse(this.Caster, m, StatType.Dex);
                SpellHelper.AddStatCurse(this.Caster, m, StatType.Int);
                SpellHelper.DisableSkillCheck = false;

                Timer t = (Timer)m_UnderEffect[m];

                TimeSpan duration = ComputeDuration(Caster, m);

                if (this.Caster.Player && m.Player /*&& Caster != m */ && t == null)	//On OSI you CAN curse yourself and get this effect.
                {
                    int percentage = (int)(SpellHelper.GetOffsetScalar(Caster, m, true) * 100);
                    string buffFormat = String.Format("{0}\t{0}\t{0}\t{1}\t{1}\t{1}\t{1}", percentage, 10);

                    BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Curse, 1075835, duration, m, buffFormat));

                    m_UnderEffect[m] = t = Timer.DelayCall(duration, new TimerStateCallback(RemoveEffect), m);
                    m.UpdateResistances();
                }

                if (m.Spell != null)
                    m.Spell.OnCasterHurt();

                m.Paralyzed = false;

                m.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist);
                m.PlaySound(0x1E1);

                HarmfulSpell(m);
        }

        private static TimeSpan ComputeDuration(Mobile caster, Mobile defender)
        {
            double seconds = SpellHelper.GetDuration(caster, defender).TotalSeconds;

            Resilience song = Spellsong.GetEffectSpellsong<Resilience>(defender);

            if (song != null)
                seconds = seconds - (song.CurseReduction * seconds / 100.0);

            return TimeSpan.FromSeconds(seconds);
        }

        private class CurseSpellTarget : Target
        {
            private readonly CurseSpell m_Owner;
            public CurseSpellTarget(CurseSpell owner) : base(Core.ML ? 10 : 12, false, TargetFlags.Harmful)
            {
                this.m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    this.m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                this.m_Owner.FinishSequence();
            }
        }
    }
}