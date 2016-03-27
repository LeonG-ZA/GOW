using System;
using Server.Items;
using Server.Targeting;

namespace Server.Engines.Craft
{
    public class Resmelt
    {
        public static void Do(Mobile from, CraftSystem craftSystem, IUsesRemaining tool)
        {
            int num = craftSystem.CanCraft(from, tool, null);

            if (num > 0 && num != 1044267)
            {
                from.SendGump(new CraftGump(from, craftSystem, tool, num));
            }
            else
            {
                from.Target = new ResmeltTarget(craftSystem, tool);
                from.SendLocalizedMessage(1044273); // Target an item to recycle.
            }
        }

        private class ResmeltTarget : Target
        {
            private readonly CraftSystem m_CraftSystem;
            private readonly IUsesRemaining m_Tool;

            public ResmeltTarget(CraftSystem craftSystem, IUsesRemaining tool)
                : base(2, false, TargetFlags.None)
            {
                m_CraftSystem = craftSystem;
                m_Tool = tool;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                int num = this.m_CraftSystem.CanCraft(from, this.m_Tool, null);

                if (num > 0)
                {
                    if (num == 1044267)
                    {
                        bool anvil, forge;

                        DefBlacksmithy.CheckAnvilAndForge(from, 2, out anvil, out forge);

                        if (!anvil)
                        {
                            num = 1044266; // You must be near an anvil
                        }
                        else if (!forge)
                        {
                            num = 1044265; // You must be near a forge.
                        }
                    }

                    from.SendGump(new CraftGump(from, this.m_CraftSystem, this.m_Tool, num));
                }
                else
                {
                    bool success = false;
                    bool isStoreBought = false;
                    bool lackMining = false;

                    Process(m_CraftSystem, from, targeted, true, out success, out isStoreBought, out lackMining);

                    if (lackMining)
                    {
                        from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, 1044269)); // You have no idea how to work this metal.
                        return;
                    }

                    if (success)
                    {
                        // You melt the item down into ingots.
                        from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, isStoreBought ? 500418 : 1044270));
                    }
                    else
                    {
                        // You can't melt that down into ingots.
                        from.SendGump(new CraftGump(from, m_CraftSystem, m_Tool, 1044272));
                    }
                }
            }
        }

        public static void Process(CraftSystem system, Mobile from, object targeted, bool playSound, out bool success, out bool isStoreBought, out bool lackMining)
        {
            success = false;
            isStoreBought = false;
            lackMining = false;

            if (targeted is BaseArmor)
            {
                success = DoResmelt(system, from, (BaseArmor)targeted, ((BaseArmor)targeted).Resource, playSound);
                isStoreBought = !((BaseArmor)targeted).PlayerConstructed;
                lackMining = !(CheckResourceSkill(((BaseArmor)targeted).Resource, from.Skills[SkillName.Mining].Value));
            }
            else if (targeted is BaseWeapon)
            {
                success = DoResmelt(system, from, (BaseWeapon)targeted, ((BaseWeapon)targeted).Resource, playSound);
                isStoreBought = !((BaseWeapon)targeted).PlayerConstructed;
                lackMining = !(CheckResourceSkill(((BaseWeapon)targeted).Resource, from.Skills[SkillName.Mining].Value));
            }
            else if (targeted is DragonBardingDeed)
            {
                success = DoResmelt(system, from, (DragonBardingDeed)targeted, ((DragonBardingDeed)targeted).Resource, playSound);
                isStoreBought = false;
                lackMining = false;
            }
        }

        private static bool CheckResourceSkill(CraftResource resource, double skill)
        {
            double reqSkill = 0;

            switch (resource)
            {
                case CraftResource.DullCopper: reqSkill = 65.0; break;
                case CraftResource.ShadowIron: reqSkill = 70.0; break;
                case CraftResource.Copper: reqSkill = 75.0; break;
                case CraftResource.Bronze: reqSkill = 80.0; break;
                case CraftResource.Gold: reqSkill = 85.0; break;
                case CraftResource.Agapite: reqSkill = 90.0; break;
                case CraftResource.Verite: reqSkill = 95.0; break;
                case CraftResource.Valorite: reqSkill = 99.0; break;
            }

            return (skill >= reqSkill);
        }

        private static bool DoResmelt(CraftSystem system, Mobile from, Item item, CraftResource resource, bool playSound)
        {
            try
            {
                if (Ethics.Ethic.IsImbued(item))
                {
                    return false;
                }

                if (CraftResources.GetType(resource) != CraftResourceType.Metal)
                {
                    return false;
                }

                CraftResourceInfo info = CraftResources.GetInfo(resource);

                if (info == null || info.ResourceTypes.Length == 0)
                {
                    return false;
                }

                if (!CheckResourceSkill(info.Resource, from.Skills[SkillName.Mining].Value))
                {
                    return false;
                }

                CraftItem craftItem = system.CraftItems.SearchFor(item.GetType());

                if (craftItem == null || craftItem.Resources.Count == 0)
                {
                    return false;
                }

                CraftRes craftResource = craftItem.Resources.GetAt(0);

                if (craftResource.Amount < 2)
                {
                    return false;
                }

                Type resourceType = info.ResourceTypes[0];
                Item ingot = (Item)Activator.CreateInstance(resourceType);

                if (item is DragonBardingDeed || (item is BaseArmor && ((BaseArmor)item).PlayerConstructed) || (item is BaseWeapon && ((BaseWeapon)item).PlayerConstructed) || (item is BaseClothing && ((BaseClothing)item).PlayerConstructed))
                {
                    ingot.Amount = craftResource.Amount / 2;
                }
                else
                {
                    ingot.Amount = 1;
                }

                item.Delete();
                from.AddToBackpack(ingot);

                if (playSound)
                {
                    from.PlaySound(0x2A);
                    from.PlaySound(0x240);
                }
                return true;
            }
            catch
            {
            }
            return false;
        }
    }
}