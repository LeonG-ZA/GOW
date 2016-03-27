using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

/* to do
Stun, At about 35% health it goes into a rage, at which point it has a good chance of dealing out hits with "stunning force" which leave the player paralyzed and unable to move, attack or cast for about 5 seconds.
*/


namespace Server.Mobiles
{
	 [CorpseName( "a molten elemental corpse" )]
	 public class MoltenEarthElemental : BaseCreature
	 {
		 
		  [Constructable]
		  public MoltenEarthElemental () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
			  {
			   Name = "a molten earth elemental";
			   Body = 14;
			   Hue = Utility.RandomOrangeHue();
			   BaseSoundID = 268;
			   SetStr( 460, 498 );
			   SetDex( 79, 95 );
			   SetInt( 92, 107 );
			   SetHits( 1276, 1351 );
			   SetDamage( 17, 19 );

			   SetDamageType( ResistanceType.Physical, 50 );
			   SetDamageType( ResistanceType.Fire, 50 );

			   SetResistance( ResistanceType.Physical, 50, 65 );
			   SetResistance( ResistanceType.Fire, 50, 60 );
			   SetResistance( ResistanceType.Cold, 40, 50 );
			   SetResistance( ResistanceType.Poison, 55, 65 );
			   SetResistance( ResistanceType.Energy, 50, 60 );

			   SetSkill( SkillName.MagicResist, 95.0, 100.0 );
			   SetSkill( SkillName.Tactics, 90.0, 100.0 );
			   SetSkill( SkillName.Wrestling, 110.0, 120.0 );

			   Fame = 2500;
			   Karma = -2500;

			   VirtualArmor = 34;

			   //ControlSlots = 2;
			   //PackItem( new FertileDirt( Utility.RandomMinMax( 1, 4 ) ) );
			   //PackItem( new IronOre( 3 ) ); // TODO: Five small iron ore
			   //PackItem( new MandrakeRoot() );
			   ////shamecrystal
			  }
			  
		  public override void GenerateLoot()
		  {
		   AddLoot( LootPack.Average );
		   AddLoot( LootPack.Meager );
		   AddLoot( LootPack.Gems );
		  }
		  
		public override bool HasBreath{ get{ return true; } } // fire breath
		  public override bool BleedImmune{ get{ return true; } }
		  public override int TreasureMapLevel{ get{ return 1; } }
		  
		  public MoltenEarthElemental ( Serial serial ) : base( serial )
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