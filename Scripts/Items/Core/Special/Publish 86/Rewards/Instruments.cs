using Server;
using System;

namespace Server.Items
{
    public class NaturesTears : BaseInstrument
    {
        public override int LabelNumber { get { return 1154373; } }

        [Constructable]
        public NaturesTears() : base(0, 0, 0)
        {
            Slayer = SlayerName.Fey;
            Weight = 10;
            Hue = 0x081B;
            UsesRemaining = 450;

            switch (Utility.RandomMinMax(0, 2))
            {
                case 0:
                    {
                        ItemID = 0xE9C;
                        SuccessSound = 0x38;
                        FailureSound = 0x39;
                        break;
                    }
                case 1:
                    {
                        ItemID = 0xEB3;
                        SuccessSound = 0x4C;
                        FailureSound = 0x4D;
                        break;
                    }
                default:
                    {
                        ItemID = 0xEB2;
                        SuccessSound = 0x45;
                        FailureSound = 0x46;
                        break;
                    }

            }
        }

        public NaturesTears(Serial serial):base(serial)
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

    public class ArachnidDoom : BaseInstrument
    {
        public override int LabelNumber { get { return 1017401; } }

        [Constructable]
        public ArachnidDoom()
            : base(0, 0, 0)
        {
            Slayer = SlayerName.ArachnidDoom;
            Weight = 10;
            Hue = 0x0798;
            UsesRemaining = 450;

            switch (Utility.RandomMinMax(0, 2))
            {
                case 0:
                    {
                        ItemID = 0xE9C;
                        SuccessSound = 0x38;
                        FailureSound = 0x39;
                        break;
                    }
                case 1:
                    {
                        ItemID = 0xEB3;
                        SuccessSound = 0x4C;
                        FailureSound = 0x4D;
                        break;
                    }
                default:
                    {
                        ItemID = 0xEB2;
                        SuccessSound = 0x45;
                        FailureSound = 0x46;
                        break;
                    }

            }
        }

        public ArachnidDoom(Serial serial)
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

    public class PrimordialDecay : BaseInstrument
    {
        public override int LabelNumber { get { return 1154723; } }

        [Constructable]
        public PrimordialDecay()
            : base(0, 0, 0)
        {
            Slayer = SlayerName.ElementalBan;
            Weight = 10;
            Hue = 0x0787;
            UsesRemaining = 450;

            switch (Utility.RandomMinMax(0, 2))
            {
                case 0:
                    {
                        ItemID = 0xE9C;
                        SuccessSound = 0x38;
                        FailureSound = 0x39;
                        break;
                    }
                case 1:
                    {
                        ItemID = 0xEB3;
                        SuccessSound = 0x4C;
                        FailureSound = 0x4D;
                        break;
                    }
                default:
                    {
                        ItemID = 0xEB2;
                        SuccessSound = 0x45;
                        FailureSound = 0x46;
                        break;
                    }

            }
        }

        public PrimordialDecay(Serial serial)
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