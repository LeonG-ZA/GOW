using Server;
using System;

namespace Server.Items
{
    public class CleanupMushrooms1 : Item
    {
        public override int LabelNumber { get { return 1023340; } }

        [Constructable]
        public CleanupMushrooms1()
            : base(0x0D11)
        {
            Weight = 1;
        }
        public CleanupMushrooms1(Serial serial)
            : base(serial)
        {

        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
    public class CleanupMushrooms2 : Item
    {
        public override int LabelNumber { get { return 1023340; } }

        [Constructable]
        public CleanupMushrooms2()
            : base(0x0D12)
        {
            Weight = 1;
        }
        public CleanupMushrooms2(Serial serial)
            : base(serial)
        {

        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
    public class CleanupMushrooms3 : Item
    {
        public override int LabelNumber { get { return 1023340; } }

        [Constructable]
        public CleanupMushrooms3()
            : base(0x0D0F)
        {
            Weight = 1;
        }
        public CleanupMushrooms3(Serial serial)
            : base(serial)
        {

        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}