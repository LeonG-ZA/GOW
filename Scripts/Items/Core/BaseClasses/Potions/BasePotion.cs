using System;
using System.Collections.Generic;
using Server.Engines.Craft;
using Server.Engines.ConPVP;

namespace Server.Items
{
    public enum PotionEffect
    {
        Nightsight,
        CureLesser,
        Cure,
        CureGreater,
        Agility,
        AgilityGreater,
        Strength,
        StrengthGreater,
        PoisonLesser,
        Poison,
        PoisonGreater,
        PoisonDeadly,
        Refresh,
        RefreshTotal,
        GreaterRefresh,
        HealLesser,
        Heal,
        HealGreater,
        ExplosionLesser,
        Explosion,
        ExplosionGreater,
        Conflagration,
        ConflagrationGreater,
        MaskOfDeath,
        MaskOfDeathGreater,
        ConfusionBlast,
        ConfusionBlastGreater,
        Invisibility,
        Parasitic,
        Darkglow,
		ExplodingTarPotion,
    }

    public abstract class BasePotion : Item, ICraftable, ICommodity
    {
        private PotionEffect m_PotionEffect;

        public PotionEffect PotionEffect
        {
            get
            {
                return m_PotionEffect;
            }
            set
            {
                m_PotionEffect = value;
                InvalidateProperties();
            }
        }

        int ICommodity.DescriptionNumber
        {
            get
            {
                return LabelNumber;
            }
        }
        bool ICommodity.IsDeedable
        {
            get
            {
                return (Core.ML);
            }
        }

        public override int LabelNumber { get { return 1041314 + (int)m_PotionEffect; } }

        public BasePotion(int itemID, PotionEffect effect)
            : base(itemID)
        {
            m_PotionEffect = effect;

            Stackable = Core.ML;
            Weight = 1.0;
        }

        public BasePotion(Serial serial)
            : base(serial)
        {
        }

        public virtual bool RequireFreeHand { get { return true; } }

        public virtual TimeSpan GetNextDrinkTime(Mobile from)
        {
            return TimeSpan.Zero;
        }

        public virtual void SetNextDrinkTime(Mobile from)
        {
        }

        public static bool HasFreeHand(Mobile m)
        {
            Item handOne = m.FindItemOnLayer(Layer.OneHanded);
            Item handTwo = m.FindItemOnLayer(Layer.TwoHanded);

            if (handTwo is BaseWeapon)
                handOne = handTwo;

            return (handOne == null || handTwo == null);
        }
        public static bool HasBalancedWeapon(Mobile m)
        {
            Item handOne = m.FindItemOnLayer(Layer.OneHanded);
            Item handTwo = m.FindItemOnLayer(Layer.TwoHanded);

            if (handOne is BaseWeapon)
            {
                if (((BaseWeapon)handOne).WeaponAttributes.Balanced != 0)
                    return true;
            }

            if (handTwo is BaseWeapon)
            {
                if (((BaseWeapon)handTwo).WeaponAttributes.Balanced != 0)
                    return true;
            }

            return false;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
                return;

            if (from.InRange(GetWorldLocation(), 1))
            {
                if (!RequireFreeHand || HasFreeHand(from) || HasBalancedWeapon(from))
                {
                    if (this is BaseMaskOfDeathPotion)
                    {
                        if (BaseMaskOfDeathPotion.UnderEffect(from))
                        {
                            from.SendLocalizedMessage(502173); // You are already under a similar effect.

                            return;
                        }
                    }

                    TimeSpan ts = GetNextDrinkTime(from);

                    int totalSeconds = (int)ts.TotalSeconds;
                    int totalMinutes = 0;
                    int totalHours = 0;

                    if (totalSeconds >= 60)
                    {
                        totalMinutes = (totalSeconds + 59) / 60;
                    }

                    if (totalMinutes >= 60)
                    {
                        totalHours = (totalSeconds + 3599) / 3600;
                    }

                    if (totalHours > 0)
                    {
                        from.SendLocalizedMessage(1072529, String.Format("{0}	#1072532", totalHours));

                        return;
                    }
                    else if (totalMinutes > 0)
                    {
                        from.SendLocalizedMessage(1072529, String.Format("{0}	#1072531", totalMinutes));

                        return;
                    }
                    else if (totalSeconds > 0)
                    {
                        from.SendLocalizedMessage(1072529, String.Format("{0}	#1072530", totalSeconds));

                        return;
                    }
                    else
                    {
                        Drink(from);
                    }
                }
                else
                {
                    from.SendLocalizedMessage(502172); // You must have a free hand to drink a potion.
                }
            }
            else
            {
                from.SendLocalizedMessage(502138); // That is too far away for you to use
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((int)m_PotionEffect);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 1:
                case 0:
                    {
                        m_PotionEffect = (PotionEffect)reader.ReadInt();
                        break;
                    }
            }

            if (version == 0)
                Stackable = Core.ML;
        }

        public abstract void Drink(Mobile from);

        public static void PlayDrinkEffect(Mobile m)
        {
            m.RevealingAction();

            m.PlaySound(0x2D6);

            if (!DuelContext.IsFreeConsume(m))
            {
                m.AddToBackpack(new Bottle());
            }

            if (m.Body.IsHuman && !m.Mounted)
            {
                m.Animate(34, 5, 1, true, false, 0);
            }
        }

        public static int EnhancePotions(Mobile m)
        {
            int EP = AosAttributes.GetValue(m, AosAttribute.EnhancePotions);
            int skillBonus = m.Skills.Alchemy.Fixed / 330 * 10;

            if (Core.ML && EP > 50 && m.IsPlayer())
            {
                EP = 50;
            }

            return (EP + skillBonus);
        }

        public static TimeSpan Scale(Mobile m, TimeSpan v)
        {
            if (!Core.AOS)
                return v;

            double scalar = 1.0 + (0.01 * EnhancePotions(m));

            return TimeSpan.FromSeconds(v.TotalSeconds * scalar);
        }

        public static double Scale(Mobile m, double v)
        {
            if (!Core.AOS)
                return v;

            double scalar = 1.0 + (0.01 * EnhancePotions(m));

            return v * scalar;
        }

        public static int Scale(Mobile m, int v)
        {
            if (!Core.AOS)
                return v;

            return ItemAttributes.Scale(v, 100 + EnhancePotions(m));
        }

        public override bool StackWith(Mobile from, Item dropped, bool playSound)
        {
            if (dropped is BasePotion && ((BasePotion)dropped).m_PotionEffect == m_PotionEffect)
                return base.StackWith(from, dropped, playSound);

            return false;
        }

        #region ICraftable Members
        //public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, IUsesRemaining tool, CraftItem craftItem, int resHue)
        {
            if (craftSystem is DefAlchemy)
            {
                Container pack = from.Backpack;

                if (pack != null)
                {
                    if ((int)PotionEffect >= (int)PotionEffect.Invisibility)
                        return 1;

                    List<PotionKeg> kegs = pack.FindItemsByType<PotionKeg>();

                    for (int i = 0; i < kegs.Count; ++i)
                    {
                        PotionKeg keg = kegs[i];

                        if (keg == null)
                            continue;

                        if (keg.Held <= 0 || keg.Held >= 100)
                            continue;

                        if (keg.Type != PotionEffect)
                            continue;

                        ++keg.Held;

                        Consume();
                        from.AddToBackpack(new Bottle());

                        return -1; // signal placed in keg
                    }
                }
            }

            return 1;
        }
        #endregion
    }
}