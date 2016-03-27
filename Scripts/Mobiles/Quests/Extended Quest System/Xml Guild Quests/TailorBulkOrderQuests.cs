using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.Quests
{
    #region Base Quest
    public class BaseTailorBulkOrderQuest : BaseGuildQuest
    {
        public override object Title { get { return GetQuestName(Rank); } }

        public override object Description { get { return "I am responsible for issuing tasks to Guild members. I currently have something available if you are interested?"; } }

        public override object Refuse { get { return "I shall reserve this task for another member of the Guild instead."; } }

        public override object Uncomplete { get { return "The Guild will happily reward your efforts once the order is complete."; } }

        public override object Complete { get { return "Thank you, please accept this reward on behalf of the Guild."; } }

        public CraftResource Resource = CraftResource.None;

        public BaseTailorBulkOrderQuest()
            : this(1)
        {
        }

        public BaseTailorBulkOrderQuest(int rank)
            : base("Tailor")
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

            switch (Utility.Random(5))
            {
                case 0:
                    AddObjective(new GuildCraftObjective(typeof(LeatherChest), "leather tunics", amount, 0x13CC, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(LeatherLegs), "leather legs", amount, 0x13CB, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(LeatherArms), "leather arms", amount, 0x13CD, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(LeatherGloves), "leather gloves", amount, 0x13C6, exceptional, GetQuestResource(Rank)));
                    break;
                case 1:
                    AddObjective(new GuildCraftObjective(typeof(StuddedChest), "studded tunics", amount, 0x13DB, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(StuddedLegs), "studded legs", amount, 0x13DA, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(StuddedArms), "studded arms", amount, 0x13DC, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(StuddedGloves), "studded gloves", amount, 0x13D5, exceptional, GetQuestResource(Rank)));
                    break;
                case 2:
                    AddObjective(new GuildCraftObjective(typeof(BoneChest), "bone tunics", amount, 0x144F, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(BoneLegs), "bone legs", amount, 0x1452, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(BoneArms), "bone arms", amount, 0x144E, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(BoneGloves), "bone gloves", amount, 0x1450, exceptional, GetQuestResource(Rank)));
                    break;
                case 3:
                    AddObjective(new GuildCraftObjective(typeof(Boots), "boots", amount, 0x170B, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(ThighBoots), "thigh boots", amount, 0x1711, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(Shoes), "shoes", amount, 0x170F, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(Sandals), "sandals", amount, 0x170D, exceptional, GetQuestResource(Rank)));
                    break;
                case 4:
                    AddObjective(new GuildCraftObjective(typeof(LeatherBustierArms), "leather bustiers", amount, 0x1C0A, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(StuddedBustierArms), "studded bustiers", amount, 0x1C0C, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(LeatherShorts), "leather shorts", amount, 0x1C00, exceptional, GetQuestResource(Rank)));
                    AddObjective(new GuildCraftObjective(typeof(LeatherSkirt), "leather skirts", amount, 0x1C08, exceptional, GetQuestResource(Rank)));
                    break;
            }

            string rewardString = "Marks of the Cloth";
            string standing = "Guild Standing";
            AddReward(new BaseReward(rewardString));
            AddReward(new BaseReward(standing));
        }

        private string GetQuestName(int rank)
        {
            switch (rank)
            {
                default:
                    return "Tailor Guild Order";
                case 1:
                    return "Novice Tailor Guild Order";
                case 2:
                    return "Apprentice Tailor Guild Order";
                case 3:
                    return "Journeyman Tailor Guild Order";
                case 4:
                    return "Expert Tailor Guild Order";
                case 5:
                    return "Master Tailor Guild Order";

            }
        }

        private CraftResource GetQuestResource(int rank)
        {
            switch (rank)
            {
                default:
                    return CraftResource.None;
                case 1:
                    return CraftResource.RegularLeather;
                case 2:
                    if (Utility.Random(100) > 50)
                    {
                        return CraftResource.RegularLeather;
                    }
                    else
                    {
                        return CraftResource.SpinedLeather;
                    }
                case 3:
                    if (Utility.Random(100) > 50)
                    {
                        return CraftResource.SpinedLeather;
                    }
                    else
                    {
                        return CraftResource.HornedLeather;
                    }
                case 4:
                    if (Utility.Random(99) > 66)
                    {
                        return CraftResource.SpinedLeather;
                    }
                    else if (Utility.Random(99) > 66)
                    {
                        return CraftResource.HornedLeather;
                    }
                    else
                    {
                        return CraftResource.BarbedLeather;
                    }
                case 5:
                    if (Utility.Random(100) > 50)
                    {
                        return CraftResource.HornedLeather;
                    }
                    else
                    {
                        return CraftResource.BarbedLeather;
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

    public class TailorBulkOrderQuestRank1 : BaseTailorBulkOrderQuest
    {
        public TailorBulkOrderQuestRank1()
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

    public class TailorBulkOrderQuestRank2 : BaseTailorBulkOrderQuest
    {
        public TailorBulkOrderQuestRank2()
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

    public class TailorBulkOrderQuestRank3 : BaseTailorBulkOrderQuest
    {
        public TailorBulkOrderQuestRank3()
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

    public class TailorBulkOrderQuestRank4 : BaseTailorBulkOrderQuest
    {
        public TailorBulkOrderQuestRank4()
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

    public class TailorBulkOrderQuestRank5 : BaseTailorBulkOrderQuest
    {
        public TailorBulkOrderQuestRank5()
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
