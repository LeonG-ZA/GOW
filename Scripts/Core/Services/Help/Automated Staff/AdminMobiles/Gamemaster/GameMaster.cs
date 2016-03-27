using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Items;
using Server.ContextMenus;
using Server.Multis;
using Server.Regions;
using Server.Engines.ChampionSpawns;
using Server.Spells;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Accounting;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    public class GameMaster_GM : BaseGuildmaster
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        public override NpcGuild NpcGuild { get { return NpcGuild.MerchantsGuild; } }

        #region Interactions: Based On Keywords

        private bool m_Gated;

        #endregion Edited By: A.A.R

        #region NPC Auto-Deletion Timer

        private DateTime m_npcAutoDelete;

        #endregion Edited By: A.A.R

        #region Automated Greetings For Players

        private static bool m_Talked;

        string[] npcSpeech = new string[]
        { 
            "Welcome traveller! how may I assist thee?",
        };

        #endregion Edited By: A.A.R

        [Constructable]
        public GameMaster_GM()
            : base("merchant")
        {

//----------This Randomizes The Sex Of The NPC--------------------//

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

//----------This Creates A Random Look To The NPC-----------------//

            Title = "[GM]";
            NameHue = 11;

            VendorAccessLevel = AccessLevel.Counselor;
            AccessLevel = AccessLevel.GameMaster;

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

//----------This Toggles The NPC Movement: On Or Off--------------//

            CantWalk = true;

//----------This Makes The NPC Equip HandHeld Items---------------//

            switch (Utility.Random(3))
            {
                case 0: AddItem(new BookOfNinjitsu()); break;
                case 1: AddItem(new BookOfBushido()); break;
                case 2: AddItem(new BookOfChivalry()); break;
            }

//----------This Sets What Cloth The NPC Will Wear----------------//

            StaffRobe robe = new StaffRobe();
            robe.AccessLevel = AccessLevel.GameMaster;
            robe.Movable = false;
            robe.Hue = 0x26;
            robe.LootType = LootType.Blessed;
            AddItem(robe);

//----------NPC Auto-Deletion Timer (Timer Set At 5min)----------//

            m_npcAutoDelete = DateTime.UtcNow + TimeSpan.FromSeconds(180);
        }

        public override void OnThink()
        {
            base.OnThink();
            if (m_npcAutoDelete <= DateTime.UtcNow)
            {
                this.Delete();
            }
        }

//----------This Gives Your Staff A Monthly Gift-----------------//

        public class GameMaster_Entry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Mobile m_Giver;

            public GameMaster_Entry(Mobile from, Mobile giver)
                : base(6146, 3)
            {
                m_Mobile = from;
                m_Giver = giver;
            }

            public override void OnClick()
            {
                if (!(m_Mobile is PlayerMobile))
                    return;

                PlayerMobile mobile = (PlayerMobile)m_Mobile;
                {
                    if (!mobile.HasGump(typeof(GameMaster_Talk)))
                    {
                        mobile.SendGump(new GameMaster_Talk());
                    }
                }
            }
        }


//------This Code Makes This NPC Behave As An NPC Vendor----------//

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBGameMaster());
        }

