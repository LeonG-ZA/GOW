#region References
using System;
using System.Globalization;
using Server.Buff.Icons;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;
#endregion

namespace Server
{
    public class PoisonImpl : Poison
    {
        [CallPriority(10)]
        public static void Configure()
        {
            if (Core.AOS)
            {
                Register(new PoisonImpl("Lesser", 0, 4, 16, 7.5, 3.0, 2.25, 10, 4));
                Register(new PoisonImpl("Regular", 1, 8, 18, 10.0, 3.0, 3.25, 10, 3));
                Register(new PoisonImpl("Greater", 2, 12, 20, 15.0, 3.0, 4.25, 10, 2));
                Register(new PoisonImpl("Deadly", 3, 16, 30, 30.0, 3.0, 5.25, 15, 2));
                Register(new PoisonImpl("Lethal", 4, 20, 50, 35.0, 3.0, 5.25, 20, 2));
            }
            else
            {
                Register(new PoisonImpl("Lesser", 0, 4, 26, 2.500, 3.5, 3.0, 10, 2));
                Register(new PoisonImpl("Regular", 1, 5, 26, 3.125, 3.5, 3.0, 10, 2));
                Register(new PoisonImpl("Greater", 2, 6, 26, 6.250, 3.5, 3.0, 10, 2));
                Register(new PoisonImpl("Deadly", 3, 7, 26, 12.500, 3.5, 4.0, 10, 2));
                Register(new PoisonImpl("Lethal", 4, 9, 26, 25.000, 3.5, 5.0, 10, 2));
            }

            if (Core.ML)
            {
                Register(new PoisonImpl("LesserDarkglow", 10, 4, 16, 7.5, 3.0, 2.25, 10, 4));
                Register(new PoisonImpl("RegularDarkglow", 11, 8, 18, 10.0, 3.0, 3.25, 10, 3));
                Register(new PoisonImpl("GreaterDarkglow", 12, 12, 20, 15.0, 3.0, 4.25, 10, 2));
                Register(new PoisonImpl("DeadlyDarkglow", 13, 16, 30, 30.0, 3.0, 5.25, 15, 2));

                Register(new PoisonImpl("LesserParasitic", 14, 4, 16, 7.5, 3.0, 2.25, 10, 4));
                Register(new PoisonImpl("RegularParasitic", 15, 8, 18, 10.0, 3.0, 3.25, 10, 3));
                Register(new PoisonImpl("GreaterParasitic", 16, 12, 20, 15.0, 3.0, 4.25, 10, 2));
                Register(new PoisonImpl("DeadlyParasitic", 17, 16, 30, 30.0, 3.0, 5.25, 15, 2));
                Register(new PoisonImpl("LethalParasitic", 18, 20, 50, 35.0, 3.0, 5.25, 20, 2));
            }
        }

        public static Poison IncreaseLevel(Poison oldPoison)
        {
            Poison newPoison = oldPoison == null ? null : GetPoison(oldPoison.Level + 1);

            return newPoison ?? oldPoison;
        }

        // Info
        private readonly string _Name;
        private readonly int _Level;

        // Damage
        private readonly int _Minimum;
        private readonly int _Maximum;
        private readonly double _Scalar;

        // Timers
        private readonly TimeSpan _Delay;
        private readonly TimeSpan _Interval;
        private readonly int _Count;

        private readonly int _MessageInterval;

        public override string Name { get { return _Name; } }
        public override int Level { get { return _Level; } }

        public override int RealLevel
        {
            get
            {
                if (_Level >= 14)
                {
                    return _Level - 14;
                }

                if (_Level >= 10)
                {
                    return _Level - 10;
                }

                return _Level;
            }
        }

        public override int LabelNumber
        {
            get
            {
                if (_Level >= 14)
                {
                    return 1072852; // parasitic poison charges: ~1_val~
                }

                if (_Level >= 10)
                {
                    return 1072853; // darkglow poison charges: ~1_val~
                }

                return 1062412 + _Level; // ~poison~ poison charges: ~1_val~
            }
        }

