using System;

namespace Server.Items 
{
    public class StatueSouth : BaseStatue
    { 
        [Constructable] 
        public StatueSouth()
            : base(0x139A)
        {
        }

        public StatueSouth(Serial serial)
            : base(serial)
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

    public class StatueSouth2 : BaseStatue
    { 
        [Constructable] 
        public StatueSouth2()
            : base(0x1227)
        { 
            this.Weight = 10; 
        }

        public StatueSouth2(Serial serial)
            : base(serial)
        { 
        }

        public override bool ForceShowProperties
        {
            get
            {
                return ObjectPropertyList.Enabled;
            }
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

    public class StatueNorth : BaseStatue
    { 
        [Constructable] 
        public StatueNorth()
            : base(0x139B)
        { 
            this.Weight = 10; 
        }

        public StatueNorth(Serial serial)
            : base(serial)
        { 
        }

        public override bool ForceShowProperties
        {
            get
            {
                return ObjectPropertyList.Enabled;
            }
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

    public class StatueWest : BaseStatue
    { 
        [Constructable] 
        public StatueWest()
            : base(0x1226)
        { 
            this.Weight = 10; 
        }

        public StatueWest(Serial serial)
            : base(serial)
        { 
        }

        public override bool ForceShowProperties
        {
            get
            {
                return ObjectPropertyList.Enabled;
            }
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

    public class StatueEast : BaseStatue
    { 
        [Constructable] 
        public StatueEast()
            : base(0x139C)
        { 
            this.Weight = 10; 
        }

        public StatueEast(Serial serial)
            : base(serial)
        { 
        }

        public override bool ForceShowProperties
        {
            get
            {
                return ObjectPropertyList.Enabled;
            }
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

    public class StatueEast2 : BaseStatue
    { 
        [Constructable] 
        public StatueEast2()
            : base(0x1224)
        { 
            this.Weight = 10; 
        }

        public StatueEast2(Serial serial)
            : base(serial)
        { 
        }

        public override bool ForceShowProperties
        {
            get
            {
                return ObjectPropertyList.Enabled;
            }
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

    public class StatueSouthEast : BaseStatue
    { 
        [Constructable] 
        public StatueSouthEast()
            : base(0x1225)
        { 
            this.Weight = 10; 
        }

        public StatueSouthEast(Serial serial)
            : base(serial)
        { 
        }

        public override bool ForceShowProperties
        {
            get
            {
                return ObjectPropertyList.Enabled;
            }
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

    public class BustSouth : BaseStatue
    { 
        [Constructable] 
        public BustSouth()
            : base(0x12CB)
        { 
            this.Weight = 10; 
        }

        public BustSouth(Serial serial)
            : base(serial)
        { 
        }

        public override bool ForceShowProperties
        {
            get
            {
                return ObjectPropertyList.Enabled;
            }
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

    public class BustEast : BaseStatue
    { 
        [Constructable] 
        public BustEast()
            : base(0x12CA)
        { 
            this.Weight = 10; 
        }

        public BustEast(Serial serial)
            : base(serial)
        { 
        }

        public override bool ForceShowProperties
        {
            get
            {
                return ObjectPropertyList.Enabled;
            }
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

    public class StatuePegasus : BaseStatue
    { 
        [Constructable] 
        public StatuePegasus()
            : base(0x139D)
        { 
            this.Weight = 10; 
        }

        public StatuePegasus(Serial serial)
            : base(serial)
        { 
        }

        public override bool ForceShowProperties
        {
            get
            {
                return ObjectPropertyList.Enabled;
            }
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

    public class StatuePegasus2 : BaseStatue
    { 
        [Constructable] 
        public StatuePegasus2()
            : base(0x1228)
        { 
            this.Weight = 10; 
        }

        public StatuePegasus2(Serial serial)
            : base(serial)
        { 
        }

        public override bool ForceShowProperties
        {
            get
            {
                return ObjectPropertyList.Enabled;
            }
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

    public class SmallTowerSculpture : BaseStatue
    {
        [Constructable]
        public SmallTowerSculpture()
            : base(0x241A)
        {
            this.Weight = 20.0;
        }

        public SmallTowerSculpture(Serial serial)
            : base(serial)
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