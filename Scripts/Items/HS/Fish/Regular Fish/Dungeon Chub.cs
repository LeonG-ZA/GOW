using System;

namespace Server.Items
{
    public class DungeonChub : BaseHighSeasFish
    {
        [Constructable]
        public DungeonChub()
            : base(0x4306)
        {
            this.Weight = Utility.RandomMinMax(10, 20);
        }

        public DungeonChub(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116424;
            }
        }//dungeon chub

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