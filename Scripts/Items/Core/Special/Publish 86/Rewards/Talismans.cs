using System;
using Server;

namespace Server.Items
{
    public class LuckyCharm : BaseTalisman, ITokunoDyable
    {
        public override int LabelNumber { get { return 1154725; } }

        [Constructable]
        public LuckyCharm()
            : base(0x2F5B)
        {
            Hue = 0x0783;
            Weight = 1;
            Attributes.RegenHits = 1;
            Attributes.RegenStam = 1;
            Attributes.RegenMana = 1;
            Attributes.Luck = 150;

        }
        public LuckyCharm(Serial serial)
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

    public class WizardsCurio : BaseTalisman, ITokunoDyable
    {
        public override int LabelNumber { get { return 1154729; } }

        [Constructable]
        public WizardsCurio()
            : base(0x2F58)
        {
            Hue = 0x0778;
            Weight = 1;
            SkillBonuses.Skill_1_Name = SkillName.EvalInt;
            SkillBonuses.Skill_1_Value = 10;
            Attributes.RegenMana = 1;
            Attributes.SpellDamage = 5;
            Attributes.LowerRegCost = 10;
        }
        public WizardsCurio(Serial serial)
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

    public class SoldiersMedal : BaseTalisman, ITokunoDyable
    {
        public override int LabelNumber { get { return 1154726; } }

        [Constructable]
        public SoldiersMedal()
            : base(0x2F5B)
        {
            Hue = 0x076E;
            Weight = 1;
            SkillBonuses.Skill_1_Name = SkillName.Tactics;
            SkillBonuses.Skill_1_Value = 10;
            Attributes.RegenStam = 2;
            Attributes.AttackChance = 5;
            Attributes.WeaponDamage = 20;
        }
        public SoldiersMedal(Serial serial)
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

    public class DuelistsEdge : BaseTalisman, ITokunoDyable
    {
        public override int LabelNumber { get { return 1154727; } }

        [Constructable]
        public DuelistsEdge()
            : base(0x2F58)
        {
            Hue = 0x076E;
            Weight = 1;
            SkillBonuses.Skill_1_Name = SkillName.Anatomy;
            SkillBonuses.Skill_1_Value = 10;
            Attributes.RegenStam = 2;
            Attributes.AttackChance = 5;
            Attributes.WeaponDamage = 20;
        }
        public DuelistsEdge(Serial serial)
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

    public class MysticsMemento : BaseTalisman, ITokunoDyable
    {
        public override int LabelNumber { get { return 1154730; } }

        [Constructable]
        public MysticsMemento()
            : base(0x2F5B)
        {
            Hue = 0x0778;
            Weight = 1;
            SkillBonuses.Skill_1_Name = SkillName.Focus;
            SkillBonuses.Skill_1_Value = 10;
            Attributes.RegenMana = 1;
            Attributes.SpellDamage = 5;
            Attributes.LowerRegCost = 10;
        }
        public MysticsMemento(Serial serial)
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

    public class NecromancersPhylactery : BaseTalisman, ITokunoDyable
    {
        public override int LabelNumber { get { return 1154728; } }

        [Constructable]
        public NecromancersPhylactery():base(0x2F5B)
        {
            Hue = 0x0783;
            Weight = 1;
            SkillBonuses.Skill_1_Name = SkillName.SpiritSpeak;
            SkillBonuses.Skill_1_Value = 10;
            Attributes.RegenMana = 1;
            Attributes.SpellDamage = 5;
            Attributes.LowerRegCost = 10;
        }
        public NecromancersPhylactery(Serial serial):base(serial)
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