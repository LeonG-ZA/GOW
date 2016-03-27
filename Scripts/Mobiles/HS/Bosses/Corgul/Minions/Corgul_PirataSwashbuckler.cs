using System;
using Server.Items;

namespace Server.Mobiles
{
    public class Corgul_PirataSwashbuckler : BaseCreature
    {
        [Constructable]
        public Corgul_PirataSwashbuckler()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
			Name = "Soulbound Swashbuckler";
            Body = 400;

            SetStr(250);
            SetDex(280);
            SetInt(68);
			
			SetHits( 500 );
			SetStam( 250 );
			SetMana( 200 );

            SetDamage(20, 25);

            SetSkill(SkillName.MagicResist, 99.6);
			SetSkill(SkillName.Swords, 120.0);
			SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.Tactics, 77.1);
            SetSkill(SkillName.Wrestling, 45.7);

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 30);
			SetDamageType( ResistanceType.Poison, 80);
			SetDamageType( ResistanceType.Energy, 10);
			

			SetResistance( ResistanceType.Physical, 70 );
			SetResistance( ResistanceType.Fire, 60 );
			SetResistance( ResistanceType.Cold, 10 );
			SetResistance( ResistanceType.Energy, 20 );
			
            Fame = 2500;
            Karma = -1000;
			
			AddItem(new SkullCap(Hue = 837));
            AddItem(new LeatherChest());
			AddItem(new LeatherArms());
			AddItem(new LeatherGloves());
            AddItem(new ThighBoots());
			AddItem(new Katana());

        }

        public Corgul_PirataSwashbuckler(Serial serial)
            : base(serial)
        {
        }

        public override bool ClickTitle
        {
            get
            {
                return false;
            }
        }
        public override bool AlwaysMurderer
        {
            get
            {
                return true;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
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