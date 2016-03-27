using System;
using System.Collections.Generic;
using Server.Items;
using Server.Engines.Doom;

namespace Server.Mobiles
{
    [CorpseName("a demon knight corpse")]
    public class DemonKnight : BaseCreature
    {
        [Constructable]
        public DemonKnight()
            : base(AIType.AI_Necromancer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = NameList.RandomName("demon knight");
            this.Title = "the Dark Father";
            this.Body = 318;
            this.BaseSoundID = 0x165;

            this.SetStr(500);
            this.SetDex(100);
            this.SetInt(1000);

            this.SetHits(30000);
            this.SetMana(5000);
            SetStam(100);

            this.SetDamage(17, 21);

            this.SetDamageType(ResistanceType.Physical, 20);
            this.SetDamageType(ResistanceType.Fire, 20);
            this.SetDamageType(ResistanceType.Cold, 20);
            this.SetDamageType(ResistanceType.Poison, 20);
            this.SetDamageType(ResistanceType.Energy, 20);

            this.SetResistance(ResistanceType.Physical, 30);
            this.SetResistance(ResistanceType.Fire, 30);
            this.SetResistance(ResistanceType.Cold, 30);
            this.SetResistance(ResistanceType.Poison, 30);
            this.SetResistance(ResistanceType.Energy, 30);

            this.SetSkill(SkillName.Necromancy, 120, 120.0);
            this.SetSkill(SkillName.SpiritSpeak, 120.0, 120.0);

            this.SetSkill(SkillName.DetectHidden, 80.0);
            this.SetSkill(SkillName.EvalInt, 100.0);
            this.SetSkill(SkillName.Magery, 100.0);
            this.SetSkill(SkillName.Meditation, 120.0);
            this.SetSkill(SkillName.MagicResist, 150.0);
            this.SetSkill(SkillName.Tactics, 100.0);
            this.SetSkill(SkillName.Wrestling, 120.0);

            this.Fame = 28000;
            this.Karma = -28000;

            this.VirtualArmor = 64;
        }

        public DemonKnight(Serial serial)
            : base(serial)
        {
        }

        public override bool IgnoreYoungProtection
        {
            get
            {
                return Core.ML;
            }
        }
        public override bool BardImmune
        {
            get
            {
                return !Core.SE;
            }
        }
        public override bool Unprovokable
        {
            get
            {
                return Core.SE;
            }
        }
        public override bool AreaPeaceImmune
        {
            get
            {
                return Core.SE;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 1;
            }
        }

        public override WeaponAbility GetWeaponAbility()
        {
            switch ( Utility.Random(3) )
            {
                default:
                case 0: return WeaponAbility.DoubleStrike;
                case 1: return WeaponAbility.WhirlwindAttack;
                case 2: return WeaponAbility.CrushingBlow;
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (!Summoned && !NoKillAwards)
                DoomArtifactGiver.CheckArtifactGiving(this);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(20, 40));
        }

        private static bool m_InHere;

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (from != null && from != this && !m_InHere)
            {
                m_InHere = true;
                ItemAttributes.Damage(from, this, Utility.RandomMinMax(8, 20), 100, 0, 0, 0, 0);

                MovingEffect(from, 0xECA, 10, 0, false, false, 0, 0);
                PlaySound(0x491);

                if (0.05 > Utility.RandomDouble())
                {
                    Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(CreateBones_Callback), from);
                }

                if (ActiveSpeed >= 0.2 && Hits > 1500)
                {
                    ActiveSpeed = 0.01;
                    Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerCallback(RestoreSpeed));
                }

                m_InHere = false;
            }
        }

        public void RestoreSpeed()
        {
            ActiveSpeed = 0.2;
        }

        public virtual void CreateBones_Callback(object state)
        {
            Mobile from = (Mobile)state;
            Map map = from.Map;

            if (map == null)
            {
                return;
            }

            int count = Utility.RandomMinMax(1, 3);

            for (int i = 0; i < count; ++i)
            {
                int x = from.X + Utility.RandomMinMax(-1, 1);
                int y = from.Y + Utility.RandomMinMax(-1, 1);
                int z = from.Z;

                if (!map.CanFit(x, y, z, 16, false, true))
                {
                    z = map.GetAverageZ(x, y);

                    if (z == from.Z || !map.CanFit(x, y, z, 16, false, true))
                    {
                        continue;
                    }
                }

                UnholyBone bone = new UnholyBone();

                bone.Hue = 0;
                bone.Name = "unholy bones";
                bone.ItemID = Utility.Random(0xECA, 9);

                bone.MoveToWorld(new Point3D(x, y, z), map);
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