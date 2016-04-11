using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Commands
{
    public class EventCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("EventShowLink", AccessLevel.GameMaster, new CommandEventHandler(EventShowLink_OnCommand));
            CommandSystem.Register("EventSL", AccessLevel.GameMaster, new CommandEventHandler(EventShowLink_OnCommand));
            CommandSystem.Register("EventCreateLink", AccessLevel.GameMaster, new CommandEventHandler(EventCreateLink_OnCommand));
            CommandSystem.Register("EventCL", AccessLevel.GameMaster, new CommandEventHandler(EventCreateLink_OnCommand));
            CommandSystem.Register("EventFollowLink", AccessLevel.GameMaster, new CommandEventHandler(EventFollowLink_OnCommand));
            CommandSystem.Register("EventFL", AccessLevel.GameMaster, new CommandEventHandler(EventFollowLink_OnCommand));
        }

        [Usage("EventShowLink <Ttxmans EventSystem item> | EventSL<...>")]
        [Description("Show numbers over items in link")]
        private static void EventShowLink_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new EventShowLinkTarget();
            e.Mobile.SendMessage("Please target a Ttxmans EventSystem item");
        }
        private class EventShowLinkTarget : Target
        {
            public EventShowLinkTarget()
                : base(20, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                if (!(targ is IToggle) || !(targ is Item) || (targ as Item).Deleted)
                {
                    from.SendMessage("You can only target Ttxmans EventSystem items");
                    from.Target = new EventShowLinkTarget();
                    return;
                }
                IToggle togle = targ as IToggle;
                ArrayList l = new ArrayList();
                bool cyclic = false;
                while (togle != null)
                {
                    if (l.Contains(togle))
                    {
                        cyclic = true;
                        break;
                    }
                    l.Add(togle);
                    togle = togle.EventLink as IToggle;
                }
                Item itm;
                for (int i = 0; i < l.Count; i++)
                {
                    itm = l[i] as Item;
                    if (itm == null)
                    {
                        continue;
                    }
                    if (!cyclic || i < l.Count - 1)
                    {
                        itm.PublicOverheadMessage(0, 143, false, i.ToString());
                    }
                    else
                    {
                        itm.PublicOverheadMessage(0, 143, false, "CyclicLink");
                    }
                }
            }

        }

        [Usage("EventCreateLink <Ttxmans EventSystem item> | EventCL<...>")]
        [Description("Creates a link")]
        private static void EventCreateLink_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new EventCreateLinkTarget(null);
            e.Mobile.SendMessage("Please target a Ttxmans EventSystem item");
        }
        private class EventCreateLinkTarget : Target
        {
            private IToggle m_toggle = null;

            public EventCreateLinkTarget(IToggle toggle) : base(20, false, TargetFlags.None)
            {
                m_toggle = toggle;
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                if (!(targ is IToggle) || !(targ is Item) || (targ as Item).Deleted)
                {
                    from.SendMessage("You can only target Ttxmans EventSystem items");
                    from.Target = new EventCreateLinkTarget(m_toggle);
                    return;
                }
                if (m_toggle != null && (targ is BaseToggle))
                {
                    from.SendMessage("Now you can only target Ttxmans EventSystem togglers");
                    from.Target = new EventCreateLinkTarget(m_toggle);
                    return;
                }
                else if ((targ is BaseToggle) && m_toggle == null)
                {
                    m_toggle = targ as IToggle;
                    from.SendMessage("Please target next Ttxmans EventSystem toggler");
                    from.Target = new EventCreateLinkTarget(m_toggle);
                    return;
                }

                IToggle togle = targ as IToggle;
                if (m_toggle != null)
                {
                    m_toggle.EventLink = togle as Item;
                }
                m_toggle = togle;

                from.SendMessage("Please target next Ttxmans EventSystem toggler");
                from.Target = new EventCreateLinkTarget(m_toggle);

            }

        }

        [Usage("EventFollowLink <Ttxmans EventSystem item> | EventFL<...>")]
        [Description("Creates a link")]
        private static void EventFollowLink_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new EventFollowLinkTarget();
            e.Mobile.SendMessage("Please target a Ttxmans EventSystem item");
        }

        private class EventFollowLinkTarget : Target
        {
            public EventFollowLinkTarget() : base(20, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                if (!(targ is IToggle) || !(targ is Item) || (targ as Item).Deleted)
                {
                    from.SendMessage("You can only target Ttxmans EventSystem items");
                    from.Target = new EventFollowLinkTarget();
                    return;
                }

                Item trg = (targ as IToggle).EventLink as Item;
                if (trg == null)
                {
                    from.SendMessage("This Ttxmans EventSystem item is at the end of link");
                    return;
                }
                Map map = trg.Map;
                if (map == null || map == Map.Internal)
                {
                    return;
                }
                from.Map = map;
                from.Location = trg.Location;
                from.SendMessage("Please target next Ttxmans EventSystem item");
                from.Target = new EventFollowLinkTarget();
            }

        }
    }
}
