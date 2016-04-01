#region References
using System;
#endregion

namespace Server
{
	public enum Expansion
	{
		None,
		T2A,
		UOR,
		UOTD,
		LBR,
		AOS,
		SE,
		ML,
		SA,
		HS,
        TOL
	}

	[Flags]
	public enum ClientFlags
	{
		None = 0x00000000,
		Felucca = 0x00000001,
		Trammel = 0x00000002,
		Ilshenar = 0x00000004,
		Malas = 0x00000008,
		Tokuno = 0x00000010,
		TerMur = 0x00000020,
		Unk1 = 0x00000040,
		Unk2 = 0x00000080,
		UOTD = 0x00000100,
        Unk3 = 0x00000200
	}

    [Flags]
    public enum FeatureFlags
    {
        None = 0x00000000,
        T2A = 0x00000001,
        UOR = 0x00000002,
        UOTD = 0x00000004,
        LBR = 0x00000008,
        AOS = 0x00000010,
        SixthCharacterSlot = 0x00000020,
        SE = 0x00000040,
        ML = 0x00000080,
        EigthAge = 0x00000100,
        NinthAge = 0x00000200, /* Crystal/Shadow Custom House Tiles */
        TenthAge = 0x00000400,
        InceasedStorage = 0x00000800, /* Increased Housing/Bank Storage */
        SeventhCharacterSlot = 0x00001000,
        RoleplayFaces = 0x00002000,
        TrialAccount = 0x00004000,
        LiveAccount = 0x00008000,
        SA = 0x00010000,
        HS = 0x00020000,
        Gothic = 0x00040000,
        Rustic = 0x00080000,
        Jungle = 0x100000,
        Shadowguard = 0x200000,
        TOL = 0x400000,

        ExpansionNone = None,
        ExpansionT2A = T2A,
        ExpansionUOR = ExpansionT2A | UOR,
        ExpansionUOTD = ExpansionUOR | UOTD,
        ExpansionLBR = ExpansionUOTD | LBR,
        ExpansionAOS = ExpansionLBR | AOS | LiveAccount,
        ExpansionSE = ExpansionAOS | SE,
        ExpansionML = ExpansionSE | ML | NinthAge,
        ExpansionSA = ExpansionML | SA | Gothic | Rustic,
        ExpansionHS = ExpansionSA | HS,
        ExpansionTOL = ExpansionHS | TOL | Jungle | Shadowguard,
        ExpansionTOLEC = ExpansionTOL | RoleplayFaces
    }

    [Flags]
    public enum CharacterListFlags
    {
        None = 0x00000000,
        Unk1 = 0x00000001,
        OverwriteConfigButton = 0x00000002,
        OneCharacterSlot = 0x00000014,
        ContextMenus = 0x00000008,
        SlotLimit = 0x00000010,
        AOS = 0x00000020,
        SixthCharacterSlot = 0x00000040,
        SE = 0x00000080,
        ML = 0x00000100,
        KR = 0x00000200,
        UO3DClientType = 0x00000400,
        Unk2 = 0x00000600,//Old KR
        Unk3 = 0x00000800,//Unknown flag for UO:KR client
        SeventhCharacterSlot = 0x00001000,
        SA = 0x00002000,//Stygian Abyss// old Unk4
        NewMovementSystem = 0x00004000,
        NewFeluccaAreas = 0x00008000,

        ExpansionNone = ContextMenus, //
        ExpansionT2A = ContextMenus, //
        ExpansionUOR = ContextMenus, // None
        ExpansionUOTD = ContextMenus, //
        ExpansionLBR = ContextMenus, //
        ExpansionAOS = ContextMenus | AOS,
        ExpansionSE = ExpansionAOS | SE,
        ExpansionML = ExpansionSE | ML,
        ExpansionSA = ExpansionML,
        ExpansionHS = ExpansionSA,
        ExpansionTOL = ExpansionHS,
        ExpansionTOLEC = ExpansionTOL | KR
    }

    [Flags]
    public enum CustomHousingFlags
    {
        None = 0x0,
        AOS = 0x10,
        SE = 0x40,
        ML = 0x80,
        Crystal = 0x200,
        SA = 0x10000,
        HS = 0x20000,
        Gothic = 0x40000,
        Rustic = 0x80000,
        Jungle = 0x100000,
        Shadowguard = 0x200000,
        TOL = 0x400000,

        HousingAOS = AOS,
        HousingSE = HousingAOS | SE,
        HousingML = HousingSE | ML | Crystal,
        HousingSA = HousingML | SA | Gothic | Rustic,
        HousingHS = HousingSA | HS,
        HousingTOL = HousingHS | TOL | Jungle | Shadowguard,
        HousingTOLEC = HousingTOL
    }

    public class ExpansionInfo
	{
		public static ExpansionInfo[] Table { get { return m_Table; } }

