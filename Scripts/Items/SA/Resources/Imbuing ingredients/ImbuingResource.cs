using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items.ImbuingResource
{
	public enum ImbuingResource
	{
		// Primary
		MagicalResidue = 1031697,
		EnchantedEssence = 1031698,
		RelicFragment = 1031699,

		// Secondary
		Emerald = 1023856,
		Diamond = 1023878,
		Amethyst = 1023862,
		Citrine = 1023861,
		Tourmaline = 1023864,
		Amber = 1023877,
		Ruby = 1023859,
		Sapphire = 1023857,
		StarSapphire = 1023855,

		// Full old
		LuminescentFungi = 1032689,
		FireRuby = 1032695,
		BlueDiamond = 1032696,
		Turquoise = 1032691,
		WhitePearl = 1032694,
		ParasiticPlant = 1032688,

		// Full new
		EssenceBalance = 1113324,
		EssenceAchievement,
		EssencePassion,
		EssencePrecision,
		EssenceDirection,
		SpiderCarapace,
		DaemonClaw,
		VialOfVitriol,
		FeyWings,
		VileTentacles,
		VoidCore,
		GoblinBlood,
		LavaSerpentCrust,
		UndyingFlesh,
		EssenceDiligence,
		EssenceFeeling,
		EssenceControl,
		EssenceSingularity,
		EssenceOrder,
		EssencePersistence,
		CrystallineBlackrock,
		SeedRenewal,
		ElvenFletching,
		CrystalShards,
		Lodestone,
		DelicateScales,
		AbyssalCloth,
		CrushedGlass,
		ArcanicRuneStone,
		PoweredIron,
		VoidOrb,
		BouraPelt,
		ChagaMushroom,
		SilverSnakeSkin,
		FaeryDust,
		SlithTongue,
		RaptorTeeth,
		BottleIchor,
		ReflectiveWolfEye,
	}

	public class Resources
	{
		public static void Initialize()
		{
			foreach ( ImbuingResource resource in Enum.GetValues( typeof( ImbuingResource ) ) )
			{
				Type type = ScriptCompiler.FindTypeByFullName( String.Format( "Server.Items.{0}", resource.ToString() ) );

				if ( type != null )
					m_ResourceTable.Add( resource, type );
			}
		}

		public static Dictionary<ImbuingResource, Type> m_ResourceTable = new Dictionary<ImbuingResource, Type>();

		public static Type GetType( ImbuingResource resource )
		{
			if ( m_ResourceTable.ContainsKey( resource ) )
				return m_ResourceTable[resource];
			else
				return null;
		}

		private static bool AddNeededResource( Dictionary<Type, int> types, ImbuingResource resource, int amount )
		{
			Type type = GetType( resource );

			if ( type != null )
			{
				types.Add( type, amount );
				return true;
			}
			else
				return false;
		}
	}
}