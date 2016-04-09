using System.Collections;
using Server.Commands;
using Server.Items;
using Server.Prompts;
using Server.Mobiles;
using Server.FeaturesConfiguration;

namespace Server
{
    public class Comm
    {
        public static void Initialize()
        {
            if (FeaturesConfig.FeatCommEnabled)
            {
                CommandSystem.Register("Comm", AccessLevel.Player, new CommandEventHandler(Comm_OnCommand));
            }
        }

        [Usage("Comm [string]")]
        [Description("If you have a comm unit equipped, will send a message to all others on the same facet as you with a like channel.")]
        private static void Comm_OnCommand(CommandEventArgs e)
        {
            string speechstring = e.ArgString;
            Mobile m = e.Mobile;
            if (m != null)
            {
                CommUnit unit = m.FindItemOnLayer(Layer.Earrings) as CommUnit;
                if (unit != null)
                {
                    bool towerinrange = false;
                    IPooledEnumerable eable = m.Map.GetItemsInRange(m.Location, 1000);
                    foreach (Item i in eable)
                    {
                        if (i is CommTower && towerinrange == false)
                        {
                            towerinrange = true;
                        }
                    }
                    eable.Free();
                    ArrayList list = new ArrayList();
                    foreach (Mobile mob in World.Mobiles.Values)
                    {
                        CommUnit unit2 = mob.FindItemOnLayer(Layer.Earrings) as CommUnit;
                        if (unit2 != null)
                        {
                            if (unit2.Channel == unit.Channel && mob is PlayerMobile)
                            {
                                list.Add(mob);
                            }
                        }
                    }
                    for (int i = 0; i < list.Count; ++i)
                    {
                        Mobile mobile = (Mobile)list[i];
                        mobile.SendMessage(0, "{0}: {1}", m.Name, speechstring);
                    }
                }
            }
        }
        private class MessagePrompt : Prompt
        {
            private CommUnit unit;
            private Mobile m;
            public MessagePrompt(CommUnit comm, Mobile mob)
            {
                unit = comm;
                m = mob;
            }
            public override void OnResponse(Mobile from, string text)
            {
                bool towerin = false;
                Item tower = null;
                IPooledEnumerable eable = m.Map.GetItemsInRange(m.Location, 500);
                foreach (Item i in eable)
                {
                    if (i != null)
                    {
                        if (i is CommTower)
                        {
                            if (towerin != true)
                            {
                                towerin = true;
                                tower = i;
                            }
                        }
                    }
                }
                eable.Free();
                if (towerin != false)
                {
                    if (tower != null)
                    {
                        ArrayList list = new ArrayList();
                        IPooledEnumerable eable2 = tower.Map.GetMobilesInRange(tower.Location, 1000);
                        foreach (Mobile mb in eable2)
                        {
                            if (mb != null)
                            {
                                if (mb is PlayerMobile)
                                {
                                    CommUnit unit2 = mb.FindItemOnLayer(Layer.Earrings) as CommUnit;
                                    if (unit2 != null)
                                    {
                                        bool channelgood = false;
                                        if (unit2.Channel == unit.Channel)
                                        {
                                            channelgood = true;
                                        }
                                        if (channelgood == true)
                                        {
                                            list.Add(mb);
                                        }
                                    }
                                }
                            }
                        }
                        eable2.Free();
                        for (int i = 0; i < list.Count; ++i)
                        {
                            Mobile mobile = (Mobile)list[i];
                            mobile.SendMessage(0, "{0}: {1}", from.Name, text);
                        }
                    }
                }
                else
                {
                    from.SendMessage(0, "You must be in range of a Comm Tower to use the Personal Communicator.");
                }
            }
        }
    }
}