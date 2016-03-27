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
    public class ServerGreeter : BaseGuildmaster
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        public override bool ClickTitle { get { return false; } }
        public override NpcGuild NpcGuild { get { return NpcGuild.BardsGuild; } }

        private static bool m_Talked;

        string[] npcSpeech = new string[]
        { 
            "Trash Bags For Sale!",
        };

        [Constructable]
        public ServerGreeter(): base("merchant")
        {

//----------This Randomizes The Sex Of The NPC For Individuality---------//

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

//--------This Creates A Random Look To The NPC For Individuality--------//

            Title = "[SG]";
            NameHue = 11;

            VendorAccessLevel = AccessLevel.Player;
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

//-------------This Toggles The NPC Movement: On Or Off------------------//

            CantWalk = true;
            CanSwim = false;

//--------------This Makes The NPC Equip HandHeld Items------------------//

            switch (Utility.Random(3))
            {
                case 0: AddItem(new BookOfNinjitsu()); break;
                case 1: AddItem(new BookOfBushido()); break;
                case 2: AddItem(new BookOfChivalry()); break;
            }

//-------------This Sets What Clothes The NPC Will Wear------------------//

            AddItem(new ShortPants(Utility.RandomNeutralHue()));
            AddItem(new Shirt(Utility.RandomNeutralHue()));
            AddItem(new Sandals(Utility.RandomNeutralHue()));
        }

//------This Gives The NPC Some Active Emotion In The Game---------------//

        public void Emote()
        {
            switch (Utility.Random(3))
            {
                case 0:
                    PlaySound(Female ? 785 : 1056);
                    Say("*cough!*");
                    break;
                case 1:
                    PlaySound(Female ? 818 : 1092);
                    Say("*sniff*");
                    break;
                default:
                    break;
            }
        }

//------This Code Makes This NPC Behave As An AccessLevel Vendor---------//

        public override bool IsActiveVendor { get { return true; } }

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBServerGreeter());
        }

//------Automated Greeting Timer For All Approaching Players-------------//

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {       
            if (m_Talked == false)
            {
                if (m.InRange(this, 4))
                {
                    m_Talked = true;
                    SayRandom(npcSpeech, this);
                    this.Move(GetDirectionTo(m.Location));

                    SpamTimer t = new SpamTimer();
                    t.Start();
                }
            }
        }

        private class SpamTimer : Timer
        {
            public SpamTimer(): base(TimeSpan.FromSeconds(20))
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

//-----Server Players Get Jailed For Saying Inappropriate Words----------//

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(this.Location, 5))
                return true;
            return base.HandlesOnSpeech(from);
        }

        private List<string> m_UnacceptableWords = new List<string>(new string[]
        {
            "ass","asshole","blowjob","bitch","bitches","biatch","biatches","breasts","chinc","chink","cunnilingus","cum","cumstain","cocksucker","clit",
            "chigaboo","cunt","clitoris","cock","dick","dickhead","dyke","dildo","fuck","fucktard","felatio","fag","faggot","hitler","jigaboo","jizzm",
            "jizz","jiz","jism","jiss","jis","jerkoff","jackoff", "kyke","kike","klit","lezbo","lesbo","nigga","niggas","nigger","piss","penis","prick",
            "pussy","retard","retarded","spic","shit","shithead","spunk","spunker","smeg","smegg","twat","tit","tits","titties", "tittys","titie","tities",
            "tity","tard","vagina","wop","wigger","wiger"
        });

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

                from.MoveToWorld(new Point3D(1483, 1617, 20), Map.Trammel);

                from.SendMessage("You Have Been Jailed For Using Inappropriate Language And/Or Out Of Character, Real-World, References In Front Of A Staff Member");
                return;
            }
        }

//-----NPC Talk Context Menu Selection On The Mobile For Tips------------//

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            list.Add(new GameMaster_Entry(from, this));
        }

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

//------------------------------------------------------------------------//

        public ServerGreeter(Serial serial): base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version 0      
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}