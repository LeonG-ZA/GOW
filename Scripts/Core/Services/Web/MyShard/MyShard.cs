using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Server.Accounting;
using Server.Commands;
using Server.Guilds;
using Server.Misc;
using Server.Mobiles;
using Server.WebConfiguration;
using Server.MainConfiguration;

namespace Server.Engines.MyShard
{
    public class MyShard : Timer
    {
        public const char LineStart = '\"';
        public const string EntrySep = "\",\"";
        public const string LineEnd = "\"\n";
        private static readonly double CpuInterval = 0.1;// Processor runs every 0.1 seconds
        private static readonly double CpuPercent = 0.25;// Processor runs for 25% of Interval, or ~25ms. This should take around 25% cpu
        private static Timer m_Timer;
        private static DatabaseCommandQueue m_Command;
        private static ArrayList m_MobilesToUpdate = new ArrayList();
        private readonly DateTime m_StartTime;
        private readonly ArrayList m_Items = new ArrayList();
        private Stage m_Stage;
        private ArrayList m_List;
        private List<IAccount> m_Collecting;
        private int m_Index;
        private string m_SkillsPath;
        private string m_LayersPath;
        private string m_MobilesPath;
        private StreamWriter m_OpSkills;
        private StreamWriter m_OpLayers;
        private StreamWriter m_OpMobiles;
        public MyShard()
            : base(TimeSpan.FromSeconds(CpuInterval), TimeSpan.FromSeconds(CpuInterval))
        {
            this.m_List = new ArrayList();
            this.m_Collecting = new List<IAccount>();

            this.m_StartTime = DateTime.UtcNow;
            Console.WriteLine("MyShard: Updating character database");
        }

        private enum Stage
        {
            CollectingMobiles,
            DumpingMobiles,
            CollectingGuilds,
            DumpingGuilds,
            Complete
        }
        public static void Initialize()
        {
            if (WebConfig.WebMyShardEnabled)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), WebConfig.WebCharacterUpdateInterval, new TimerCallback(Begin));

                CommandSystem.Register("UpdateMyShard", AccessLevel.Administrator, new CommandEventHandler(UpdateMyShard_OnCommand));

