using System;
using System.Collections.Generic;
using Server.Engines.XmlSpawner2;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Fifth;
using Server.Spells.Ninjitsu;
using Server.Spells.Seventh;

namespace Server.Services.BaseAttributes
{
    [PropertyObject]
    public abstract class BaseAttributes
    {
        private readonly Item _Owner;
        private uint _Names;
        private int[] _Values;

        private static readonly int[] _Empty = new int[0];

        public bool IsEmpty
        {
            get
            {
                return (_Names == 0);
            }
        }
        public Item Owner
        {
            get
            {
                return _Owner;
            }
        }

        public BaseAttributes(Item owner)
        {
            _Owner = owner;
            _Values = _Empty;
        }

        public BaseAttributes(Item owner, BaseAttributes other)
        {
            _Owner = owner;
            _Values = new int[other._Values.Length];
            other._Values.CopyTo(_Values, 0);
            _Names = other._Names;
        }

        public BaseAttributes(Item owner, GenericReader reader)
        {
            _Owner = owner;

            int version = reader.ReadByte();

            switch (version)
            {
                case 1:
                    {
                        _Names = reader.ReadUInt();
                        _Values = new int[reader.ReadEncodedInt()];

                        for (int i = 0; i < _Values.Length; ++i)
                        {
                            _Values[i] = reader.ReadEncodedInt();
                        }

                        break;
                    }
                case 0:
                    {
                        _Names = reader.ReadUInt();
                        _Values = new int[reader.ReadInt()];

                        for (int i = 0; i < _Values.Length; ++i)
                        {
                            _Values[i] = reader.ReadInt();
                        }

                        break;
                    }
            }
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write((byte)1); // version;

            writer.Write((uint)_Names);
            writer.WriteEncodedInt((int)_Values.Length);

            for (int i = 0; i < _Values.Length; ++i)
            {
                writer.WriteEncodedInt((int)_Values[i]);
            }
        }

        public int GetValue(int bitmask)
        {
            if (!Core.AOS)
            {
                return 0;
            }

            uint mask = (uint)bitmask;

            if ((_Names & mask) == 0)
            {
                return 0;
            }

            int index = GetIndex(mask);

            if (index >= 0 && index < _Values.Length)
            {
                return _Values[index];
            }

            return 0;
        }

        public void SetValue(int bitmask, int value)
        {
            if ((bitmask == (int)AosWeaponAttribute.DurabilityBonus) && (this is AosWeaponAttributes))
            {
                if (_Owner is BaseWeapon)
                {
                    ((BaseWeapon)_Owner).UnscaleDurability();
                }
            }
            else if ((bitmask == (int)AosArmorAttribute.DurabilityBonus) && (this is AosArmorAttributes))
            {
                if (_Owner is BaseArmor)
                {
                    ((BaseArmor)_Owner).UnscaleDurability();
                }
                else if (_Owner is BaseClothing)
                {
                    ((BaseClothing)_Owner).UnscaleDurability();
                }
            }

            uint mask = (uint)bitmask;

            if (value != 0)
            {
                if ((_Names & mask) != 0)
                {
                    int index = GetIndex(mask);

                    if (index >= 0 && index < _Values.Length)
                    {
                        _Values[index] = value;
                    }
                }
                else
                {
                    int index = GetIndex(mask);

                    if (index >= 0 && index <= _Values.Length)
                    {
                        int[] old = _Values;
                        _Values = new int[old.Length + 1];

                        for (int i = 0; i < index; ++i)
                        {
                            _Values[i] = old[i];
                        }

                        _Values[index] = value;

                        for (int i = index; i < old.Length; ++i)
                        {
                            _Values[i + 1] = old[i];
                        }

                        _Names |= mask;
                    }
                }
            }
            else if ((_Names & mask) != 0)
            {
                int index = GetIndex(mask);

                if (index >= 0 && index < _Values.Length)
                {
                    _Names &= ~mask;

                    if (_Values.Length == 1)
                    {
                        _Values = _Empty;
                    }
                    else
                    {
                        int[] old = _Values;
                        _Values = new int[old.Length - 1];

                        for (int i = 0; i < index; ++i)
                        {
                            _Values[i] = old[i];
                        }

                        for (int i = index + 1; i < old.Length; ++i)
                        {
                            _Values[i - 1] = old[i];
                        }
                    }
                }
            }

            if ((bitmask == (int)AosWeaponAttribute.DurabilityBonus) && (this is AosWeaponAttributes))
            {
                if (_Owner is BaseWeapon)
                {
                    ((BaseWeapon)_Owner).ScaleDurability();
                }
            }
            else if ((bitmask == (int)AosArmorAttribute.DurabilityBonus) && (this is AosArmorAttributes))
            {
                if (_Owner is BaseArmor)
                {
                    ((BaseArmor)_Owner).ScaleDurability();
                }
                else if (_Owner is BaseClothing)
                {
                    ((BaseClothing)_Owner).ScaleDurability();
                }
            }

            if (_Owner != null && _Owner.Parent is Mobile)
            {
                Mobile m = (Mobile)_Owner.Parent;

                m.CheckStatTimers();
                m.UpdateResistances();
                m.Delta(MobileDelta.Stat | MobileDelta.WeaponDamage | MobileDelta.Hits | MobileDelta.Stam | MobileDelta.Mana);

                if (this is AosSkillBonuses)
                {
                    ((AosSkillBonuses)this).Remove();
                    ((AosSkillBonuses)this).AddTo(m);
                }
            }

            if (_Owner != null)
            {
                _Owner.InvalidateProperties();
            }
        }

        private int GetIndex(uint mask)
        {
            int index = 0;
            uint ourNames = _Names;
            uint currentBit = 1;

            while (currentBit != mask)
            {
                if ((ourNames & currentBit) != 0)
                {
                    ++index;
                }

                if (currentBit == 0x80000000)
                {
                    return -1;
                }

                currentBit <<= 1;
            }
            return index;
        }
    }
}