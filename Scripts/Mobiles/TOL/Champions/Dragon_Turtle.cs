using System;
using Server.Engines.ChampionSpawns;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a Dragon Turtle corpse")]
    public class Dragon_Turtle : BaseChampion
    {
        [Constructable]
        public Dragon_Turtle()
            : base(AIType.AI_Melee)
        {
            Name = "a Dragon Turtle";
            Body = 1288;

            SetStr(770, 772);
            SetDex(180, 199);
            SetInt(500, 510);

            SetHits(20000, 25000);

            SetDamage(25, 37);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 70, 78);
            SetResistance(ResistanceType.Fire, 60, 66);
            SetResistance(ResistanceType.Cold, 60, 65);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 70, 72);

            SetSkill(SkillName.MagicResist, 100.8, 125.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.3, 120.7);
            SetSkill(SkillName.Poisoning, 0.2, 5.8);
            SetSkill(SkillName.Anatomy, 0.4, 5.1);

            Fame = 18000;
            Karma = -18000;
        }

        public Dragon_Turtle(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType
        {
            get
            {
                return ChampionSkullType.Infuse;
            }
        }

        public override Type[] UniqueList
        {
            get
            {
                return new Type[] { typeof(Quell) };
            }
        }
        public override Type[] SharedList
        {
            get
            {
                return new Type[] { typeof(TheMostKnowledgePerson), typeof(OblivionsNeedle) };
            }
        }
        public override Type[] DecorativeList
        {
            get
            {
                return new Type[] { typeof(Pier), typeof(MonsterStatuette) };
            }
        }
        public override MonsterStatuetteType[] StatueTypes
        {
            get
            {
                return new MonsterStatuetteType[] { MonsterStatuetteType.DreadHorn };
            }
        }

        public override bool Unprovokable
        {
            get
            {
                return true;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Regular;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 5;
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

            if (this.BaseSoundID == 357)
                this.BaseSoundID = 0x451;
        }
    }
}