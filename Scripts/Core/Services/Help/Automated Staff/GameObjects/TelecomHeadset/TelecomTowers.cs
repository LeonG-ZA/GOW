namespace Server.Items
{
    public class CommTower : Item
    {
        [Constructable]
        public CommTower() : base(2853)
        {
            Name = "Comm Tower";
            Movable = false;
        }

        public CommTower(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}