		private static readonly ExpansionInfo[] m_Table = new[]
		{
			new ExpansionInfo(0, "None", ClientFlags.None, FeatureFlags.ExpansionNone, CharacterListFlags.ExpansionNone, CustomHousingFlags.None),
			new ExpansionInfo(1, "The Second Age", ClientFlags.Felucca, FeatureFlags.ExpansionT2A, CharacterListFlags.ExpansionT2A, CustomHousingFlags.None),
			new ExpansionInfo(2, "Renaissance", ClientFlags.Trammel, FeatureFlags.ExpansionUOR, CharacterListFlags.ExpansionUOR, CustomHousingFlags.None),
			new ExpansionInfo(3, "Third Dawn", ClientFlags.Ilshenar, FeatureFlags.ExpansionUOTD, CharacterListFlags.ExpansionUOTD, CustomHousingFlags.None),
			new ExpansionInfo(4, "Blackthorn's Revenge", ClientFlags.Ilshenar, FeatureFlags.ExpansionLBR, CharacterListFlags.ExpansionLBR, CustomHousingFlags.None),
			new ExpansionInfo(5, "Age of Shadows", ClientFlags.Malas, FeatureFlags.ExpansionAOS, CharacterListFlags.ExpansionAOS, CustomHousingFlags.HousingAOS),
			new ExpansionInfo(6, "Samurai Empire", ClientFlags.Tokuno, FeatureFlags.ExpansionSE, CharacterListFlags.ExpansionSE, CustomHousingFlags.HousingSE),
			new ExpansionInfo(7, "Mondain's Legacy", new ClientVersion("5.0.0a"), FeatureFlags.ExpansionML, CharacterListFlags.ExpansionML, CustomHousingFlags.HousingML), 
			new ExpansionInfo(8, "Stygian Abyss", ClientFlags.TerMur, FeatureFlags.ExpansionSA, CharacterListFlags.ExpansionSA, CustomHousingFlags.HousingSA),
			new ExpansionInfo(9, "High Seas", new ClientVersion("7.0.9.0"), FeatureFlags.ExpansionHS, CharacterListFlags.ExpansionHS, CustomHousingFlags.HousingHS),
            new ExpansionInfo(10, "Time Of Legends", new ClientVersion("7.0.45.65"), FeatureFlags.ExpansionTOL, CharacterListFlags.ExpansionTOL, CustomHousingFlags.HousingTOL),   
            new ExpansionInfo(11, "Time Of Legends EC", new ClientVersion("67.0.47.0"), FeatureFlags.ExpansionTOLEC, CharacterListFlags.ExpansionTOLEC, CustomHousingFlags.HousingTOLEC)
        };

		private readonly string m_Name;
		private readonly int m_ID;

        private readonly CustomHousingFlags m_CustomHousingFlag;
        private readonly ClientFlags m_ClientFlags;
		private readonly FeatureFlags m_SupportedFeatures;
		private readonly CharacterListFlags m_CharListFlags;

		private readonly ClientVersion m_RequiredClient; // Used as an alternative to the flags

		public string Name { get { return m_Name; } }
		public int ID { get { return m_ID; } }
		public ClientFlags ClientFlags { get { return m_ClientFlags; } }
		public FeatureFlags SupportedFeatures { get { return m_SupportedFeatures; } }
		public CharacterListFlags CharacterListFlags { get { return m_CharListFlags; } }
        public CustomHousingFlags CustomHousingFlags { get { return m_CustomHousingFlag; } }
		public ClientVersion RequiredClient { get { return m_RequiredClient; } }

        public ExpansionInfo(int id, string name, ClientFlags clientFlags, FeatureFlags supportedFeatures, CharacterListFlags charListFlags, CustomHousingFlags CustomHousingFlags)
        {
            m_Name = name;
            m_ID = id;
            m_ClientFlags = clientFlags;
            m_SupportedFeatures = supportedFeatures;
            m_CharListFlags = charListFlags;
            m_CustomHousingFlag = CustomHousingFlags;
        }

        public ExpansionInfo(int id, string name, ClientVersion requiredClient, FeatureFlags supportedFeatures, CharacterListFlags charListFlags, CustomHousingFlags CustomHousingFlags)
        {
            m_Name = name;
            m_ID = id;
            m_SupportedFeatures = supportedFeatures;
            m_CharListFlags = charListFlags;
            m_CustomHousingFlag = CustomHousingFlags;
            m_RequiredClient = requiredClient;
        }

		public static ExpansionInfo GetInfo(Expansion ex)
		{
			return GetInfo((int)ex);
		}

		public static ExpansionInfo GetInfo(int ex)
		{
			int v = ex;

			if (v < 0 || v >= m_Table.Length)
			{
				v = 0;
			}

			return m_Table[v];
		}

		public static ExpansionInfo CurrentExpansion { get { return GetInfo(Core.Expansion); } }

		public override string ToString()
		{
			return m_Name;
		}
	}
}