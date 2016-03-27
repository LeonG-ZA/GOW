using System;

namespace Server.Items
{
    public enum ShadowguardWallTypes
    {
        Corner,
        SouthWall,
        EastWall,
        CornerPost,
        SouthWallPlain,
        EastWallPlain,
        CornerPlain,
        CornerArchPlain,
        SouthWindow,
        EastWindow,
        CornerMedium,
        SouthWallMedium,
        EastWallMedium,
        CornerPostMedium,
        CornerShort,
        SouthWallShort,
        EastWallShort,
        CornerPostShort,
        CornerArch,
        SouthArch,
        WestArch,
        EastArchPlain,
        NorthArch,
        SouthCenterArchTall,
        EastCenterArchTall,
        EastCornerArchTall,
        SouthCornerArchTall,
        SmallSouthWall,
        SmallEastWall,
        SmallCorner,
        SmallPost

    }

    public class ShadowguardWalls : BaseWall
    {
        [Constructable]
        public ShadowguardWalls(ShadowguardWallTypes type)
            : base(0x9B15 + (int)type)
        {
        }

        public ShadowguardWalls(Serial serial)
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

    public class ShadowguardArchSouthPlain : Item
    {
        [Constructable]
        public ShadowguardArchSouthPlain()
            : base(0x9B87)
        {
            Movable = false;
        }

        public ShadowguardArchSouthPlain(Serial serial)
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

    public class ShadowguardArchEast : Item
    {
        [Constructable]
        public ShadowguardArchEast()
            : base(0x9B88)
        {
            Movable = false;
        }

        public ShadowguardArchEast(Serial serial)
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


    public class ShadowguardPlainPost : Item
    {
        [Constructable]
        public ShadowguardPlainPost()
            : base(0x9BD4)
        {
            Movable = false;
        }

        public ShadowguardPlainPost(Serial serial)
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

    public enum ShadowguardWhiteWallTypes
    {
        Corner,
        SouthWall,
        EastWall,
        CornerPost,
        SouthArch,
        WestArch,
        EastArch,
        NorthArch,
        CornerArch

    }

    public class ShadowguardWhiteWalls : BaseWall
    {
        [Constructable]
        public ShadowguardWhiteWalls(ShadowguardWhiteWallTypes type)
            : base(0x9B7E + (int)type)
        {
        }

        public ShadowguardWhiteWalls(Serial serial)
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

    public enum ShadowguardSmallWhiteWallTypes
    {
        Corner,
        SouthWall,
        EastWall,
        CornerPost
    }

    public class ShadowguardSmallWhiteWalls : BaseWall
    {
        [Constructable]
        public ShadowguardSmallWhiteWalls(ShadowguardSmallWhiteWallTypes type)
            : base(0x9BD0 + (int)type)
        {
        }

        public ShadowguardSmallWhiteWalls(Serial serial)
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