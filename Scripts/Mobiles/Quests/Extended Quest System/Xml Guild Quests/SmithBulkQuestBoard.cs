using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Engines.Quests
{		
    public class SmithBulkQuestBoard : BaseGuildQuestBoard
	{
		[Constructable]
        public SmithBulkQuestBoard(int rank) : base("Blacksmith", rank)
		{
            Rank = rank;
		}

        [Constructable]
        public SmithBulkQuestBoard() : base("Blacksmith", 1)
        {
        }

        public SmithBulkQuestBoard(Serial serial)
            : base(serial)
		{
		}

        public override void OnDoubleClick(Mobile m)
        {
            XmlQuestGuildBlacksmith guild = (XmlQuestGuildBlacksmith)XmlAttach.FindAttachment(m, typeof(XmlQuestGuildBlacksmith));

            if (guild == null)
                XmlAttach.AttachTo(m, new XmlQuestGuildBlacksmith());

            base.OnDoubleClick(m);
        }

        public override Type[] Quests
        {
            get
            {
                switch (Rank)
                {
                    default:
                        return new Type[] { typeof(BaseSmithBulkOrderQuest) };
                    case 1:
                        return new Type[] { typeof(SmithBulkOrderQuestRank1) };
                    case 2:
                        return new Type[] { typeof(SmithBulkOrderQuestRank2) };
                    case 3:
                        return new Type[] { typeof(SmithBulkOrderQuestRank3) };
                    case 4:
                        return new Type[] { typeof(SmithBulkOrderQuestRank4) };
                    case 5:
                        return new Type[] { typeof(SmithBulkOrderQuestRank5) };
                }
            }
        }
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}