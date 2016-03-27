using System;

namespace Server.Items
{
    public interface IArmor : IMagicalItem, IMagicalBonus, IResistances, ISkillBonuses, ISAAbsorption
    {
        void UnscaleDurability();
        void ScaleDurability();
        AosArmorAttributes ArmorAttribute { get; set; }
    }
}
