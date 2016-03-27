using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.Quests
{
    public class TailorBulkQuestGiver : BaseGuildQuester
    {
        [Constructable]
        public TailorBulkQuestGiver(int rank)
            : base("Tailor", rank)
        {
            Rank = rank;

            SetSkill(SkillName.Meditation, 60.0, 83.0);
            SetSkill(SkillName.Focus, 60.0, 83.0);
        }

        [Constructable]
        public TailorBulkQuestGiver()
            : base("Tailor", 1)
        {
            SetSkill(SkillName.Meditation, 60.0, 83.0);
            SetSkill(SkillName.Focus, 60.0, 83.0);
        }

        public TailorBulkQuestGiver(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile m)
        {
            XmlQuestGuildTailor guild = (XmlQuestGuildTailor)XmlAttach.FindAttachment(m, typeof(XmlQuestGuildTailor));

            if (guild == null)
                XmlAttach.AttachTo(m, new XmlQuestGuildTailor());

            base.OnDoubleClick(m);
        }

        public override Type[] Quests
        {
            get
            {
                switch (Rank)
                {
                    default:
                        return new Type[] { typeof(BaseTailorBulkOrderQuest) };
                    case 1:
                        return new Type[] { typeof(TailorBulkOrderQuestRank1) };
                    case 2:
                        return new Type[] { typeof(TailorBulkOrderQuestRank2) };
                    case 3:
                        return new Type[] { typeof(TailorBulkOrderQuestRank3) };
                    case 4:
                        return new Type[] { typeof(TailorBulkOrderQuestRank4) };
                    case 5:
                        return new Type[] { typeof(TailorBulkOrderQuestRank5) };
                }
            }
        }

        public override void InitOutfit()
        {
            base.InitOutfit();
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