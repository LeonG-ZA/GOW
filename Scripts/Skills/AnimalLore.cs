using System;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

namespace Server.SkillHandlers
{
    public class AnimalLore
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.AnimalLore].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.Target = new AnimalLoreTarget();

            m.SendLocalizedMessage(500328); // What animal should I look at?

            return TimeSpan.FromSeconds(1.0);
        }

        private class AnimalLoreTarget : Target
        {
            public AnimalLoreTarget()
                : base(8, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!from.Alive)
                {
                    from.SendLocalizedMessage(500331); // The spirits of the dead are not the province of animal lore.
                }
                else if (targeted is BaseCreature)
                {
                    BaseCreature bc = (BaseCreature)targeted;

                    if (!bc.IsDeadPet)
                    {
                        if (bc.Body.IsAnimal || bc.Body.IsMonster || bc.Body.IsSea)
                        {
                            if (!bc.Controlled && from.Skills[SkillName.AnimalLore].Value < 100.0)
                            {
                                from.SendLocalizedMessage(1049674); // At your skill level, you can only lore tamed creatures.
                            }
                            else if (!bc.Controlled && !bc.Tamable && from.Skills[SkillName.AnimalLore].Value < 110.0)
                            {
                                from.SendLocalizedMessage(1049675); // At your skill level, you can only lore tamed or tameable creatures.
                            }
                            else if (!from.CheckTargetSkill(SkillName.AnimalLore, bc, 0.0, 120.0))
                            {
                                from.SendLocalizedMessage(500334); // You can't think of anything you know offhand.
                            }
                            else if ((bc is LadyMelisande || bc is DreadHorn || bc is Travesty || bc is MonstrousInterredGrizzle || bc is ChiefParoxysmus || bc is ShimmeringEffusion) && 0.01 < Utility.RandomDouble())
                            {
                                from.SendLocalizedMessage(500334); // You can't think of anything you know offhand.
                            }
                            else
                            {
                                from.CloseGump(typeof(AnimalLoreGump));
                                from.SendGump(new AnimalLoreGump(bc));
                            }
                        }
                        else
                        {
                            from.SendLocalizedMessage(500329); // That's not an animal!
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(500331); // The spirits of the dead are not the province of animal lore.
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500329); // That's not an animal!
                }
            }
        }
    }

    public class AnimalLoreGump : Gump
    {
        private static string FormatSkill(BaseCreature bc, SkillName name)
        {
            Skill skill = bc.Skills[name];

            if (skill.Base < 10.0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0:F1}</div>", skill.Value);
        }

        private static string FormatAttributes(int cur, int max)
        {
            if (max == 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0}/{1}</div>", cur, max);
        }

        private static string FormatStat(int val)
        {
            if (val == 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0}</div>", val);
        }

        private static string FormatDouble(double val)
        {
            if (val == 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0:F1}</div>", val);
        }

        private static string FormatElement(int val)
        {
            if (val <= 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0}%</div>", val);
        }

        private static string FormatDamage(int min, int max)
        {
            if (min <= 0 || max <= 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0}-{1}</div>", min, max);
        }

        private const int LabelColor = 0x24E5;

        public AnimalLoreGump(BaseCreature bc)
            : base(250, 50)
        {
            // OSI sends TextString in a rare order, we need to use that order
            // or KR client will not show the gump correctly, so we will have
            // to intern the strings before build the gump.

            // 0 - Skills, Anatomy
            Intern(FormatSkill(bc, SkillName.Anatomy));

            // 1 - Skills, Healing & Poisoning
            if (bc is CuSidhe)
                Intern(FormatSkill(bc, SkillName.Healing));
            else
                Intern(FormatSkill(bc, SkillName.Poisoning));

            // 2 - Skills, Resisting Spells
            Intern(FormatSkill(bc, SkillName.MagicResist));

            // 3 - Skills, Tactics
            Intern(FormatSkill(bc, SkillName.Tactics));

            // 4 - Skills, Wrestling
            Intern(FormatSkill(bc, SkillName.Wrestling));

            // 5 - Skills, EvalInt
            Intern(FormatSkill(bc, SkillName.EvalInt));

            // 6 - Skills, Magery
            Intern(FormatSkill(bc, SkillName.Magery));

            // 7 - Skills, Meditation
            Intern(FormatSkill(bc, SkillName.Meditation));

            // 8 - Name
            Intern(String.Format("<center><i>{0}</i></center>", bc.Name));

            // 9 - Str
            Intern(FormatStat(bc.Str));

            // 10 - Hits
            Intern(FormatAttributes(bc.Hits, bc.HitsMax));

            // 11 - Dex
            Intern(FormatStat(bc.Dex));

            // 12 - Stam
            Intern(FormatAttributes(bc.Stam, bc.StamMax));

            // 13 - Int
            Intern(FormatStat(bc.Int));

            // 14 - Mana
            Intern(FormatAttributes(bc.Mana, bc.ManaMax));

            // 15 - Barding Difficulty			
            Intern(FormatDouble(bc.Uncalmable ? 0.0 : Items.BaseInstrument.GetBaseDifficulty(bc)));

            // 16 - Resistances - Physical
            Intern(FormatElement(bc.PhysicalResistance));

            // 17 - Resistances - Fire
            Intern(FormatElement(bc.FireResistance));

            // 18 - Resistances - Cold
            Intern(FormatElement(bc.ColdResistance));

            // 19 - Resistances - Poison
            Intern(FormatElement(bc.PoisonResistance));

            // 20 - Resistances - Energy
            Intern(FormatElement(bc.EnergyResistance));

            // 21 - Damage - Physical
            Intern(FormatElement(bc.PhysicalDamage));

            // 22 - Damage - Fire
            Intern(FormatElement(bc.FireDamage));

            // 23 - Damage - Cold
            Intern(FormatElement(bc.ColdDamage));

            // 24 - Damage - Poison
            Intern(FormatElement(bc.PoisonDamage));

            // 25 - Damage - Energy
            Intern(FormatElement(bc.EnergyDamage));

            // 26 - Base Damage Min-Max
            Intern(String.Format("{0}-{1}", bc.DamageMin, bc.DamageMax));

            AddPage(0);

            AddImage(100, 100, 2080);
            AddImage(118, 137, 2081);
            AddImage(118, 207, 2081);
            AddImage(118, 277, 2081);
            AddImage(118, 347, 2083);

            AddHtmlIntern(147, 108, 210, 18, 8, false, false);

            AddButton(240, 77, 2093, 2093, 2, GumpButtonType.Reply, 0);

            AddImage(140, 138, 2091);
            AddImage(140, 335, 2091);

            int pages = 5;
            int page = 0;

            #region Attributes
            AddPage(++page);

            AddImage(128, 152, 2086);
            AddHtmlLocalized(147, 150, 160, 18, 1049593, 200, false, false); // Attributes

            AddHtmlLocalized(153, 168, 160, 18, 1049578, LabelColor, false, false); // Hits
            AddHtmlIntern(280, 168, 75, 18, 10, false, false);

            AddHtmlLocalized(153, 186, 160, 18, 1049579, LabelColor, false, false); // Stamina
            AddHtmlIntern(280, 186, 75, 18, 12, false, false);

            AddHtmlLocalized(153, 204, 160, 18, 1049580, LabelColor, false, false); // Mana
            AddHtmlIntern(280, 204, 75, 18, 14, false, false);

            AddHtmlLocalized(153, 222, 160, 18, 1028335, LabelColor, false, false); // Strength
            AddHtmlIntern(320, 222, 35, 18, 9, false, false);

            AddHtmlLocalized(153, 240, 160, 18, 3000113, LabelColor, false, false); // Dexterity
            AddHtmlIntern(320, 240, 35, 18, 11, false, false);

            AddHtmlLocalized(153, 258, 160, 18, 3000112, LabelColor, false, false); // Intelligence
            AddHtmlIntern(320, 258, 35, 18, 13, false, false);

            int y = 276;

            AddHtmlLocalized(153, 276, 160, 18, 1070793, LabelColor, false, false); // Barding Difficulty
            AddHtmlIntern(320, y, 35, 18, 15, false, false);

            y += 18;

            AddImage(128, y + 2, 2086);
            AddHtmlLocalized(147, y, 160, 18, 1049594, 200, false, false); // Loyalty Rating
            y += 18;

            AddHtmlLocalized(153, y, 160, 18, (!bc.Controlled || bc.Loyalty == 0) ? 1061643 : 1049594 + (int)bc.Loyalty, LabelColor, false, false);

            AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
            AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, pages);
            #endregion

            #region Resistances
            AddPage(++page);

            AddImage(128, 152, 2086);
            AddHtmlLocalized(147, 150, 160, 18, 1061645, 200, false, false); // Resistances

            AddHtmlLocalized(153, 168, 160, 18, 1061646, LabelColor, false, false); // Physical
            AddHtmlIntern(320, 168, 35, 18, 16, false, false);

            AddHtmlLocalized(153, 186, 160, 18, 1061647, LabelColor, false, false); // Fire
            AddHtmlIntern(320, 186, 35, 18, 17, false, false);

            AddHtmlLocalized(153, 204, 160, 18, 1061648, LabelColor, false, false); // Cold
            AddHtmlIntern(320, 204, 35, 18, 18, false, false);

            AddHtmlLocalized(153, 222, 160, 18, 1061649, LabelColor, false, false); // Poison
            AddHtmlIntern(320, 222, 35, 18, 19, false, false);

            AddHtmlLocalized(153, 240, 160, 18, 1061650, LabelColor, false, false); // Energy
            AddHtmlIntern(320, 240, 35, 18, 20, false, false);

            AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
            AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            #endregion

            #region Damage
            AddPage(++page);

            AddImage(128, 152, 2086);
            AddHtmlLocalized(147, 150, 160, 18, 1017319, 200, false, false); // Damage

            AddHtmlLocalized(153, 168, 160, 18, 1061646, LabelColor, false, false); // Physical
            AddHtmlIntern(320, 168, 35, 18, 21, false, false);

            AddHtmlLocalized(153, 186, 160, 18, 1061647, LabelColor, false, false); // Fire
            AddHtmlIntern(320, 186, 35, 18, 22, false, false);

            AddHtmlLocalized(153, 204, 160, 18, 1061648, LabelColor, false, false); // Cold
            AddHtmlIntern(320, 204, 35, 18, 23, false, false);

            AddHtmlLocalized(153, 222, 160, 18, 1061649, LabelColor, false, false); // Poison
            AddHtmlIntern(320, 222, 35, 18, 24, false, false);

            AddHtmlLocalized(153, 240, 160, 18, 1061650, LabelColor, false, false); // Energy
            AddHtmlIntern(320, 240, 35, 18, 25, false, false);

            AddHtmlLocalized(153, 258, 160, 18, 1076750, LabelColor, false, false); // Base Damage
            AddHtmlIntern(320, 258, 38, 18, 26, false, false);

            AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
            AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            #endregion

            #region Skills
            AddPage(++page);

            AddImage(128, 152, 2086);
            AddHtmlLocalized(147, 150, 160, 18, 3001030, 200, false, false); // Combat Ratings

            AddHtmlLocalized(153, 168, 160, 18, 1044103, LabelColor, false, false); // Wrestling
            AddHtmlIntern(320, 168, 35, 18, 4, false, false);

            AddHtmlLocalized(153, 186, 160, 18, 1044087, LabelColor, false, false); // Tactics
            AddHtmlIntern(320, 186, 35, 18, 3, false, false);

            AddHtmlLocalized(153, 204, 160, 18, 1044086, LabelColor, false, false); // Magic Resistance
            AddHtmlIntern(320, 204, 35, 18, 2, false, false);

            AddHtmlLocalized(153, 222, 160, 18, 1044061, LabelColor, false, false); // Anatomy
            AddHtmlIntern(320, 222, 35, 18, 0, false, false);

            if (bc is CuSidhe)
                AddHtmlLocalized(153, 240, 160, 18, 1044077, LabelColor, false, false); // Healing
            else
                AddHtmlLocalized(153, 240, 160, 18, 1044090, LabelColor, false, false); // Poisoning
            AddHtmlIntern(320, 240, 35, 18, 1, false, false);

            AddImage(128, 260, 2086);
            AddHtmlLocalized(147, 258, 160, 18, 3001032, 200, false, false); // Lore & Knowledge

            AddHtmlLocalized(153, 276, 160, 18, 1044085, LabelColor, false, false); // Magery
            AddHtmlIntern(320, 276, 35, 18, 6, false, false);

            AddHtmlLocalized(153, 294, 160, 18, 1044076, LabelColor, false, false); // Evaluating Intelligence
            AddHtmlIntern(320, 294, 35, 18, 5, false, false);

            AddHtmlLocalized(153, 312, 160, 18, 1044106, LabelColor, false, false); // Meditation
            AddHtmlIntern(320, 312, 35, 18, 7, false, false);

            AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
            AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            #endregion

            #region Misc
            AddPage(++page);

            AddImage(128, 152, 2086);
            AddHtmlLocalized(147, 150, 160, 18, 1049563, 200, false, false); // Preferred Foods

            int foodPref = 3000340;

            if ((bc.FavoriteFood & FoodType.FruitsAndVegies) != 0)
                foodPref = 1049565; // Fruits and Vegetables
            else if ((bc.FavoriteFood & FoodType.GrainsAndHay) != 0)
                foodPref = 1049566; // Grains and Hay
            else if ((bc.FavoriteFood & FoodType.Fish) != 0)
                foodPref = 1049568; // Fish
            else if ((bc.FavoriteFood & FoodType.Meat) != 0)
                foodPref = 1049564; // Meat

            AddHtmlLocalized(153, 168, 160, 18, foodPref, LabelColor, false, false);

            AddImage(128, 188, 2086);
            AddHtmlLocalized(147, 186, 160, 18, 1049569, 200, false, false); // Pack Instincts

            int packInstinct = 3000340;

            if ((bc.PackInstinct & PackInstinct.Canine) != 0)
                packInstinct = 1049570; // Canine
            else if ((bc.PackInstinct & PackInstinct.Ostard) != 0)
                packInstinct = 1049571; // Ostard
            else if ((bc.PackInstinct & PackInstinct.Feline) != 0)
                packInstinct = 1049572; // Feline
            else if ((bc.PackInstinct & PackInstinct.Arachnid) != 0)
                packInstinct = 1049573; // Arachnid
            else if ((bc.PackInstinct & PackInstinct.Daemon) != 0)
                packInstinct = 1049574; // Daemon
            else if ((bc.PackInstinct & PackInstinct.Bear) != 0)
                packInstinct = 1049575; // Bear
            else if ((bc.PackInstinct & PackInstinct.Equine) != 0)
                packInstinct = 1049576; // Equine
            else if ((bc.PackInstinct & PackInstinct.Bull) != 0)
                packInstinct = 1049577; // Bull
            else if ((bc.PackInstinct & PackInstinct.Boura) != 0)
                packInstinct = 1112552; // Boura
            else if ((bc.PackInstinct & PackInstinct.Raptor) != 0)
                packInstinct = 1113442; // Raptor

            AddHtmlLocalized(153, 204, 160, 18, packInstinct, LabelColor, false, false);

            AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, 1);
            AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            #endregion
        }
    }
}
        #region old gump
        /*
        public AnimalLoreGump(BaseCreature bc)
            : base(250, 50)
        {
            this.AddPage(0);

            this.AddImage(100, 100, 2080);
            this.AddImage(118, 137, 2081);
            this.AddImage(118, 207, 2081);
            this.AddImage(118, 277, 2081);
            this.AddImage(118, 347, 2083);

            this.AddHtml(147, 108, 210, 18, String.Format("<center><i>{0}</i></center>", bc.Name), false, false);

            this.AddButton(240, 77, 2093, 2093, 2, GumpButtonType.Reply, 0);

            this.AddImage(140, 138, 2091);
            this.AddImage(140, 335, 2091);

            int pages = (Core.AOS ? 5 : 3);
            int page = 0;

            #region Attributes
            this.AddPage(++page);

            this.AddImage(128, 152, 2086);
            this.AddHtmlLocalized(147, 150, 160, 18, 1049593, 200, false, false); // Attributes

            this.AddHtmlLocalized(153, 168, 160, 18, 1049578, LabelColor, false, false); // Hits
            this.AddHtml(280, 168, 75, 18, FormatAttributes(bc.Hits, bc.HitsMax), false, false);

            this.AddHtmlLocalized(153, 186, 160, 18, 1049579, LabelColor, false, false); // Stamina
            this.AddHtml(280, 186, 75, 18, FormatAttributes(bc.Stam, bc.StamMax), false, false);

            this.AddHtmlLocalized(153, 204, 160, 18, 1049580, LabelColor, false, false); // Mana
            this.AddHtml(280, 204, 75, 18, FormatAttributes(bc.Mana, bc.ManaMax), false, false);

            this.AddHtmlLocalized(153, 222, 160, 18, 1028335, LabelColor, false, false); // Strength
            this.AddHtml(320, 222, 35, 18, FormatStat(bc.Str), false, false);

            this.AddHtmlLocalized(153, 240, 160, 18, 3000113, LabelColor, false, false); // Dexterity
            this.AddHtml(320, 240, 35, 18, FormatStat(bc.Dex), false, false);

            this.AddHtmlLocalized(153, 258, 160, 18, 3000112, LabelColor, false, false); // Intelligence
            this.AddHtml(320, 258, 35, 18, FormatStat(bc.Int), false, false);

            if (Core.AOS)
            {
                int y = 276;

                if (Core.SE)
                {
                    double bd = Items.BaseInstrument.GetBaseDifficulty(bc);
                    if (bc.Uncalmable)
                        bd = 0;

                    this.AddHtmlLocalized(153, 276, 160, 18, 1070793, LabelColor, false, false); // Barding Difficulty
                    this.AddHtml(320, y, 35, 18, FormatDouble(bd), false, false);

                    y += 18;
                }

                this.AddImage(128, y + 2, 2086);
                this.AddHtmlLocalized(147, y, 160, 18, 1049594, 200, false, false); // Loyalty Rating
                y += 18;

                this.AddHtmlLocalized(153, y, 160, 18, (!bc.Controlled || bc.Loyalty == 0) ? 1061643 : 1049595 + (bc.Loyalty / 10), LabelColor, false, false);
            }
            else
            {
                this.AddImage(128, 278, 2086);
                this.AddHtmlLocalized(147, 276, 160, 18, 3001016, 200, false, false); // Miscellaneous

                this.AddHtmlLocalized(153, 294, 160, 18, 1049581, LabelColor, false, false); // Armor Rating
                this.AddHtml(320, 294, 35, 18, FormatStat(bc.VirtualArmor), false, false);
            }

            this.AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
            this.AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, pages);
            #endregion

            #region Resistances
            if (Core.AOS)
            {
                this.AddPage(++page);

                this.AddImage(128, 152, 2086);
                this.AddHtmlLocalized(147, 150, 160, 18, 1061645, 200, false, false); // Resistances

                this.AddHtmlLocalized(153, 168, 160, 18, 1061646, LabelColor, false, false); // Physical
                this.AddHtml(320, 168, 35, 18, FormatElement(bc.PhysicalResistance), false, false);

                this.AddHtmlLocalized(153, 186, 160, 18, 1061647, LabelColor, false, false); // Fire
                this.AddHtml(320, 186, 35, 18, FormatElement(bc.FireResistance), false, false);

                this.AddHtmlLocalized(153, 204, 160, 18, 1061648, LabelColor, false, false); // Cold
                this.AddHtml(320, 204, 35, 18, FormatElement(bc.ColdResistance), false, false);

                this.AddHtmlLocalized(153, 222, 160, 18, 1061649, LabelColor, false, false); // Poison
                this.AddHtml(320, 222, 35, 18, FormatElement(bc.PoisonResistance), false, false);

                this.AddHtmlLocalized(153, 240, 160, 18, 1061650, LabelColor, false, false); // Energy
                this.AddHtml(320, 240, 35, 18, FormatElement(bc.EnergyResistance), false, false);

                this.AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
                this.AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            }
            #endregion

            #region Damage
            if (Core.AOS)
            {
                this.AddPage(++page);

                this.AddImage(128, 152, 2086);
                this.AddHtmlLocalized(147, 150, 160, 18, 1017319, 200, false, false); // Damage

                this.AddHtmlLocalized(153, 168, 160, 18, 1061646, LabelColor, false, false); // Physical
                this.AddHtml(320, 168, 35, 18, FormatElement(bc.PhysicalDamage), false, false);

                this.AddHtmlLocalized(153, 186, 160, 18, 1061647, LabelColor, false, false); // Fire
                this.AddHtml(320, 186, 35, 18, FormatElement(bc.FireDamage), false, false);

                this.AddHtmlLocalized(153, 204, 160, 18, 1061648, LabelColor, false, false); // Cold
                this.AddHtml(320, 204, 35, 18, FormatElement(bc.ColdDamage), false, false);

                this.AddHtmlLocalized(153, 222, 160, 18, 1061649, LabelColor, false, false); // Poison
                this.AddHtml(320, 222, 35, 18, FormatElement(bc.PoisonDamage), false, false);

                this.AddHtmlLocalized(153, 240, 160, 18, 1061650, LabelColor, false, false); // Energy
                this.AddHtml(320, 240, 35, 18, FormatElement(bc.EnergyDamage), false, false);

                if (Core.ML)
                {
                    this.AddHtmlLocalized(153, 258, 160, 18, 1076750, LabelColor, false, false); // Base Damage
                    this.AddHtml(300, 258, 55, 18, FormatDamage(bc.DamageMin, bc.DamageMax), false, false);
                }

                this.AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
                this.AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            }
            #endregion

            #region Skills
            this.AddPage(++page);

            this.AddImage(128, 152, 2086);
            this.AddHtmlLocalized(147, 150, 160, 18, 3001030, 200, false, false); // Combat Ratings

            this.AddHtmlLocalized(153, 168, 160, 18, 1044103, LabelColor, false, false); // Wrestling
            this.AddHtml(320, 168, 35, 18, FormatSkill(bc, SkillName.Wrestling), false, false);

            this.AddHtmlLocalized(153, 186, 160, 18, 1044087, LabelColor, false, false); // Tactics
            this.AddHtml(320, 186, 35, 18, FormatSkill(bc, SkillName.Tactics), false, false);

            this.AddHtmlLocalized(153, 204, 160, 18, 1044086, LabelColor, false, false); // Magic Resistance
            this.AddHtml(320, 204, 35, 18, FormatSkill(bc, SkillName.MagicResist), false, false);

            this.AddHtmlLocalized(153, 222, 160, 18, 1044061, LabelColor, false, false); // Anatomy
            this.AddHtml(320, 222, 35, 18, FormatSkill(bc, SkillName.Anatomy), false, false);

            if (bc is CuSidhe)
            {
                this.AddHtmlLocalized(153, 240, 160, 18, 1044077, LabelColor, false, false); // Healing
                this.AddHtml(320, 240, 35, 18, FormatSkill(bc, SkillName.Healing), false, false);
            }
            else
            {
                this.AddHtmlLocalized(153, 240, 160, 18, 1044090, LabelColor, false, false); // Poisoning
                this.AddHtml(320, 240, 35, 18, FormatSkill(bc, SkillName.Poisoning), false, false);
            }

            this.AddImage(128, 260, 2086);
            this.AddHtmlLocalized(147, 258, 160, 18, 3001032, 200, false, false); // Lore & Knowledge

            this.AddHtmlLocalized(153, 276, 160, 18, 1044085, LabelColor, false, false); // Magery
            this.AddHtml(320, 276, 35, 18, FormatSkill(bc, SkillName.Magery), false, false);

            this.AddHtmlLocalized(153, 294, 160, 18, 1044076, LabelColor, false, false); // Evaluating Intelligence
            this.AddHtml(320, 294, 35, 18, FormatSkill(bc, SkillName.EvalInt), false, false);

            this.AddHtmlLocalized(153, 312, 160, 18, 1044106, LabelColor, false, false); // Meditation
            this.AddHtml(320, 312, 35, 18, FormatSkill(bc, SkillName.Meditation), false, false);

            this.AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
            this.AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            #endregion

            #region Misc
            this.AddPage(++page);

            this.AddImage(128, 152, 2086);
            this.AddHtmlLocalized(147, 150, 160, 18, 1049563, 200, false, false); // Preferred Foods

            int foodPref = 3000340;

            if ((bc.FavoriteFood & FoodType.FruitsAndVegies) != 0)
                foodPref = 1049565; // Fruits and Vegetables
            else if ((bc.FavoriteFood & FoodType.GrainsAndHay) != 0)
                foodPref = 1049566; // Grains and Hay
            else if ((bc.FavoriteFood & FoodType.Fish) != 0)
                foodPref = 1049568; // Fish
            else if ((bc.FavoriteFood & FoodType.Meat) != 0)
                foodPref = 1049564; // Meat
            else if ((bc.FavoriteFood & FoodType.Eggs) != 0)
                foodPref = 1044477; // Eggs

            this.AddHtmlLocalized(153, 168, 160, 18, foodPref, LabelColor, false, false);

            this.AddImage(128, 188, 2086);
            this.AddHtmlLocalized(147, 186, 160, 18, 1049569, 200, false, false); // Pack Instincts

            int packInstinct = 3000340;

            if ((bc.PackInstinct & PackInstinct.Canine) != 0)
                packInstinct = 1049570; // Canine
            else if ((bc.PackInstinct & PackInstinct.Ostard) != 0)
                packInstinct = 1049571; // Ostard
            else if ((bc.PackInstinct & PackInstinct.Feline) != 0)
                packInstinct = 1049572; // Feline
            else if ((bc.PackInstinct & PackInstinct.Arachnid) != 0)
                packInstinct = 1049573; // Arachnid
            else if ((bc.PackInstinct & PackInstinct.Daemon) != 0)
                packInstinct = 1049574; // Daemon
            else if ((bc.PackInstinct & PackInstinct.Bear) != 0)
                packInstinct = 1049575; // Bear
            else if ((bc.PackInstinct & PackInstinct.Equine) != 0)
                packInstinct = 1049576; // Equine
            else if ((bc.PackInstinct & PackInstinct.Bull) != 0)
                packInstinct = 1049577; // Bull

            this.AddHtmlLocalized(153, 204, 160, 18, packInstinct, LabelColor, false, false);

            if (!Core.AOS)
            {
                this.AddImage(128, 224, 2086);
                this.AddHtmlLocalized(147, 222, 160, 18, 1049594, 200, false, false); // Loyalty Rating

                this.AddHtmlLocalized(153, 240, 160, 18, (!bc.Controlled || bc.Loyalty == 0) ? 1061643 : 1049595 + (bc.Loyalty / 10), LabelColor, false, false);
            }

            this.AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, 1);
            this.AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            #endregion
        }
         */
        #endregion