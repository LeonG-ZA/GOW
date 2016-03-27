using System;

namespace Server.Items
{
    public class SmithingPressComponent : SpecialVeteranAddonComponent
    {
        [Constructable]
        public SmithingPressComponent(int itemID)
            : base(itemID)
        {
        }

        public SmithingPressComponent(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1123577;
            }
        }// smithing press

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

    public class SmithingPressBoxComponent : SpecialVeteranAddonComponentBox
    {
        [Constructable]
        public SmithingPressBoxComponent(int itemID)
            : base(itemID)
        {
        }

        public SmithingPressBoxComponent(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1123593;
            }
        }// crates

        public override Type[] AllowedTools
        {
            get
            {
                return new Type[] { typeof(Tongs), typeof(SmithHammer), typeof(SledgeHammer) };
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
}
