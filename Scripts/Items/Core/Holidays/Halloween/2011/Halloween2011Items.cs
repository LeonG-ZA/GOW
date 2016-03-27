using System;

namespace Server.Items
{
    public class LargeDyingPlant2 : Item
    {
        [Constructable]
        public LargeDyingPlant2()
            : base(0x42B9)
        {
            Weight = 10.0;
        }

        public LargeDyingPlant2(Serial serial)
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

    public class DyingPlant2 : Item
    {
        [Constructable]
        public DyingPlant2()
            : base(0x42BA)
        {
            Weight = 5.0;
        }

        public DyingPlant2(Serial serial)
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

    [FlipableAttribute(0x4688, 0x4689)]
    public class BlackCatStatue : Item
    {
        [Constructable]
        public BlackCatStatue()
            : base(0x4688)
        {
            Weight = 5.0;
        }

        public BlackCatStatue(Serial serial)
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

    [Flipable(0xEE3, 0xEE4, 0xEE5, 0xEE6)]
    public class HalloweenWeb : Item
    {

        [Constructable]
        public HalloweenWeb()
            : base(0xEE3)
        {
            Weight = 1.0;
            Name = " A Spooky Spiderweb";
            Hue = Utility.RandomList(43, 1175, 1151);
            Light = LightType.Circle150;
            LootType = LootType.Blessed;
        }

        public HalloweenWeb(Serial serial)
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