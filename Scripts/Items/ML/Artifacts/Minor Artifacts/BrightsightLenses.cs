using System;

namespace Server.Items
{
    //[Alterable(typeof(DefTinkering), typeof(GargishGlasses), true)]
    public class BrightsightLenses : BaseArmor
    {
        public override int LabelNumber { get { return 1075039; } } // Brightsight Lenses

        public override int BasePhysicalResistance { get { return 2; } }
        public override int BaseFireResistance { get { return 4; } }
        public override int BaseColdResistance { get { return 3; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 3; } }

        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        public override int AosStrReq { get { return 45; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }

        [Constructable]
        public BrightsightLenses()
            : base(0x2FB8)
        {
            Layer = Layer.Helm;
            Weight = 2.0;
            Hue = 1281;

            Attributes.NightSight = 1;
            Attributes.RegenMana = 3;
            ArmorAttributes.SelfRepair = 3;
        }

        public BrightsightLenses(Serial serial)
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