using System;
using Server.Items;

namespace Server.Engines.Craft
{
    public class DefAlchemy : CraftSystem
    {
        public override SkillName MainSkill
        {
            get
            {
                return SkillName.Alchemy;
            }
        }

        public override int GumpTitleNumber
        {
            get
            {
                return 1044001;
            }// <CENTER>ALCHEMY MENU</CENTER>
        }

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                {
                    m_CraftSystem = new DefAlchemy();
                }

                return m_CraftSystem;
            }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.0; // 0%
        }

        private DefAlchemy()
            : base(1, 1, 1.25)// base( 1, 1, 3.1 )
        {
        }

        /*
        public override int CanCraft(Mobile from, BaseTool tool, Type itemType)
        {
            if (tool == null || tool.Deleted || tool.UsesRemaining < 0)
                return 1044038; // You have worn out your tool!
            else if (!BaseTool.CheckAccessible(tool, from))
                return 1044263; // The tool must be on your person to use.

            return 0;
        }
         */
        public override int CanCraft(Mobile from, IUsesRemaining tool, Type itemType)
        {
            if (tool == null || ((Item)tool).Deleted || tool.UsesRemaining < 0)
                return 1044038; // You have worn out your tool!
            else if (!BaseTool.CheckAccessible((BaseTool)tool, from))
                return 1044263; // The tool must be on your person to use.

            return 0;
        }

        public override void PlayCraftEffect(Mobile from)
        {
            from.PlaySound(0x242);
        }

        private static readonly Type typeofPotion = typeof(BasePotion);

        public static bool IsPotion(Type type)
        {
            return typeofPotion.IsAssignableFrom(type);
        }

        public override int PlayEndingEffect(Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item)
        {
            if (toolBroken)
            {
                from.SendLocalizedMessage(1044038); // You have worn out your tool
            }

            if (failed)
            {
                if (IsPotion(item.ItemType))
                {
                    from.AddToBackpack(new Bottle());
                    return 500287; // You fail to create a useful potion.
                }
                else
                {
                    return 1044043; // You failed to create the item, and some of your materials are lost.
                }
            }
            else
            {
                from.PlaySound(0x240); // Sound of a filling bottle

                if (IsPotion(item.ItemType))
                {
                    if (quality == -1)
                    {
                        return 1048136; // You create the potion and pour it into a keg.
                    }
                    else
                    {
                        return 500279; // You pour the potion into a bottle...
                    }
                }
                else
                {
                    return 1044154; // You create the item.
                }
            }
        }

        public override void InitCraftList()
        {
            int index = -1;

            #region HealingAndCurative

            index = this.AddCraft(typeof(RefreshPotion), 1116348, 1044538, -25.0, 25.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(BlackPearl), 1044353, 1, 1042081);
            if (Core.SA)
            {
                index = this.AddCraft(typeof(GreaterRefreshPotion), 1116348, 1041327, 25.0, 75.0, typeof(Bottle), 1044529, 1, 1044558);
                this.AddRes(index, typeof(BlackPearl), 1044353, 5, 1042081);
            }
            else
            {
                index = this.AddCraft(typeof(TotalRefreshPotion), 1116348, 1044539, 25.0, 75.0, typeof(Bottle), 1044529, 1, 1044558);
                this.AddRes(index, typeof(BlackPearl), 1044353, 5, 1042081);
            }
            index = this.AddCraft(typeof(LesserHealPotion), 1116348, 1044543, -25.0, 25.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Ginseng), 1044356, 1, 1042081);
            index = this.AddCraft(typeof(HealPotion), 1116348, 1044544, 15.0, 65.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Ginseng), 1044356, 3, 1042081);
            index = this.AddCraft(typeof(GreaterHealPotion), 1116348, 1044545, 55.0, 105.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Ginseng), 1044356, 7, 1042081);
            index = this.AddCraft(typeof(LesserCurePotion), 1116348, 1044552, -10.0, 40.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Garlic), 1044355, 1, 1042081);
            index = this.AddCraft(typeof(CurePotion), 1116348, 1044553, 25.0, 75.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Garlic), 1044355, 3, 1042081);
            index = this.AddCraft(typeof(GreaterCurePotion), 1116348, 1044554, 65.0, 115.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Garlic), 1044355, 6, 1042081);
            index = this.AddCraft(typeof(ElixirOfRebirth), 1116348, 1112762, 65.0, 115.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(MedusaBlood), 1031702, 1, 1044253);
            this.AddRes(index, typeof(SpidersSilk), 1044360, 3, 1042081);
            this.SetNeededExpansion(index, Expansion.SA);

            #endregion

            #region Enhancement

            index = AddCraft(typeof(AgilityPotion), 1116349, 1044540, 15.0, 65.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Bloodmoss), 1044354, 1, 1042081);

            index = AddCraft(typeof(GreaterAgilityPotion), 1116349, 1044541, 35.0, 85.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Bloodmoss), 1044354, 3, 1042081);

            index = AddCraft(typeof(NightSightPotion), 1116349, 1044542, -25.0, 25.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(SpidersSilk), 1044360, 1, 1042081);

            index = AddCraft(typeof(StrengthPotion), 1116349, 1044546, 25.0, 75.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(MandrakeRoot), 1044357, 2, 1042081);

            index = AddCraft(typeof(GreaterStrengthPotion), 1116349, 1044547, 45.0, 95.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(MandrakeRoot), 1044357, 5, 1042081);

            index = AddCraft(typeof(InvisibilityPotion), 1116349, 1074860, 65.0, 115.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Bloodmoss), 1044354, 4, 1042081);
            this.AddRes(index, typeof(Nightshade), 1044358, 3, 1042081);
            this.SetNeededExpansion(index, Expansion.ML);

            #endregion

            #region Toxic

            index = AddCraft(typeof(LesserPoisonPotion), 1116350, 1044548, -5.0, 45.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Nightshade), 1044358, 1, 1042081);

            index = AddCraft(typeof(PoisonPotion), 1116350, 1044549, 15.0, 65.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Nightshade), 1044358, 2, 1042081);

            index = AddCraft(typeof(GreaterPoisonPotion), 1116350, 1044550, 55.0, 105.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Nightshade), 1044358, 4, 1042081);

            index = AddCraft(typeof(DeadlyPoisonPotion), 1116350, 1044551, 90.0, 140.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(Nightshade), 1044358, 8, 1042081);

            index = AddCraft(typeof(ParasiticPotion), 1116350, 1072942, 65.0, 115.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(ParasiticPlant), 1073474, 5, 1042081);
            this.SetNeededExpansion(index, Expansion.ML);

            index = AddCraft(typeof(DarkglowPotion), 1116350, 1072943, 65.0, 115.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(LuminescentFungi), 1073475, 5, 1042081);
            this.SetNeededExpansion(index, Expansion.ML);

            index = AddCraft(typeof(ScouringToxin), 1116350, 1112292, 75.0, 100.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(ToxicVenomSac), 1112291, 1, 1044253);
            this.SetNeededExpansion(index, Expansion.SA);

            #endregion

            #region Explosive

            index = AddCraft(typeof(LesserExplosionPotion), 1116351, 1044555, 5.0, 55.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(SulfurousAsh), 1044359, 3, 1042081);

            index = AddCraft(typeof(ExplosionPotion), 1116351, 1044556, 35.0, 85.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(SulfurousAsh), 1044359, 5, 1042081);

            index = AddCraft(typeof(GreaterExplosionPotion), 1116351, 1044557, 65.0, 115.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(SulfurousAsh), 1044359, 10, 1042081);

            index = AddCraft(typeof(ConflagrationPotion), 1116351, 1072096, 55.0, 105.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(GraveDust), 1023983, 5, 1042081);
            this.SetNeededExpansion(index, Expansion.SE);

            index = AddCraft(typeof(GreaterConflagrationPotion), 1116351, 1072099, 70.0, 120.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(GraveDust), 1023983, 10, 1042081);
            this.SetNeededExpansion(index, Expansion.SE);

            index = AddCraft(typeof(ConfusionBlastPotion), 1116351, 1072106, 50.0, 100.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(PigIron), 1023978, 5, 1042081);
            this.SetNeededExpansion(index, Expansion.SE);

            index = AddCraft(typeof(GreaterConfusionBlastPotion), 1116351, 1072109, 65.0, 115.0, typeof(Bottle), 1044529, 1, 1044558);
            this.AddRes(index, typeof(PigIron), 1023978, 10, 1042081);
            this.SetNeededExpansion(index, Expansion.SE);

            // TODO (HS)
            // 161 1095826 black powder
            // 162 1116304 match cord
            // 163 1116305 fuse cord

            #endregion

            #region Strange Brew

            index = AddCraft(typeof(SmokeBomb), 1116353, 1030248, 90.0, 120.0, typeof(Eggs), 1044477, 1, 1044253);
            this.AddRes(index, typeof(Ginseng), 1044356, 3, 1044364);
            this.SetNeededExpansion(index, Expansion.SE);

            index = AddCraft(typeof(HoveringWisp), 1116353, 1072881, 75.0, 125.0, typeof(CapturedEssence), 1032686, 4, 1042081);
            this.SetNeededExpansion(index, Expansion.ML);

            index = AddCraft(typeof(NaturalDye), 1116353, 1112136, 75.0, 100.0, typeof(PlantPigment), 1112132, 1, 1044253);
            this.AddRes(index, typeof(ColorFixative), 1112135, 1, 1044253);
            this.SetNeededExpansion(index, Expansion.SA);

            #endregion

            #region Ingredients

            index = AddCraft(typeof(PlantPigment), 1044495, 1112132, 75.0, 100.0, typeof(PlantClippings), 1112131, 1, 1044253);
            this.AddRes(index, typeof(Bottle), 1044250, 1, 1044253);
            this.SetNeededExpansion(index, Expansion.SA);

            index = AddCraft(typeof(ColorFixative), 1044495, 1112135, 75.0, 100.0, typeof(SilverSerpentVenom), 1112173, 1, 1044253);
            this.AddRes(index, typeof(BaseBeverage), 1022503, 1, 1044253);
            //this.BeverageType = BeverageType.Wine;
            this.SetNeededExpansion(index, Expansion.SA);

            index = AddCraft(typeof(CrystalGranules), 1044495, 1112329, 75.0, 100.0, typeof(ShimmeringCrystals), 1075095, 1, 1044253);
            this.SetNeededExpansion(index, Expansion.SA);

            //index = AddCraft(typeof(CrystalDust), 1044495, 1112328, 75.0, 100.0, typeof(ICrystal), 1074261, 4, 1044253);
            //this.SetNeededExpansion(index, Expansion.SA);

            index = AddCraft(typeof(SoftenedReeds), 1044495, 1112249, 75.0, 100.0, typeof(DryReeds), 1112248, 2, 1112250);
            this.AddRes(index, typeof(ScouringToxin), 1112292, 1, 1112326);
            this.SetNeededExpansion(index, Expansion.SA);

            index = AddCraft(typeof(VialOfVitriol), 1044495, 1113331, 90.0, 115.0, typeof(ParasiticPotion), 1072942, 1, 1044253);
            this.AddRes(index, typeof(Nightshade), 1044358, 30, 1044366);
            this.AddSkill(index, SkillName.Magery, 75.0, 80.0);
            this.SetNeededExpansion(index, Expansion.SA);

            index = AddCraft(typeof(BottleIchor), 1044495, 1113361, 90.0, 115.0, typeof(DarkglowPotion), 1072943, 1, 1044253);
            this.AddRes(index, typeof(SpidersSilk), 1044360, 30, 1044368);
            this.AddSkill(index, SkillName.Magery, 75.0, 80.0);
            this.SetNeededExpansion(index, Expansion.SA);

            if (Core.HS)
            {
                index = AddCraft(typeof(Potash), 1044495, 1116319, 0.0, 50.0, typeof(Board), 1044041, 1, 1044253);
                this.AddRes(index, typeof(BaseBeverage), 1046458, 1, 1044253);
                //this.BeverageType = BeverageType.Water;
                this.SetNeededExpansion(index, Expansion.HS);
            }

            #endregion
            /*
            if(Core.HS)
            {
                index = this.AddCraft(typeof(GoldDust), 1098336, 1098337, 90.0, 120.0, typeof(Gold), 3000083, 1000, 1150747);
                this.ForceNonExceptional(index);
                this.SetNeededExpansion(index, Expansion.HS);

                index = this.AddCraft(typeof(NexusCore), 1098336, 1153501, 90.0, 120.0, typeof(MandrakeRoot), 1015013, 10, 1044253);
                AddRes(index, typeof(SpidersSilk), 1015007, 10, 1044253);
                AddRes(index, typeof(DarkSapphire), 1032690, 5, 1044253);
                AddRes(index, typeof(CrushedGlass), 1113351, 5, 1044253);
                this.ForceNonExceptional(index);
                this.SetNeededExpansion(index, Expansion.HS);
            }
             */
        }
    }
}