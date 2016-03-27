using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Items
{
    public class AcidSlime : Item
    {
        private readonly TimeSpan m_Duration;
        private readonly int m_MinDamage;
        private readonly int m_MaxDamage;
        private readonly DateTime m_Created;
        private readonly Timer m_Timer;
        private bool m_Drying;
        [Constructable]
        public AcidSlime()
            : this(TimeSpan.FromSeconds(10.0), 5, 10)
        {
        }

        [Constructable]
        public AcidSlime(TimeSpan duration, int minDamage, int maxDamage)
            : base(0x122A)
        {
            this.Hue = 0x3F;
            this.Movable = false;
            this.m_MinDamage = minDamage;
            this.m_MaxDamage = maxDamage;
            this.m_Created = DateTime.UtcNow;
            this.m_Duration = duration;
            m_Drying = false;
            this.m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), new TimerCallback(OnTick));
        }

        public AcidSlime(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "slime";
            }
        }
        public override void OnAfterDelete()
        {
            if (this.m_Timer != null)
                this.m_Timer.Stop();
        }

        private void OnTick()
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan age = now - this.m_Created;

            if (age > this.m_Duration)
            {
                this.Delete();
            }
            else
            {
                if (!this.m_Drying && age > (this.m_Duration - age))
                {
                    this.m_Drying = true;
                    this.ItemID = 0x122B;
                }

                List<Mobile> toDamage = new List<Mobile>();


                foreach (Mobile m in GetMobilesInRange(0))
                {
                    if (IsValidTarget(m))
                    {
                        toDamage.Add(m);
                    }
                }

                for (int i = 0; i < toDamage.Count; i++)
                {
                    Damage(toDamage[i]);
                }
            }
        }

        private bool IsValidTarget(Mobile m)
        {
            BaseCreature bc = m as BaseCreature;

            if (m.Alive && m.AccessLevel == AccessLevel.Player &&
            m is PlayerMobile)
            {
                return true;
            }
            else if (bc != null && !bc.IsDeadBondedPet
            && (bc.Controlled || bc.Summoned))
            {
                return true;
            }
            return false;
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (IsValidTarget(m))
            {
                Damage(m);
            }
            return true;
        }

        public void Damage(Mobile m)
        {
            int damage = Utility.RandomMinMax(m_MinDamage, m_MaxDamage);
            if (Core.AOS)
                ItemAttributes.Damage(m, damage, 0, 0, 0, 100, 0);
            else
                m.Damage(damage);
        }

        public override void Serialize(GenericWriter writer)
        {
            //Don't serialize these
        }

        public override void Deserialize(GenericReader reader)
        {
        }
    }
}