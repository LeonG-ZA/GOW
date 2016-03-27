using System;
namespace Server.Items
{
    class FletchingStationComponent : SpecialVeteranAddonComponent
    {        
        [Constructable]
        public FletchingStationComponent(int itemID)
            : base(itemID)
        {
        }

        public FletchingStationComponent(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1124006;
            }
        }// fletching station

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

    public class FletchingStationBoxComponent : SpecialVeteranAddonComponentBox
    {
        [Constructable]
        public FletchingStationBoxComponent(int itemID)
            : base(itemID)
        {
        }

        public FletchingStationBoxComponent(Serial serial)
            : base(serial)
        {
        }

        public override Type[] AllowedTools {
            get
            {
                return new Type[] { typeof(FletcherTools) };
            }
        }

        public override int LabelNumber
        {
            get
            {
                return 1124027;
            }
        }// rack

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
