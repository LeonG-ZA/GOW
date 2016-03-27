using System;

namespace Server.Items
{
    [Flipable(0x232A, 0x232B)]
    public class GiftBox2008 : GiftBox
    {
        private static readonly int[] RareNeonHues =
        {
            1153,
            1154,
            1167,
            1177,
            1174,
            1152,
            1,
            1180,
            1179,
            1178,
            1098,
            1099,
            1081,
            1067,
            1087,
            1085,
            1076,
            1086,
            1091,
            1097,
            1089,
            1066,
            1096,
            1083,
            1090,
            1093,
            1062,
            1078,
            1069,
            1059,
            1073,
            1084,
            1094,
            1064,
            1095,
            1074,
            1070,
            1092,
            1068,
            1088,
            1065,
            1082,
            1060,
            1071,
            1080,
            1075,
            1077,
            1063,
            1079,
            1061,
            1100,
            1101,
            1150,
            1167,
            1151,
            1161
        };

        public static int RandomRareNeonBoxHue
        {
            get
            {
                return RareNeonHues[Utility.Random(RareNeonHues.Length)];
            }
        }

        [Constructable]
        public GiftBox2008()
        {
            Hue = GiftBoxHues.RandomGiftBoxHue;
        }

        public GiftBox2008(Serial serial)
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