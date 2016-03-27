using System;

namespace Server.Items
{
    [TypeAlias("Server.Items.GargishEarringsArmor")]
    public class GargishEarrings : BaseArmor
    {
        public override int BasePhysicalResistance { get { return 1; } }
        public override int BaseFireResistance { get { return 2; } }
        public override int BaseColdResistance { get { return 2; } }
        public override int BasePoisonResistance { get { return 2; } }
        public override int BaseEnergyResistance { get { return 3; } }
        public override int InitMinHits { get { return 25; } }
        public override int InitMaxHits { get { return 35; } }

        public override int AosStrReq { get { return 10; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }
        public override CraftResource DefaultResource { get { return CraftResource.Iron; } }

        public override Race RequiredRace { get { return Race.Gargoyle; } }

        [Constructable]
        public GargishEarrings()
            : base(0x4213)
        {
            Layer = Layer.Earrings;
            Weight = 1.0;
        }

        public GargishEarrings(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version < 1)
            {
                int oldHue = Hue;
                Resource = CraftResource.Iron;
                Hue = oldHue;

                ArmorAttributes.MageArmor = 0;
            }
        }
    }
}