using System;
using Server.Items;

namespace Server.Mobiles
{
    public class Corgul_PirataRaider : BaseCreature
    {
        [Constructable]
        public Corgul_PirataRaider()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
			Name = "Soulbound Pirate Raider";
            Body = 400;

            SetStr(150);
            SetDex(140);
            SetInt(68);
			
			SetHits( 450 );
			SetStam( 250 );
			SetMana( 200 );

            SetDamage(18, 23);

            SetSkill(SkillName.MagicResist, 99.6);
			SetSkill(SkillName.Swords, 96.7);
			SetSkill(SkillName.Anatomy, 81.9);
            SetSkill(SkillName.Tactics, 77.1);
            SetSkill(SkillName.Wrestling, 45.7);

			SetDamageType( ResistanceType.Physical, 65 );
			SetDamageType( ResistanceType.Fire, 23 );
			SetDamageType( ResistanceType.Cold, 20);
			SetDamageType( ResistanceType.Poison, 20);
			SetDamageType( ResistanceType.Energy, 20);
			

			SetResistance( ResistanceType.Physical, 70 );
			SetResistance( ResistanceType.Fire, 54 );
			SetResistance( ResistanceType.Cold, 45 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 30 );
			
            Fame = 2000;
            Karma = -1000;
			
			AddItem(new TricorneHat());
            AddItem(new StuddedChest());
            AddItem(new ThighBoots());
			AddItem(new BoneHarvester());
			AddItem(new Cloak());

        }

        public Corgul_PirataRaider(Serial serial)
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