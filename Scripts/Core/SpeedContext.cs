using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Network;

namespace Server.Mobiles
{
	public class SpeedContext
	{
        public enum SpeedState
        {
            Slow = -1,
            None = 0,
            Fast = 1
        }

        private SpeedState m_Speed;
        private bool m_Permanent;
        //Some Systems could not use predefined timespans, therefore they need to use permanent contexts, which they have to remove again. Permanend contexts have a higher priority than timed ones and can't be compensated;
        private string m_Name;
        private Mobile m_Mobile;
        private SpeedTimer m_Timer;

        public SpeedState Speed
        {
            get { return m_Speed; }            
        }

        public SpeedTimer STimer
        {
            get { return m_Timer; }
        }

        public string Name
        {
            get { return m_Name; }
        }

        public Mobile Mobile
        {
            get { return m_Mobile; }
        }

        public bool Permanent
        {
            get { return m_Permanent; }
        }

        public SpeedContext(Mobile m, SpeedState speed, TimeSpan time, bool permanent, string name)
        {
            m_Mobile = m;
            m_Speed = speed;
            m_Name = name;
            m_Permanent = permanent;

            if (!permanent)
            {
                m_Timer = new SpeedTimer(this, time);
                m_Timer.Start();
            }

            if (!m_Table.ContainsKey(m))
                m_Table.Add(m, new List<SpeedContext>());

          /*foreach (SpeedContext c in m_Table[m]) 
            {
                if (c.Name == name)
                    RemoveContext(c);
            }*/

            RemoveContext(m, name); //If a new context with the same name as the old one gets added, the old will be replaced by the new one

            m_Table[m].Add(this);

            UpdateSpeed(m);
        }

        public SpeedContext(Mobile m, SpeedState speed, string name)
            : this(m, speed, TimeSpan.Zero, true, name)
        {
        }

        public SpeedContext(Mobile m, SpeedState speed, TimeSpan duration, string name)
            : this(m, speed, duration, false, name)
        {
        }

        /*public static bool AddSpeedContext(Mobile m, SpeedState speed, TimeSpan time, bool permanent, string name)
        {
            SpeedContext context = new SpeedContext(m, speed, time, permanent, name);

            if (!m_Table.ContainsKey(m))
                m_Table.Add(m, new List<SpeedContext>());

            foreach (SpeedContext c in m_Table[m]) //If a new context with the same name as the old one gets added, the old will be replaced by the new one
            {
                if (c.Name == name)
                    RemoveContext(c);
            }

            m_Table[m].Add(context);

            UpdateSpeed(m);
        }*/

        public static void RemoveContext(List<SpeedContext> list)
        {
            foreach (SpeedContext c in list)
            {
                if (!c.Permanent)
                    c.STimer.Stop();

                m_Table[c.Mobile].Remove(c);

                UpdateSpeed(c.Mobile);
            }
        }

        public static void RemoveContext(Mobile m, string name)
        {
            if (m == null || name == null)
                return;

            List<SpeedContext> list = new List<SpeedContext>();

            if (m_Table.ContainsKey(m))
            {
                if (m_Table[m] != null)
                {
                    foreach (SpeedContext c in m_Table[m])
                    {
                        if (c != null && c.Name == name)
                            list.Add(c);
                    }

                    if(list.Count > 0)
                        RemoveContext(list);
                }
            }
        }

        public static SpeedState ComputeSpeed(Mobile m)
        {
            int speed = 0;

            foreach (SpeedContext c in m_Table[m])
            {
                speed += (int)c.Speed;
                //Console.WriteLine("Speed of {0} changed by {1}. It is now {2}.", m.RawName, (int)c.Speed, speed);
            }

            if (speed > 1)
                speed = 1;
            else if (speed < -1)
                speed = -1;

            return (SpeedState)speed;
        }

        public static void UpdateSpeed(Mobile m)
        {
            switch (ComputeSpeed(m))
            {
                case SpeedState.Slow:
                    m.Send(SpeedControl.WalkSpeed);
                    break;
                default:
                case SpeedState.None:
                    m.Send(SpeedControl.Disable);
                    break;
                case SpeedState.Fast:
                    m.Send(SpeedControl.MountSpeed);
                    break;
            }
        }

        private static Dictionary<Mobile, List<SpeedContext>> m_Table = new Dictionary<Mobile, List<SpeedContext>>();

        public class SpeedTimer : Timer
        {
            private SpeedContext m_Context;

            public SpeedTimer(SpeedContext context, TimeSpan duration)
                : base(duration)
            {
                m_Context = context;
            }

            protected override void OnTick()
            {
                StopTimer();
            }

            public void StopTimer()
            {
                SpeedContext.RemoveContext(m_Context.Mobile, m_Context.Name);
                Stop();
            }
        }
	}
}