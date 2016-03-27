using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Targeting;
using Server.Gumps;
using Server.Items;

namespace Server.Targets
{
    public class AIControlMobileTarget : Target
    {
        private readonly List<BaseAI> m_List;
        private readonly OrderType m_Order;
        private readonly BaseCreature m_Mobile;
        
        public AIControlMobileTarget(BaseAI ai, OrderType order)
            : base(-1, false, (order == OrderType.Attack ? TargetFlags.Harmful : TargetFlags.None))
        {
            m_List = new List<BaseAI>();
            m_Order = order;

            AddAI(ai);
            m_Mobile = ai.m_Mobile;
        }

        public OrderType Order
        {
            get
            {
                return m_Order;
            }
        }
        public void AddAI(BaseAI ai)
        {
            if (!m_List.Contains(ai))
                m_List.Add(ai);
        }
        /*
        protected override void OnTarget(Mobile from, object o)
        {
            if (o is Mobile)
            {
                Mobile m = (Mobile)o;
                for (int i = 0; i < m_List.Count; ++i)
                    m_List[i].EndPickTarget(from, m, m_Order);
            }
        }
         */

        protected override void OnTarget(Mobile from, object o)
        {
            //if ( o is Mobile )
            //{
            for (int i = 0; i < m_List.Count; ++i)
            {
                ((BaseAI)m_List[i]).EndPickTarget(from, o, m_Order);
            }
            //}
        }
    }
}