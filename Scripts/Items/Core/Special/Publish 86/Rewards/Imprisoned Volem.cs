using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Vollem  corpse")]
    public class CrystalVollem : BaseCreature
    {
        [Constructable]
        public CrystalVollem()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Vollem";
            Body = 0x0125;
            SetStr(496, 525);
            SetDex(86, 105);
            SetInt(86, 125);

            SetHits(298, 315);

            SetDamage(16, 22);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 40);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 85.5, 100);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 80.5, 92.5);
            SetSkill(SkillName.Magery, 10.4, 50);
            SetSkill(SkillName.EvalInt, 10.4, 50);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 90;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 80;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("A Vollem");
        }

        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool StatLossAfterTame { get { return false; } }

        public CrystalVollem(Serial serial)
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


namespace Server.Items
{
    public class ImprisonedVollem : BaseImprisonedMobile
    {
        public override BaseCreature Summon { get { return new CrystalVollem(); } }

        [Constructable]
        public ImprisonedVollem()
            : base(0x1F1C)
        {
            Name = "An Imprisoned Vollem";
            Weight = 1.0;
            Hue = 0x0482;
        }

        public ImprisonedVollem(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Skills[SkillName.AnimalTaming].Value >= 80)
                base.OnDoubleClick(from);
            else
                from.SendMessage("You must have 80 Animal Taming to release this beast.");
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