        public PoisonImpl(string name, int level, int min, int max, double percent, double delay, double interval, int count, int messageInterval)
        {
            _Name = name;
            _Level = level;
            _Minimum = min;
            _Maximum = max;
            _Scalar = percent * 0.01;
            _Delay = TimeSpan.FromSeconds(delay);
            _Interval = TimeSpan.FromSeconds(interval);
            _Count = count;
            _MessageInterval = messageInterval;
        }

        public override Timer ConstructTimer(Mobile m)
        {
            return new PoisonTimer(m, this);
        }

        public class PoisonTimer : Timer
        {
            private readonly PoisonImpl _Poison;
            private readonly Mobile _Mobile;
            private Mobile _From;
            private int _LastDamage;
            private int _Index;

            public Mobile From { get { return _From; } set { _From = value; } }

            public PoisonTimer(Mobile m, PoisonImpl p)
                : base(p._Delay, p._Interval)
            {
                _From = m;
                _Mobile = m;
                _Poison = p;

                int damage = 1 + (int)(m.Hits * p._Scalar);

                BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Poison, 1017383, 1075633, TimeSpan.FromSeconds((int)((p._Count + 1) * p._Interval.TotalSeconds)), m, String.Format("{0}\t{1}", damage, (int)p._Interval.TotalSeconds)));
            }

            protected override void OnTick()
            {
                if ((Core.AOS && _Poison.RealLevel < 4 && TransformationSpell.UnderTransformation(_Mobile, typeof(VampiricEmbraceSpell))) || (_Poison.RealLevel < 3 && OrangePetals.UnderEffect(_Mobile)) || AnimalForm.UnderTransformation(_Mobile, typeof(Unicorn)))
                {
                    if (_Mobile.CurePoison(_Mobile))
                    {
                        _Mobile.Poison = null;
                        BuffInfo.RemoveBuff(_Mobile, BuffIcon.Poison);

                        _Mobile.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1053092);//* You feel yourself resisting the effects of the poison *
                        _Mobile.NonlocalOverheadMessage(MessageType.Regular, 0x3B2, 1071958, _Mobile.Name);//* ~1_VAL~ seems resistant to the poison *

                        Stop();
                        return;
                    }
                }

                if (_Index++ == _Poison._Count)
                {
                    _Mobile.SendLocalizedMessage(502136); // The poison seems to have worn off.
                    _Mobile.Poison = null;
                    BuffInfo.RemoveBuff(_Mobile, BuffIcon.Poison);

                    Stop();
                    return;
                }

                int damage;

                if (!Core.AOS && _LastDamage != 0 && Utility.RandomBool())
                {
                    damage = _LastDamage;
                }
                else
                {
                    damage = 1 + (int)(_Mobile.Hits * _Poison._Scalar);

                    if (damage < _Poison._Minimum)
                    {
                        damage = _Poison._Minimum;
                    }
                    else if (damage > _Poison._Maximum)
                    {
                        damage = _Poison._Maximum;
                    }

                    _LastDamage = damage;
                }

                if (_From != null)
                {
                    _From.DoHarmful(_Mobile, true);
                }

                IHonorTarget honorTarget = _Mobile as IHonorTarget;
                if (honorTarget != null && honorTarget.ReceivedHonorContext != null)
                {
                    honorTarget.ReceivedHonorContext.OnTargetPoisoned();
                }

                if (Core.ML)
                {
                    if (_From != null && _Mobile != _From && !_From.InRange(_Mobile.Location, 1) && _Poison._Level >= 10 &&
                        _Poison._Level <= 13) // darkglow
                    {
                        _From.SendLocalizedMessage(1072850); // Darkglow poison increases your damage!

                        Stop();

                        new DarkglowTimer(_Mobile, _From, _Poison, _Index).Start();
                    }

                    if (_From != null && _Mobile != _From && _From.InRange(_Mobile.Location, 1) && _Poison._Level >= 14 &&
                        _Poison._Level <= 18) // parasitic
                    {
                        Stop();

                        new ParasiticTimer(_Mobile, _From, _Poison, _Index).Start();
                    }
                }

                ItemAttributes.Damage(_Mobile, _From, damage, 0, 0, 0, 100, 0);

                if (0.60 <= Utility.RandomDouble())
                // OSI: randomly revealed between first and third damage tick, guessing 60% chance
                {
                    _Mobile.RevealingAction();
                }

