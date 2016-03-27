using System;
using System.Collections.Generic;
using System.Xml;
using Server;
using Server.Commands;
using Server.Network;
using Server.Mobiles;
using Server.Multis;
using Server.WebConfiguration;

namespace Server.UOGPS
{
    public class UOGPS
    {
        //if you want to use another directory then where your GOW.exe is, change to something like
        // OutputDirectory =@"D:/var/www"
        // !!! This directory should excist !!!
        public static readonly string OutputDirectory = WebConfig.WebOutputDirectory;
         
        //this will enable|disable auto generation of player house locations at each world save
        //set to false to disable it
        public static readonly bool PlayersLGenAtSave = WebConfig.WebPlayersLGenAtSave;
  
        //this will enable auto generation of player locations at each world save
        //set to false to disable it
        public static readonly bool PlayersHGenAtSave = WebConfig.WebPlayersHGenAtSave;

        private static Dictionary<string, Point3D> playersList = new Dictionary<string, Point3D>();
        private static List<string> playersMap = new List<string>();
        private static Dictionary<string, Point3D> playersHouseList = new Dictionary<string, Point3D>();
        private static List<string> playersHouseMap = new List<string>();

        public static void Initialize()
        {
            if (WebConfig.WebGPSEnabled)
            {
                EventSink.WorldSave += new WorldSaveEventHandler(EventSink_WorldSave);
                CommandSystem.Register("gpshouse", AccessLevel.Seer, new CommandEventHandler(GPSHouse_OnCommand));
                CommandSystem.Register("gpsplayer", AccessLevel.GameMaster, new CommandEventHandler(GPSPlayer_OnCommand));
            }
        }

        [Usage("Gpshouse")]//ok
        [Description("Will generate house positions, might couse lag if you have to much houses")]
        private static void GPSHouse_OnCommand(CommandEventArgs e)
        {
            World.Broadcast(0x43, true, "Our satellites are recording players house positions!");
            getPlayersHousePosition();
            World.Broadcast(0x43, true, string.Format("There where {0} {1} spotted by our satellites!", playersHouseMap.Count.ToString(), playersHouseMap.Count > 1 ? "houses" : "house"));
            xmlPlayerWriter("playersHousePosistion.xml", true);

        }

        [Usage("Gpsplayer")]//ok
        [Description("Will generate current online player positions")]
        private static void GPSPlayer_OnCommand(CommandEventArgs e)
        {
            World.Broadcast(0x43, true, "Our satellites are recording players positions!");
            getOnlinePlayersPosition();
            World.Broadcast(0x43, true, string.Format("There where {0} {1} spotted by our satellites!", playersMap.Count.ToString(), playersMap.Count > 1 ? "players" : "player"));
            xmlPlayerWriter("playersPosistion.xml", false);
        }

        public static void EventSink_WorldSave(WorldSaveEventArgs e)
        {
                try
                {
                    if (PlayersLGenAtSave)
                    {
                        World.Broadcast(0x43, true, "Our satellites are recording player positions!");
                        getOnlinePlayersPosition();
                        World.Broadcast(0x43, true, string.Format("There where {0} {1} spotted by our satellites!", playersMap.Count.ToString(), playersMap.Count > 1 ? "players" : "player"));
                        xmlPlayerWriter("playersPosistion.xml", false);
                    }
                    if (PlayersHGenAtSave)
                    {
                        World.Broadcast(0x43, true, "Our satellites are recording players house positions!");
                        getPlayersHousePosition();
                        World.Broadcast(0x43, true, string.Format("There where {0} {1} spotted by our satellites!", playersHouseMap.Count.ToString(), playersHouseMap.Count > 1 ? "houses" : "house"));
                        xmlPlayerWriter("playersHousePosistion.xml", true);
                    }
                }
                catch
                {
                }
        }

        private static void clearLists()
        {
            playersList.Clear();
            playersMap.Clear();
            playersHouseList.Clear();
            playersHouseMap.Clear();
        }

        private static void getOnlinePlayersPosition()
        {
            clearLists();
            int id = 0;
            foreach (NetState nt in NetState.Instances)
            {
                Mobile m = nt.Mobile;
                if (m != null && m is PlayerMobile)
                {
                    //attach id so we get "unique names" to make sure we have unique Tkey
                    playersList.Add(m.Name + "." + id, m.Location);
                    playersMap.Add(m.Map.ToString());
                    id++;
                }
            }
        }

        private static void getPlayersHousePosition()
        {
            clearLists();
            BaseHouse bh;
            int id = 0;
            foreach (KeyValuePair<Serial, Item> kvp in World.Items)
            {
                if (kvp.Value is BaseHouse)
                {
                    if ((BaseHouse)kvp.Value != null)
                    {
                        bh = (BaseHouse)kvp.Value;
                        //attach id so we get "unique names" to make sure we have unique Tkey
                        playersHouseList.Add(bh.Owner.Name + "." + id, bh.Location);
                        playersHouseMap.Add(bh.Map.ToString());
                        id++;
                    }

                }
            }
        }

        private static void xmlPlayerWriter(string fileName, bool isHouse)
        {
            using (XmlWriter writer = XmlWriter.Create(OutputDirectory == null ? (Core.BaseDirectory + "/" + fileName) : (OutputDirectory + "/" + fileName)))
            {

                if (isHouse)
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("PlayersHouse");

                    int counter = 0;
                    foreach (KeyValuePair<string, Point3D> kvp in playersHouseList)
                    {
                        writer.WriteStartElement("Player");

                        writer.WriteElementString("name", kvp.Key);
                        writer.WriteElementString("x", kvp.Value.X.ToString());
                        writer.WriteElementString("y", kvp.Value.Y.ToString());
                        writer.WriteElementString("map", playersHouseMap[counter].ToString());

                        writer.WriteEndElement();

                        counter++;
                    }
                    writer.WriteEndElement();

                    writer.WriteEndDocument();
                }
                else
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("Players");

                    int counter = 0;
                    foreach (KeyValuePair<string, Point3D> kvp in playersList)
                    {
                        writer.WriteStartElement("Player");

                        writer.WriteElementString("name", kvp.Key);
                        writer.WriteElementString("x", kvp.Value.X.ToString());
                        writer.WriteElementString("y", kvp.Value.Y.ToString());
                        writer.WriteElementString("map", playersMap[counter].ToString());

                        writer.WriteEndElement();

                        counter++;
                    }
                    writer.WriteEndElement();

                    writer.WriteEndDocument();
                }

            }
        }

    }
}