//----------------------------------------------------------------//

        #region Automated Greetings For Players

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.InRange(this, 3) && m is PlayerMobile)
            {
                if (!m.HasGump(typeof(GM_StaffKeywords)))
                    m.SendGump(new GM_StaffKeywords());
            }

            if (!m.InRange(this, 3) && m is PlayerMobile)
            {
                if (m.HasGump(typeof(GM_StaffKeywords)))
                    m.CloseGump(typeof(GM_StaffKeywords));
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
                    SpamTimer_GM t = new SpamTimer_GM();
                    t.Start();
                }
            }
        }

        private class SpamTimer_GM : Timer
        {
            public SpamTimer_GM()
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

        #endregion Edited By: A.A.R

        #region Interactions: Based On Keywords

        #region Keyword Listing - A Quick Reference
        /*   
            > serverinfo....... //launch browser
            > tosagreement..... //launch browser          
            > serverrules...... //launch browser
            > meetourstaff..... //launch browser
            > showcredits...... //launch browser
                 
            > reportplayer..... //gump displayed
            > reportlag........ //gump displayed  
            > reportguild...... //gump displayed
            > reportdefect..... //gump displayed
            > reportadmin...... //gump displayed
            > livesupport...... //page livestaff

            > teleportme....... //launches gates
            > relocateme....... //moves stuck pm
            > retrievebody..... //retrieves body
            > retrievepets..... //retrieves pets
            > accounthelp...... //gump displayed                     
        */
        #endregion Edited By: A.A.R

        #region NPC GameMaster - Unacceptable Words

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

        #endregion Edited By: A.A.R

        private bool ContainsUnacceptableWords(string speech)
        {
            string[] speechArray = speech.Split(' ');

            foreach (string word in speechArray)
            {
                if (m_UnacceptableWords.Contains(word.ToLower())) //line 300
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

        #endregion Edited By: Morxeton

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

                #region reportplayer
                //Lag Sucks, Especially When Hunting, Let Us Know About It!

                case ("reportplayer"):
                    {
                        args.Mobile.SendGump(new ReportPlayer());
                        Say(String.Format("Thank you for your report {0}! We'll get this resolved.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("mailto:yourname@yoursitename.com");
                        break;
                    }

                #endregion Edited By: A.A.R

                #region reportlag
                //Lag Sucks, Especially When Hunting, Let Us Know About It!

                case ("reportlag"):
                    {
                        args.Mobile.SendGump(new ReportLag());
                        Say(String.Format("Thank you for your report {0}! Sorry about the lag.", args.Mobile.Name));
                        break;
                    }

                #endregion Edited By: A.A.R

                #region reportguild
                //Mob Mentality Can Take Over Sometimes, Report Them!

                case ("reportguild"):
                    {
                        args.Mobile.SendGump(new ReportGuild());
                        Say(String.Format("Thank you for your report {0}! We'll get this resolved.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("mailto:yourname@yoursitename.com");
                        break;
                    }

                #endregion Edited By: A.A.R

                #region reportdefect
                //Facets Aren't Perfect, Keep Track Of Game Map And Item Defects

                case ("reportdefect"):
                    {
                        args.Mobile.SendGump(new ReportDefect());
                        Say(String.Format("Thank you for your report {0}! We'll be fixing this soon.", args.Mobile.Name));
                        break;
                    }

                #endregion Edited By: A.A.R

                #region reportadmin
                //Staff Can Get Out Of Hand At Times, Report Them!

                case ("reportadmin"):
                    {
                        args.Mobile.SendGump(new ReportAdmin());
                        Say(String.Format("Thank you for your report {0}! We'll get this resolved.", args.Mobile.Name));
                        args.Mobile.LaunchBrowser("mailto:yourname@yoursitename.com");
                        break;
                    }

                #endregion Edited By: A.A.R

                #region livesupport
                //Allows Players To Page Real Staff Members Online

                case ("livesupport"):
                    {
                        args.Mobile.SendGump(new Server.Engines.Help.HelpGump(args.Mobile));
                        break;
                    }

                #endregion Edited By: A.A.R

                //Player Assistance

                #region teleportme
                //Assists Players In Finding Their Way After Getting Lost

                case ("teleportme"):
                    {
                        if (from.Region is Regions.Jail || from.Region is Regions.DungeonRegion || from.Region is ChampionSpawnRegion)
                        {
                            Say(String.Format("HA!! We can't let you escape that easily!", args.Mobile.Name));
                            return;
                        }

                        if (m_Gated == false)
                        {
                            m_Gated = true;
                            args.Mobile.SendGump(new GM_TeleportMe());
                            break;
                        }

                        Say(String.Format("I've already sent you a gate out of here, please be patient.", args.Mobile.Name));
                        Say(String.Format("My mana won't regenerate for thirty minutes.", args.Mobile.Name));
                        break;
                    }

                #endregion Edited By: A.A.R

                #region relocateme
                //Will Move Stuck Players, (Not Teleport Or Gate Them), To A Custom Location

                case ("relocateme"):
                    {
                        if (from.Region is Regions.Jail || from.Region is Regions.DungeonRegion || from.Region is ChampionSpawnRegion)
                        {
                            Say(String.Format("HA!! We can't let you escape that easily!", args.Mobile.Name));
                            return;
                        }

                        switch (Utility.Random(6))
                        {
                            #region Editing Instructions

                            #region Add Custom Locations
                            /*
                                Simply replace the cases we're using with the commented out ones below with these ones and
                                fill in the x,y,z coordinates you want. This is an alternative to the random spawning points
                                we're using. This option only spawns to one of 3 definative locations of your choosing. If you
                                decide to add more locations just increase the case number by one and copy the format of the 
                                commented out cases below to ensure everything compiles right.
                             
                                case 0: from.MoveToWorld(new Point3D(1490, 1617, 20), Map.Trammel); break;
                                case 1: from.MoveToWorld(new Point3D(1490, 1617, 20), Map.Trammel); break;
                                case 2: from.MoveToWorld(new Point3D(1495, 1617, 20), Map.Trammel); break;
                                case 3: from.MoveToWorld(new Point3D(1500, 1617, 20), Map.Trammel); break;

                                Example On How To Edit This:
                                case #: from.MoveToWorld(new Point3D(xCoordinate, yCoordinate, zCoordinate), Map.FacetName); break;
                             
                                You can find the ' x, y, and z ' coordinates by typing ' [where ' in the game world and it will 
                                display the ' x, y, and z ' coordinates in that exact order on the bottom left of your monitor.     
                            */
                            #endregion Edited By: A.A.R

                            #region Add Random Locations
                            /*
                                The x,y,z coordinates are your characters position to find out your custom facets x,y,z
                                coordinates: in-game - goto the location and type,' [where ' and then plug those numbers
                                below. The numbers will appear in the x,y,z order in-game so its relatively easy to do.

                                case 0: from.MoveToWorld(new Point3D(from.X+1, from.Y, from.Z), from.Map); break;
                                case 1: from.MoveToWorld(new Point3D(from.X+2, from.Y, from.Z), from.Map); break;
                                case 2: from.MoveToWorld(new Point3D(from.X+3, from.Y, from.Z), from.Map); break;
                                case 3: from.MoveToWorld(new Point3D(from.X, from.Y+1, from.Z), from.Map); break;
                                case 4: from.MoveToWorld(new Point3D(from.X, from.Y+2, from.Z), from.Map); break;
                                case 5: from.MoveToWorld(new Point3D(from.X, from.Y+3, from.Z), from.Map); break;
                             
                                Example On How To Edit This:
                                case 0: from.MoveToWorld(new Point3D(from.EditX, from.EditY, from.EditZ), from.Map); break;

                                Leaving the settings: ' from.X ', ' from.Y ', and the ' from.Z ' spawns your character on itself;
                                that is your location. Adding a ' +3 ' to make ' from.Y+3 ' spawns your character 3 tiles away
                                from your itself on the Y axis of the game; likewise adding a ' +2 ' to make ' from.X+2 ' spawns
                                your character 2 tiles away from itself on the X axis of the game. Z is just altitude so its best
                                to leave the Z axis of the game at ' from.Z ', by default this will move your character wherever
                                the variables below takes you, but leaves your feet touching the ground at all times.
                            */
                            #endregion Edited By: A.A.R

                            #endregion Edited By: A.A.R

                            case 0: from.MoveToWorld(new Point3D(from.X + 1, from.Y, from.Z), from.Map); break;
                            case 1: from.MoveToWorld(new Point3D(from.X + 2, from.Y, from.Z), from.Map); break;
                            case 2: from.MoveToWorld(new Point3D(from.X + 3, from.Y, from.Z), from.Map); break;
                            case 3: from.MoveToWorld(new Point3D(from.X, from.Y + 1, from.Z), from.Map); break;
                            case 4: from.MoveToWorld(new Point3D(from.X, from.Y + 2, from.Z), from.Map); break;
                            case 5: from.MoveToWorld(new Point3D(from.X, from.Y + 3, from.Z), from.Map); break;
                        }

                        break;
                    }

                #endregion Edited By: A.A.R

                #region retrievepets
                //Makes It Easier For Players To Retrieve Their Pets

                case ("retrievepets"):
                    {
                        List<Mobile> summonablePets = CalculateSummonablePets(args.Mobile);

                        if (summonablePets.Count > 0)
                        {
                            args.Mobile.SendGump(new GM_RetrievePet(summonablePets));
                            Say(String.Format("Would you like me to summon your pets {0}?", args.Mobile.Name));
                        }
                        else
                        {
                            Say("You don't have any eligible pets to summon!");
                        }

                        break;
                    }

                #endregion Edited By: Morxeton

                #region retrievebody
                //Makes It Easier For Players To Retrieve Their Corpses

                case ("retrievebody"):
                    {
                        Container corpse = from.Corpse;

                        if (from.Corpse == null)
                        {
                            Say(String.Format("HA! Thou art trying to fool me!! This will teach you!", args.Mobile.Name));
                            from.BoltEffect(0);
                            from.SendMessage("You feel a big jolt of electricity!");
                            from.Damage(Utility.Random(20, 55));
                            return;
                        }

                        Effects.SendLocationEffect(new Point3D(from.X, from.Y, from.Z), from.Map, 0x3709, 13);
                        from.PlaySound(0x208);

                        Point3D fromLoc = corpse.Location;
                        corpse.MoveToWorld(from.Location, from.Map);
                        break;
                    }

                #endregion Edited By: A.A.R

                #region accounthelp
                //Allows Players To View Their Account Info And Change Their Password

                case ("accounthelp"):
                    {
                        args.Mobile.SendGump(new AccountLogin(args.Mobile));
                        Say(String.Format("Please verify your account login information. Thank you.", args.Mobile.Name));
                        break;
                    }

                #endregion Edited By: A.A.R

            }
        }

        #region retrievepets: CalculateSummonablePets

        private List<Mobile> CalculateSummonablePets(Mobile from)
        {
            List<Mobile> summonablePets = new List<Mobile>();

            PlayerMobile pm = (PlayerMobile)from;

            foreach (Mobile pet in pm.AllFollowers)
            {
                if (!(pet is IMount) || ((IMount)pet).Rider == null)
                {
                    summonablePets.Add(pet);
                }
            }

            return summonablePets;
        }

        #endregion Edited By: Morxeton

        #endregion Edited By: A.A.R

        #region Click The NPC To Open Up A Gump

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new GameMaster_Entry(from, this));
        }

        #endregion Edited By: A.A.R

        public override bool ClickTitle { get { return false; } }
        public override bool IsActiveVendor { get { return true; } }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            Mobile m = from;
            PlayerMobile mobile = m as PlayerMobile;

            from.SendMessage("I appreciate the offer, but I do this job out of the love for the game.");
            return false;
        }

        public GameMaster_GM(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version

            #region NPC Auto-Deletion Timer

            writer.Write((DateTime)m_npcAutoDelete);

            #endregion Edited By: A.A.R
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        #region NPC Auto-Deletion Timer

                        m_npcAutoDelete = reader.ReadDateTime();

                        #endregion Edited By: A.A.R
                    }
                    break;

                default:
                    {
                        
                    }
                    break;
            }
        }
    }
}



