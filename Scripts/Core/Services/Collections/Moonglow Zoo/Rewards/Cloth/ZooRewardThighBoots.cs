namespace Server.Items
{
    public class ZooRewardThighBoots : ThighBoots
    {
        public override int LabelNumber { get { return 1073221; } } // Britannia Royal Zoo Member

        [Constructable]
        public ZooRewardThighBoots()
        {
            ItemID = 0x1712;
            Hue = 1365;
        }

        public ZooRewardThighBoots(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            /*int version = */
            reader.ReadInt();
        }
    }
}