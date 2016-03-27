using System;
using Server;
using Server.Items;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
    [CorpseName("a fairy dragon corpse")]
    public class FairyDragon : BaseCreature
    {
        [Constructable]
        public FairyDragon()
            : base(AIType.AI_Mystic, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a fairy dragon";
            Body = 718;

            SetStr(509, 600);
            SetDex(95, 125);
            SetInt(440, 570);

            SetHits(383, 425);

            SetDamage(15, 20);

            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Cold, 25);
            SetDamageType(ResistanceType.Poison, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 15, 30);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 65.2, 73.2);
            SetSkill(SkillName.MagicResist, 111.5, 128.0);
            SetSkill(SkillName.Tactics, 81.7, 92.2);
            SetSkill(SkillName.Wrestling, 80.9, 96.3);
            SetSkill(SkillName.Mysticism, 61.2, 76.8);

            Fame = 15000;
            Karma = -15000;

            PackGold(1000); // Add extra richness to these guys.
        }

        public override int GetAttackSound() { return 0x5E8; }
        public override int GetDeathSound() { return 0x5E9; }
        public override int GetHurtSound() { return 0x5EA; }
        public override int GetIdleSound() { return 0x5EB; }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            Region reg = Region.Find(c.GetWorldLocation(), c.Map);
            if (0.25 > Utility.RandomDouble() && reg.Name == "Fairy Dragon Lair")
            {
                switch (Utility.Random(2))
                {
                    case 0: c.DropItem(new EssenceDiligence()); break;
                    case 1: c.DropItem(new FaeryDust()); break;
                }
            }
            if (Utility.RandomDouble() <= 0.25)
            {
                switch (Utility.Random(2))
                {
                    case 0:
                        c.DropItem(new FeyWings());
                        break;
                    case 1:
                        c.DropItem(new FairyDragonWing());
                        break;

                }
            }
            if (0.1 > Utility.RandomDouble())
                c.DropItem(new DraconicOrb());
        }

        public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
        public override int LoyaltyPointsAward { get { return 2; } }

        public override HideType HideType { get { return HideType.Barbed; } }
        public override int Hides { get { return 7; } }
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override int Meat { get { return 1; } }
        public override int DragonBlood { get { return 4; } }

        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override int TreasureMapLevel { get { return 3; } }

        public FairyDragon(Serial serial)
            : base(serial)
        {
        }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			
			reader.ReadInt();
		}
	}
}