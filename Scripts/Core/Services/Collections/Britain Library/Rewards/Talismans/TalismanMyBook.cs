namespace Server.Items
{
    public class TalismanMyBook : BaseTalisman, ICollectionItem
    {
        public override int LabelNumber { get { return 1073355; } } // Library Talisman - My Book

        [Constructable]
        public TalismanMyBook()
            : base(0x2F5A)
        {
            Weight = 1.0;

            Skill = SkillName.Inscribe;
            SuccessBonus = GetRandomSuccessful();
            ExceptionalBonus = GetRandomExceptional();
            Attributes.BonusInt = 5;
            Attributes.BonusMana = 2;
        }

        public TalismanMyBook(Serial serial)
            : base(serial)
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

            /*int version = */
            reader.ReadInt();
        }
    }
}