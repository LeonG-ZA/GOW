using System;
using Server.Items;
using Server.Items.MusicBox;
using Server.Network;

namespace Server.Mobiles
{
	 [CorpseName( "a clay golem corpse" )]
	 public class ClayGolem : BaseCreature
	 {
		  //private bool m_Stunning;

		  public override bool IsScaredOfScaryThings{ get{ return false; } }
		  
		  
		  [Constructable]
		  public ClayGolem( ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8 )
			  {
				   Name = "a clay golem";
				   Body = 752;
					Hue= 2207;
					
					
				   SetStr( 390, 600 );
				   SetDex( 100, 135 );
				   SetInt( 110, 150 );

				   SetHits( 750, 850 );

				   SetDamage( 13, 24 );

				   SetDamageType( ResistanceType.Physical, 100 );

				   SetResistance( ResistanceType.Physical, 45, 55 );

				   SetResistance( ResistanceType.Fire, 50, 60 );

				   SetResistance( ResistanceType.Cold, 45, 55 );
				   SetResistance( ResistanceType.Poison, 99 );
				   SetResistance( ResistanceType.Energy, 35, 45 );

				   SetSkill( SkillName.MagicResist, 160.0, 190.0 );
				   SetSkill( SkillName.Tactics, 85.0, 120.0 );
				   SetSkill( SkillName.Wrestling, 85.0, 120.0 );

				   
				   Fame = 4000;
				   Karma = -4000;

				   VirtualArmor = 40;
					
				}


		  public override void GenerateLoot()
				{
				   AddLoot( LootPack.Average );
				   //shame crystal
			   }
			  

		  public override int GetAngerSound()
			  {
			   return 541;
			  }

		  public override int GetIdleSound()
			  {
			  
				return 542;

			  }

		  public override int GetDeathSound()
			  {
			  
				return 545;

			  
			  }

		  public override int GetAttackSound()
			  {
			   return 562;
			  }

		  public override int GetHurtSound()
			  {
			   
				return 320;

			  }

		  
		  public override bool BleedImmune{ get{ return true; } }

		  
		  //public override bool BardImmune{ get{ return !Core.AOS || Controlled; } }
		  public override Poison PoisonImmune{ get{ return Poison.Greater; } }

		  public ClayGolem( Serial serial ) : base( serial )
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