using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an fire daemon corpse")]
    public class FireDaemon : AuraCreature
    {
        [Constructable]
        public FireDaemon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "an fire daemon";
            this.Body = 0x310;
            this.BaseSoundID = 0x47D;

            this.SetStr(549, 1199);
            this.SetDex(136, 206);
            this.SetInt(202, 336);

            this.SetHits(1111, 1113);

            this.SetDamage(7, 14);

            AuraType = ResistanceType.Fire;
            MinAuraDelay = 5;
            MaxAuraDelay = 10;
            MinAuraDamage = 7;
            MaxAuraDamage = 14;
            AuraRange = 2;

            this.SetDamageType(ResistanceType.Physical, 20);
            this.SetDamageType(ResistanceType.Fire, 80);

            this.SetResistance(ResistanceType.Physical, 55, 60);
            this.SetResistance(ResistanceType.Fire, 100, 100);
            this.SetResistance(ResistanceType.Cold, -8, 57);
            this.SetResistance(ResistanceType.Poison, 20, 22);
            this.SetResistance(ResistanceType.Energy, 30, 36);

            this.SetSkill(SkillName.MagicResist, 98.1, 132.6);
            this.SetSkill(SkillName.Tactics, 86.9, 95.5);
            this.SetSkill(SkillName.Wrestling, 42.2, 98.8);
            this.SetSkill(SkillName.Magery, 97.1, 100.8);
            this.SetSkill(SkillName.EvalInt, 91.1, 91.8);
            this.SetSkill(SkillName.Meditation, 45.4, 94.1);

            this.Fame = 15000;
            this.Karma = -15000;

            this.VirtualArmor = 55;
        }

        public FireDaemon(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Deadly;
            }
        }
        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.ConcussionBlow;
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Average, 2);
        }
        public override void OnDeath(Container c)
        {

            base.OnDeath(c);
            Region reg = Region.Find(c.GetWorldLocation(), c.Map);
            if (1.0 > Utility.RandomDouble() && reg.Name == "Crimson Veins")
            {
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new EssencePrecision());                
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new DaemonClaw());
            }
            
            if (1.0 > Utility.RandomDouble() && reg.Name == "Fire Temple Ruins")
            {
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new EssenceOrder());
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new DaemonClaw());
            }
            if (1.0 > Utility.RandomDouble() && reg.Name == "Lava Caldera")
            {
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new EssencePassion());
                if (Utility.RandomDouble() < 0.6)
                    c.DropItem(new DaemonClaw());
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