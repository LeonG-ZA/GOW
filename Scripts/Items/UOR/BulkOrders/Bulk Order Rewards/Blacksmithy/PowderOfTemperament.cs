using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class PowderOfTemperament : Item, IUsesRemaining
    {
        private int m_UsesRemaining;
        [Constructable]
        public PowderOfTemperament()
            : this(10)
        {
        }

        [Constructable]
        public PowderOfTemperament(int charges)
            : base(4102)
        {
            Weight = 1.0;
            Hue = 2419;
            UsesRemaining = charges;
        }

        public PowderOfTemperament(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get
            {
                return m_UsesRemaining;
            }
            set
            {
                m_UsesRemaining = value;
                InvalidateProperties();
            }
        }
        public bool ShowUsesRemaining
        {
            get
            {
                return true;
            }
            set
            {
            }
        }
        public override int LabelNumber
        {
            get
            {
                return 1049082;
            }
        }// powder of fortifying
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((int)m_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 0:
                    {
                        m_UsesRemaining = reader.ReadInt();
                        break;
                    }
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060584, m_UsesRemaining.ToString()); // uses remaining: ~1_val~
        }

        public virtual void DisplayDurabilityTo(Mobile m)
        {
            LabelToAffix(m, 1017323, AffixType.Append, ": " + m_UsesRemaining.ToString()); // Durability
        }

        public override void OnSingleClick(Mobile from)
        {
            DisplayDurabilityTo(from);

            base.OnSingleClick(from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
                from.Target = new InternalTarget(this);
            else
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
        }

        private class InternalTarget : Target
        {
            private readonly PowderOfTemperament m_Powder;
            public InternalTarget(PowderOfTemperament powder)
                : base(2, false, TargetFlags.None)
            {
                m_Powder = powder;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Powder.Deleted || m_Powder.UsesRemaining <= 0)
                {
                    from.SendLocalizedMessage(1049086); // You have used up your powder of temperament.
                    return;
                }

                if (targeted is IDurability && ((IDurability)targeted).Brittle || AosAttribute.Brittle > 0)
                {
                    from.SendLocalizedMessage(1049083); // You cannot use the powder on that item.
                    return;
                }
                else if (targeted is BaseArmor /*&& (DefBlacksmithy.CraftSystem.CraftItems.SearchForSubclass( targeted.GetType() ) != null)*/ )
                {
                    BaseArmor ar = (BaseArmor)targeted;

                    if (!ar.CanFortify)
                    {
                        from.SendLocalizedMessage(1049083); // You cannot use the powder on that item.
                        return;
                    }
                    //BaseArmor Ti = (BaseArmor)item as BaseArmor;
                    //if (AosAttribute.Brittle > 0)
                    //{
                    //    from.SendLocalizedMessage(1149799); // That cannot be used on brittle items.
                    //    return;
                    //}


                    if (ar.IsChildOf(from.Backpack) && m_Powder.IsChildOf(from.Backpack))
                    {
                        int origMaxHP = ar.MaxHitPoints;
                        int origCurHP = ar.HitPoints;

                        int initMaxHP = 255;

                        ar.UnscaleDurability();

                        if (ar.MaxHitPoints < initMaxHP)
                        {
                            int bonus = initMaxHP - ar.MaxHitPoints;

                            if (bonus > 10)
                                bonus = 10;

                            ar.MaxHitPoints += bonus;
                            ar.HitPoints += bonus;

                            ar.ScaleDurability();

                            if (ar.MaxHitPoints > 255)
                                ar.MaxHitPoints = 255;

                            if (ar.HitPoints > 255)
                                ar.HitPoints = 255;

                            if (ar.MaxHitPoints > origMaxHP)
                            {
                                from.SendLocalizedMessage(1049084); // You successfully use the powder on the item.

                                --m_Powder.UsesRemaining;

                                if (m_Powder.UsesRemaining <= 0)
                                {
                                    from.SendLocalizedMessage(1049086); // You have used up your powder of temperament.
                                    m_Powder.Delete();
                                }
                            }
                            else
                            {
                                ar.MaxHitPoints = origMaxHP;
                                ar.HitPoints = origCurHP;
                                from.SendLocalizedMessage(1049085); // The item cannot be improved any further.
                            }
                        }
                        else
                        {
                            from.SendLocalizedMessage(1049085); // The item cannot be improved any further.
                            ar.ScaleDurability();
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    }
                }
                else if (targeted is BaseWeapon)
                {
                    BaseWeapon wep = (BaseWeapon)targeted;

                    if (!wep.CanFortify)
                    {
                        from.SendLocalizedMessage(1049083); // You cannot use the powder on that item.
                        return;
                    }

                    if (wep.IsChildOf(from.Backpack) && m_Powder.IsChildOf(from.Backpack))
                    {
                        int origMaxHP = wep.MaxHitPoints;
                        int origCurHP = wep.HitPoints;

                        int initMaxHP = 255;

                        wep.UnscaleDurability();

                        if (wep.MaxHitPoints < initMaxHP)
                        {
                            int bonus = initMaxHP - wep.MaxHitPoints;

                            if (bonus > 10)
                                bonus = 10;

                            wep.MaxHitPoints += bonus;
                            wep.HitPoints += bonus;

                            wep.ScaleDurability();

                            if (wep.MaxHitPoints > 255)
                                wep.MaxHitPoints = 255;

                            if (wep.HitPoints > 255)
                                wep.HitPoints = 255;

                            if (wep.MaxHitPoints > origMaxHP)
                            {
                                from.SendLocalizedMessage(1049084); // You successfully use the powder on the item.

                                --m_Powder.UsesRemaining;

                                if (m_Powder.UsesRemaining <= 0)
                                {
                                    from.SendLocalizedMessage(1049086); // You have used up your powder of temperament.
                                    m_Powder.Delete();
                                }
                            }
                            else
                            {
                                wep.MaxHitPoints = origMaxHP;
                                wep.HitPoints = origCurHP;
                                from.SendLocalizedMessage(1049085); // The item cannot be improved any further.
                            }
                        }
                        else
                        {
                            from.SendLocalizedMessage(1049085); // The item cannot be improved any further.
                            wep.ScaleDurability();
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    }
                }
                else if (targeted is BaseClothing)
                {
                    BaseClothing clothing = (BaseClothing)targeted;

                    if (!clothing.CanFortify)
                    {
                        from.SendLocalizedMessage(1049083); // You cannot use the powder on that item.
                        return;
                    }

                    if (clothing.IsChildOf(from.Backpack) && m_Powder.IsChildOf(from.Backpack))
                    {
                        int origMaxHP = clothing.MaxHitPoints;
                        int origCurHP = clothing.HitPoints;

                        int initMaxHP = 255;

                        if (clothing.MaxHitPoints < initMaxHP)
                        {
                            int bonus = initMaxHP - clothing.MaxHitPoints;

                            if (bonus > 10)
                                bonus = 10;

                            clothing.MaxHitPoints += bonus;
                            clothing.HitPoints += bonus;

                            if (clothing.MaxHitPoints > 255)
                                clothing.MaxHitPoints = 255;

                            if (clothing.HitPoints > 255)
                                clothing.HitPoints = 255;

                            if (clothing.MaxHitPoints > origMaxHP)
                            {
                                from.SendLocalizedMessage(1049084); // You successfully use the powder on the item.

                                --m_Powder.UsesRemaining;

                                if (m_Powder.UsesRemaining <= 0)
                                {
                                    from.SendLocalizedMessage(1049086); // You have used up your powder of temperament.
                                    m_Powder.Delete();
                                }
                            }
                            else
                            {
                                clothing.MaxHitPoints = origMaxHP;
                                clothing.HitPoints = origCurHP;
                                from.SendLocalizedMessage(1049085); // The item cannot be improved any further.
                            }
                        }
                        else
                        {
                            from.SendLocalizedMessage(1049085); // The item cannot be improved any further.
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1049083); // You cannot use the powder on that item.
                }

                /*
                if (targeted is IDurability && targeted is Item)
                {
                    IDurability wearable = (IDurability)targeted;
                    Item item = (Item)targeted;

                    if (!wearable.CanFortify)
                    {
                        from.SendLocalizedMessage(1049083); // You cannot use the powder on that item.
                        return;
                    }

                    if (item is BaseArmor)
                    {
                        BaseArmor Ti = (BaseArmor)item as BaseArmor;
                        if (Ti.Attributes.Brittle > 0)
                        {
                            from.SendLocalizedMessage(1149799); // That cannot be used on brittle items.
                            return;
                        }
                    }
                    else if (item is BaseJewel)
                    {
                        BaseJewel Ti = (BaseJewel)item as BaseJewel;
                        if (Ti.Attributes.Brittle > 0)
                        {
                            from.SendLocalizedMessage(1149799); // That cannot be used on brittle items.
                            return;
                        }
                    }
                    else if (item is BaseHat)
                    {
                        BaseHat Ti = (BaseHat)item as BaseHat;
                        if (Ti.Attributes.Brittle > 0)
                        {
                            from.SendLocalizedMessage(1149799); // That cannot be used on brittle items.
                            return;
                        }
                    }
                    else if (item is BaseWeapon)
                    {
                        BaseWeapon Ti = (BaseWeapon)item as BaseWeapon;
                        if (Ti.Attributes.Brittle > 0)
                        {
                            from.SendLocalizedMessage(1149799); // That cannot be used on brittle items.
                            return;
                        }
                    }

                    if ((item.IsChildOf(from.Backpack) || (Core.ML && item.Parent == from)) && m_Powder.IsChildOf(from.Backpack))
                    {
                        int origMaxHP = wearable.MaxHitPoints;
                        int origCurHP = wearable.HitPoints;

                        if (origMaxHP > 0)
                        {
                            int initMaxHP = Core.AOS ? 255 : wearable.InitMaxHits;

                            wearable.UnscaleDurability();

                            if (wearable.MaxHitPoints < initMaxHP)
                            {
                                int bonus = initMaxHP - wearable.MaxHitPoints;

                                if (bonus > 10)
                                    bonus = 10;

                                wearable.MaxHitPoints += bonus;
                                wearable.HitPoints += bonus;

                                wearable.ScaleDurability();

                                if (wearable.MaxHitPoints > 255)
                                    wearable.MaxHitPoints = 255;
                                if (wearable.HitPoints > 255)
                                    wearable.HitPoints = 255;

                                if (wearable.MaxHitPoints > origMaxHP)
                                {
                                    from.SendLocalizedMessage(1049084); // You successfully use the powder on the item.
                                    from.PlaySound(0x247);

                                    --m_Powder.UsesRemaining;

                                    if (m_Powder.UsesRemaining <= 0)
                                    {
                                        from.SendLocalizedMessage(1049086); // You have used up your powder of fortifying.
                                        m_Powder.Delete();
                                    }
                                }
                                else
                                {
                                    wearable.MaxHitPoints = origMaxHP;
                                    wearable.HitPoints = origCurHP;
                                    from.SendLocalizedMessage(1049085); // The item cannot be improved any further.
                                }
                            }
                            else
                            {
                                from.SendLocalizedMessage(1049085); // The item cannot be improved any further.
                                wearable.ScaleDurability();
                            }
                        }
                        else
                        {
                            from.SendLocalizedMessage(1049083); // You cannot use the powder on that item.
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1049083); // You cannot use the powder on that item.
                }
                 */
            }
        }
    }
}