using System;
using Server.Targeting;
using Server.Buff.Icons;
using System.Collections;

namespace Server.Spells.Third
{
    public class BlessSpell : MagerySpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo("Bless", "Rel Sanct", 203, 9061, Reagent.Garlic, Reagent.MandrakeRoot);
        public BlessSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle
        {
            get
            {
                return SpellCircle.Third;
            }
        }

        private static readonly Hashtable m_UnderEffect = new Hashtable();

        public static void RemoveEffect(object state)
        {
            Mobile m = (Mobile)state;

            m_UnderEffect.Remove(m);
        }

        public static bool UnderEffect(Mobile m)
        {
            return m_UnderEffect.Contains(m);
        }

        public override bool CheckCast()
        {
            if (Engines.ConPVP.DuelContext.CheckSuddenDeath(this.Caster))
            {
                this.Caster.SendMessage(0x22, "You cannot cast this spell when in sudden death.");
                return false;
            }

            return base.CheckCast();
        }

        public override void OnCast()
        {
            this.Caster.Target = new BlessSpellTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!this.Caster.CanSee(m))
            {
                this.Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (this.CheckBSequence(m))
            {
                SpellHelper.Turn(this.Caster, m);

                SpellHelper.AddStatBonus(this.Caster, m, StatType.Str);
                SpellHelper.DisableSkillCheck = true;
                SpellHelper.AddStatBonus(this.Caster, m, StatType.Dex);
                SpellHelper.AddStatBonus(this.Caster, m, StatType.Int);
                SpellHelper.DisableSkillCheck = false;

                Timer t = (Timer)m_UnderEffect[m];

                m.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                m.PlaySound(0x1EA);

                TimeSpan duration = SpellHelper.GetDuration(Caster, m);
                int percentage = (int)(SpellHelper.GetOffsetScalar(Caster, m, true) * 100);
                BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Bless, 1075847, duration, m, String.Format("{0}\t{0}\t{0}", percentage.ToString())));

               m_UnderEffect[m] = t = Timer.DelayCall(duration, new TimerStateCallback(RemoveEffect), m);
            }

            this.FinishSequence();
        }

        private class BlessSpellTarget : Target
        {
            private readonly BlessSpell m_Owner;
            public BlessSpellTarget(BlessSpell owner) : base(Core.ML ? 10 : 12, false, TargetFlags.Beneficial)
            {
                this.m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    this.m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                this.m_Owner.FinishSequence();
            }
        }
    }
}