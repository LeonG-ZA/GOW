using System;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
    [CorpseName("a lava elemental corpse")]
    public class LavaElemental : BaseCreature
    {
        [Constructable]
        public LavaElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a lava elemental";
            Body = 720;

            SetStr(452, 500);
            SetDex(167, 185);
            SetInt(367, 414);

            SetHits(273, 304);

            SetDamage(12, 18);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Poison, 90);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 85.0, 110.0);
            SetSkill(SkillName.Tactics, 85.0, 100.0);
            SetSkill(SkillName.Wrestling, 75.0, 90.0);
            SetSkill(SkillName.EvalInt, 80.0, 95.0);
            SetSkill(SkillName.Magery, 90.0, 95.0);
            SetSkill(SkillName.Meditation, 85.0, 110.0);

            Fame = 12500;
            Karma = -12500;

            AddItem(new Nightshade(4));
            AddItem(new SulfurousAsh(5));
            AddItem(new LesserPoisonPotion());
            PackScroll(5, 7);
            PackGem(2);
        }

        public LavaElemental(Serial serial)
            : base(serial)
        {
        }

        public override int GetAttackSound() { return 0x60A; }
        public override int GetDeathSound() { return 0x60B; }
        public override int GetHurtSound() { return 0x60C; }
        public override int GetIdleSound() { return 0x60D; }

        public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
        public override int LoyaltyPointsAward { get { return 20; } }
        public override int TreasureMapLevel { get { return 5; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 1);
            AddLoot(LootPack.Rich, 1);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            Region reg = Region.Find(c.GetWorldLocation(), c.Map);
            if (0.25> Utility.RandomDouble() && reg.Name == "Crimson Veins")
            {
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new EssencePrecision());
            }
           
            if (0.25 > Utility.RandomDouble() && reg.Name == "Fire Temple Ruins")
            {
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new EssenceOrder());
            }
            if (0.25 > Utility.RandomDouble() && reg.Name == "Lava Caldera")
            {
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new EssencePassion());
            }

            if (0.2 > Utility.RandomDouble())
            {
                c.DropItem(new LavaSerpentCrust());
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}