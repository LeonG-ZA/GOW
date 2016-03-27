using System;
using System.Text;
using Server.Commands;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Items.ImbuingResource;
using Server.TestCenterConfiguration;

namespace Server.Misc
{
    public class TestCenter
    {
        public static void Initialize()
        {
            if (TestCenterConfig.TestCenterEnabled)
            {
                AccountHandler.RestrictCharacterDeletion = false;
                EventSink.Speech += new SpeechEventHandler(EventSink_Speech);
                EventSink.CharacterCreated += new CharacterCreatedEventHandler(EventSink_CharacterCreated);
            }
        }

        private static void EventSink_Speech(SpeechEventArgs args)
        {
            if (!args.Handled)
            {
                if (Insensitive.StartsWith(args.Speech, "set"))
                {
                    Mobile from = args.Mobile;

                    string[] split = args.Speech.Split(' ');

                    if (split.Length == 3)
                    {
                        try
                        {
                            string name = split[1];
                            double value = Convert.ToDouble(split[2]);

                            if (Insensitive.Equals(name, "str"))
                            {
                                ChangeStrength(from, (int)value);
                            }
                            else if (Insensitive.Equals(name, "dex"))
                            {
                                ChangeDexterity(from, (int)value);
                            }
                            else if (Insensitive.Equals(name, "int"))
                            {
                                ChangeIntelligence(from, (int)value);
                            }
                            else
                            {
                                ChangeSkill(from, name, value);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                else if (Insensitive.Equals(args.Speech, "help"))
                {
                    args.Mobile.SendGump(new TCHelpGump());

                    args.Handled = true;
                }
            }
        }

        private static void EventSink_CharacterCreated(CharacterCreatedEventArgs args)
        {
            PlayerMobile pm = (PlayerMobile)args.Mobile;

            new WelcomeTimer(pm).Start();
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => { FillBankbox(pm); });
        }

        private static void ChangeStrength(Mobile from, int value)
        {
            if (value < 10 || value > 125)
            {
                from.SendLocalizedMessage(1005628); // Stats range between 10 and 125.
            }
            else
            {
                if ((value + from.RawDex + from.RawInt) > from.StatCap)
                {
                    from.SendLocalizedMessage(1005629); // You can not exceed the stat cap.  Try setting another stat lower first.
                }
                else
                {
                    from.RawStr = value;
                    from.SendLocalizedMessage(1005630); // Your stats have been adjusted.
                }
            }
        }

        private static void ChangeDexterity(Mobile from, int value)
        {
            if (value < 10 || value > 125)
            {
                from.SendLocalizedMessage(1005628); // Stats range between 10 and 125.
            }
            else
            {
                if ((from.RawStr + value + from.RawInt) > from.StatCap)
                {
                    from.SendLocalizedMessage(1005629); // You can not exceed the stat cap.  Try setting another stat lower first.
                }
                else
                {
                    from.RawDex = value;
                    from.SendLocalizedMessage(1005630); // Your stats have been adjusted.
                }
            }
        }

        private static void ChangeIntelligence(Mobile from, int value)
        {
            if (value < 10 || value > 125)
            {
                from.SendLocalizedMessage(1005628); // Stats range between 10 and 125.
            }
            else
            {
                if ((from.RawStr + from.RawDex + value) > from.StatCap)
                {
                    from.SendLocalizedMessage(1005629); // You can not exceed the stat cap.  Try setting another stat lower first.
                }
                else
                {
                    from.RawInt = value;
                    from.SendLocalizedMessage(1005630); // Your stats have been adjusted.
                }
            }
        }

        private static void ChangeSkill(Mobile from, string name, double value)
        {
            SkillName index;

            if (!Enum.TryParse(name, true, out index) || (!Core.SE && (int)index > 51) || (!Core.AOS && (int)index > 48))
            {
                from.SendLocalizedMessage(1005631); // You have specified an invalid skill to set.
                return;
            }

            Skill skill = from.Skills[index];

            if (skill != null)
            {
                if (value < 0 || value > skill.Cap)
                {
                    from.SendMessage(String.Format("Your skill in {0} is capped at {1:F1}.", skill.Info.Name, skill.Cap));
                }
                else
                {
                    int newFixedPoint = (int)(value * 10.0);
                    int oldFixedPoint = skill.BaseFixedPoint;

                    if (((skill.Owner.Total - oldFixedPoint) + newFixedPoint) > skill.Owner.Cap)
                    {
                        from.SendMessage("You can not exceed the skill cap.  Try setting another skill lower first.");
                    }
                    else
                    {
                        skill.BaseFixedPoint = newFixedPoint;
                    }
                }
            }
            else
            {
                from.SendLocalizedMessage(1005631); // You have specified an invalid skill to set.
            }
        }

        public class TCHelpGump : Gump
        {
            public TCHelpGump()
                : base(40, 40)
            {
                AddPage(0);
                AddBackground(0, 0, 160, 120, 5054);

                AddButton(10, 10, 0xFB7, 0xFB9, 1, GumpButtonType.Reply, 0);
                AddLabel(45, 10, 0x34, "Generation Of Worlds");

                AddButton(10, 35, 0xFB7, 0xFB9, 2, GumpButtonType.Reply, 0);
                AddLabel(45, 35, 0x34, "List of skills");

                AddButton(10, 60, 0xFB7, 0xFB9, 3, GumpButtonType.Reply, 0);
                AddLabel(45, 60, 0x34, "Command list");

                AddButton(10, 85, 0xFB1, 0xFB3, 0, GumpButtonType.Reply, 0);
                AddLabel(45, 85, 0x34, "Close");
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                switch ( info.ButtonID )
                {
                    case 1:
                        {
                            sender.LaunchBrowser("http://generationofworlds.mygamesonline.org/Forums/app.php/portal");
                            break;
                        }
                    case 2: // List of skills
                        {
                            string[] strings = Enum.GetNames(typeof(SkillName));

                            Array.Sort(strings);

                            StringBuilder sb = new StringBuilder();

                            if (strings.Length > 0)
                            {
                                sb.Append(strings[0]);
                            }

                            for (int i = 1; i < strings.Length; ++i)
                            {
                                string v = strings[i];

                                if ((sb.Length + 1 + v.Length) >= 256)
                                {
                                    sender.Send(new AsciiMessage(Server.Serial.MinusOne, -1, MessageType.Label, 0x35, 3, "System", sb.ToString()));
                                    sb = new StringBuilder();
                                    sb.Append(v);
                                }
                                else
                                {
                                    sb.Append(' ');
                                    sb.Append(v);
                                }
                            }

                            if (sb.Length > 0)
                            {
                                sender.Send(new AsciiMessage(Server.Serial.MinusOne, -1, MessageType.Label, 0x35, 3, "System", sb.ToString()));
                            }

                            break;
                        }
                    case 3: // Command list
                        {
                            sender.Mobile.SendAsciiMessage(0x482, "The command prefix is \"{0}\"", CommandSystem.Prefix);
                            CommandHandlers.Help_OnCommand(new CommandEventArgs(sender.Mobile, "help", "", new string[0]));

                            break;
                        }
                }
            }
        }
        #region Bank Box
        private static Item MakeNewbie(Item item)
        {
            if (!Core.AOS)
            {
                item.LootType = LootType.Newbied;
            }

            return item;
        }

        private static void PlaceItemIn(Container parent, int x, int y, Item item)
        {
            parent.AddItem(item);
            item.Location = new Point3D(x, y, 0);
        }

        private static Item MakePotionKeg(PotionEffect type, int hue)
        {
            PotionKeg keg = new PotionKeg();

            keg.Held = 100;
            keg.Type = type;
            keg.Hue = hue;

            return keg;
        }

        private static void AddPowerScrolls(BankBox bank)
        {
            Bag bag = new Bag();

            for (int i = 0; i < PowerScroll.Skills.Length; ++i)
            {
                bag.DropItem(new PowerScroll(PowerScroll.Skills[i], 120.0));
            }

            bag.DropItem(new StatCapScroll(250));

            bank.DropItem(bag);
        }

        private static void FillBankbox(Mobile m)
        {
            if (Core.TOL)
            {
                FillBankTOL(m);
                return;
            }

            BankBox bank = m.BankBox;

            bank.DropItem(new BankCheck(TestCenterConfig.UORBankCheck));

            // Full spellbook
            Spellbook book = new Spellbook();

            book.Content = ulong.MaxValue;

            bank.DropItem(book);

            Bag bag = new Bag();

            for (int i = 0; i < 5; ++i)
            {
                bag.DropItem(new Moonstone(MoonstoneType.Felucca));
            }

            // Felucca moonstones
            bank.DropItem(bag);

            bag = new Bag();

            for (int i = 0; i < 5; ++i)
            {
                bag.DropItem(new Moonstone(MoonstoneType.Trammel));
            }

            // Trammel moonstones
            bank.DropItem(bag);

            // Treasure maps
            bank.DropItem(new TreasureMap(1, Map.Trammel));
            bank.DropItem(new TreasureMap(2, Map.Trammel));
            bank.DropItem(new TreasureMap(3, Map.Trammel));
            bank.DropItem(new TreasureMap(4, Map.Trammel));
            bank.DropItem(new TreasureMap(5, Map.Trammel));

            // Bag containing 50 of each reagent
            bank.DropItem(new BagOfAllReagents(TestCenterConfig.UORBagOfAllReagents));

            // Craft tools
            bank.DropItem(MakeNewbie(new Scissors()));
            bank.DropItem(MakeNewbie(new SewingKit(TestCenterConfig.UORSewingKit)));
            bank.DropItem(MakeNewbie(new SmithHammer(TestCenterConfig.UORSmithHammer)));
            bank.DropItem(MakeNewbie(new FletcherTools(TestCenterConfig.UORFletcherTools)));
            bank.DropItem(MakeNewbie(new DovetailSaw(TestCenterConfig.UORDovetailSaw)));
            bank.DropItem(MakeNewbie(new MortarPestle(TestCenterConfig.UORMortarPestle)));
            bank.DropItem(MakeNewbie(new ScribesPen(TestCenterConfig.UORScribesPen)));
            bank.DropItem(MakeNewbie(new TinkerTools(TestCenterConfig.UORTinkerTools)));

            // A few dye tubs
            bank.DropItem(new Dyes());
            bank.DropItem(new DyeTub());
            bank.DropItem(new DyeTub());
            bank.DropItem(new BlackDyeTub());

            DyeTub darkRedTub = new DyeTub();

            darkRedTub.DyedHue = 0x485;
            darkRedTub.Redyable = false;

            bank.DropItem(darkRedTub);

            // Some food
            bank.DropItem(MakeNewbie(new Apple(TestCenterConfig.UORApple)));

            // Resources
            bank.DropItem(MakeNewbie(new Feather(TestCenterConfig.UORFeather)));
            bank.DropItem(MakeNewbie(new BoltOfCloth(TestCenterConfig.UORBoltOfCloth)));
            bank.DropItem(MakeNewbie(new BlankScroll(TestCenterConfig.UORBlankScroll)));
            bank.DropItem(MakeNewbie(new Hides(TestCenterConfig.UORHides)));
            bank.DropItem(MakeNewbie(new Bandage(TestCenterConfig.UORBandage)));
            bank.DropItem(MakeNewbie(new Bottle(TestCenterConfig.UORBottle)));
            bank.DropItem(MakeNewbie(new Log(TestCenterConfig.UORLog)));

            bank.DropItem(MakeNewbie(new IronIngot(TestCenterConfig.UORIronIngot)));
            bank.DropItem(MakeNewbie(new DullCopperIngot(TestCenterConfig.UORDullCopperIngot)));
            bank.DropItem(MakeNewbie(new ShadowIronIngot(TestCenterConfig.UORShadowIronIngot)));
            bank.DropItem(MakeNewbie(new CopperIngot(TestCenterConfig.UORCopperIngot)));
            bank.DropItem(MakeNewbie(new BronzeIngot(TestCenterConfig.UORBronzeIngot)));
            bank.DropItem(MakeNewbie(new GoldIngot(TestCenterConfig.UORGoldIngot)));
            bank.DropItem(MakeNewbie(new AgapiteIngot(TestCenterConfig.UORAgapiteIngot)));
            bank.DropItem(MakeNewbie(new VeriteIngot(TestCenterConfig.UORVeriteIngot)));
            bank.DropItem(MakeNewbie(new ValoriteIngot(TestCenterConfig.UORValoriteIngot)));

            // Reagents
            bank.DropItem(MakeNewbie(new BlackPearl(TestCenterConfig.UORBlackPearl)));
            bank.DropItem(MakeNewbie(new Bloodmoss(TestCenterConfig.UORBloodmoss)));
            bank.DropItem(MakeNewbie(new Garlic(TestCenterConfig.UORGarlic)));
            bank.DropItem(MakeNewbie(new Ginseng(TestCenterConfig.UORGinseng)));
            bank.DropItem(MakeNewbie(new MandrakeRoot(TestCenterConfig.UORMandrakeRoot)));
            bank.DropItem(MakeNewbie(new Nightshade(TestCenterConfig.UORNightshade)));
            bank.DropItem(MakeNewbie(new SulfurousAsh(TestCenterConfig.UORSulfurousAsh)));
            bank.DropItem(MakeNewbie(new SpidersSilk(TestCenterConfig.UORSpidersSilk)));

            // Some extra starting gold
            bank.DropItem(MakeNewbie(new Gold(TestCenterConfig.UORGold)));

            // 5 blank recall runes
            for (int i = 0; i < 5; ++i)
            {
                bank.DropItem(MakeNewbie(new RecallRune()));
            }

            AddPowerScrolls(bank);
        }

        private static void FillBankTOL(Mobile m)
        {
            BankBox bank = m.BankBox;

            if (bank == null)
            {
                return;
            }

            bank.MaxItems = int.MaxValue;

            // The new TOL bankboxes don't have powerscrolls, they are automatically 'applied':

            for (int i = 0; i < PowerScroll.Skills.Length; ++i)
            {
                m.Skills[PowerScroll.Skills[i]].Cap = 120.0;
            }

            Container cont;
            Container cont2;

            // Begin box of money
            cont = new WoodenBox();
            cont.ItemID = 0xE7D;
            cont.Hue = 0x489;

            PlaceItemIn(cont, 17, 52, new BankCheck(TestCenterConfig.BankCheck));
            PlaceItemIn(cont, 28, 52, new BankCheck(TestCenterConfig.BankCheck));

            PlaceItemIn(cont, 17, 91, new Factions.Silver(TestCenterConfig.Silver));
            PlaceItemIn(cont, 34, 91, new Gold(TestCenterConfig.GOLD));

            PlaceItemIn(bank, 38, 142, cont);
            // End box of money

            // Begin box of Magery Items
            cont = new WoodenBox();
            cont.ItemID = 0xE7D;
            cont.Name = "Magery Items";
            cont.Hue = 1195;

            // Begin bag of spell casting stuff
            cont2 = new Backpack();
            cont2.Hue = 0x480;
            cont2.Name = "Spell Casting Stuff";

            PlaceItemIn(cont2, 45, 107, new Spellbook(UInt64.MaxValue));
            PlaceItemIn(cont2, 64, 107, new NecromancerSpellbook((UInt64)0xFFFF));
            PlaceItemIn(cont2, 83, 107, new BookOfChivalry((UInt64)0x3FF));
            PlaceItemIn(cont2, 102, 107, new BookOfBushido()); //Default ctor = full
            PlaceItemIn(cont2, 121, 107, new BookOfNinjitsu()); //Default ctor = full
            PlaceItemIn(cont2, 102, 149, new SpellweavingSpellbook((UInt64)0xFFFF));
            PlaceItemIn(cont2, 121, 149, new MysticismSpellbook((UInt64)0xFFFF));

            Runebook runebook = new Runebook(20);
            runebook.CurCharges = runebook.MaxCharges;
            PlaceItemIn(cont2, 141, 107, runebook);

            Item toHue = new BagOfReagents(TestCenterConfig.BagOfReagents);
            toHue.Hue = 0x2D;
            PlaceItemIn(cont2, 45, 128, toHue);

            toHue = new BagOfNecroReagents(TestCenterConfig.BagOfNecroReagents);
            toHue.Hue = 0x488;
            PlaceItemIn(cont2, 64, 128, toHue);

            toHue = new BagOfMysticReagents(TestCenterConfig.BagOfMysticReagents);
            toHue.Hue = 0x48F;
            PlaceItemIn(cont2, 141, 128, toHue);

            for (int i = 0; i < 6; ++i)
            {
                PlaceItemIn(cont2, 45 + (i * 10), 74, new RecallRune());
            }

            PlaceItemIn(cont, 47, 91, cont2);
            // End bag of spell casting stuff

            // Begin bag of potion kegs
            cont2 = new Backpack();
            cont2.Name = "Various Potion Kegs";

            PlaceItemIn(cont2, 45, 149, MakePotionKeg(PotionEffect.CureGreater, 0x2D));
            PlaceItemIn(cont2, 69, 149, MakePotionKeg(PotionEffect.HealGreater, 0x499));
            PlaceItemIn(cont2, 93, 149, MakePotionKeg(PotionEffect.PoisonDeadly, 0x46));
            PlaceItemIn(cont2, 117, 149, MakePotionKeg(PotionEffect.RefreshTotal, 0x21));
            PlaceItemIn(cont2, 141, 149, MakePotionKeg(PotionEffect.ExplosionGreater, 0x74));

            PlaceItemIn(cont2, 93, 82, new Bottle(TestCenterConfig.Bottle));

            PlaceItemIn(cont, 78, 91, cont2);
            // End bag of potion kegs

            GoldRing ring = new GoldRing();
            ring.Name = "Ring Of Arcane Tactics";
            ring.Attributes.CastRecovery = 3;
            ring.Attributes.CastSpeed = 1;
            PlaceItemIn(cont, 109, 90, ring);

            GoldBracelet bracelet = new GoldBracelet();
            bracelet.Name = "Farmer's Band Of Mastery";
            bracelet.Attributes.CastRecovery = 3;
            bracelet.Attributes.CastSpeed = 1;
            PlaceItemIn(cont, 139, 95, bracelet);

            PlaceItemIn(bank, 63, 142, cont);
            // End box of Magery Items

            // Begin bag of ethereals
            cont = new Backpack();
            cont.Hue = 0x490;
            cont.Name = "Bag Of Ethy's!";

            cont.DropItem(new EtherealHorse());
            cont.DropItem(new EtherealOstard());
            cont.DropItem(new EtherealLlama());
            cont.DropItem(new EtherealKirin());
            cont.DropItem(new EtherealUnicorn());
            cont.DropItem(new EtherealRidgeback());
            cont.DropItem(new EtherealSwampDragon());
            cont.DropItem(new EtherealBeetle());

            PlaceItemIn(bank, 43, 124, cont);
            // End bag of ethereals

            // Begin box of Artifacts
            cont = new WoodenBox();
            cont.ItemID = 0xE7D;
            cont.Hue = 1170;
            cont.Name = "Artifacts";

            // Begin bag of minor artifacts
            cont2 = new Bag();
            cont2.Hue = 1167;
            cont2.Name = "Minor Artifacts";

            cont2.DropItem(new LunaLance());
            cont2.DropItem(new VioletCourage());
            cont2.DropItem(new CavortingClub());
            cont2.DropItem(new CaptainQuacklebushsCutlass());
            cont2.DropItem(new NightsKiss());
            cont2.DropItem(new ShipModelOfTheHMSCape());
            cont2.DropItem(new AdmiralHeartyRum());
            cont2.DropItem(new CandelabraOfSouls());
            cont2.DropItem(new IolosLute());
            cont2.DropItem(new GwennosHarp());
            cont2.DropItem(new ArcticDeathDealer());
            cont2.DropItem(new EnchantedTitanLegBone());
            cont2.DropItem(new NoxRangersHeavyCrossbow());
            cont2.DropItem(new BlazeOfDeath());
            cont2.DropItem(new DreadPirateHat());
            cont2.DropItem(new BurglarsBandana());
            cont2.DropItem(new GoldBricks());
            cont2.DropItem(new AlchemistsBauble());
            cont2.DropItem(new PhillipsWoodenSteed());
            cont2.DropItem(new PolarBearMask());
            cont2.DropItem(new BowOfTheJukaKing());
            cont2.DropItem(new GlovesOfThePugilist());
            cont2.DropItem(new OrcishVisage());
            cont2.DropItem(new StaffOfPower());
            cont2.DropItem(new ShieldOfInvulnerability());
            cont2.DropItem(new HeartOfTheLion());
            cont2.DropItem(new ColdBlood());
            cont2.DropItem(new GhostShipAnchor());
            cont2.DropItem(new SeahorseStatuette());
            cont2.DropItem(new WrathOfTheDryad());
            cont2.DropItem(new PixieSwatter());

            PlaceItemIn(cont, 17, 83, cont2);
            // End bag of minor artifacts

            // Begin Bag of Major Artifacts
            cont2 = new Bag();
            cont2.Hue = 1266;
            cont2.Name = "Major Artifacts";

            cont2.DropItem(new GauntletsOfNobility());
            cont2.DropItem(new MidnightBracers());
            cont2.DropItem(new VoiceOfTheFallenKing());
            cont2.DropItem(new OrnateCrownOfTheHarrower());
            cont2.DropItem(new HelmOfInsight());
            cont2.DropItem(new HolyKnightsBreastplate());
            cont2.DropItem(new ArmorOfFortune());
            cont2.DropItem(new TunicOfFire());
            cont2.DropItem(new LeggingsOfBane());
            cont2.DropItem(new ArcaneShield());
            cont2.DropItem(new Aegis());
            cont2.DropItem(new RingOfTheVile());
            cont2.DropItem(new BraceletOfHealth());
            cont2.DropItem(new RingOfTheElements());
            cont2.DropItem(new OrnamentOfTheMagician());
            cont2.DropItem(new DivineCountenance());
            cont2.DropItem(new JackalsCollar());
            cont2.DropItem(new HuntersHeaddress());
            cont2.DropItem(new HatOfTheMagi());
            cont2.DropItem(new ShadowDancerLeggings());
            cont2.DropItem(new SpiritOfTheTotem());
            cont2.DropItem(new BladeOfInsanity());
            cont2.DropItem(new AxeOfTheHeavens());
            cont2.DropItem(new TheBeserkersMaul());
            cont2.DropItem(new Frostbringer());
            cont2.DropItem(new BreathOfTheDead());
            cont2.DropItem(new TheDragonSlayer());
            cont2.DropItem(new BoneCrusher());
            cont2.DropItem(new StaffOfTheMagi());
            cont2.DropItem(new SerpentsFang());
            cont2.DropItem(new LegacyOfTheDreadLord());
            cont2.DropItem(new TheTaskmaster());
            cont2.DropItem(new TheDryadBow());
            cont2.DropItem(new TitansHammer());
            cont2.DropItem(new InquisitorsResolution());
            cont2.DropItem(new BladeOfTheRighteous());
            cont2.DropItem(new ZyronicClaw());

            PlaceItemIn(cont, 90, 83, cont2);
            // End Bag of Major Artifacts

            // Begin bag of Tokuno minor artifacts
            cont2 = new Bag();
            cont2.Hue = 1281;
            cont2.Name = "Tokuno Minor Artifacts";

            cont2.DropItem(new PeasantsBokuto());
            cont2.DropItem(new DragonNunchaku());
            cont2.DropItem(new TheDestroyer());
            cont2.DropItem(new HanzosBow());
            cont2.DropItem(new Exiler());
            cont2.DropItem(new PilferedDancerFans());
            cont2.DropItem(new DemonForks());
            cont2.DropItem(new BlackLotusHood());
            cont2.DropItem(new DaimyosHelm());
            cont2.DropItem(new ArmsOfTacticalExcellence());
            cont2.DropItem(new AncientFarmersKasa());
            cont2.DropItem(new GlovesOfTheSun());
            cont2.DropItem(new LegsOfStability());
            cont2.DropItem(new AncientSamuraiDo());
            cont2.DropItem(new PigmentsOfTokuno());
            cont2.DropItem(new FluteOfRenewal());
            cont2.DropItem(new AncientUrn());
            cont2.DropItem(new HonorableSwords());
            cont2.DropItem(new ChestOfHeirlooms());
            cont2.DropItem(new TomeOfEnlightenment());

            PlaceItemIn(cont, 53, 83, cont2);
            // End bag of Tokuno minor artifacts

            // Begin bag of Tokuno major artifacts
            cont2 = new Bag();
            cont2.Hue = 1281;
            cont2.Name = "Tokuno Major Artifacts";

            cont2.DropItem(new DarkenedSky());
            cont2.DropItem(new KasaOfTheRajin());
            cont2.DropItem(new RuneBeetleCarapace());
            cont2.DropItem(new Stormgrip());
            cont2.DropItem(new SwordOfTheStampede());
            cont2.DropItem(new SwordsOfProsperity());
            cont2.DropItem(new TheHorselord());
            cont2.DropItem(new TomeOfLostKnowledge());
            cont2.DropItem(new WindsEdge());
            //cont2.DropItem(new PigmentsOfTokunoMajor(PigmentsType.ParagonGold, 50));
            //cont2.DropItem(new PigmentsOfTokunoMajor(PigmentsType.VioletCouragePurple, 50));
            //cont2.DropItem(new PigmentsOfTokunoMajor(PigmentsType.InvulnerabilityBlue, 50));
            //cont2.DropItem(new PigmentsOfTokunoMajor(PigmentsType.LunaWhite, 50));
            //cont2.DropItem(new PigmentsOfTokunoMajor(PigmentsType.DryadGreen, 50));
            //cont2.DropItem(new PigmentsOfTokunoMajor(PigmentsType.ShadowDancerBlack, 50));
            //cont2.DropItem(new PigmentsOfTokunoMajor(PigmentsType.BerserkerRed, 50));
            //cont2.DropItem(new PigmentsOfTokunoMajor(PigmentsType.NoxGreen, 50));
            //cont2.DropItem(new PigmentsOfTokunoMajor(PigmentsType.RumRed, 50));
            //cont2.DropItem(new PigmentsOfTokunoMajor(PigmentsType.FireOrange, 50));

            PlaceItemIn(cont, 127, 83, cont2);
            // End bag of Tokuno major artifacts

            // Begin bag of ML Minor artifacts
            cont2 = new Bag();
            cont2.Hue = 1167;
            cont2.Name = "Minor Artifacts";

            cont2.DropItem(new AegisOfGrace());
            cont2.DropItem(new BladeDance());
            cont2.DropItem(new BloodwoodSpirit());
            cont2.DropItem(new Bonesmasher());
            cont2.DropItem(new Boomstick());
            cont2.DropItem(new BrightsightLenses());
            cont2.DropItem(new FeyLeggings());
            cont2.DropItem(new FleshRipper());
            cont2.DropItem(new HelmOfSwiftness());
            cont2.DropItem(new PadsOfTheCuSidhe());
            cont2.DropItem(new QuiverOfRage());
            cont2.DropItem(new QuiverOfElements());
            cont2.DropItem(new RaedsGlory());
            cont2.DropItem(new RighteousAnger());
            cont2.DropItem(new RobeOfTheEclipse());
            cont2.DropItem(new RobeOfTheEquinox());
            cont2.DropItem(new SoulSeeker());
            cont2.DropItem(new TalonBite());
            cont2.DropItem(new TotemOfVoid());
            cont2.DropItem(new WildfireBow());
            cont2.DropItem(new Windsong());

            PlaceItemIn(cont, 140, 83, cont2);
            // End bag of ML Minor artifacts

            PlaceItemIn(bank, 63, 106, cont);
            // End box of Artifacts

            // Begin box of General Resources
            cont = new WoodenBox();
            cont.ItemID = 0xE7D;
            cont.Hue = 1193;
            cont.Name = "Genereal Resources";

            // Begin bag of raw materials
            cont2 = new Bag();
            cont2.Name = "Raw Materials Bag";

            PlaceItemIn(cont2, 92, 60, new BarbedLeather(TestCenterConfig.BarbedLeather));
            PlaceItemIn(cont2, 92, 68, new HornedLeather(TestCenterConfig.HornedLeather));
            PlaceItemIn(cont2, 92, 76, new SpinedLeather(TestCenterConfig.SpinedLeather));
            PlaceItemIn(cont2, 92, 84, new Leather(TestCenterConfig.Leather));

            PlaceItemIn(cont2, 30, 118, new Cloth(TestCenterConfig.Cloth));
            PlaceItemIn(cont2, 30, 84, new Board(TestCenterConfig.Board));
            PlaceItemIn(cont2, 57, 80, new BlankScroll(TestCenterConfig.BlankScroll));
            PlaceItemIn(cont2, 57, 80, new Bone(TestCenterConfig.Bone));

            PlaceItemIn(cont2, 30, 35, new DullCopperIngot(TestCenterConfig.DullCopperIngot));
            PlaceItemIn(cont2, 37, 35, new ShadowIronIngot(TestCenterConfig.ShadowIronIngot));
            PlaceItemIn(cont2, 44, 35, new CopperIngot(TestCenterConfig.CopperIngot));
            PlaceItemIn(cont2, 51, 35, new BronzeIngot(TestCenterConfig.BronzeIngot));
            PlaceItemIn(cont2, 58, 35, new GoldIngot(TestCenterConfig.GoldIngot));
            PlaceItemIn(cont2, 65, 35, new AgapiteIngot(TestCenterConfig.AgapiteIngot));
            PlaceItemIn(cont2, 72, 35, new VeriteIngot(TestCenterConfig.VeriteIngot));
            PlaceItemIn(cont2, 79, 35, new ValoriteIngot(TestCenterConfig.ValoriteIngot));
            PlaceItemIn(cont2, 86, 35, new IronIngot(TestCenterConfig.IronIngot));

            PlaceItemIn(cont2, 30, 59, new RedScales(TestCenterConfig.RedScales));
            PlaceItemIn(cont2, 36, 59, new YellowScales(TestCenterConfig.YellowScales));
            PlaceItemIn(cont2, 42, 59, new BlackScales(TestCenterConfig.BlackScales));
            PlaceItemIn(cont2, 48, 59, new GreenScales(TestCenterConfig.GreenScales));
            PlaceItemIn(cont2, 54, 59, new WhiteScales(TestCenterConfig.WhiteScales));
            PlaceItemIn(cont2, 60, 59, new BlueScales(TestCenterConfig.BlueScales));

            PlaceItemIn(cont, 40, 93, cont2);
            // End bag of raw materials

            // Begin bag of tools
            cont2 = new Bag();
            cont2.Name = "Tool Bag";

            cont2.DropItem(new TinkerTools(TestCenterConfig.TinkerTools));
            cont2.DropItem(new HousePlacementTool());
            cont2.DropItem(new DovetailSaw(TestCenterConfig.DovetailSaw));
            cont2.DropItem(new Scissors());
            cont2.DropItem(new MortarPestle(TestCenterConfig.MortarPestle));
            cont2.DropItem(new ScribesPen(TestCenterConfig.ScribesPen));
            cont2.DropItem(new SmithHammer(TestCenterConfig.SmithHammer));
            cont2.DropItem(new TwoHandedAxe());
            cont2.DropItem(new FletcherTools(TestCenterConfig.FletcherTools));
            cont2.DropItem(new SewingKit(TestCenterConfig.SewingKit));
            cont2.DropItem(new Clippers(TestCenterConfig.Clippers));

            PlaceItemIn(cont, 90, 93, cont2);
            // End bag of tools

            // Begin bag of runic tools
            cont2 = new Backpack();
            cont2.Name = "Runic Tool Bag";

            PlaceItemIn(cont2, 54, 74, new RunicHammer(CraftResource.DullCopper, TestCenterConfig.RunicHammerDullCopper));
            PlaceItemIn(cont2, 64, 74, new RunicHammer(CraftResource.ShadowIron, TestCenterConfig.RunicHammerShadowIron));
            PlaceItemIn(cont2, 74, 74, new RunicHammer(CraftResource.Copper, TestCenterConfig.RunicHammerCopper));
            PlaceItemIn(cont2, 84, 74, new RunicHammer(CraftResource.Bronze, TestCenterConfig.RunicHammerBronze));
            PlaceItemIn(cont2, 94, 74, new RunicHammer(CraftResource.Gold, TestCenterConfig.RunicHammerGold));
            PlaceItemIn(cont2, 104, 74, new RunicHammer(CraftResource.Agapite, TestCenterConfig.RunicHammerAgapite));
            PlaceItemIn(cont2, 114, 74, new RunicHammer(CraftResource.Verite, TestCenterConfig.RunicHammerVerite));
            PlaceItemIn(cont2, 124, 74, new RunicHammer(CraftResource.Valorite, TestCenterConfig.RunicHammerValorite));

            PlaceItemIn(cont2, 54, 90, new RunicSewingKit(CraftResource.SpinedLeather, TestCenterConfig.RunicSewingKitSpinedLeather));
            PlaceItemIn(cont2, 64, 90, new RunicSewingKit(CraftResource.HornedLeather, TestCenterConfig.RunicSewingKitHornedLeather));
            PlaceItemIn(cont2, 74, 90, new RunicSewingKit(CraftResource.BarbedLeather, TestCenterConfig.RunicSewingKitBarbedLeather));

            PlaceItemIn(cont2, 54, 107, new RunicFletcherTool(CraftResource.OakWood, TestCenterConfig.RunicFletcherToolOakWood));
            PlaceItemIn(cont2, 69, 107, new RunicFletcherTool(CraftResource.AshWood, TestCenterConfig.RunicFletcherToolAshWood));
            PlaceItemIn(cont2, 83, 107, new RunicFletcherTool(CraftResource.YewWood, TestCenterConfig.RunicFletcherToolYewWood));
            PlaceItemIn(cont2, 97, 107, new RunicFletcherTool(CraftResource.Heartwood, TestCenterConfig.RunicFletcherToolHeartwood));

            PlaceItemIn(cont2, 93, 90, new RunicDovetailSaw(CraftResource.OakWood, TestCenterConfig.RunicDovetailSawOakWood));
            PlaceItemIn(cont2, 102, 90, new RunicDovetailSaw(CraftResource.AshWood, TestCenterConfig.RunicDovetailSawAshWood));
            PlaceItemIn(cont2, 112, 90, new RunicDovetailSaw(CraftResource.YewWood, TestCenterConfig.RunicDovetailSawYewWood));
            PlaceItemIn(cont2, 122, 90, new RunicDovetailSaw(CraftResource.Heartwood, TestCenterConfig.RunicDovetailSawHeartwood));

            PlaceItemIn(cont, 65, 67, cont2);
            // End bag of runic tools

            // Begin bag of recipes
            cont2 = new Bag();
            cont2.Name = "Bag of Recipes";
            cont2.Hue = 2301;

            for (int i = 0; i <= 92; i++)
            {
                cont2.DropItem(new RecipeScroll(i));
            }

            PlaceItemIn(cont, 115, 93, cont2);
            // End bag of recipes

            // Begin bag of archery ammo
            cont2 = new Bag();
            cont2.Name = "Bag Of Archery Ammo";

            PlaceItemIn(cont2, 48, 76, new Arrow(TestCenterConfig.Arrow));
            PlaceItemIn(cont2, 72, 76, new Bolt(TestCenterConfig.Bolt));

            PlaceItemIn(cont, 65, 93, cont2);
            // End bag of archery ammo

            // Begin bag of Wood
            cont2 = new Bag();
            cont2.Hue = 1321;
            cont2.Name = "Bag of Wood";

            cont2.DropItem(new AshBoard(TestCenterConfig.AshBoard));
            cont2.DropItem(new YewBoard(TestCenterConfig.YewBoard));
            cont2.DropItem(new OakBoard(TestCenterConfig.OakBoard));
            cont2.DropItem(new HeartwoodBoard(TestCenterConfig.HeartwoodBoard));
            cont2.DropItem(new BloodwoodBoard(TestCenterConfig.BloodwoodBoard));
            cont2.DropItem(new FrostwoodBoard(TestCenterConfig.FrostwoodBoard));

            PlaceItemIn(cont, 139, 93, cont2);
            // End bag of Wood

            // Begin Bag of Imbuing Materials
            cont2 = new Bag();
            cont2.Hue = 0x4B;
            cont2.Name = "Bag of Imbuing Materials";

            foreach (ImbuingResource resource in Enum.GetValues(typeof(ImbuingResource)))
            {
                try
                {
                    Type type = ScriptCompiler.FindTypeByFullName(String.Format("Server.Items.{0}", resource.ToString()));

                    Item item = (Item)Activator.CreateInstance(type);
                    item.Amount = TestCenterConfig.ImbuingResource;

                    cont2.DropItem(item);
                }
                catch
                {
                }
            }

            PlaceItemIn(cont, 16, 67, cont2);
            // End Bag of Imbuing Materials

            // Begin Bag of Elven Materials
            cont2 = new Bag();
            cont2.Hue = 1195;
            cont2.Name = "Bag of Elven Materials";

            cont2.DropItem(new BarkFragment(TestCenterConfig.BarkFragment));
            cont2.DropItem(new Blight(TestCenterConfig.Blight));
            cont2.DropItem(new BlueDiamond(TestCenterConfig.BlueDiamond));
            cont2.DropItem(new BrilliantAmber(TestCenterConfig.BrilliantAmber));
            cont2.DropItem(new CapturedEssence(TestCenterConfig.CapturedEssence));
            cont2.DropItem(new Corruption(TestCenterConfig.Corruption));
            cont2.DropItem(new DarkSapphire(TestCenterConfig.DarkSapphire));
            cont2.DropItem(new DiseasedBark(TestCenterConfig.DiseasedBark));
            cont2.DropItem(new DreadHornMane(TestCenterConfig.DreadHornMane));
            cont2.DropItem(new EcruCitrine(TestCenterConfig.EcruCitrine));
            cont2.DropItem(new EnchantedSwitch(TestCenterConfig.EnchantedSwitch));
            cont2.DropItem(new EyeOfTheTravesty(TestCenterConfig.EyeOfTheTravesty));
            cont2.DropItem(new FireRuby(TestCenterConfig.FireRuby));
            cont2.DropItem(new GrizzledBones(TestCenterConfig.GrizzledBones));
            cont2.DropItem(new HollowPrism(TestCenterConfig.HollowPrism));
            cont2.DropItem(new JeweledFiligree(TestCenterConfig.JeweledFiligree));
            cont2.DropItem(new LardOfParoxysmus(TestCenterConfig.LardOfParoxysmus));
            cont2.DropItem(new LuminescentFungi(TestCenterConfig.LuminescentFungi));
            cont2.DropItem(new Muculent(TestCenterConfig.Muculent));
            cont2.DropItem(new ParasiticPlant(TestCenterConfig.ParasiticPlant));
            cont2.DropItem(new PerfectEmerald(TestCenterConfig.PerfectEmerald));
            cont2.DropItem(new PristineDreadHorn(TestCenterConfig.PristineDreadHorn));
            cont2.DropItem(new Putrefaction(TestCenterConfig.Putrefication));
            cont2.DropItem(new RunedPrism(TestCenterConfig.RunedPrism));
            cont2.DropItem(new Scourge(TestCenterConfig.Scourge));
            cont2.DropItem(new SwitchItem(TestCenterConfig.SwitchItem));
            cont2.DropItem(new Taint(TestCenterConfig.Taint));
            cont2.DropItem(new Turquoise(TestCenterConfig.Turquoise));
            cont2.DropItem(new WhitePearl(TestCenterConfig.WhitePearl));

            PlaceItemIn(cont, 40, 67, cont2);
            // End Bag of Elven Materials

            PlaceItemIn(bank, 88, 142, cont);
            // End box of General Resources

            // Begin box of Armor Set Pieces
            cont = new WoodenBox();
            cont.ItemID = 0xE7D;
            cont.Hue = 1194;
            cont.Name = "Armor Set Pieces";

            // Begin Bag of Juggernaut Set
            cont2 = new Bag();
            cont2.Hue = 1177;
            cont2.Name = "Juggernaut Set";

            cont2.DropItem(new MalekisHonor());
            cont2.DropItem(new Evocaricus());

            PlaceItemIn(cont, 17, 63, cont2);
            // End Bag of Juggernaut Set

            // Begin Bag of Hunter Set Armor
            cont2 = new Bag();
            cont2.Hue = 1177;
            cont2.Name = "Hunter Set Armor";

            cont2.DropItem(new HunterGloves());
            cont2.DropItem(new HunterLegs());
            cont2.DropItem(new HunterArms());
            cont2.DropItem(new HunterChest());

            PlaceItemIn(cont, 40, 63, cont2);
            // End Bag of Hunter Set Armor

            // Begin Bag of Paladin Set Armor
            cont2 = new Bag();
            cont2.Hue = 1177;
            cont2.Name = "Paladin Set Armor";

            cont2.DropItem(new PaladinArms());
            cont2.DropItem(new PaladinChest());
            cont2.DropItem(new PaladinGloves());
            cont2.DropItem(new PaladinGorget());
            cont2.DropItem(new PaladinHelm());
            cont2.DropItem(new PaladinLegs());

            PlaceItemIn(cont, 65, 63, cont2);
            // End Bag of Paladin Set Armor

            // Begin Bag of Necromancer Set Armor
            cont2 = new Bag();
            cont2.Hue = 1177;
            cont2.Name = "Necromancer Set Armor";

            cont2.DropItem(new DeathGloves());
            cont2.DropItem(new DeathBoneHelm());
            cont2.DropItem(new DeathLegs());
            cont2.DropItem(new DeathArms());
            cont2.DropItem(new DeathChest());

            PlaceItemIn(cont, 90, 63, cont2);
            // End Bag of Necromancer Set Armor

            // Begin Bag of Acolyte Set Armor
            cont2 = new Bag();
            cont2.Hue = 1177;
            cont2.Name = "Acolyte Set Armor";

            cont2.DropItem(new GreymistGloves());
            cont2.DropItem(new GreymistLegs());
            cont2.DropItem(new GreymistArms());
            cont2.DropItem(new GreymistChest());

            PlaceItemIn(cont, 115, 63, cont2);
            // End Bag of Acolyte Set Armor

            // Begin Bag of Marksman Set
            cont2 = new Bag();
            cont2.Hue = 1177;
            cont2.Name = "Marksman Set";

            cont2.DropItem(new Feathernock());
            cont2.DropItem(new Swiftflight());

            PlaceItemIn(cont, 139, 63, cont2);
            // End Bag of Marksman Set

            // Begin Bag of Monstrous Interred Grizzle Set Armor
            cont2 = new Bag();
            cont2.Hue = 1177;
            cont2.Name = "Monstrous Interred Grizzle Set Armor";

            cont2.DropItem(new GrizzleGauntlets());
            cont2.DropItem(new GrizzleGreaves());
            cont2.DropItem(new GrizzleHelm());
            cont2.DropItem(new GrizzleTunic());
            cont2.DropItem(new GrizzleVambraces());

            PlaceItemIn(cont, 17, 89, cont2);
            // End Bag of Monstrous Interred Grizzle Set Armor

            // Begin Bag of Warrior Set Armor
            cont2 = new Bag();
            cont2.Hue = 1177;
            cont2.Name = "Warrior Set Armor";

            cont2.DropItem(new DarkwoodChest());
            cont2.DropItem(new DarkwoodCrown());
            cont2.DropItem(new DarkwoodGloves());
            cont2.DropItem(new DarkwoodGorget());
            cont2.DropItem(new DarkwoodPauldrons());
            cont2.DropItem(new DarkwoodLegs());

            PlaceItemIn(cont, 40, 89, cont2);
            // End Bag of Warrior Set Armor

            // Begin Bag of Mage Set Armor
            cont2 = new Bag();
            cont2.Hue = 1177;
            cont2.Name = "Mage Set Armor";

            cont2.DropItem(new LeafweaveGloves());
            cont2.DropItem(new LeafweaveLegs());
            cont2.DropItem(new LeafweavePauldrons());
            cont2.DropItem(new LeafweaveChest());

            PlaceItemIn(cont, 65, 89, cont2);
            // End Bag of Mage Set Armor

            // Begin Bag of Assassin Set Armor
            cont2 = new Bag();
            cont2.Hue = 1177;
            cont2.Name = "Assassin Set Armor";

            cont2.DropItem(new AssassinGloves());
            cont2.DropItem(new AssassinLegs());
            cont2.DropItem(new AssassinArms());
            cont2.DropItem(new AssassinChest());

            PlaceItemIn(cont, 90, 89, cont2);
            // End Bag of Assassin Set Armor

            // Begin Bag of Myrmidon Set Armor
            cont2 = new Bag();
            cont2.Hue = 1177;
            cont2.Name = "Myrmidon Set Armor";

            cont2.DropItem(new MyrmidonArms());
            cont2.DropItem(new MyrmidonChest());
            cont2.DropItem(new MyrmidonGloves());
            cont2.DropItem(new MyrmidonGorget());
            cont2.DropItem(new MyrmidonBascinet());
            cont2.DropItem(new MyrmidonLegs());

            PlaceItemIn(cont, 115, 89, cont2);
            // End Bag of Myrmidon Set Armor

            PlaceItemIn(bank, 113, 142, cont);
            // End box of Armor Set Pieces

            PlaceItemIn(bank, 118, 111, new ChargerOfTheFallen());

            PromotionalToken pt = new PromotionalToken(PromotionalType.SoulStoneFragment);

            m.SendAsciiMessage("A token has been placed in your backpack. Double-click it to redeem your promotion.");

            m.AddToBackpack(pt);
        }
        #endregion
    }
}