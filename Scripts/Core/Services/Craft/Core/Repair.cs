using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.Craft
{
    public class Repair
    {
        public Repair()
        {
        }

        public static void Do(Mobile from, CraftSystem craftSystem, double skill_level, Item contract)
        {
            from.Target = new RepairTarget(craftSystem, skill_level, contract);
            from.SendLocalizedMessage(1044276); // Target an item to repair.
        }

        //public static void Do(Mobile from, CraftSystem craftSystem, BaseTool tool)
        public static void Do(Mobile from, CraftSystem craftSystem, IUsesRemaining tool)
        {
            from.Target = new RepairTarget(craftSystem, tool);
            from.SendLocalizedMessage(1044276); // Target an item to repair.
        }

        /*
        public static void Do(Mobile from, CraftSystem craftSystem, BaseTool tool)
        {
            from.Target = new RepairTarget(craftSystem, tool);
            from.SendLocalizedMessage(1044276); // Target an item to repair.
        }

        public static void Do(Mobile from, CraftSystem craftSystem, RepairDeed deed)
        {
            from.Target = new RepairTarget(craftSystem, deed);
            from.SendLocalizedMessage(1044276); // Target an item to repair.
        }
         */

        private class RepairTarget : Target
        {
            private readonly CraftSystem m_CraftSystem;
            //private readonly BaseTool m_Tool;
            private readonly IUsesRemaining m_Tool;
            //private readonly RepairDeed m_Deed;

            private double m_SkillLevel = 100.0;
            private Item m_Contract = null;

            public RepairTarget(CraftSystem craftSystem, IUsesRemaining tool)
                : base(2, false, TargetFlags.None)
            {
                m_CraftSystem = craftSystem;
                m_Tool = tool;
            }

            //public RepairTarget(CraftSystem craftSystem, BaseTool tool, double skill_level, Item contract)
                //: base(2, false, TargetFlags.None)
            public RepairTarget(CraftSystem craftSystem, double skill_level, Item contract)
                : base(2, false, TargetFlags.None)
            {
                m_CraftSystem = craftSystem;
                m_Tool = null;

                m_SkillLevel = skill_level;
                m_Contract = contract;
            }

            //public RepairTarget(CraftSystem craftSystem, RepairDeed deed)
            //    : base(2, false, TargetFlags.None)
            //{
            //    m_CraftSystem = craftSystem;
            //    m_Deed = deed;
           // }

            private int GetWeakenChance(Mobile mob, SkillName skill, int curHits, int maxHits)
            {
                // 40% - (1% per hp lost) - (1% per 10 craft skill)
                //return (40 + (maxHits - curHits)) - (int)(((m_Deed != null) ? m_Deed.SkillLevel : mob.Skills[skill].Value) / 10);
                return (40 + (maxHits - curHits)) - (int)(mob.Skills[skill].Value / 10);
            }

            private bool CheckWeaken(Mobile mob, SkillName skill, int curHits, int maxHits)
            {
                return (GetWeakenChance(mob, skill, curHits, maxHits) > Utility.Random(100));
            }

            private int GetRepairDifficulty(int curHits, int maxHits)
            {
                return (((maxHits - curHits) * 1250) / Math.Max(maxHits, 1)) - 250;
            }

            private bool CheckRepairDifficulty(Mobile mob, SkillName skill, int curHits, int maxHits)
            {
                double difficulty = GetRepairDifficulty(curHits, maxHits) * 0.1;

                if (m_Contract != null)
                {
                    double chance = (m_SkillLevel - difficulty + 25.0) / 50.0;

                    return (chance >= Utility.RandomDouble());
                }

                return mob.CheckSkill(skill, difficulty - 25.0, difficulty + 25.0);
                /*
                if (m_Deed != null)
                {
                    double value = m_Deed.SkillLevel;
                    double minSkill = difficulty - 25.0;
                    double maxSkill = difficulty + 25;

                    if (value < minSkill)
                        return false; // Too difficult
                    else if (value >= maxSkill)
                        return true; // No challenge

                    double chance = (value - minSkill) / (maxSkill - minSkill);

                    return (chance >= Utility.RandomDouble());
                }
                else
                {
                    return mob.CheckSkill(skill, difficulty - 25.0, difficulty + 25.0);
                }
                 */
            }

            //private bool CheckDeed(Mobile from)
            //{
                //if (m_Deed != null)
                //{
                //    return m_Deed.Check(from);
                //}

                //return true;
            //}

            public RepairTarget(CraftSystem craftSystem, BaseTool tool)
                : base(2, false, TargetFlags.None)
            {
                m_CraftSystem = craftSystem;
                m_Tool = tool;
            }

            private static void EndGolemRepair(object state)
            {
               ((Mobile)state).EndAction(typeof(Golem));
            }

            private bool IsSpecialClothing(BaseClothing clothing)
            {
                // Clothing repairable but not craftable
                if (m_CraftSystem is DefTailoring)
                {
                    return (clothing is BearMask) ||
                           (clothing is DeerMask) ||
                           (clothing is TheMostKnowledgePerson) ||
                           (clothing is TheRobeOfBritanniaAri) ||
                           (clothing is EmbroideredOakLeafCloak);
                }

                return false;
            }

            private bool IsSpecialWeapon(BaseWeapon weapon)
            {
                // Weapons repairable but not craftable
                if (m_CraftSystem is DefTinkering)
                {
                    return (weapon is Cleaver) ||
                           (weapon is Hatchet) ||
                           (weapon is Pickaxe) ||
                           (weapon is ButcherKnife) ||
                           (weapon is SkinningKnife);
                }
                else if (m_CraftSystem is DefCarpentry)
                {
                    return (weapon is Club) ||
                           (weapon is BlackStaff) ||
                           (weapon is MagicWand)
                           #region Temporary
                           // TODO: Make these items craftable
                           ||
                           (weapon is WildStaff);
                    #endregion
                }
                else if (m_CraftSystem is DefBlacksmithy)
                {
                    return (weapon is Pitchfork)
                           #region Temporary
                           // TODO: Make these items craftable
                           ||
                           (weapon is RadiantScimitar) ||
                           (weapon is WarCleaver) ||
                           (weapon is ElvenSpellblade) ||
                           (weapon is AssassinSpike) ||
                           (weapon is Leafblade) ||
                           (weapon is RuneBlade) ||
                           (weapon is ElvenMachete) ||
                           (weapon is OrnateAxe) ||
                           (weapon is DiamondMace);
                    #endregion
                }
                #region Temporary
                // TODO: Make these items craftable
                else if (m_CraftSystem is DefBowFletching)
                {
                    return (weapon is ElvenCompositeLongbow) ||
                           (weapon is MagicalShortbow);
                }
                #endregion

                return false;
            }

            private bool IsSpecialArmor(BaseArmor armor)
            {
                // Armor repairable but not craftable
                #region Temporary
                // TODO: Make these items craftable
                if (m_CraftSystem is DefTailoring)
                {
                    return (armor is LeafTonlet) ||
                           (armor is LeafArms) ||
                           (armor is LeafChest) ||
                           (armor is LeafGloves) ||
                           (armor is LeafGorget) ||
                           (armor is LeafLegs) ||
                           (armor is HideChest) ||
                           (armor is HideGloves) ||
                           (armor is HideGorget) ||
                           (armor is HidePants) ||
                           (armor is HidePauldrons);
                }
                else if (m_CraftSystem is DefCarpentry)
                {
                    return (armor is WingedHelm) ||
                           (armor is RavenHelm) ||
                           (armor is VultureHelm) ||
                           (armor is WoodlandArms) ||
                           (armor is WoodlandChest) ||
                           (armor is WoodlandGloves) ||
                           (armor is WoodlandGorget) ||
                           (armor is WoodlandLegs);
                }
                else if (m_CraftSystem is DefBlacksmithy)
                {
                    return (armor is Circlet) ||
                           (armor is RoyalCirclet) ||
                           (armor is GemmedCirclet);
                }
                else if (m_CraftSystem is DefTinkering)
                {
                    return (armor is BrightsightLenses || armor is ElvenGlasses || armor is GargishGlasses || armor is BaseGlasses);
                }
                #endregion

                return false;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                int number;

                if (m_CraftSystem is DefBlacksmithy)
                {
                    bool anvil, forge;
                    DefBlacksmithy.CheckAnvilAndForge(from, 2, out anvil, out forge);

                    if (anvil && forge)
                    {
                        // You must be near a forge and and anvil to repair items.
                        number = 1044282;
                    }
                }

                if (!AllowsRepair(targeted as Item, m_CraftSystem))
                {
                    from.SendLocalizedMessage(500426); // You can't repair that.  
                    return;
                }

                //if (!CheckDeed(from))
                    //return;

                //bool usingDeed = (m_Deed != null);
                //bool toDelete = false;

                // TODO: Make an IRepairable

                //if (m_CraftSystem.CanCraft(from, m_Tool, targeted.GetType()) == 1044267)
                //{
                    //number = 1044282; // You must be near a forge and and anvil to repair items. * Yes, there are two and's *
                //}
                //if (m_CraftSystem is DefTinkering && targeted is Golem)
                if (m_CraftSystem is DefTinkering && targeted is BaseCreature && ((BaseCreature)targeted).IsGolem)
                {
                    //Golem g = (Golem)targeted;
                    BaseCreature g = (BaseCreature)targeted;
                    int damage = g.HitsMax - g.Hits;

                    if (g.IsDeadBondedPet)
                    {
                        number = 500426; // You can't repair that.
                    }
                    else if (damage <= 0)
                    {
                        number = 500423; // That is already in full repair.
                    }
                    else
                    {
                        //double skillValue = (usingDeed) ? m_Deed.SkillLevel : from.Skills[SkillName.Tinkering].Value;
                        double skillValue = from.Skills[SkillName.Tinkering].Value;

                        if (skillValue < 60.0)
                        {
                            number = 1044153; // You don't have the required skills to attempt this item.	//TODO: How does OSI handle this with deeds with golems?
                        }
                        else if (!from.CanBeginAction(typeof(Golem)))
                        {
                            number = 501789; // You must wait before trying again.
                        }
                        else
                        {
                            if (damage > (int)(skillValue * 0.3))
                            {
                                damage = (int)(skillValue * 0.3);
                            }

                            damage += 30;

                            if (!from.CheckSkill(SkillName.Tinkering, 0.0, 100.0))
                            {
                                damage /= 2;
                            }

                            Container pack = from.Backpack;

                            if (pack != null)
                            {
                                int v = pack.ConsumeUpTo(typeof(IronIngot), (damage + 4) / 5);

                                if (v > 0)
                                {
                                    g.Hits += v * 5;

                                    number = 1044279; // You repair the item.
                                    //toDelete = true;

                                    from.BeginAction(typeof(Golem));
                                    Timer.DelayCall(TimeSpan.FromSeconds(12.0), new TimerStateCallback(EndGolemRepair), from);
                                    
                                    if (m_Contract != null)
                                    {
                                        m_Contract.Delete();
                                    }
                                }
                                else
                                {
                                    number = 1044037; // You do not have sufficient metal to make that.
                                }
                            }
                            else
                            {
                                number = 1044037; // You do not have sufficient metal to make that.
                            }
                        }
                    }
                }
                //else if (targeted is IFactionArtifact)
                //{
                    // That item cannot be repaired
                    //number = 1044277;
               // }
                else if (targeted is BaseWeapon)
                {
                    BaseWeapon weapon = (BaseWeapon)targeted;
                    SkillName skill = m_CraftSystem.MainSkill;
                    int toWeaken = 0;

                    if (Core.AOS)
                    {
                        toWeaken = 1;
                    }
                    //else if (skill != SkillName.Tailoring)
                    //{
                        //double skillLevel = (usingDeed) ? m_Deed.SkillLevel : from.Skills[skill].Base;

                        //if (skillLevel >= 90.0)
                           // toWeaken = 1;
                        //else if (skillLevel >= 70.0)
                          //  toWeaken = 2;
                        //else
                           // toWeaken = 3;
                   // }
                    if (m_CraftSystem.CraftItems.SearchForSubclass(weapon.GetType()) == null && !IsSpecialWeapon(weapon))
                    {
                        if (m_Contract != null)
                        {
                            // You cannot repair that item with this type of repair contract.
                            number = 1061136;
                        }
                        else
                        {
                            // You cannot repair that using this type of tool.
                            number = 1061139;
                        }
                    }
                   // if (m_CraftSystem.CraftItems.SearchForSubclass(weapon.GetType()) == null && !IsSpecialWeapon(weapon))
                    //{
                    //    number = (usingDeed) ? 1061136 : 1044277; // That item cannot be repaired. // You cannot repair that item with this type of repair contract.
                    //}
                    else if (!weapon.IsChildOf(from.Backpack) && (!Core.ML || weapon.Parent != from))
                    {
                        number = 1044275; // The item must be in your backpack to repair it.
                    }
                    else if (!Core.AOS && weapon.PoisonCharges != 0)
                    {
                        number = 1005012; // You cannot repair an item while a caustic substance is on it.
                    }
                    else if (weapon.MaxHitPoints <= 0 || weapon.HitPoints == weapon.MaxHitPoints)
                    {
                        number = 1044281; // That item is in full repair
                    }
                    else if (weapon.MaxHitPoints <= toWeaken)
                    {
                        number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
                    }
                    else if (weapon.Attributes.NoRepairs > 0) // Cannot be Repaired
                    {
                        number = 1044277; // That item cannot be repaired.
                    }
                    else if (weapon.BlockRepair)
                    //else if (targeted is DragonsEnd || targeted is KatrinasCrook || targeted is JaanasStaff) // Quick fix
                    {
                        number = 1044277; // That item cannot be repaired.
                    }
                    else
                    {
                        if (CheckWeaken(from, skill, weapon.HitPoints, weapon.MaxHitPoints))
                        {
                            weapon.MaxHitPoints -= toWeaken;
                            weapon.HitPoints = Math.Max(0, weapon.HitPoints - toWeaken);
                        }

                        if (CheckRepairDifficulty(from, skill, weapon.HitPoints, weapon.MaxHitPoints))
                        {
                            number = 1044279; // You repair the item.
                            m_CraftSystem.PlayCraftEffect(from);
                            weapon.HitPoints = weapon.MaxHitPoints;
                        }
                        else
                        {
                            if (m_Contract != null)
                            {
                                // You fail to repair the item and the repair contract is destroyed.
                                number = 1061137;
                            }
                            else
                            {
                                // You fail to repair the item.
                                number = 1044280;
                            }

                            m_CraftSystem.PlayCraftEffect(from);
                        }

                        if (m_Contract != null)
                        {
                            m_Contract.Delete();
                        }
                        //else
                        //{
                          //  number = (usingDeed) ? 1061137 : 1044280; // You fail to repair the item. [And the contract is destroyed]
                           // m_CraftSystem.PlayCraftEffect(from);
                       // }

                        //toDelete = true;
                    }
                }
                else if (targeted is BaseArmor)
                {
                    BaseArmor armor = (BaseArmor)targeted;
                    SkillName skill = m_CraftSystem.MainSkill;
                    int toWeaken = 0;

                    if (Core.AOS)
                    {
                        toWeaken = 1;
                    }
                    /*else if (skill != SkillName.Tailoring)
                    {
                        double skillLevel = (usingDeed) ? m_Deed.SkillLevel : from.Skills[skill].Base;

                        if (skillLevel >= 90.0)
                            toWeaken = 1;
                        else if (skillLevel >= 70.0)
                            toWeaken = 2;
                        else
                            toWeaken = 3;
                    }
                     */
                    if (m_CraftSystem.CraftItems.SearchForSubclass(armor.GetType()) == null && !IsSpecialArmor(armor))
                    {
                        if (m_Contract != null)
                        {
                            // You cannot repair that item with this type of repair contract.
                            number = 1061136;
                        }
                        else
                        {
                            // You cannot repair that using this type of tool.
                            number = 1061139;
                        }
                    }
                    //if (m_CraftSystem.CraftItems.SearchForSubclass(armor.GetType()) == null && !IsSpecialArmor(armor))
                    //{
                    //    number = (usingDeed) ? 1061136 : 1044277; // That item cannot be repaired. // You cannot repair that item with this type of repair contract.
                    //}
                    else if (!armor.IsChildOf(from.Backpack) && (!Core.ML || armor.Parent != from))
                    {
                        number = 1044275; // The item must be in your backpack to repair it.
                    }
                    else if (armor.MaxHitPoints <= 0 || armor.HitPoints == armor.MaxHitPoints)
                    {
                        number = 1044281; // That item is in full repair
                    }
                    else if (armor.MaxHitPoints <= toWeaken)
                    {
                        number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
                    }
                    else if (armor.Attributes.NoRepairs > 0)// Cannot be Repaired
                    {
                        number = 1044277; // That item cannot be repaired.
                    }
                    else if (armor.BlockRepair)
                    //else if (armor is LordBlackthornsExemplar || armor is SentinelsGuard)// quick fix
                    {
                        number = 1044277; // That item cannot be repaired.
                    }
                    else
                    {
                        if (CheckWeaken(from, skill, armor.HitPoints, armor.MaxHitPoints))
                        {
                            armor.MaxHitPoints -= toWeaken;
                            armor.HitPoints = Math.Max(0, armor.HitPoints - toWeaken);
                        }

                        if (CheckRepairDifficulty(from, skill, armor.HitPoints, armor.MaxHitPoints))
                        {
                            number = 1044279; // You repair the item.
                            m_CraftSystem.PlayCraftEffect(from);
                            armor.HitPoints = armor.MaxHitPoints;
                        }
                        else
                        {
                            if (m_Contract != null)
                            {
                                // You fail to repair the item and the repair contract is destroyed.
                                number = 1061137;
                            }
                            else
                            {
                                // You fail to repair the item.
                                number = 1044280;
                            }

                            m_CraftSystem.PlayCraftEffect(from);
                        }

                        if (m_Contract != null)
                        {
                            m_Contract.Delete();
                        }
                        /*
                        else
                        {
                            number = (usingDeed) ? 1061137 : 1044280; // You fail to repair the item. [And the contract is destroyed]
                            m_CraftSystem.PlayCraftEffect(from);
                        }

                        toDelete = true;
                         */
                    }
                }
                else if (targeted is BaseJewel && ((BaseJewel)targeted).TimesImbued > 0)
                {
                    BaseJewel jewel = (BaseJewel)targeted;
                    SkillName skill = m_CraftSystem.MainSkill;
                    int toWeaken = 0;

                    if (Core.AOS)
                    {
                        toWeaken = 1;
                    }
                    /*else if (skill != SkillName.Tailoring)
                    {
                        double skillLevel = (usingDeed) ? m_Deed.SkillLevel : from.Skills[skill].Base;

                        if (skillLevel >= 90.0)
                            toWeaken = 1;
                        else if (skillLevel >= 70.0)
                            toWeaken = 2;
                        else
                            toWeaken = 3;
                    }
                     */
                    if (m_CraftSystem.CraftItems.SearchForSubclass(jewel.GetType()) == null && !(jewel is SilverBracelet || jewel is SilverRing))
                    {
                        if (m_Contract != null)
                        {
                            // You cannot repair that item with this type of repair contract.
                            number = 1061136;
                        }
                        else
                        {
                            // That item cannot be repaired
                            number = 1044277;
                        }
                    }
                    ///if (m_CraftSystem.CraftItems.SearchForSubclass(jewel.GetType()) == null)
                    //{
                      //  number = (usingDeed) ? 1061136 : 1044277; // That item cannot be repaired. // You cannot repair that item with this type of repair contract.
                   // }
                    else if (!jewel.IsChildOf(from.Backpack))
                    {
                        number = 1044275; // The item must be in your backpack to repair it.
                    }
                    else if (jewel.MaxHitPoints <= 0 || jewel.HitPoints == jewel.MaxHitPoints)
                    {
                        number = 1044281; // That item is in full repair
                    }
                    else if (jewel.MaxHitPoints <= toWeaken)
                    {
                        number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
                    }
                    else if (jewel.BlockRepair)
                    {
                        number = 1044277; // That item cannot be repaired.
                    }
                    else
                    {
                        if (CheckWeaken(from, skill, jewel.HitPoints, jewel.MaxHitPoints))
                        {
                            jewel.MaxHitPoints -= toWeaken;
                            jewel.HitPoints = Math.Max(0, jewel.HitPoints - toWeaken);
                        }

                        if (CheckRepairDifficulty(from, skill, jewel.HitPoints, jewel.MaxHitPoints))
                        {
                            number = 1044279; // You repair the item.
                            m_CraftSystem.PlayCraftEffect(from);
                            jewel.HitPoints = jewel.MaxHitPoints;
                        }
                        else
                        {
                            if (m_Contract != null)
                            {
                                // You fail to repair the item and the repair contract is destroyed.
                                number = 1061137;
                            }
                            else
                            {
                                // You fail to repair the item.
                                number = 1044280;
                            }

                            m_CraftSystem.PlayCraftEffect(from);
                        }

                        if (m_Contract != null)
                        {
                            m_Contract.Delete();
                        }
                        //else
                        //{
                        //    number = (usingDeed) ? 1061137 : 1044280; // You fail to repair the item. [And the contract is destroyed]
                        //    m_CraftSystem.PlayCraftEffect(from);
                        //}

                        //toDelete = true;
                    }
                }
                else if (targeted is BaseClothing)
                {
                    BaseClothing clothing = (BaseClothing)targeted;
                    SkillName skill = m_CraftSystem.MainSkill;
                    int toWeaken = 0;

                    if (Core.AOS)
                    {
                        toWeaken = 1;
                    }
                        /*
                    else if (skill != SkillName.Tailoring)
                    {
                        double skillLevel = (usingDeed) ? m_Deed.SkillLevel : from.Skills[skill].Base;

                        if (skillLevel >= 90.0)
                            toWeaken = 1;
                        else if (skillLevel >= 70.0)
                            toWeaken = 2;
                        else
                            toWeaken = 3;
                    }
                         */
                    if (m_CraftSystem.CraftItems.SearchForSubclass(clothing.GetType()) == null && !IsSpecialClothing(clothing))
                    {
                        if (m_Contract != null)
                        {
                            // You cannot repair that item with this type of repair contract.
                            number = 1061136;
                        }
                        else
                        {
                            // That item cannot be repaired
                            number = 1044277;
                        }
                    }

                    //if (m_CraftSystem.CraftItems.SearchForSubclass(clothing.GetType()) == null && !IsSpecialClothing(clothing) && !((targeted is TribalMask) || (targeted is HornedTribalMask)))
                    //{
                    //    number = (usingDeed) ? 1061136 : 1044277; // That item cannot be repaired. // You cannot repair that item with this type of repair contract.
                    //}
                    else if (!clothing.IsChildOf(from.Backpack) && (!Core.ML || clothing.Parent != from))
                    {
                        number = 1044275; // The item must be in your backpack to repair it.
                    }
                    else if (clothing.MaxHitPoints <= 0 || clothing.HitPoints == clothing.MaxHitPoints)
                    {
                        number = 1044281; // That item is in full repair
                    }
                    else if (clothing.MaxHitPoints <= toWeaken)
                    {
                        number = 1044278; // That item has been repaired many times, and will break if repairs are attempted again.
                    }
                    else if (clothing.Attributes.NoRepairs > 0) // Cannot be Repaired
                    {
                        number = 1044277; // That item cannot be repaired.
                    }
                    else if (clothing.BlockRepair)// quick fix
                    {
                        number = 1044277; // That item cannot be repaired.
                    }
                    else
                    {
                        if (CheckWeaken(from, skill, clothing.HitPoints, clothing.MaxHitPoints))
                        {
                            clothing.MaxHitPoints -= toWeaken;
                            clothing.HitPoints = Math.Max(0, clothing.HitPoints - toWeaken);
                        }

                        if (CheckRepairDifficulty(from, skill, clothing.HitPoints, clothing.MaxHitPoints))
                        {
                            number = 1044279; // You repair the item.
                            m_CraftSystem.PlayCraftEffect(from);
                            clothing.HitPoints = clothing.MaxHitPoints;
                        }
                        else
                        {
                            if (m_Contract != null)
                            {
                                // You fail to repair the item and the repair contract is destroyed.
                                number = 1061137;
                            }
                            else
                            {
                                // You fail to repair the item.
                                number = 1044280;
                            }

                            m_CraftSystem.PlayCraftEffect(from);
                        }

                        if (m_Contract != null)
                        {
                            m_Contract.Delete();
                        }
                            /*
                        else
                        {
                            number = (usingDeed) ? 1061137 : 1044280; // You fail to repair the item. [And the contract is destroyed]
                            m_CraftSystem.PlayCraftEffect(from);
                        }

                        toDelete = true;
                             */
                    }
                }
                else if (targeted is Item)
                {
                    number = 1044283; // You cannot repair that.

                    SkillName skill = m_CraftSystem.MainSkill;

                    double value = from.Skills[skill].Value;

                    if (targeted is BlankScroll)
                    {
                        Item contract = null;

                        if (value < 50.0)
                        {
                            // You must be at least apprentice level to create a repair service contract.
                            number = 1047005;
                        }
                        else
                        {
                            if (skill == SkillName.Blacksmith)
                            {
                                contract = new RepairContract(ContractType.Blacksmith, value, from.Name);
                            }
                            else if (skill == SkillName.Carpentry)
                            {
                                contract = new RepairContract(ContractType.Carpenter, value, from.Name);
                            }
                            else if (skill == SkillName.Fletching)
                            {
                                contract = new RepairContract(ContractType.Fletcher, value, from.Name);
                            }
                            else if (skill == SkillName.Tailoring)
                            {
                                contract = new RepairContract(ContractType.Tailor, value, from.Name);
                            }
                            else if (skill == SkillName.Tinkering)
                            {
                                contract = new RepairContract(ContractType.Tinker, value, from.Name);
                            }
                        }

                        if (contract != null)
                        {
                            from.AddToBackpack(contract);

                            number = 1044154; // You create the item.

                            BlankScroll scroll = targeted as BlankScroll;

                            if (scroll.Amount >= 2)
                            {
                                scroll.Amount -= 1;
                            }
                            else
                            {
                                scroll.Delete();
                            }
                        }
                    }
                }
                else
                {
                    number = 500426; // You can't repair that.
                }

                if (m_Tool != null)
                {
                    //CraftContext context = m_CraftSystem.GetContext( from );

                    from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, number));
                }
                else
                {
                    from.SendLocalizedMessage(number);
                }
                    /*
                else if (!usingDeed && targeted is BlankScroll)
                {
                    SkillName skill = m_CraftSystem.MainSkill;

                    if (from.Skills[skill].Value >= 50.0)
                    {
                        ((BlankScroll)targeted).Consume(1);
                        RepairDeed deed = new RepairDeed(RepairDeed.GetTypeFor(m_CraftSystem), from.Skills[skill].Value, from);
                        from.AddToBackpack(deed);

                        number = 500442; // You create the item and put it in your backpack.
                    }
                    else
                        number = 1047005; // You must be at least apprentice level to create a repair service contract.
                }
                else if (targeted is Item)
                {
                    number = (usingDeed) ? 1061136 : 1044277; // That item cannot be repaired. // You cannot repair that item with this type of repair contract.
                }
                else
                {
                    number = 500426; // You can't repair that.
                }

                if (!usingDeed)
                {
                    CraftContext context = m_CraftSystem.GetContext(from);
                    from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, number));
                }
                else
                {
                    from.SendLocalizedMessage(number);

                    if (toDelete)
                        m_Deed.Delete();
                }
                     */
            }
        }

        public static bool AllowsRepair(Item item, CraftSystem system)
        {
            if (item == null)
                return false;
            // Implement IRepairable checks here if need be. Heads up, system could be null so do null checks! 

            return ((item is BaseArmor && ((BaseArmor)item).CanRepair) ||
                    (item is BaseWeapon && ((BaseWeapon)item).CanRepair) ||
                    (item is BaseClothing && ((BaseClothing)item).CanRepair) ||
                    (item is BaseJewel && ((BaseJewel)item).CanRepair));
        }
    }
}