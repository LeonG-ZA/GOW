using System;

namespace Server.Items
{
    public class DungeonPike : BaseHighSeasFish
    {
        [Constructable]
        public DungeonPike()
            : base(0x44C3)
        {
        	Hue = 2727;
        }

        public DungeonPike(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1116107;
            }
        }//dungeon pike

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