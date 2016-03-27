using Server.Engines.Craft;
using System;

namespace Server.Items
{
    [Alterable(typeof(DefTinkering), typeof(GargishGlasses), true)]
    public class ElvenGlasses : BaseArmor
    {
        private int m_HitLowerDefend = 0;

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitLowerDefend
        {
            get { return m_HitLowerDefend; }
            set
            {
                m_HitLowerDefend = value;
                InvalidateProperties();
            }
        }

        public override int BasePhysicalResistance { get { return 2; } }
        public override int BaseFireResistance { get { return 4; } }
        public override int BaseColdResistance { get { return 3; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 3; } }

        public override int InitMinHits { get { return 36; } }
        public override int InitMaxHits { get { return 48; } }

        public override int AosStrReq { get { return 45; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }
        public override CraftResource DefaultResource { get { return CraftResource.Iron; } }

        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

        [Constructable]
        public ElvenGlasses()
            : base(0x2FB8)
        {
            Weight = 2.0;
        }

        public ElvenGlasses(Serial serial)
            : base(serial)
        {
        }

        public override void AddMagicalProperties(ObjectPropertyList list)
        {
            int prop;

            if ((prop = HitLowerDefend) > 0)
                list.Add(1060425, prop.ToString()); // hit lower defense ~1_val~%

            base.AddMagicalProperties(list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write((int)m_HitLowerDefend);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            /*int version = */
            reader.ReadInt();

            m_HitLowerDefend = reader.ReadInt();
        }
    }
}