                if ((_Index % _Poison._MessageInterval) == 0)
                {
                    _Mobile.OnPoisoned(_From, _Poison, _Poison);
                }
            }
        }

        public class ParasiticTimer : Timer
        {
            public Mobile _Mobile;
            public Mobile _From;
            private int _Damage;
            private readonly int _MaxCount;
            private int _Count;
            private readonly PoisonImpl _Poison;
            private int _Index;

            public ParasiticTimer(Mobile m, Mobile from, PoisonImpl poison, int Index)
                : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(5.0))
            {
                Random rnd = new Random();
                this._Mobile = m;
                this._From = from;
                this._Count = 0;
                this._MaxCount = (int)(rnd.Next(70, 75) / 5);
                this._Poison = poison;
                this._Index = Index;
            }

            protected override void OnTick()
            {
                Random dmg = new Random();
                this._Damage = dmg.Next(25, 33);
                this._Count++;

                if (this._Count > this._MaxCount || this._Mobile == null)
                {
                    _Mobile.SendLocalizedMessage(502136); // The poison seems to have worn off.
                    _Mobile.Poison = null;
                    this.Stop();
                    return;
                }

                _Mobile.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1042863);//* You feel extremely weak and are in severe pain! *

                _Mobile.NonlocalOverheadMessage(MessageType.Regular, 0x3B2, 1042864, _Mobile.Name);//* ~1_PLAYER_NAME~ is wracked with extreme pain. *

                int toHeal = Math.Min(_From.HitsMax - _From.Hits, _Damage);
                if (toHeal > 0)
                {
                    _From.SendLocalizedMessage(1060203, toHeal.ToString(CultureInfo.InvariantCulture));
                    // You have had ~1_HEALED_AMOUNT~ hit points of damage healed.
                    _From.Heal(toHeal, _Mobile, false);
                }
                if (_Mobile != null)
                    ItemAttributes.Damage(_Mobile, _From, _Damage, 0, 0, 0, 100, 0);

                if (0.60 <= Utility.RandomDouble())
                // OSI: randomly revealed between first and third damage tick, guessing 60% chance
                {
                    _Mobile.RevealingAction();
                }
                if ((_Index % _Poison._MessageInterval) == 0)
                {
                    _Mobile.OnPoisoned(_From, _Poison, _Poison);
                }
            }
        }

        public class DarkglowTimer : Timer
        {
            public Mobile _Mobile;
            public Mobile _From;
            private int _Damage;
            private readonly int _MaxCount;
            private int _Count;
            private readonly PoisonImpl _Poison;
            private int _Index;


            public DarkglowTimer(Mobile m, Mobile from, PoisonImpl poison, int Index)
                : base(TimeSpan.FromSeconds(4.0), TimeSpan.FromSeconds(4.0))
            {
                Random rnd = new Random();
                this._Mobile = m;
                this._From = from;
                this._Count = 0;
                this._MaxCount = (int)(rnd.Next(45, 60) / 4);
                this._Poison = poison;
                this._Index = Index;
            }

            protected override void OnTick()
            {
                Random dmg = new Random();
                this._Damage = dmg.Next(14, 20);
                this._Count++;

                if (this._Count > this._MaxCount || this._Mobile == null)
                {
                    _Mobile.SendLocalizedMessage(502136); // The poison seems to have worn off.
                    _Mobile.Poison = null;
                    this.Stop();
                    return;
                }

                _Mobile.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1042861);//* You begin to feel pain throughout your body! *

                _Mobile.NonlocalOverheadMessage(MessageType.Regular, 0x3B2, 1042862, _Mobile.Name);//* ~1_PLAYER_NAME~ stumbles around in confusion and pain. *

                if (_Mobile != null)
                    ItemAttributes.Damage(_Mobile, _From, _Damage, 0, 0, 0, 100, 0);

                if (0.60 <= Utility.RandomDouble())
                // OSI: randomly revealed between first and third damage tick, guessing 60% chance
                {
                    _Mobile.RevealingAction();
                }

                if ((_Index % _Poison._MessageInterval) == 0)
                {
                    _Mobile.OnPoisoned(_From, _Poison, _Poison);
                }
            }
        }
    }
}