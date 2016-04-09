using System;
using System.Collections.Generic;
using Server.Items;
using Server.ContextMenus;
using Server.Gumps;

namespace Server.Mobiles
{
    public class Counselor_PR : BaseGuildmaster
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }
        public override NpcGuild NpcGuild { get { return NpcGuild.MerchantsGuild; } }
        private static bool m_Talked;

        string[] npcSpeech = new string[]
        { 
            "Welcome traveller! how may I assist thee?",     
        };

        [Constructable]
        public Counselor_PR()
            : base("merchant")
        {
            if (this.Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }

            Title = "[PR]";
            NameHue = 11;

            VendorAccessLevel = AccessLevel.Player;
            AccessLevel = AccessLevel.Counselor;

            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();

            Item hair = new Item(Utility.RandomList(0x203B, 0x2049, 0x2048, 0x204A));
            hair.Hue = Utility.RandomNondyedHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            if (Utility.RandomBool() && !this.Female)
            {
                Item beard = new Item(Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D));

                beard.Hue = hair.Hue;
                beard.Layer = Layer.FacialHair;
                beard.Movable = false;

                AddItem(beard);
            }

            CantWalk = false;

            switch (Utility.Random(3))
            {
                case 0: AddItem(new BookOfNinjitsu()); break;
                case 1: AddItem(new BookOfBushido()); break;
                case 2: AddItem(new BookOfChivalry()); break;
            }

            StaffRobe robe = new StaffRobe();
            robe.AccessLevel = AccessLevel.Counselor;
            robe.Movable = false;
            robe.Hue = 0x3;
            robe.LootType = LootType.Blessed;
            AddItem(robe);
        }

        public class Counselor_Entry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Mobile m_Giver;

            public Counselor_Entry(Mobile from, Mobile giver)
                : base(6146, 3)
            {
                m_Mobile = from;
                m_Giver = giver;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                {
                    return;
                }

                PlayerMobile mobile = (PlayerMobile)m_Mobile;
                {
                    if (!mobile.HasGump(typeof(Counselor_Talk)))
                    {
                        mobile.SendGump(new Counselor_Talk());
                    }
                }
            }
        }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBCounselor());
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.InRange(this, 3) && m is PlayerMobile)
            {
                if (!m.HasGump(typeof(PR_StaffKeywords)))
                {
                    m.SendGump(new PR_StaffKeywords());
                }
            }
            if (!m.InRange(this, 3) && m is PlayerMobile)
            {
                if (m.HasGump(typeof(PR_StaffKeywords)))
                {
                    m.CloseGump(typeof(PR_StaffKeywords));
                }
            }

            if (m_Talked == false)
            {
                if (m.InRange(this, 4))
                {
                    m_Talked = true;
                    SayRandom(npcSpeech, this);
                    this.Move(GetDirectionTo(m.Location));
                    m.SendMessage("Please use the keywords list to get the help that you need.");

                    // Start timer to prevent spam 
                    SpamTimer_PR t = new SpamTimer_PR();
                    t.Start();
                }
            }
        }

        private class SpamTimer_PR : Timer
        {
            public SpamTimer_PR()
                : base(TimeSpan.FromSeconds(20))
            {
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                m_Talked = false;
            }
        }

        private static void SayRandom(string[] say, Mobile m)
        {
            m.Say(say[Utility.Random(say.Length)]);
        }

        #region Interactions: Based On Keywords

        #region Keyword Listing - A Quick Reference
        /*   
            > serverinfo....... //launch browser
            > tosagreement..... //launch browser          
            > serverrules...... //launch browser
            > meetourstaff..... //launch browser
            > showcredits...... //launch browser
  
            > livesupport...... //page livestaff
                 
            > skillcap..........//text displayed
            > skills........... //launch browser
            > statcap.......... //text displayed 
            > playerguide...... //launch browser
            > bestiary......... //launch browser
                    
            > events........... //launch browser
            > eventrequest..... //gump displayed
            > hiring........... //submition gump
            > suggestion....... //submition gump
            > donations........ //submition gump                          
        */
        #endregion

        #region NPC Counselors - Unacceptable Words

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(this.Location, 5))
                return true;
            return base.HandlesOnSpeech(from);
        }

        #region Unacceptable Word List

        private List<string> m_UnacceptableWords = new List<string>(new string[]{"ass","asshole","blowjob","bitch","bitches","biatch","biatches","breasts","chinc","chink","cunnilingus","cum","cumstain","cocksucker","clit",
                "chigaboo","cunt","clitoris","cock","dick","dickhead","dyke","dildo","fuck","fucktard","felatio","fag","faggot","hitler","jigaboo","jizzm","jizz","jiz","jism","jiss","jis","jerkoff","jackoff", "kyke","kike",
                "klit","lezbo","lesbo","nigga","niggas","nigger","piss","penis","prick","pussy","retard","retarded","spic","shit","spunk","spunker","smeg","smegg","twat","tit","tits","titties", "tittys","titie","tities",
                "tity","tard","vagina","wop","wigger","wiger"});

        #endregion

        private bool ContainsUnacceptableWords(string speech)
        {
            string[] speechArray = speech.Split(' ');

            foreach (string word in speechArray)
            {
                if (m_UnacceptableWords.Contains(word.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        public override void OnSpeech(SpeechEventArgs args)
        {
            string said = args.Speech.ToLower();
            Mobile from = args.Mobile;

            if (ContainsUnacceptableWords(said))
            {
                from.MoveToWorld(new Point3D(1483, 1617, 20), Map.Trammel); //Location To Send Players If They Say Unacceptable Words
                from.SendMessage("You Have Been Jailed For Using Inappropriate Language And/Or Out Of Character, Real-World, References In Front Of A Staff Member");
                return;
            }

        #endregion

            switch (said)
            {
                //General Information

                #region serverinfo
                //Some People Are Interested About How Your Server Came To Be. Tell Them!

                case ("serverinfo"):
                    {
                        Say(String.Format("Ahhh! Inquisitive minds want to know?! Allow me to redirect your request.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://unforgetable6910.wix.com/distantland");
                        break;
                    }

                #endregion Edited By: A.A.R

                #region tosagreement
                //A Players Inability To Understand The Consequences For Breaking The Servers Rules Makes Them Stupid.

                case ("tosagreement"):
                    {
                        Say(String.Format("Sure! Allow me to redirect you to our website. Thank you.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://unforgetable6910.wix.com/distantland");
                        break;
                    }

                #endregion Edited By: A.A.R

                #region serverrules
                //Some People Want To Play By The Rules And/Or Learn To Get Around Them! Either Way, This Should Help.

                case ("serverrules"):
                    {
                        Say(String.Format("A good law abiding adventurer! 'Tis a pleasure to meet you.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://unforgetable6910.wix.com/distantland");
                        break;
                    }

                #endregion Edited By: A.A.R

                #region meetourstaff
                //Every Server Website Should Have A Page Devoted To Staff Introductions!

                case ("meetourstaff"):
                    {
                        Say(String.Format("Sure! Please be patient while I redirect you to our website. Thank you.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://unforgetable6910.wix.com/distantland");
                        break;
                    }

                #endregion Edited By: A.A.R

                #region showcredits
                //Someone Aside From You Has Also Worked Their Ass Of To Make Your Server What It Is, Give Them Credit!

                case ("showcredits"):
                    {
                        Say(String.Format("Sure! Please be patient while I redirect you to our website. Thank you.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://unforgetable6910.wix.com/distantland");
                        break;
                    }

                #endregion Edited By: A.A.R

                //Player Reporting

                #region livesupport
                //Allows Players To Page Real Staff Members Online

                case ("livesupport"):
                    {
                        args.Mobile.SendGump(new Server.Engines.Help.HelpGump(args.Mobile));
                        break;
                    }

                #endregion Edited By: A.A.R

                //MMORPG Help Desk

                #region skillcap
                //Helps Players Figure Out What The Skill Cap Is

                case ("skillcap"):
                    {
                        Say(String.Format("Our server currently has a maximum skillcap of 840. There are no plans to increase this number. A skill cap of 840 allows players to set a maximum of 7 skills to 120.", args.Mobile.Name));
                        break;
                    }

                #endregion Edited By: A.A.R

                #region skills
                //Sometimes Players Need Information On Skills And Skill Gain

                case ("skills"):
                    {
                        Say(String.Format("My apologies {0}, I am forbidden to assist thee with skill training. However, If you tell me the name of the skill you're having issues with, then I'll be more than happy to redirect you to our online skill guide.", args.Mobile.Name));
                        break;
                    }

                #region Player Skill Guide References

                case ("alchemy"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/alchemy");
                        break;
                    }

                case ("anatomy"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/anatomy");
                        break;
                    }

                case ("animal lore"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/animallore");
                        break;
                    }

                case ("animal taming"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/animaltaming");
                        break;
                    }

                case ("archery"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/archery");
                        break;
                    }

                case ("armslore"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/armslore");
                        break;
                    }

                case ("begging"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/begging");
                        break;
                    }

                case ("blacksmithy"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/blacksmithy");
                        break;
                    }

                case ("bowcraft"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/bowcraft");
                        break;
                    }

                case ("fletching"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/fletching");
                        break;
                    }

                case ("bushido"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/bushido");
                        break;
                    }

                case ("camping"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/camping");
                        break;
                    }

                case ("carpentry"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/carpentry");
                        break;
                    }

                case ("cartography"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/cartography");
                        break;
                    }

                case ("chivalry"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/chivalry");
                        break;
                    }

                case ("cooking"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/cooking");
                        break;
                    }

                case ("detect hidden"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/detecthidden");
                        break;
                    }

                case ("discordance"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/discordance");
                        break;
                    }

                case ("evaluating intelligence"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/evaluatingintelligence");
                        break;
                    }

                case ("fencing"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/fencing");
                        break;
                    }

                case ("fishing"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/fishing");
                        break;
                    }

                case ("focus"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/focus");
                        break;
                    }

                case ("forensic evaluation"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/forensicevaluation");
                        break;
                    }

                case ("healing"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/healing");
                        break;
                    }

                case ("herding"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/herding");
                        break;
                    }

                case ("hiding"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/hiding");
                        break;
                    }

                case ("imbuing"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/imbuing");
                        break;
                    }

                case ("inscription"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/inscription");
                        break;
                    }

                case ("item identification"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/itemidentification");
                        break;
                    }

                case ("lockpicking"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/lockpicking");
                        break;
                    }

                case ("lumberjacking"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/lumberjacking");
                        break;
                    }

                case ("macefighting"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/macefighting");
                        break;
                    }

                case ("magery"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/magery");
                        break;
                    }

                case ("meditation"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/meditation");
                        break;
                    }

                case ("mining"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/mining");
                        break;
                    }

                case ("musicianship"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/musicianship");
                        break;
                    }

                case ("mysticism"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/mysticism");
                        break;
                    }

                case ("necromancy"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/necromancy");
                        break;
                    }

                case ("ninjitsu"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/ninjitsu");
                        break;
                    }

                case ("parrying"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/parrying");
                        break;
                    }

                case ("peacemaking"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/peacemaking");
                        break;
                    }

                case ("poisoning"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/poisoning");
                        break;
                    }

                case ("provocation"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/provocation");
                        break;
                    }

                case ("removetrap"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/removetrap");
                        break;
                    }

                case ("resisting spells"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/resistingspells");
                        break;
                    }

                case ("snooping"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/snooping");
                        break;
                    }

                case ("spellweaving"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/spellweaving");
                        break;
                    }

                case ("spiritspeak"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/spiritspeak");
                        break;
                    }

                case ("stealing"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/stealing");
                        break;
                    }

                case ("stealth"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/stealth");
                        break;
                    }

                case ("swordsmanship"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/swordsmanship");
                        break;
                    }

                case ("tactics"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/tactics");
                        break;
                    }

                case ("tailoring"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/tailoring");
                        break;
                    }

                case ("taste identification"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/tasteidentification");
                        break;
                    }

                case ("throwing"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/throwing");
                        break;
                    }

                case ("tinkering"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/tinkering");
                        break;
                    }

                case ("tracking"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/tracking");
                        break;
                    }

                case ("veterinary"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/veterinary");
                        break;
                    }

                case ("wrestling"):
                    {
                        Say(String.Format("Thank you {0}, allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide/skills/wrestling");
                        break;
                    }

                #endregion Edited By: A.A.R

                #endregion Edited By: A.A.R

                #region statcap
                //Helps Players Figure Out What The Stat Cap Is

                case ("statcap"):
                    {
                        Say(String.Format("Our server currently has a maximum statcap of 300. There are no plans to increase this number. A stat cap of 300 allows players to evenly set their strength, dexterity, and intelligence to 100.", args.Mobile.Name));
                        break;
                    }

                #endregion Edited By: A.A.R

                #region playerguide
                //Directs Players To Your Servers Online PlayGuide For Assistance

                case ("playguide"):
                    {
                        Say(String.Format("'Tis good to keep up-to-date on things {0}. Allow me to redirect you to our online playguide.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/playguide");
                        break;
                    }

                #endregion Edited By: A.A.R

                #region bestiary

                case ("bestiary"):
                    {
                        Say(String.Format("We've got a lot of creatures on the server {0}. Allow me to redirect you to our online bestiary.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/bestiary");
                        break;
                    }

                #endregion Edited By: A.A.R

                //Player Involvment

                #region events
                //Some People Are Interested About How Your Server Came To Be. Tell Them!

                case ("events"):
                    {
                        Say(String.Format("Ahhh! Inquisitive minds want to know?! Allow me to redirect your request.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/scheduledevents");
                        break;
                    }

                #endregion Edited By: A.A.R

                #region eventrequest
                //Some People Are Interested About How Your Server Came To Be. Tell Them!

                case ("eventrequest"):
                    {
                        args.Mobile.SendGump(new PR_EventRequest());
                        Say(String.Format("Event requests are always welcome! {0}. Thank you for being proactive. If you have any other event ideas, please let us know!", args.Mobile.Name));
                        break;
                    }

                #endregion Edited By: A.A.R

                #region hiring
                //A Toggle! Just Uncomment Which Response You'd Like To Give Your Players

                #region Yes, Staff Is Hiring
                //An Easy Way Of Directing Your Players To Your Staff Member Application
                /*
                case ("hiring"):
                    {
                        Say(String.Format("Absolutely {0}! Staffing positions are now available. Please visit our website for more information. Thank you for your interest and we hope to hear from you soon!", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/staffapplication");
                        break;
                    }
                */
                #endregion Edited By: A.A.R

                #region No, We're Not Hiring
                //A Nice Way Of Saying, "Dude! Stop Asking If You Can Be A Staff Member!!"

                case ("hiring"):
                    {
                        Say(String.Format("Our apologies {0}, We're just not hiring at this time. We'll post available positions on our website, as soon as they open up, please be patient and check back soon. Thank you.", args.Mobile.Name));
                        break;
                    }

                #endregion Edited By: A.A.R

                #endregion Edited By: A.A.R

                #region suggestion
                //Everyone Has Their Own Ideas On How They Think Things Should Be

                case ("suggestion"):
                    {
                        args.Mobile.SendGump(new SuggestionBox());
                        Say(String.Format("We would really appreciate your input {0}. Thank you. If you have any other suggestions, please let us know!", args.Mobile.Name));
                        break;
                    }

                #endregion Edited By: A.A.R

                #region donations
                //Makes It Easier For Players To Donate Funds To Your Server

                case ("donate"):
                    {
                        Say(String.Format("Donations are very welcome, but not required to play. Money received helps keep this server running stable and lag free! Contributors will receive special priviledges and incentives for their support. If you'd like more information then please visit our website. Thank you.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("http://www.yoursitename.com/donationpage");
                        break;
                    }

                #endregion Edited By: A.A.R

            }
        }

        #endregion

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new Counselor_Entry(from, this));
        }

        public override bool ClickTitle { get { return false; } }
        public override bool IsActiveVendor { get { return true; } }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            Mobile m = from;
            PlayerMobile mobile = m as PlayerMobile;

            from.SendMessage("I appreciate the offer, but I do this job out of the love for the game.");
            return false;
        }

        public Counselor_PR(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}


