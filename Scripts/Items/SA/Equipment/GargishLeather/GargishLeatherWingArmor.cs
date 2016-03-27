using System;

namespace Server.Items
{
    [FlipableAttribute(0x457E, 0x457F)]
    public class GargishLeatherWingArmor : BaseClothing, IQuiver
    {
        private int m_PhysicalDamage;
        private int m_FireDamage;
        private int m_ColdDamage;
        private int m_PoisonDamage;
        private int m_EnergyDamage;
        private int m_ChaosDamage;
        private int m_DirectDamage;
        private int m_DamageIncrease;

        [CommandProperty(AccessLevel.GameMaster)]
        public int PhysicalDamage { get { return m_PhysicalDamage; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FireDamage { get { return m_FireDamage; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ColdDamage { get { return m_ColdDamage; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PoisonDamage { get { return m_PoisonDamage; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EnergyDamage { get { return m_EnergyDamage; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ChaosDamage { get { return m_ChaosDamage; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DirectDamage { get { return m_DirectDamage; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DamageIncrease { get { return m_DamageIncrease; } }

        public override int LabelNumber { get { return 1115389; } }// gargish leather wing armor

        [Constructable]
        public GargishLeatherWingArmor()
            : base(0x457E, Layer.Cloak)
        {
            Weight = 2.0;
        }

        public GargishLeatherWingArmor(Serial serial)
            : base(serial)
        {
        }
        public override Race RequiredRace
        {
            get
            {
                return Race.Gargoyle;
            }
        }
        public override bool CanBeWornByGargoyles
        {
            get
            {
                return true;
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            AddElementalDamageProperties(list);

            if (DamageIncrease != 0)
                list.Add(1074762, DamageIncrease.ToString()); // Damage Modifier: ~1_PERCENT~%
        }

        private void AddElementalDamageProperties(ObjectPropertyList list)
        {
            if (DirectDamage != 0)
                list.Add(1079978, DirectDamage.ToString()); // Direct Damage: ~1_PERCENT~%

            if (PhysicalDamage != 0)
                list.Add(1060403, PhysicalDamage.ToString()); // physical damage ~1_val~%

            if (FireDamage != 0)
                list.Add(1060405, FireDamage.ToString()); // fire damage ~1_val~%

            if (ColdDamage != 0)
                list.Add(1060404, ColdDamage.ToString()); // cold damage ~1_val~%

            if (PoisonDamage != 0)
                list.Add(1060406, PoisonDamage.ToString()); // poison damage ~1_val~%

            if (EnergyDamage != 0)
                list.Add(1060407, EnergyDamage.ToString()); // energy damage ~1_val~%

            if (ChaosDamage != 0)
                list.Add(1072846, ChaosDamage.ToString()); // chaos damage ~1_val~%
        }

        private static void SetSaveFlag(ref SaveFlag flags, SaveFlag toSet, bool setIf)
        {
            if (setIf)
                flags |= toSet;
        }

        private static bool GetSaveFlag(SaveFlag flags, SaveFlag toGet)
        {
            return ((flags & toGet) != 0);
        }

        [Flags]
        private enum SaveFlag
        {
            None = 0x00000000,
            DamageIncrease = 0x00000001
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            SaveFlag flags = SaveFlag.None;

            SetSaveFlag(ref flags, SaveFlag.DamageIncrease, m_DamageIncrease != 0);

            writer.WriteEncodedInt((int)flags);

            if (GetSaveFlag(flags, SaveFlag.DamageIncrease))
                writer.Write((int)m_DamageIncrease);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            SaveFlag flags = (SaveFlag)reader.ReadEncodedInt();

            if (GetSaveFlag(flags, SaveFlag.DamageIncrease))
                m_DamageIncrease = reader.ReadInt();
        }

        #region Alter
        public override void AlterFrom(BaseQuiver orig)
        {
            base.AlterFrom(orig);

            m_PhysicalDamage = orig.PhysicalDamage;
            m_FireDamage = orig.FireDamage;
            m_ColdDamage = orig.ColdDamage;
            m_PoisonDamage = orig.PoisonDamage;
            m_EnergyDamage = orig.EnergyDamage;
            m_ChaosDamage = orig.ChaosDamage;
            m_DirectDamage = orig.DirectDamage;
            m_DamageIncrease = orig.DamageIncrease;
        }
        #endregion
    }
}