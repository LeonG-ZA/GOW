using System;

namespace Server.Items
{
    public interface IMagicalItem
    {
        AosAttributes Attributes { get; set; }
    }

    public interface IMagicalBonus
    {
        int GetAttributeBonus(AosAttributes attr);
    }

    public interface IResistances
    {
        AosElementAttribute Resistances { get; set; }
    }

    public interface ISkillBonuses
    {
        AosSkillBonuses SkillBonuses { get; set; }
    }

    public interface IWeapon2 : IMagicalItem, IMagicalBonus, IResistances, ISkillBonuses, ISAAbsorption
    {
        int GetMaxRange(Mobile attacker, Mobile defender);
        int MaxRange { get; }
        void OnBeforeSwing(Mobile attacker, Mobile defender);
        TimeSpan OnSwing(Mobile attacker, Mobile defender);
        void GetStatusDamage(Mobile from, out int min, out int max);
        void UnscaleDurability();
        void ScaleDurability();
        TimeSpan GetDelay(Mobile attacker);
        AosWeaponAttributes WeaponAttributes { get; set; }
    }
}
