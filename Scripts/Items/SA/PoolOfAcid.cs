using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Items
{
    public class PoolOfAcid : Item
    {
        private readonly TimeSpan _Duration;
        private readonly int _MinDamage;
        private readonly int _MaxDamage;
        private readonly DateTime _Created;
        private readonly Timer _Timer;
        private bool _Drying;

        [Constructable]
        public PoolOfAcid()
            : this(TimeSpan.FromSeconds(10.0), 2, 5)
        {
        }

        [Constructable]
        public PoolOfAcid(TimeSpan duration, int minDamage, int maxDamage)
            : base(0x122A)
        {
            Hue = 0x3F;
            Movable = false;
            _MinDamage = minDamage;
            _MaxDamage = maxDamage;
            _Created = DateTime.UtcNow;
            _Duration = duration;
            _Drying = false;

            _Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1), new TimerCallback(OnTick));
        }

        public PoolOfAcid(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "a pool of acid";
            }
        }
        public override void OnAfterDelete()
        {
            if (_Timer != null)
                _Timer.Stop();
        }

        private void OnTick()
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan age = now - _Created;

            if (age > _Duration)
            {
                Delete();
            }
            else
            {
                if (!_Drying && age > (_Duration - age))
                {
                    _Drying = true;
                    ItemID = 0x122B;
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

            if (m.Alive && m.AccessLevel == AccessLevel.Player && m is PlayerMobile)
            {
                return true;
            }
            else if (bc != null && !bc.IsDeadBondedPet && (bc.Controlled || bc.Summoned))
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
            int damage = Utility.RandomMinMax(_MinDamage, _MaxDamage);
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