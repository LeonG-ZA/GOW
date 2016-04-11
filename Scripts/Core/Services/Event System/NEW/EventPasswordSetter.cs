using Server.Mobiles;

namespace Server.Items
{
    public class EventPasswordSetter : BaseToggler
    {
        public enum IntPasswordType : byte
        {
            RandomNumber1kTO100k,
            RandomWord6chars,
            RandomWordFromPass
        }
        public enum IntReactionType : byte
        {
            TalkWithDungTalkingToggler,
            TalkWithTalkingVendor
        }
        private string m_pass;
        private string[] m_passwords = new string[] { };
        private IntPasswordType m_PasswordType = IntPasswordType.RandomNumber1kTO100k;
        private IntReactionType m_ReactionType = IntReactionType.TalkWithDungTalkingToggler;
        private TalkingVendor m_TalkingVendor;
        private EventTalkingToggler m_DungTalkingToggler;
        private EventSpeechToggle m_TargetedDungSpeechToggle;

        [CommandProperty(AccessLevel.GameMaster)]
        public string Pass
        {
            get { return m_pass; }
            set
            {
                if (value == null)
                {
                    m_pass = "";
                    m_passwords = new string[] { };
                }
                else
                {
                    m_pass = value.ToLower();
                    m_passwords = m_pass.Split(';');
                }
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public IntPasswordType PasswordType
        {
            get { return m_PasswordType; }
            set { m_PasswordType = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public IntReactionType ReactionType
        {
            get { return m_ReactionType; }
            set { m_ReactionType = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TalkingVendor TalkingVendor
        {
            get { return m_TalkingVendor; }
            set { m_TalkingVendor = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public EventTalkingToggler DungTalkingToggler
        {
            get { return m_DungTalkingToggler; }
            set { m_DungTalkingToggler = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public EventSpeechToggle TargetedDungSpeechToggle
        {
            get { return m_TargetedDungSpeechToggle; }
            set { m_TargetedDungSpeechToggle = value; }
        }

        [Constructable]
        public EventPasswordSetter() : this(0x1726)
        {
        }

        [Constructable]
        public EventPasswordSetter(int ItemID) : base(ItemID)
        {
            m_pass = "";
            Name = "EventPasswordSetter";
        }


        public EventPasswordSetter(Serial serial) : base(serial)
        {
        }

        private static string ValidChars = "abcdefghijklmnopqrstuvwz1234567890";
        public override bool Toggle(byte state, Mobile who, int sid)
        {
            if (sid == lsid)
            {
                return false;
            }
            lsid = sid;


            bool ven = true;
            if (state > 0)
            {
                string text = "";

                switch (m_PasswordType)
                {
                    case IntPasswordType.RandomNumber1kTO100k:
                        {
                            text = Utility.RandomMinMax(1000, 100000).ToString();
                            break;
                        }
                    case IntPasswordType.RandomWord6chars:
                        {
                            int maxl = ValidChars.Length - 1;
                            text = "";
                            for (int i = 0; i < 6; i++)
                            {
                                text += ValidChars[Utility.RandomMinMax(0, maxl)];
                            }
                            break;
                        }
                    case IntPasswordType.RandomWordFromPass:
                        {
                            if (m_passwords == null || m_passwords.Length == 0)
                            {
                                text = "";
                            }
                            else
                            {
                                text = m_passwords[Utility.RandomMinMax(0, m_passwords.Length - 1)];
                            }
                            break;
                        }
                }
                if (m_TargetedDungSpeechToggle != null)
                {
                    m_TargetedDungSpeechToggle.Pass = text;
                }

                switch (m_ReactionType)
                {
                    case IntReactionType.TalkWithDungTalkingToggler:
                        {
                            if (m_DungTalkingToggler != null)
                            {
                                m_DungTalkingToggler.Messages = text;
                            }
                            break;
                        }
                    case IntReactionType.TalkWithTalkingVendor:
                        {
                            if (m_TalkingVendor != null)
                            {
                                m_TalkingVendor.PublicOverheadMessage(0, m_TalkingVendor.MessageHue, false, text);
                            }
                            break;
                        }
                }
            }
            else
            {
                ven = true;
            }

            ven = ven & ((Link == null || Link.Deleted) ? true : Link.Toggle(state, who, sid));
            return true & ven;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);//version
            writer.Write((string)m_pass);
            writer.Write((byte)m_PasswordType);
            writer.Write((byte)m_ReactionType);
            writer.Write(m_TalkingVendor);
            writer.Write(m_TargetedDungSpeechToggle);
            writer.Write(m_DungTalkingToggler);

        }


        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_pass = reader.ReadString();
            m_PasswordType = (IntPasswordType)reader.ReadByte();
            m_ReactionType = (IntReactionType)reader.ReadByte();
            m_TalkingVendor = reader.ReadMobile() as TalkingVendor;
            m_TargetedDungSpeechToggle = reader.ReadItem() as EventSpeechToggle;
            m_DungTalkingToggler = reader.ReadItem() as EventTalkingToggler;
            m_passwords = m_pass.Split(';');
        }
    }
}