using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Buff.Icons
{
    public class BuffInfo
    {
        public static bool Enabled
        {
            get
            {
                return Core.ML;
            }
        }

        public static void Initialize()
        {
            if (Enabled)
            {
                EventSink.ClientVersionReceived += new ClientVersionReceivedHandler(delegate(ClientVersionReceivedArgs args)
                {
                    PlayerMobile pm = args.State.Mobile as PlayerMobile;
					
                    if (pm != null)
                        Timer.DelayCall(TimeSpan.Zero, pm.ResendBuffs);
                });
            }
        }

        #region Properties
        private readonly BuffIcon m_ID;
        public BuffIcon ID { get { return m_ID; } }

        private readonly int m_TitleCliloc;
        public int TitleCliloc { get { return m_TitleCliloc; } }

        private readonly int m_SecondaryCliloc;
        public int SecondaryCliloc { get { return m_SecondaryCliloc; } }

        private readonly TimeSpan m_TimeLength;
        public TimeSpan TimeLength { get { return m_TimeLength; } }

        private readonly DateTime m_TimeStart;
        public DateTime TimeStart { get { return m_TimeStart; } }

        private readonly Timer m_Timer;
        public Timer Timer { get { return m_Timer; } }

        private readonly bool m_RetainThroughDeath;
        public bool RetainThroughDeath { get { return m_RetainThroughDeath; } }

        private readonly TextDefinition m_Args;
        public TextDefinition Args { get { return m_Args; } }
        #endregion

        #region Constructors
        public BuffInfo(BuffIcon iconID, int titleCliloc)
            : this(iconID, titleCliloc, titleCliloc + 1)
        {
        }

        public BuffInfo(BuffIcon iconID, int titleCliloc, int secondaryCliloc)
        {
            m_ID = iconID;
            m_TitleCliloc = titleCliloc;
            m_SecondaryCliloc = secondaryCliloc;
        }

        public BuffInfo(BuffIcon iconID, int titleCliloc, TimeSpan length, Mobile m)
            : this(iconID, titleCliloc, titleCliloc + 1, length, m)
        {
        }

        //Only the timed one needs to Mobile to know when to automagically remove it.
        public BuffInfo(BuffIcon iconID, int titleCliloc, int secondaryCliloc, TimeSpan length, Mobile m)
            : this(iconID, titleCliloc, secondaryCliloc)
        {
            m_TimeLength = length;
            m_TimeStart = DateTime.UtcNow;

            m_Timer = Timer.DelayCall(length, new TimerCallback(
                delegate
                {
                    PlayerMobile pm = m as PlayerMobile;
                    if (pm == null)
                    {
                        return;
                    }

                    pm.RemoveBuff(this);
                }));
        }

        public BuffInfo(BuffIcon iconID, int titleCliloc, TextDefinition args)
            : this(iconID, titleCliloc, titleCliloc + 1, args)
        {
        }

        public BuffInfo(BuffIcon iconID, int titleCliloc, int secondaryCliloc, TextDefinition args)
            : this(iconID, titleCliloc, secondaryCliloc)
        {
            m_Args = args;
        }

        public BuffInfo(BuffIcon iconID, int titleCliloc, bool retainThroughDeath)
            : this(iconID, titleCliloc, titleCliloc + 1, retainThroughDeath)
        {
        }

        public BuffInfo(BuffIcon iconID, int titleCliloc, int secondaryCliloc, bool retainThroughDeath)
            : this(iconID, titleCliloc, secondaryCliloc)
        {
            m_RetainThroughDeath = retainThroughDeath;
        }

        public BuffInfo(BuffIcon iconID, int titleCliloc, TextDefinition args, bool retainThroughDeath)
            : this(iconID, titleCliloc, titleCliloc + 1, args, retainThroughDeath)
        {
        }

        public BuffInfo(BuffIcon iconID, int titleCliloc, int secondaryCliloc, TextDefinition args, bool retainThroughDeath)
            : this(iconID, titleCliloc, secondaryCliloc, args)
        {
            m_RetainThroughDeath = retainThroughDeath;
        }

        public BuffInfo(BuffIcon iconID, int titleCliloc, TimeSpan length, Mobile m, TextDefinition args)
            : this(iconID, titleCliloc, titleCliloc + 1, length, m, args)
        {
        }

        public BuffInfo(BuffIcon iconID, int titleCliloc, int secondaryCliloc, TimeSpan length, Mobile m, TextDefinition args)
            : this(iconID, titleCliloc, secondaryCliloc, length, m)
        {
            m_Args = args;
        }

        public BuffInfo(BuffIcon iconID, int titleCliloc, TimeSpan length, Mobile m, TextDefinition args, bool retainThroughDeath)
            : this(iconID, titleCliloc, titleCliloc + 1, length, m, args, retainThroughDeath)
        {
        }

        public BuffInfo(BuffIcon iconID, int titleCliloc, int secondaryCliloc, TimeSpan length, Mobile m, TextDefinition args, bool retainThroughDeath)
            : this(iconID, titleCliloc, secondaryCliloc, length, m)
        {
            m_Args = args;
            m_RetainThroughDeath = retainThroughDeath;
        }
        #endregion

        #region Convenience Methods
        public static void AddBuff(Mobile m, BuffInfo b)
        {
            PlayerMobile pm = m as PlayerMobile;

            if (pm != null)
                pm.AddBuff(b);
        }

        public static void RemoveBuff(Mobile m, BuffInfo b)
        {
            PlayerMobile pm = m as PlayerMobile;

            if (pm != null)
                pm.RemoveBuff(b);
        }

        public static void RemoveBuff(Mobile m, BuffIcon b)
        {
            PlayerMobile pm = m as PlayerMobile;

            if (pm != null)
                pm.RemoveBuff(b);
        }
        #endregion
    }
}