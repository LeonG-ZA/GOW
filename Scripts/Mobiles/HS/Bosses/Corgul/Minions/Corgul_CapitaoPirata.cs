using System;
using Server.Items;

namespace Server.Mobiles
{
    public class Corgul_CapitaoPirata : BaseCreature
    {
        [Constructable]
        public Corgul_CapitaoPirata()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
			Name = "Soulbound Pirate Captain";
            Body = 400;

            SetStr(100);
            SetDex(125);
            SetInt(68);
			
			SetHits( 360 );
			SetStam( 250 );
			SetMana( 200 );

            SetDamage(15, 20);

            SetSkill(SkillName.MagicResist, 83.6);
			SetSkill(SkillName.Fencing, 91.7);
			SetSkill(SkillName.EvalInt, 81.9);
            SetSkill(SkillName.Tactics, 77.1);
            SetSkill(SkillName.Wrestling, 45.7);

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20);
			SetDamageType( ResistanceType.Poison, 20);
			SetDamageType( ResistanceType.Energy, 20);
			

			SetResistance( ResistanceType.Physical, 55 );
			SetResistance( ResistanceType.Fire, 54 );
			SetResistance( ResistanceType.Cold, 51 );
			SetResistance( ResistanceType.Poison, 50 );
			SetResistance( ResistanceType.Energy, 52 );
			
            Fame = 1025;
            Karma = -1000;
			
			AddItem(new TricorneHat());
            AddItem(new StuddedChest());
            AddItem(new ThighBoots());
			AddItem(new Pitchfork());

        }

        public Corgul_CapitaoPirata(Serial serial)
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