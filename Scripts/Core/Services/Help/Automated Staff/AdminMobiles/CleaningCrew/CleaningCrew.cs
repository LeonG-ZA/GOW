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
    public class CleaningCrew : BaseGuildmaster
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        public override bool ClickTitle { get { return false; } }
        public override NpcGuild NpcGuild { get { return NpcGuild.MerchantsGuild; } }

        #region NPC Auto-Deletion Timer

        private DateTime m_npcAutoDelete;

        #endregion Edited By: A.A.R

        #region Automated Greetings For All Approaching Players

        private static bool m_Talked;

        string[] npcSpeech = new string[]
        { 
            "Trash Bags For Sale!",
        };

        #endregion Edited By: A.A.R

        [Constructable]
        public CleaningCrew(): base("merchant")
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

            Title = "[CC]";
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

            CantWalk = false;
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

//----------NPC Auto-Deletion Timer & Server Garbage Collecting----------//

            m_npcAutoDelete = DateTime.UtcNow + TimeSpan.FromSeconds(180);
        }

        private DateTime m_NextPickup;

        public override void OnThink()
        {
            if (m_npcAutoDelete <= DateTime.UtcNow)
            {
                this.Delete();
            }

            base.OnThink();

            if (DateTime.UtcNow < m_NextPickup)
                return;

            m_NextPickup = DateTime.UtcNow + TimeSpan.FromSeconds(2.5 + (2.5 * Utility.RandomDouble()));

            ArrayList Trash = new ArrayList();
            foreach (Item item in this.GetItemsInRange(5))
            {
                if (item.Movable)
                    Trash.Add(item);
            }

            Type[] exemptlist = new Type[] 
            { 
                #region Item Deletion Exemption List

                typeof(MandrakeRoot), typeof(Ginseng), typeof(AxeOfTheHeavens) 

                #endregion Edited By: A.A.R
            };

            bool TrashIt = true;
            for (int i = 0; i < Trash.Count; i++)
            {
                for (int j = 0; j < exemptlist.Length; j++)
                {
                    if ((Trash[i]).GetType() == exemptlist[j])
                        TrashIt = false;
                }
                if (TrashIt)
                    ((Item)Trash[i]).Delete();
                TrashIt = true;
            }
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
            m_SBInfos.Add(new SBCleaningCrew());
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

//-----Player Jailing For Cussing & Item Deletion On Drag And Drop-------//

        #region Unacceptable Player Speech

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(this.Location, 5))
                return true;
            return base.HandlesOnSpeech(from);
        }

        private List<string> m_UnacceptableWords = new List<string>(new string[]
        {
            #region Prohibited Word List

            "ass","asshole","blowjob","bitch","bitches","biatch","biatches","breasts","chinc","chink","cunnilingus","cum","cumstain","cocksucker","clit",
            "chigaboo","cunt","clitoris","cock","dick","dickhead","dyke","dildo","fuck","fucktard","felatio","fag","faggot","hitler","jigaboo","jizzm",
            "jizz","jiz","jism","jiss","jis","jerkoff","jackoff", "kyke","kike","klit","lezbo","lesbo","nigga","niggas","nigger","piss","penis","prick",
            "pussy","retard","retarded","spic","shit","shithead","spunk","spunker","smeg","smegg","twat","tit","tits","titties", "tittys","titie","tities",
            "tity","tard","vagina","wop","wigger","wiger"

            #endregion Edited By: A.A.R
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
                #region Location To Send Players If They Say Unacceptable Words

                from.MoveToWorld(new Point3D(1483, 1617, 20), Map.Trammel);

                #endregion Edited By: A.A.R

                from.SendMessage("You Have Been Jailed For Using Inappropriate Language And/Or Out Of Character, Real-World, References In Front Of A Staff Member");
                return;
            }
        }

        #endregion Edited By: Morxeton

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            Mobile m = from;
            PlayerMobile mobile = m as PlayerMobile;
            dropped.Delete();

            SpeechHue = Utility.RandomDyedHue();
            this.Say("Thank you! I will dispose of your items immediately!!");
            return true;
        }

//------------------------------------------------------------------------//

        public CleaningCrew(Serial serial): base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version 0

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