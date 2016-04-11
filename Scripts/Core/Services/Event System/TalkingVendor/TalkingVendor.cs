using Server.Items;
using Server.ContextMenus;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class TalkingVendorEntry : ContextMenuEntry
    {
        private TalkingVendor m_vendor;
        private Mobile m_from;

        public TalkingVendorEntry(Mobile from, TalkingVendor vendor) : base(6146)
        {
            m_vendor = vendor;
            m_from = from;
            Enabled = from.CanSee(m_vendor) && from.InRange(m_vendor.Location, 5);
        }

        public override void OnClick()
        {
            m_vendor.BeginTalk(m_from, 0);
        }
    }

    public class TalkingVendor : BaseCreature, IToggle
    {

        #region constructors and other stuff
        public override bool PlayerRangeSensitive { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool CanBeDamaged()
        {
            return false;
        }

        [Constructable]
        public TalkingVendor() : base(AIType.AI_Vendor, FightMode.None, 2, 1, 0.5, 2)
        {
            RangeHome = 2;
            Hue = Utility.RandomSkinHue();
            NameHue = 0x35;

            PackItem(new TanBook());
            Backpack.Visible = false;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }
            InitOutfit();

        }

        public TalkingVendor(Serial serial) : base(serial)
        {
        }

        #endregion

        #region outfit
        public void InitOutfit()
        {
            switch (Utility.Random(3))
            {
                case 0: AddItem(new FancyShirt(GetRandomHue())); break;
                case 1: AddItem(new Doublet(GetRandomHue())); break;
                case 2: AddItem(new Shirt(GetRandomHue())); break;
            }

            switch (Utility.Random(4))
            {
                case 0: AddItem(new Shoes(GetShoeHue())); break;
                case 1: AddItem(new Boots(GetShoeHue())); break;
                case 2: AddItem(new Sandals(GetShoeHue())); break;
                case 3: AddItem(new ThighBoots(GetShoeHue())); break;
            }

            int hairHue = Utility.RandomHairHue();

            Utility.AssignRandomHair(this, hairHue);
            Utility.AssignRandomFacialHair(this, hairHue);

            if (Female)
            {
                switch (Utility.Random(6))
                {
                    case 0: AddItem(new ShortPants(GetRandomHue())); break;
                    case 1:
                    case 2: AddItem(new Kilt(GetRandomHue())); break;
                    case 3:
                    case 4:
                    case 5: AddItem(new Skirt(GetRandomHue())); break;
                }
            }
            else
            {
                switch (Utility.Random(2))
                {
                    case 0: AddItem(new LongPants(GetRandomHue())); break;
                    case 1: AddItem(new ShortPants(GetRandomHue())); break;
                }
            }
        }



        public virtual int GetRandomHue()
        {
            switch (Utility.Random(5))
            {
                default:
                case 0: return Utility.RandomBlueHue();
                case 1: return Utility.RandomGreenHue();
                case 2: return Utility.RandomRedHue();
                case 3: return Utility.RandomYellowHue();
                case 4: return Utility.RandomNeutralHue();
            }
        }

        public virtual int GetShoeHue()
        {
            if (0.1 > Utility.RandomDouble())
                return 0;

            return Utility.RandomNeutralHue();
        }

        #endregion

        private int m_vendorid = Utility.RandomMinMax(0, 65000);
        private Item m_link;
        private TVController m_talklink;
        private int m_messagehue;
        private bool m_ShowMessages = true;
        private bool m_AcceptMoney = true;
        private bool m_ShowContextMenu = true;
        private bool m_FaceToPlayer = true;


        #region command propertys
        [CommandProperty(AccessLevel.GameMaster)]
        public bool FaceToPlayer
        {
            get { return m_FaceToPlayer; }
            set { m_FaceToPlayer = value; }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool ShowContextMenu
        {
            get { return m_ShowContextMenu; }
            set { m_ShowContextMenu = value; }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool AcceptMoney
        {
            get { return m_AcceptMoney; }
            set { m_AcceptMoney = value; }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool ShowMessages
        {
            get { return m_ShowMessages; }
            set { m_ShowMessages = value; }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int VendorID
        {
            get { return m_vendorid; }
            set { m_vendorid = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Item EventLink
        {
            get { return m_link; }
            set { m_link = value; }
        }

        public IToggler Link
        {
            get { return m_link as IToggler; }
            set
            {
                m_link = value as Item;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TVController TalkLink
        {
            get { return m_talklink; }
            set { m_talklink = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MessageHue
        {
            get { return m_messagehue; }
            set { m_messagehue = value; }
        }
        #endregion

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (from is PlayerMobile)
            {
                PlayerMobile player = from as PlayerMobile;
                if (m_FaceToPlayer)
                {
                    Direction = GetDirectionTo(player);
                }
                if (dropped is TVInvoker && (dropped as TVInvoker).VendorID == m_vendorid)
                {
                    InvokeInterview(dropped as TVInvoker, from);
                    dropped.Delete();
                }
                else if (dropped is Gold && m_AcceptMoney)
                {
                    if (m_ShowMessages)
                    {
                        PublicOverheadMessage(0, m_messagehue, false, "I like money");
                    }
                    dropped.Delete();
                }
                else
                {
                    if (m_ShowMessages)
                    {
                        PublicOverheadMessage(0, m_messagehue, false, "I dont need this");
                    }
                    return false;
                }
                return true;
            }
            return false;
            // na false mozna tohle return base.OnDragDrop( from, dropped );
        }

        public void BeginTalk(Mobile to, int index)
        {
            if (m_talklink != null && !m_talklink.Deleted)
            {
                TVController controler = m_talklink.GetController(Rnd(), index);
                if (controler != null)
                {
                    if (controler.Message != null)
                    {
                        PublicOverheadMessage(0, m_messagehue, false, controler.Message);
                    }
                    if (m_link != null && !m_link.Deleted)
                    {
                        Link.Toggle(controler.INvokesEvent, to, Rnd());
                    }
                }
            }

        }

        public void InvokeInterview(TVInvoker invoker, Mobile from)
        {
            if (invoker.ReplyID != 0)
            {
                BeginTalk(from, invoker.ReplyID);
            }
        }
        public override bool CanBeBeneficial(Mobile target)
        {
            return false;
        }
        public override bool CanBeBeneficial(Mobile target, bool message)
        {
            return false;
        }
        public override bool CanBeBeneficial(Mobile target, bool message, bool allowDead)
        {
            return false;
        }
        public override bool CanBeHarmful(Mobile target)
        {
            return false;
        }
        public override bool CanBeHarmful(Mobile target, bool message)
        {
            return false;
        }
        public override bool CanBeHarmful(Mobile target, bool message, bool ignoreOurBlessedness)
        {
            return false;
        }

        #region serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2);//version;
            writer.Write((Item)m_link);
            writer.Write((Item)m_talklink);
            writer.Write((int)m_vendorid);
            writer.Write((int)m_messagehue);
            writer.Write((bool)m_AcceptMoney);
            writer.Write((bool)m_ShowMessages);
            writer.Write((bool)m_ShowContextMenu);
            writer.Write((bool)m_FaceToPlayer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_link = reader.ReadItem() as BaseToggler;
            m_talklink = reader.ReadItem() as TVController;
            m_vendorid = reader.ReadInt();
            m_messagehue = reader.ReadInt();
            if (version >= 1)
            {
                m_AcceptMoney = reader.ReadBool();
                m_ShowMessages = reader.ReadBool();
                m_ShowContextMenu = reader.ReadBool();
            }
            if (version >= 2)
                m_FaceToPlayer = reader.ReadBool();
        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (from.Alive && m_ShowContextMenu)
            {
                list.Add(new TalkingVendorEntry(from, this));
            }

            base.AddCustomContextEntries(from, list);
        }

        protected int Rnd()
        {
            return (Utility.Random(int.MaxValue));
        }

        #endregion
    }
}