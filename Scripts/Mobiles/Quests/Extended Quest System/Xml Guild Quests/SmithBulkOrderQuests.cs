using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.Quests
{
    #region Base Quest
    public class BaseSmithBulkOrderQuest : BaseGuildQuest
    {
        public override object Title { get { return GetQuestName(Rank); } }

        public override object Description { get { return "I am responsible for issuing tasks to Guild members. I currently have something available if you are interested?"; } }

        public override object Refuse { get { return "I shall reserve this task for another member of the Guild instead."; } }

        public override object Uncomplete { get { return "The Guild will happily reward your efforts once the order is complete."; } }

        public override object Complete { get { return "Thank you, please accept this reward on behalf of the Guild."; } }

        public CraftResource Resource = CraftResource.None;

        public BaseSmithBulkOrderQuest()
            : this(1)
        {
        }

        public BaseSmithBulkOrderQuest(int rank) : base("Blacksmith")
        {
            Rank = rank;
            TokenAmount = 5;
            int amount = 10;
            bool exceptional = false;

            if (Utility.Random(100) < 10)
            {
                TokenAmount += 1;
                exceptional = true;
            }

            switch (Utility.Random(7))
            {
                case 0:
                    AddObjective(new GuildCraftObjective(typeof(RingmailChest), "ringmail tunics", amount, 0x13EC, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(RingmailLegs), "ringmail legs", amount, 0x13F0, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(RingmailArms), "ringmail arms", amount, 0x13EE, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(RingmailGloves), "ringmail gloves", amount, 0x13EB, exceptional, GetQuestResource(Rank)));
                    break;
                case 1:
                    AddObjective(new GuildCraftObjective(typeof(ChainCoif), "chain coifs", amount, 0x13BB, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(ChainChest), "chain tunics", amount, 0x13BF, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(ChainLegs), "chain legs", amount, 0x13BE, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(PlateGorget), "plate gorgets", amount, 0x1413, exceptional, GetQuestResource(Rank)));
                    break;
                case 2:
                    AddObjective(new GuildCraftObjective(typeof(PlateChest), "plate tunics", amount, 0x1415, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(PlateLegs), "plate legs", amount, 0x1411, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(PlateArms), "plate arms", amount, 0x1410, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(PlateGloves), "plate gloves", amount, 0x1414, exceptional, GetQuestResource(Rank)));
                    break;
                case 3:
                    AddObjective(new GuildCraftObjective(typeof(Dagger), "daggers", amount, 0x0F52, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(Kryss), "kryss", amount, 0x1401, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(ShortSpear), "short spears", amount, 0x1403, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(WarFork), "war forks", amount, 0x1405, exceptional, GetQuestResource(Rank)));
                    break;
                case 4:
                    AddObjective(new GuildCraftObjective(typeof(Mace), "maces", amount, 0x0F5C, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(WarMace), "war maces", amount, 0x143B, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(WarHammer), "war hammers", amount, 0x1439, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(Maul), "mauls", amount, 0x1405, exceptional, GetQuestResource(Rank)));
                    break;
                case 5:
                    AddObjective(new GuildCraftObjective(typeof(Katana), "katanas", amount, 0x13FF, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(Longsword), "longswords", amount, 0x0F61, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(Scimitar), "scimitars", amount, 0x13B6, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(VikingSword), "viking swords", amount, 0x13B9, exceptional, GetQuestResource(Rank)));
                    break;
                case 6:
                    AddObjective(new GuildCraftObjective(typeof(Axe), "axes", amount, 0x0F49, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(DoubleAxe), "double axes", amount, 0x0F4B, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(ExecutionersAxe), "executioners axes", amount, 0x0F45, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(TwoHandedAxe), "two-handed axes", amount, 0x1443, exceptional, GetQuestResource(Rank)));
                    break;
            }

            string rewardString = "Marks of the Forge";
            string standing = "Guild Standing";
            AddReward(new BaseReward(rewardString));
            AddReward(new BaseReward(standing));
        }

        private string GetQuestName(int rank)
        {
            switch (rank)
            {
                default:
                    return "Blacksmith Guild Order";
                case 1:
                    return "Novice Blacksmith Guild Order";
                case 2:
                    return "Apprentice Blacksmith Guild Order";
                case 3:
                    return "Journeyman Blacksmith Guild Order";
                case 4:
                    return "Expert Blacksmith Guild Order";
                case 5:
                    return "Master Blacksmith Guild Order";

            }
        }

        private CraftResource GetQuestResource(int rank)
        {
                switch (rank)
                {
                    default:
                        return CraftResource.None;
                    case 1:
                        return CraftResource.Iron;
                    case 2:
                        if (Utility.Random(100) > 50)
                        {
                            return CraftResource.DullCopper;
                        }
                        else
                        {
                            return CraftResource.ShadowIron;
                        }
                    case 3:
                        if (Utility.Random(100) > 50)
                        {
                            return CraftResource.Copper;
                        }
                        else
                        {
                            return CraftResource.Bronze;
                        }
                    case 4:
                        if (Utility.Random(100) > 50)
                        {
                            return CraftResource.Gold;
                        }
                        else
                        {
                            return CraftResource.Agapite;
                        }
                    case 5:
                        if (Utility.Random(100) > 50)
                        {
                            return CraftResource.Verite;
                        }
                        else
                        {
                            return CraftResource.Valorite;
                        }
                }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    #endregion

    public class SmithBulkOrderQuestRank1 : BaseSmithBulkOrderQuest
    {
        public SmithBulkOrderQuestRank1()
            : base(1)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SmithBulkOrderQuestRank2 : BaseSmithBulkOrderQuest
    {
        public SmithBulkOrderQuestRank2()
            : base(2)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SmithBulkOrderQuestRank3 : BaseSmithBulkOrderQuest
    {
        public SmithBulkOrderQuestRank3()
            : base(3)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SmithBulkOrderQuestRank4 : BaseSmithBulkOrderQuest
    {
        public SmithBulkOrderQuestRank4()
            : base(4)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SmithBulkOrderQuestRank5 : BaseSmithBulkOrderQuest
    {
        public SmithBulkOrderQuestRank5()
            : base(5)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