                CommandSystem.Register("PublicChar", AccessLevel.Player, new CommandEventHandler(PublicChar_OnCommand));
                CommandSystem.Register("PrivateChar", AccessLevel.Player, new CommandEventHandler(PrivateChar_OnCommand));
            }
        }

        [Usage("PublicChar")]
        [Description("Enables showing extended character stats and skills in MyShard.")]
        public static void PublicChar_OnCommand(CommandEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if (pm != null)
            {
                if (pm.PublicMyShard)
                {
                    pm.SendMessage("You have already chosen to show your skills and stats.");
                }
                else
                {
                    pm.PublicMyShard = true;
                    pm.SendMessage("All of your skills and stats will now be shown publicly in MyShard.");
                }
            }
        }

        [Usage("PrivateChar")]
        [Description("Disables showing extended character stats and skills in MyShard.")]
        public static void PrivateChar_OnCommand(CommandEventArgs e)
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if (pm != null)
            {
                if (!pm.PublicMyShard)
                {
                    pm.SendMessage("You have already chosen to not show your skills and stats.");
                }
                else
                {
                    pm.PublicMyShard = false;
                    pm.SendMessage("Only a general level of your top three skills will be shown in MyShard.");
                }
            }
        }

        [Usage("UpdateMyShard")]
        [Description("Starts the process of updating the MyShard character and guild database.")]
        public static void UpdateMyShard_OnCommand(CommandEventArgs e)
        {
            if (m_Command != null && m_Command.HasCompleted)
                m_Command = null;

            if (m_Timer == null && m_Command == null)
            {
                Begin();
                e.Mobile.SendMessage("MyShard update process has been started.");
            }
            else
            {
                e.Mobile.SendMessage("MyShard database is already being updated.");
            }
        }

        public static void UpdateMyShard()
        {
            if (m_Command != null && m_Command.HasCompleted)
                m_Command = null;

            if (m_Timer == null && m_Command == null)
            {
                Begin();
            }
        }

        public static void Begin()
        {
            if (m_Command != null && m_Command.HasCompleted)
                m_Command = null;

            if (m_Timer != null || m_Command != null)
                return;

            m_Timer = new MyShard();
            m_Timer.Start();
        }

        public static void QueueMobileUpdate(Mobile m)
        {
            if (!WebConfig.WebMyShardEnabled || WebConfig.WebLoadDataInFile)
                return;

            m_MobilesToUpdate.Add(m);
        }

        public bool Process(DateTime endTime)
        {
            switch ( this.m_Stage )
            {
                case Stage.CollectingMobiles:
                    this.CollectMobiles(endTime);
                    break;
                case Stage.DumpingMobiles:
                    this.DumpMobiles(endTime);
                    break;
                case Stage.CollectingGuilds:
                    this.CollectGuilds(endTime);
                    break;
                case Stage.DumpingGuilds:
                    this.DumpGuilds(endTime);
                    break;
            }

            return (this.m_Stage == Stage.Complete);
        }

        public void CollectMobiles(DateTime endTime)
        {
            if (WebConfig.WebLoadDataInFile)
            {
                if (this.m_Index == 0)
                    this.m_Collecting.AddRange(Accounts.GetAccounts());

                for (int i = this.m_Index; i < this.m_Collecting.Count; ++i)
                {
                    IAccount acct = this.m_Collecting[i];

                    for (int j = 0; j < acct.Length; ++j)
                    {
                        Mobile mob = acct[j];

                        if (mob != null && mob.AccessLevel < WebConfig.WebHiddenAccessLevel)
                            this.m_List.Add(mob);
                    }

                    ++this.m_Index;

                    if (DateTime.UtcNow >= endTime)
                        break;
                }

                if (this.m_Index == this.m_Collecting.Count)
                {
                    this.m_Collecting = new List<IAccount>();
                    this.m_Stage = Stage.DumpingMobiles;
                    this.m_Index = 0;
                }
            }
            else
            {
                this.m_List = m_MobilesToUpdate;
                m_MobilesToUpdate = new ArrayList();
                this.m_Stage = Stage.DumpingMobiles;
                this.m_Index = 0;
            }
        }

        public void CheckConnection()
        {
            if (m_Command == null)
            {
                m_Command = new DatabaseCommandQueue("MyShard: Characeter database updated in {0:F1} seconds", "MyShard Character Database Thread");

                if (WebConfig.WebLoadDataInFile)
                {
                    this.m_OpSkills = this.GetUniqueWriter("skills", out this.m_SkillsPath);
                    this.m_OpLayers = this.GetUniqueWriter("layers", out this.m_LayersPath);
                    this.m_OpMobiles = this.GetUniqueWriter("mobiles", out this.m_MobilesPath);

                    m_Command.Enqueue("TRUNCATE TABLE MyShard_characters");
                    m_Command.Enqueue("TRUNCATE TABLE MyShard_characters_layers");
                    m_Command.Enqueue("TRUNCATE TABLE MyShard_characters_skills");
                }

                m_Command.Enqueue("TRUNCATE TABLE MyShard_guilds");
                m_Command.Enqueue("TRUNCATE TABLE MyShard_guilds_wars");
            }
        }

        public void ExecuteNonQuery(string text)
        {
            m_Command.Enqueue(text);
        }

        public void ExecuteNonQuery(string format, params string[] args)
        {
            this.ExecuteNonQuery(String.Format(format, args));
        }

        public void ExecuteNonQueryIfNull(string select, string insert)
        {
            m_Command.Enqueue(new string[] { select, insert });
        }

        public void InsertMobile(Mobile mob)
        {
            string guildTitle = mob.GuildTitle;

            if (guildTitle == null || (guildTitle = guildTitle.Trim()).Length == 0)
                guildTitle = "NULL";
            else
                guildTitle = this.SafeString(guildTitle);

            string notoTitle = this.SafeString(Titles.ComputeTitle(null, mob));
            string female = (mob.Female ? "1" : "0");
			
            bool pubBool = (mob is PlayerMobile) && (((PlayerMobile)mob).PublicMyShard);

            string pubString = (pubBool ? "1" : "0");

            string guildId = (mob.Guild == null ? "NULL" : mob.Guild.Id.ToString());

            if (WebConfig.WebLoadDataInFile)
            {
                this.m_OpMobiles.Write(LineStart);
                this.m_OpMobiles.Write(mob.Serial.Value);
                this.m_OpMobiles.Write(EntrySep);
                this.m_OpMobiles.Write(this.SafeString(mob.Name));
                this.m_OpMobiles.Write(EntrySep);
                this.m_OpMobiles.Write(mob.RawStr);
                this.m_OpMobiles.Write(EntrySep);
                this.m_OpMobiles.Write(mob.RawDex);
                this.m_OpMobiles.Write(EntrySep);
                this.m_OpMobiles.Write(mob.RawInt);
                this.m_OpMobiles.Write(EntrySep);
                this.m_OpMobiles.Write(female);
                this.m_OpMobiles.Write(EntrySep);
                this.m_OpMobiles.Write(mob.Kills);
                this.m_OpMobiles.Write(EntrySep);
                this.m_OpMobiles.Write(guildId);
                this.m_OpMobiles.Write(EntrySep);
                this.m_OpMobiles.Write(guildTitle);
                this.m_OpMobiles.Write(EntrySep);
                this.m_OpMobiles.Write(notoTitle);
                this.m_OpMobiles.Write(EntrySep);
                this.m_OpMobiles.Write(mob.Hue);
                this.m_OpMobiles.Write(EntrySep);
                this.m_OpMobiles.Write(pubString);
                this.m_OpMobiles.Write(LineEnd);
            }
            else
            {
                this.ExecuteNonQuery("INSERT INTO MyShard_characters (char_id, char_name, char_str, char_dex, char_int, char_female, char_counts, char_guild, char_guildtitle, char_nototitle, char_bodyhue, char_public ) VALUES ({0}, '{1}', {2}, {3}, {4}, {5}, {6}, {7}, {8}, '{9}', {10}, {11})", mob.Serial.Value.ToString(), this.SafeString(mob.Name), mob.RawStr.ToString(), mob.RawDex.ToString(), mob.RawInt.ToString(), female, mob.Kills.ToString(), guildId, guildTitle, notoTitle, mob.Hue.ToString(), pubString);
            }
        }

        public void InsertSkills(Mobile mob)
        {
            Skills skills = mob.Skills;
            string serial = mob.Serial.Value.ToString();

            for (int i = 0; i < skills.Length; ++i)
            {
                Skill skill = skills[i];

                if (skill.BaseFixedPoint > 0)
                {
                    if (WebConfig.WebLoadDataInFile)
                    {
                        this.m_OpSkills.Write(LineStart);
                        this.m_OpSkills.Write(serial);
                        this.m_OpSkills.Write(EntrySep);
                        this.m_OpSkills.Write(i);
                        this.m_OpSkills.Write(EntrySep);
                        this.m_OpSkills.Write(skill.BaseFixedPoint);
                        this.m_OpSkills.Write(LineEnd);
                    }
                    else
                    {
                        this.ExecuteNonQuery("INSERT INTO MyShard_characters_skills (char_id, skill_id, skill_value) VALUES ({0}, {1}, {2})", serial, i.ToString(), skill.BaseFixedPoint.ToString());
                    }
                }
            }
        }

        public void InsertItems(Mobile mob)
        {
            ArrayList items = this.m_Items;
            items.AddRange(mob.Items);
            string serial = mob.Serial.Value.ToString();

            items.Sort(LayerComparer.Instance);

            int index = 0;

            bool hidePants = false;
            bool alive = mob.Alive;
            bool hideHair = !alive;

            for (int i = 0; i < items.Count; ++i)
            {
                Item item = (Item)items[i];

                if (!LayerComparer.IsValid(item))
                    break;

                if (!alive && item.ItemID != 8270)
                    continue;

                if (item.ItemID == 0x1411 || item.ItemID == 0x141A) // plate legs
                    hidePants = true;
                else if (hidePants && item.Layer == Layer.Pants)
                    continue;

                if (!hideHair && item.Layer == Layer.Helm)
                    hideHair = true;

                this.InsertItem(serial, index++, item.ItemID, item.Hue);
            }

            if (mob.FacialHairItemID != 0 && alive)
                this.InsertItem(serial, index++, mob.FacialHairItemID, mob.FacialHairHue);

            if (mob.HairItemID != 0 && !hideHair)
                this.InsertItem(serial, index++, mob.HairItemID, mob.HairHue);

            items.Clear();
        }

        public void DeleteMobile(Mobile mob)
        {
            this.ExecuteNonQuery("DELETE FROM MyShard_characters WHERE char_id = {0}", mob.Serial.Value.ToString());
            this.ExecuteNonQuery("DELETE FROM MyShard_characters_skills WHERE char_id = {0}", mob.Serial.Value.ToString());
            this.ExecuteNonQuery("DELETE FROM MyShard_characters_layers WHERE char_id = {0}", mob.Serial.Value.ToString());
        }

        public StreamWriter GetUniqueWriter(string type, out string filePath)
        {
            if (!Directory.Exists("Web"))
            {
                Directory.CreateDirectory("Web");
            }
            if (!Directory.Exists("Web\\MyShard"))
            {
                Directory.CreateDirectory("Web\\MyShard");
            }

            string path = String.Format("{0}\\Web\\MyShard ", Core.BaseDirectory);
            filePath = Path.Combine(path, String.Format(MainConfig.MainMyShardFilePath1, type)).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);//Core.BaseDirectory


          //  filePath = Path.Combine(Core.BaseDirectory, String.Format("MySharddb_{0}.txt", type)).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            try
            {
                return new StreamWriter(filePath);
            }
            catch
            {
                for (int i = 0; i < 100; ++i)
                {
                    try
                    {
                        filePath = Path.Combine(Core.BaseDirectory, String.Format("MySharddb_{0}_{1}.txt", type, i)).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        return new StreamWriter(filePath);
                    }
                    catch
                    {
                    }
                }
            }

            return null;
        }

        public void DumpMobiles(DateTime endTime)
        {
            this.CheckConnection();

            for (int i = this.m_Index; i < this.m_List.Count; ++i)
            {
                Mobile mob = (Mobile)this.m_List[i];

                if (mob is PlayerMobile)
                    ((PlayerMobile)mob).ChangedMyShard = false;

                if (!mob.Deleted && mob.AccessLevel < WebConfig.WebHiddenAccessLevel)
                {
                    if (!WebConfig.WebLoadDataInFile)
                        this.DeleteMobile(mob);

                    this.InsertMobile(mob);
                    this.InsertSkills(mob);
                    this.InsertItems(mob);
                }
                else if (!WebConfig.WebLoadDataInFile)
                {
                    this.DeleteMobile(mob);
                }

                ++this.m_Index;

                if (DateTime.UtcNow >= endTime)
                    break;
            }

            if (this.m_Index == this.m_List.Count)
            {
                this.m_List.Clear();
                this.m_Stage = Stage.CollectingGuilds;
                this.m_Index = 0;

                if (WebConfig.WebLoadDataInFile)
                {
                    this.m_OpSkills.Close();
                    this.m_OpLayers.Close();
                    this.m_OpMobiles.Close();

                    this.ExecuteNonQuery("LOAD DATA {0}INFILE '{1}' INTO TABLE MyShard_characters FIELDS TERMINATED BY ',' ENCLOSED BY '\"' LINES TERMINATED BY '\n'", WebConfig.WebDatabaseNonLocal ? "LOCAL " : "", this.m_MobilesPath);
                    this.ExecuteNonQuery("LOAD DATA {0}INFILE '{1}' INTO TABLE MyShard_characters_skills FIELDS TERMINATED BY ',' ENCLOSED BY '\"' LINES TERMINATED BY '\n'", WebConfig.WebDatabaseNonLocal ? "LOCAL " : "", this.m_SkillsPath);
                    this.ExecuteNonQuery("LOAD DATA {0}INFILE '{1}' INTO TABLE MyShard_characters_layers FIELDS TERMINATED BY ',' ENCLOSED BY '\"' LINES TERMINATED BY '\n'", WebConfig.WebDatabaseNonLocal ? "LOCAL " : "", this.m_LayersPath);
                }
            }
        }

        public void CollectGuilds(DateTime endTime)
        {
            this.m_List.AddRange(Guild.List.Values);
            this.m_Stage = Stage.DumpingGuilds;
            this.m_Index = 0;
        }

        public void InsertGuild(Guild guild)
        {
            string guildType = "Standard";

            switch ( guild.Type )
            {
                case GuildType.Chaos:
                    guildType = "Chaos";
                    break;
                case GuildType.Order:
                    guildType = "Order";
                    break;
            }

            this.ExecuteNonQuery("INSERT INTO MyShard_guilds (guild_id, guild_name, guild_abbreviation, guild_website, guild_charter, guild_type, guild_wars, guild_members, guild_master) VALUES ({0}, '{1}', {2}, {3}, {4}, '{5}', {6}, {7}, {8})", guild.Id.ToString(), this.SafeString(guild.Name), guild.Abbreviation == "none" ? "NULL" : "'" + this.SafeString(guild.Abbreviation) + "'", guild.Website == null ? "NULL" : "'" + this.SafeString(guild.Website) + "'", guild.Charter == null ? "NULL" : "'" + this.SafeString(guild.Charter) + "'", guildType, guild.Enemies.Count.ToString(), guild.Members.Count.ToString(), guild.Leader.Serial.Value.ToString());
        }

        public void InsertWars(Guild guild)
        {
            List<Guild> wars = guild.Enemies;

            string ourId = guild.Id.ToString();

            for (int i = 0; i < wars.Count; ++i)
            {
                Guild them = wars[i];
                string theirId = them.Id.ToString();

                this.ExecuteNonQueryIfNull(
                    String.Format("SELECT guild_1 FROM MyShard_guilds_wars WHERE (guild_1={0} AND guild_2={1}) OR (guild_1={1} AND guild_2={0})", ourId, theirId),
                    String.Format("INSERT INTO MyShard_guilds_wars (guild_1, guild_2) VALUES ({0}, {1})", ourId, theirId));
            }
        }

        public void DumpGuilds(DateTime endTime)
        {
            this.CheckConnection();

            for (int i = this.m_Index; i < this.m_List.Count; ++i)
            {
                Guild guild = (Guild)this.m_List[i];

                if (!guild.Disbanded)
                {
                    this.InsertGuild(guild);
                    this.InsertWars(guild);
                }

                ++this.m_Index;

                if (DateTime.UtcNow >= endTime)
                    break;
            }

            if (this.m_Index == this.m_List.Count)
            {
                this.m_List.Clear();
                this.m_Stage = Stage.Complete;
                this.m_Index = 0;
            }
        }

        protected override void OnTick()
        {
            bool shouldExit = false;

            if(!WebConfig.WebMyShardEnabled)
            {
                m_Command.Enqueue(null);
                this.Stop();
                m_Timer = null;
                return;
            }

            try
            {
                shouldExit = this.Process(DateTime.UtcNow + TimeSpan.FromSeconds(CpuInterval * CpuPercent));

                if (shouldExit)
                    Console.WriteLine("MyShard: Database statements compiled in {0:F2} seconds", (DateTime.UtcNow - this.m_StartTime).TotalSeconds);
            }
            catch (Exception e)
            {
                Console.WriteLine("MyShard: {0}: Exception cought while processing", this.m_Stage);
                Console.WriteLine(e);
                shouldExit = true;
            }

            if (shouldExit)
            {
                m_Command.Enqueue(null);

                this.Stop();
                m_Timer = null;
            }
        }

        private void AppendCharEntity(string input, int charIndex, ref StringBuilder sb, char c)
        {
            if (sb == null)
            {
                if (charIndex > 0)
                    sb = new StringBuilder(input, 0, charIndex, input.Length + 20);
                else
                    sb = new StringBuilder(input.Length + 20);
            }

            sb.Append("&#");
            sb.Append((int)c);
            sb.Append(";");
        }

        private void AppendEntityRef(string input, int charIndex, ref StringBuilder sb, string ent)
        {
            if (sb == null)
            {
                if (charIndex > 0)
                    sb = new StringBuilder(input, 0, charIndex, input.Length + 20);
                else
                    sb = new StringBuilder(input.Length + 20);
            }

            sb.Append(ent);
        }

        private string SafeString(string input)
        {
            if (input == null)
                return "";

            StringBuilder sb = null;

            for (int i = 0; i < input.Length; ++i)
            {
                char c = input[i];

                if (c < 0x20 || c >= 0x7F)
                {
                    this.AppendCharEntity(input, i, ref sb, c);
                }
                else
                {
                    switch ( c )
                    {
                        case '&':
                            this.AppendEntityRef(input, i, ref sb, "&amp;");
                            break;
                        case '>':
                            this.AppendEntityRef(input, i, ref sb, "&gt;");
                            break;
                        case '<':
                            this.AppendEntityRef(input, i, ref sb, "&lt;");
                            break;
                        case '"':
                            this.AppendEntityRef(input, i, ref sb, "&quot;");
                            break;
                        case '\'':
                        case ':':
                        case '/':
                        case '\\':
                            this.AppendCharEntity(input, i, ref sb, c);
                            break;
                        default:
                            {
                                if (sb != null)
                                    sb.Append(c);

                                break;
                            }
                    }
                }
            }

            if (sb != null)
                return sb.ToString();

            return input;
        }

        private void InsertItem(string serial, int index, int itemID, int hue)
        {
            if (WebConfig.WebLoadDataInFile)
            {
                this.m_OpLayers.Write(LineStart);
                this.m_OpLayers.Write(serial);
                this.m_OpLayers.Write(EntrySep);
                this.m_OpLayers.Write(index);
                this.m_OpLayers.Write(EntrySep);
                this.m_OpLayers.Write(itemID);
                this.m_OpLayers.Write(EntrySep);
                this.m_OpLayers.Write(hue);
                this.m_OpLayers.Write(LineEnd);
            }
            else
            {
                this.ExecuteNonQuery("INSERT INTO MyShard_characters_layers (char_id, layer_id, item_id, item_hue) VALUES ({0}, {1}, {2}, {3})", serial, index.ToString(), itemID.ToString(), hue.ToString());
            }
        }
    }
}