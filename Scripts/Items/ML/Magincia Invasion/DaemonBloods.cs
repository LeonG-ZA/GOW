using System;

namespace Server.Items
{

    public class DaemonBloods : Item
    {
        public override int LabelNumber { get { return 1023965; } } // Daemon Blood
        public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

        [Constructable]
        public DaemonBloods()
            : base(0x122A)
        {
        }

        public DaemonBloods(Serial serial)
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

            int version = reader.ReadInt();
        }
    }
    public class DaemonBloods1 : Item
    {
        public override int LabelNumber { get { return 1023965; } } // Daemon Blood
        public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

        [Constructable]
        public DaemonBloods1()
            : base(0x122B)
        {
        }

        public DaemonBloods1(Serial serial)
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

            int version = reader.ReadInt();
        }
    }
    public class DaemonBloods2 : Item
    {
        public override int LabelNumber { get { return 1023965; } } // Daemon Blood
        public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

        [Constructable]
        public DaemonBloods2()
            : base(0x122C)
        {
        }

        public DaemonBloods2(Serial serial)
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

            int version = reader.ReadInt();
        }
    }
    public class DaemonBloods3 : Item
    {
        public override int LabelNumber { get { return 1023965; } } // Daemon Blood
        public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

        [Constructable]
        public DaemonBloods3()
            : base(0x122D)
        {
        }

        public DaemonBloods3(Serial serial)
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

            int version = reader.ReadInt();
        }
    }
    public class DaemonBloods4 : Item
    {
        public override int LabelNumber { get { return 1023965; } } // Daemon Blood
        public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

        [Constructable]
        public DaemonBloods4()
            : base(0x122E)
        {
        }

        public DaemonBloods4(Serial serial)
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

            int version = reader.ReadInt();
        }
    }
    public class DaemonBloods5 : Item
    {
        public override int LabelNumber { get { return 1023965; } } // Daemon Blood
        public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

        [Constructable]
        public DaemonBloods5()
            : base(0x122F)
        {
        }

        public DaemonBloods5(Serial serial)
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

            int version = reader.ReadInt();
        }
    }
}