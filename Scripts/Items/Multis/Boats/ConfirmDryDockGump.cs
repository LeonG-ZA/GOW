using System;
using Server.Gumps;
using Server.Network;
using Server.T2AConfiguration;

namespace Server.Multis
{
    public class ConfirmDryDockGump : Gump
    {
        private readonly Mobile m_From;
        private readonly BaseBoat m_Boat;
        public ConfirmDryDockGump(Mobile from, BaseBoat boat)
            : base(150, 200)
        {
            m_From = from;
            m_Boat = boat;

            m_From.CloseGump(typeof(ConfirmDryDockGump));

            AddPage(0);

            AddBackground(0, 0, 220, 170, 5054);
            AddBackground(10, 10, 200, 150, 3000);

            AddHtmlLocalized(20, 20, 180, 80, 1018319, true, false); // Do you wish to dry dock this boat?

            AddHtmlLocalized(55, 100, 140, 25, 1011011, false, false); // CONTINUE
            AddButton(20, 100, 4005, 4007, 2, GumpButtonType.Reply, 0);

            AddHtmlLocalized(55, 125, 140, 25, 1011012, false, false); // CANCEL
            AddButton(20, 125, 4005, 4007, 1, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (T2AConfig.T2AClassicHarborMEnabled)
            {
                //if (info.ButtonID == 2)
                //   m_Boat.EndDryDock(m_From);
            }
            else
            {
                if (info.ButtonID == 2)
                    m_Boat.EndDryDock(m_From);
            }
        }
    }
}