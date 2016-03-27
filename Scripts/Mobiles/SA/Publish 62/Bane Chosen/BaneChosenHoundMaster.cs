using System;
using Server;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a bane hound master corpse" )]
	public class BaneChosenHoundMaster : BaseCreature
	{
		[Constructable]
		public BaneChosenHoundMaster() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.4 )
		{
			Name = "a bane chosen hound master";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			SetStr( 176, 225 );
			SetDex( 81, 95 );
			SetInt( 61, 85 );
			
			SetHits( 376, 450 );

			SetDamage( 24, 26 );

			SetDamageType( ResistanceType.Physical, 25 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Cold, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 50 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.Fencing, 77.6, 92.5 );
			SetSkill( SkillName.Healing, 60.3, 90.0 );
			SetSkill( SkillName.Macing, 77.6, 92.5 );
			SetSkill( SkillName.Anatomy, 77.6, 87.5 );
			SetSkill( SkillName.MagicResist, 77.6, 97.5 );
			SetSkill( SkillName.Swords, 77.6, 92.5 );
			SetSkill( SkillName.Tactics, 77.6, 87.5 );

			Fame = 5000;
			Karma = -5000;

			CraftResource res = CraftResource.BlackScales;

			BaseWeapon melee = null;

			switch (Utility.Random( 3 ))
			{
				case 0: melee = new Kryss(); break;
				case 1: melee = new Broadsword(); break;
				case 2: melee = new Katana(); break;
			}

			melee.Movable = false;
			AddItem( melee );

			DragonHelm helm = new DragonHelm();
			helm.Resource = res;
			helm.Movable = false;
			AddItem( helm );

			DragonChest chest = new DragonChest();
			chest.Resource = res;
			chest.Movable = false;
			AddItem( chest );

			DragonArms arms = new DragonArms();
			arms.Resource = res;
			arms.Movable = false;
			AddItem( arms );

			DragonGloves gloves = new DragonGloves();
			gloves.Resource = res;
			gloves.Movable = false;
			AddItem( gloves );

			DragonLegs legs = new DragonLegs();
			legs.Resource = res;
			legs.Movable = false;
			AddItem( legs );

			ChaosShield shield = new ChaosShield();
			shield.Movable = false;
			AddItem( shield );

			AddItem( new Shirt() );
			AddItem( new Boots() );

		}

		public override int GetIdleSound()
		{
			return 0x2CE;
		}

		public override int GetDeathSound()
		{
			return 0x2CC;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override int GetAttackSound()
		{
			return 0x2C8;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.Gems );	
		}

		//public override bool HasBreath{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override bool BardImmune{ get{ return !Core.AOS; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }
		
		public override void OnDamagedBySpell( Mobile caster )
		{
			if ( caster != this && 0.05 > Utility.RandomDouble() )
			{
				BaseCreature wolf = new BaneHellHound( );

				wolf.Team = this.Team;
				wolf.MoveToWorld( this.Location, this.Map );
				wolf.Combatant = caster;

				Say( "*The bane chosen summons a beast!*"  ); 
			}

			base.OnDamagedBySpell( caster );
		}
		public override void OnGotMeleeAttack( Mobile attacker )
		{
			if ( attacker != this && 0.05 > Utility.RandomDouble() )
			{
				BaseCreature wolf = new BaneHellHound();

				wolf.Team = this.Team;
				wolf.MoveToWorld( this.Location, this.Map );
				wolf.Combatant = attacker;

				Say( "*The bane chosen summons a beast!*" ); 
			}

			base.OnGotMeleeAttack( attacker );
		}

			
		public override void AlterMeleeDamageTo( Mobile to, ref int damage )
		{
			if ( to is Dragon || to is WhiteWyrm || to is Drake || to is Nightmare || to is Hiryu || to is LesserHiryu || 
			to is Daemon || to is CuSidhe || to is Reptalon || to is ShadowDragon || to is RuneBeetle )
				damage *= 3;
		}
        /*
        public override bool IsEnemy(Mobile m) //override the is enemy, calling the mobile m being considered 
        {
            PlayerMobile pm = m as PlayerMobile;

            if (pm is PlayerMobile)
            {
                if (pm.BExp < 0)
                    return base.IsEnemy(m);
                else if (pm.BExp == 0)
                    return false; // false = not enemy
                else if (pm.BExp > 0)
                    return false; // false = not enemy
            }

            return base.IsEnemy(m);
        }
         */

		public BaneChosenHoundMaster( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
