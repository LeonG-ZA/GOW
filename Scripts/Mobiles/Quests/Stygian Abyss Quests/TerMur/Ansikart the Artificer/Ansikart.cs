using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class Ansikart : MondainQuester
    {
        private static Type[] m_Quests = new Type[]
			{
				typeof(MasteringTheSoulforgeQuest),
                typeof(ALittleSomething)
			};

        public override Type[] Quests { get { return m_Quests; } }

        public override void Advertise()
        {
            Say(1112528); // Master the art of unraveling magic.
        }

        [Constructable]
        public Ansikart()
            : base("Ansikart", "the Artificer")
        {
            SetSkill(SkillName.ItemID, 60.0, 83.0);
            SetSkill(SkillName.Imbuing, 60.0, 83.0);
        }

        public Ansikart(Serial serial)
            : base(serial)
        {
        }

        public override void InitBody()
        {
            InitStats(100, 100, 25);

            CantWalk = true;
            Race = Race.Gargoyle;

            Hue = 0x86DF;
            HairItemID = 0x425D;
            HairHue = 0x321;
        }

        public override void InitOutfit()
        {
            AddItem(new SerpentStoneStaff());
            AddItem(new GargishClothChest(1428));
            AddItem(new GargishClothArms(1445));
            AddItem(new GargishClothKilt(1443));
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