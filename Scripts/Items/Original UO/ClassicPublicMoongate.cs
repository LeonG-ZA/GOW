using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.MainConfiguration;
using Server.T2AConfiguration;

namespace Server.Items
{
	public class ClassicPublicMoongate : Item
    {
        #region ClassicPublicMoongate

        //public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		[Constructable]
		public ClassicPublicMoongate() : base( 0xF6C )
		{
			Movable = false;
			Light = LightType.Circle300;
		}

        public ClassicPublicMoongate(Serial serial)
            : base(serial)
		{
		}

        public override void OnAosSingleClick(Mobile from)
        {
            OnSingleClick(from);
        }

        public override void OnSingleClick(Mobile from)
        {
            ClassicPMEntry m_entry;
            ClassicPMList m_list;

            if (!from.Player)
                return;

            LabelTo(from, "moongate");

            if (Utility.InRange(from.Location, this.Location, 3))
            {
                GetGateEntry( this, out m_entry, out m_list );
                from.SendLocalizedMessage( m_entry.Description );
            }
            else
                from.SendLocalizedMessage( 500446 ); // That is too far away.
        }

        public override void OnDoubleClick(Mobile from)
		{
			if ( !from.Player )
				return;

			if ( from.InRange( GetWorldLocation(), 1 ) )
				UseGate( from );
			else
				from.SendLocalizedMessage( 500446 ); // That is too far away.
		}

		public override bool OnMoveOver( Mobile from )
		{
            if (!from.Player)
                return true;

			UseGate( from );
			return false;
		}

		public override bool HandlesOnMovement{ get{ return true; } }

		public override void OnMovement( Mobile from, Point3D oldLocation )
		{
			if ( from is PlayerMobile )
			{
				if ( !Utility.InRange( from.Location, this.Location, 1 ) && Utility.InRange( oldLocation, this.Location, 1 ) )
                    from.CloseGump(typeof(ClassicMoongateGump));
			}
		}

        public bool UseGate(Mobile from)
		{
            ClassicPMEntry m_entry;
            ClassicPMList m_list;

			if ( from.Criminal )
			{
				from.SendLocalizedMessage( 1005561, "", 0x22 ); // Thou'rt a criminal and cannot escape so easily.
				return false;
			}
			else if ( SpellHelper.CheckCombat( from ) )
			{
				from.SendLocalizedMessage( 1005564, "", 0x22 ); // Wouldst thou flee during the heat of battle??
				return false;
			}
			else if ( from.Spell != null )
			{
				from.SendLocalizedMessage( 1049616 ); // You are too busy to do that at the moment.
				return false;
			}
            if (Map == Map.Felucca || Map == Map.Trammel) // Old Style Moongates
            {
                GetGateEntry( this, out m_entry, out m_list );

                //{
                //    /* Debugging */
                //    Console.WriteLine("\nPlayer: {0}", from.Name);
                //    Console.WriteLine("  From: {0}, {1}, {2} on {3}", from.X, from.Y, from.Z, from.Map);
                //    Console.WriteLine("    To: {0}, {1}, {2} on {3}", m_entry.Location.X, m_entry.Location.Y, m_entry.Location.Z, m_list.Map);
                //}

                BaseCreature.TeleportPets(from, m_entry.Location, m_list.Map);

                from.Combatant = null;
                from.Warmode = false;
                from.Hidden = true;

                from.MoveToWorld(m_entry.Location, m_list.Map);
                Effects.PlaySound(m_entry.Location, m_list.Map, 0x1FE);
                
                //{
                //    /* Debugging */
                //    Console.WriteLine("Result: {0}, {1}, {2} on {3}", from.X, from.Y, from.Z, from.Map);
                //}
                return true;
            }
            else
            {
                from.CloseGump(typeof(ClassicMoongateGump));
                from.SendGump(new ClassicMoongateGump(from, this));

                if (!from.Hidden || from.AccessLevel == AccessLevel.Player)
                    Effects.PlaySound(from.Location, from.Map, 0x20E);

                return true;
            }
		}

        public static void GetGateEntry(ClassicPublicMoongate gate, out ClassicPMEntry entry, out ClassicPMList list) // For Old Style Moongates
        {
            int hours;
            int minutes;
            int cycle;
            int steps = 0;

            int gateCount;
            int gateNum;
            int destNum;

            if (gate.Map == Map.Felucca)
                list = ClassicPMList.Felucca;
            else
                list = ClassicPMList.Trammel;

            gateCount = list.Entries.Length;
            gateNum = 0;

            for (int i = 0; i < gateCount; ++i)
            {
                entry = list.Entries[i];
                if (gate.Location == entry.Location)
                {
                    gateNum = i;
                    break;
                }
            }

            Clock.GetTime(gate.Map, gate.X, gate.Y, out hours, out minutes);

            cycle = (60 * hours + minutes) % 120;
            if (cycle > 7) ++steps;
            if (cycle > 27) ++steps;
            if (cycle > 37) ++steps;
            if (cycle > 57) ++steps;
            if (cycle > 67) ++steps;
            if (cycle > 87) ++steps;
            if (cycle > 97) ++steps;
            if (cycle > 117) steps = 0;

            destNum = (gateNum + steps) % gateCount;
            entry = list.Entries[destNum];


            //{
            //    /* Debugging */            
            //    int generalNum;
            //    string exactTime;
            //    Clock.GetTime(gate.Map, gate.X, gate.Y, out generalNum, out exactTime);
            //    Console.WriteLine("\ngateNum: {0}", gateNum);
            //    Console.WriteLine("steps: {0}", steps);
            //    Console.WriteLine("destNum: {0}", destNum);
            //    Console.WriteLine("destXYZ: {0}, {1}, {2}", entry.Location.X, entry.Location.Y, entry.Location.Z);
            //    Console.WriteLine("Time: " + exactTime);
            //}
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
        #endregion

        #region MoonGen
		public static void Initialize()
		{
            CommandSystem.Register("ClassicMoonGen", AccessLevel.Administrator, new CommandEventHandler(ClassicMoonGen_OnCommand));
		}
        [Usage("ClassicMoonGen")]
        [Description("Generates Classic public moongates. Removes all old moongates.")]
        public static void ClassicMoonGen_OnCommand(CommandEventArgs e)
		{
			DeleteAll();

			int count = 0;

            if (T2AConfig.T2AClassicMoonGenEnabled)
            {
                count += ClassicMoonGen(ClassicPMList.Felucca);
            }
            else
            {
                count += ClassicMoonGen(ClassicPMList.Trammel);
                count += ClassicMoonGen(ClassicPMList.Felucca);
                count += ClassicMoonGen(ClassicPMList.Ilshenar);
                count += ClassicMoonGen(ClassicPMList.Malas);
                count += ClassicMoonGen(ClassicPMList.Tokuno);
            }

			World.Broadcast( 0x35, true, "{0} moongates generated.", count );
		}

		private static void DeleteAll()
		{
			List<Item> list = new List<Item>();

			foreach ( Item item in World.Items.Values )
			{
                if (item is ClassicPublicMoongate)
					list.Add( item );
			}

			foreach ( Item item in list )
				item.Delete();

			if ( list.Count > 0 )
				World.Broadcast( 0x35, true, "{0} moongates removed.", list.Count );
		}

        private static int ClassicMoonGen(ClassicPMList list)
		{
			foreach ( ClassicPMEntry entry in list.Entries )
			{
                Item item = new ClassicPublicMoongate();

				item.MoveToWorld( entry.Location, list.Map );

				if ( entry.Number == 1060642 ) // Umbra
					item.Hue = 0x497;
			}

			return list.Entries.Length;
		}
        #endregion
    }

    #region ClassicPMEntry
    public class ClassicPMEntry
	{
		private Point3D m_Location;
		private int m_Number;
        private int m_DescNumber; // Added to support Old Style Moongates

		public Point3D Location
		{
			get
			{
				return m_Location;
			}
		}

        public int Number
        {
            get
            {
                return m_Number;
            }
        }

        public int Description
        {
            get
            {
                return m_DescNumber;
            }
        }

        public ClassicPMEntry(Point3D loc, int number):this(loc, number, 1005397) //The moongate is cloudy, and nothing can be made out. 
        {
        }

        public ClassicPMEntry(Point3D loc, int number, int description)
        {
            m_Location = loc;
            m_Number = number;
            m_DescNumber = description;
        }
    }
    #endregion

    #region ClassicPMList
    public class ClassicPMList
	{
		private int m_Number, m_SelNumber;
		private Map m_Map;
		private ClassicPMEntry[] m_Entries;

		public int Number
		{
			get
			{
				return m_Number;
			}
		}

		public int SelNumber
		{
			get
			{
				return m_SelNumber;
			}
		}

		public Map Map
		{
			get
			{
				return m_Map;
			}
		}

		public ClassicPMEntry[] Entries
		{
			get
			{
				return m_Entries;
			}
		}

		public ClassicPMList( int number, int selNumber, Map map, ClassicPMEntry[] entries )
		{
			m_Number = number;
			m_SelNumber = selNumber;
			m_Map = map;
			m_Entries = entries;
		}

        // **** Order changed to support old style Moongates **** //
        public static readonly ClassicPMList Trammel =
			new ClassicPMList( 1012000, 1012012, Map.Trammel, new ClassicPMEntry[]
				{
					new ClassicPMEntry( new Point3D( 1336, 1997, 5 ), 1012004, 1005390 ), // Britain
					new ClassicPMEntry( new Point3D( 4467, 1283, 5 ), 1012003, 1005389 ), // Moonglow
					new ClassicPMEntry( new Point3D( 3563, 2139, 34), 1012010, 1005396 ), // Magincia
					new ClassicPMEntry( new Point3D(  643, 2067, 5 ), 1012009, 1005395 ), // Skara Brae
					new ClassicPMEntry( new Point3D( 1828, 2948,-20), 1012008, 1005394 ), // Trinsic
					new ClassicPMEntry( new Point3D( 2701,  692, 5 ), 1012007, 1005393 ), // Minoc
					new ClassicPMEntry( new Point3D(  771,  752, 5 ), 1012006, 1005392 ), // Yew
					new ClassicPMEntry( new Point3D( 1499, 3771, 5 ), 1012005, 1005391 ), // Jhelom
                    // comment out New Haven entry for OSI correct Old Style Moongates
					new ClassicPMEntry( new Point3D( 3450, 2677, 25), 1078098 )  // New Haven
				} );

        // **** Order changed to support old style Moongates **** //
        public static readonly ClassicPMList Felucca =
			new ClassicPMList( 1012001, 1012013, Map.Felucca, new ClassicPMEntry[]
				{
					new ClassicPMEntry( new Point3D( 1336, 1997, 5 ), 1012004, 1005390 ), // Britain
					new ClassicPMEntry( new Point3D( 4467, 1283, 5 ), 1012003, 1005389 ), // Moonglow
					new ClassicPMEntry( new Point3D( 3563, 2139, 34), 1012010, 1005396 ), // Magincia
					new ClassicPMEntry( new Point3D(  643, 2067, 5 ), 1012009, 1005395 ), // Skara Brae
					new ClassicPMEntry( new Point3D( 1828, 2948,-20), 1012008, 1005394 ), // Trinsic
					new ClassicPMEntry( new Point3D( 2701,  692, 5 ), 1012007, 1005393 ), // Minoc
					new ClassicPMEntry( new Point3D(  771,  752, 5 ), 1012006, 1005392 ), // Yew
					new ClassicPMEntry( new Point3D( 1499, 3771, 5 ), 1012005, 1005391 ), // Jhelom
                    // comment out Buccaneer's Den entry for OSI correct Old Style Moongates
					new ClassicPMEntry( new Point3D( 2711, 2234, 0 ), 1019001 )  // Buccaneer's Den
				} );

		public static readonly ClassicPMList Ilshenar =
			new ClassicPMList( 1012002, 1012014, Map.Ilshenar, new ClassicPMEntry[]
				{
					new ClassicPMEntry( new Point3D( 1215,  467, -13 ), 1012015 ), // Compassion
					new ClassicPMEntry( new Point3D(  722, 1366, -60 ), 1012016 ), // Honesty
					new ClassicPMEntry( new Point3D(  744,  724, -28 ), 1012017 ), // Honor
					new ClassicPMEntry( new Point3D(  281, 1016,   0 ), 1012018 ), // Humility
					new ClassicPMEntry( new Point3D(  987, 1011, -32 ), 1012019 ), // Justice
					new ClassicPMEntry( new Point3D( 1174, 1286, -30 ), 1012020 ), // Sacrifice
					new ClassicPMEntry( new Point3D( 1532, 1340, - 3 ), 1012021 ), // Spirituality
					new ClassicPMEntry( new Point3D(  528,  216, -45 ), 1012022 ), // Valor
					new ClassicPMEntry( new Point3D( 1721,  218,  96 ), 1019000 )  // Chaos
				} );

		public static readonly ClassicPMList Malas =
			new ClassicPMList( 1060643, 1062039, Map.Malas, new ClassicPMEntry[]
				{
					new ClassicPMEntry( new Point3D( 1015,  527, -65 ), 1060641 ), // Luna
					new ClassicPMEntry( new Point3D( 1997, 1386, -85 ), 1060642 )  // Umbra
				} );

		public static readonly ClassicPMList Tokuno =
			new ClassicPMList( 1063258, 1063415, Map.Tokuno, new ClassicPMEntry[]
				{
					new ClassicPMEntry( new Point3D( 1169,  998, 41 ), 1063412 ), // Isamu-Jima
					new ClassicPMEntry( new Point3D(  802, 1204, 25 ), 1063413 ), // Makoto-Jima
					new ClassicPMEntry( new Point3D(  270,  628, 15 ), 1063414 )  // Homare-Jima
				} );

		public static readonly ClassicPMList[] UORLists		= new ClassicPMList[] { Trammel, Felucca };
		public static readonly ClassicPMList[] UORListsYoung	= new ClassicPMList[] { Trammel };
		public static readonly ClassicPMList[] LBRLists		= new ClassicPMList[] { Trammel, Felucca, Ilshenar };
		public static readonly ClassicPMList[] LBRListsYoung	= new ClassicPMList[] { Trammel, Ilshenar };
		public static readonly ClassicPMList[] AOSLists		= new ClassicPMList[] { Trammel, Felucca, Ilshenar, Malas };
		public static readonly ClassicPMList[] AOSListsYoung	= new ClassicPMList[] { Trammel, Ilshenar, Malas };
		public static readonly ClassicPMList[] SELists			= new ClassicPMList[] { Trammel, Felucca, Ilshenar, Malas, Tokuno };
		public static readonly ClassicPMList[] SEListsYoung	= new ClassicPMList[] { Trammel, Ilshenar, Malas, Tokuno };
		public static readonly ClassicPMList[] RedLists		= new ClassicPMList[] { Felucca };
		public static readonly ClassicPMList[] SigilLists		= new ClassicPMList[] { Felucca };
        public static readonly ClassicPMList[] FeluccaOnlyList = new ClassicPMList[] { Felucca };
	}
    #endregion

    #region ClassicMoongateGump
    public class ClassicMoongateGump : Gump
	{
		private Mobile m_Mobile;
		private Item m_Moongate;
		private ClassicPMList[] m_Lists;

        public ClassicMoongateGump(Mobile mobile, Item moongate)
            : base(100, 100)
		{
			m_Mobile = mobile;
			m_Moongate = moongate;

			ClassicPMList[] checkLists;

			if ( mobile.Player )
			{
				if ( Factions.Sigil.ExistsOn( mobile ) )
				{
					checkLists = ClassicPMList.SigilLists;
				}
				else if ( mobile.Kills >= 5 )
				{
					checkLists = ClassicPMList.RedLists;
				}
				else
				{
                    ClientFlags flags = mobile.NetState == null ? ClientFlags.None : mobile.NetState.Flags;
                    bool young = mobile is PlayerMobile ? ((PlayerMobile)mobile).Young : false;

                    if (Core.SE && (flags & ClientFlags.Tokuno) != 0)
                    {
                        if (T2AConfig.T2AFeluccaOnlyClassicEnabled)
                        {
                            checkLists = ClassicPMList.FeluccaOnlyList;
                        }
                        else
                        {
                            checkLists = young ? ClassicPMList.SEListsYoung : ClassicPMList.SELists;
                        }
                    }
                    else if (Core.AOS && (flags & ClientFlags.Malas) != 0)
                    {
                        if (T2AConfig.T2AFeluccaOnlyClassicEnabled)
                        {
                            checkLists = ClassicPMList.FeluccaOnlyList;
                        }
                        else
                        {
                            checkLists = young ? ClassicPMList.AOSListsYoung : ClassicPMList.AOSLists;
                        }
                    }
                    else if ((flags & ClientFlags.Ilshenar) != 0)
                    {
                        if (T2AConfig.T2AFeluccaOnlyClassicEnabled)
                        {
                            checkLists = ClassicPMList.FeluccaOnlyList;
                        }
                        else
                        {
                            checkLists = young ? ClassicPMList.LBRListsYoung : ClassicPMList.LBRLists;
                        }
                    }
                    else
                    {
                        if (T2AConfig.T2AFeluccaOnlyClassicEnabled)
                        {
                            checkLists = ClassicPMList.FeluccaOnlyList;
                        }
                        else
                        {
                            checkLists = young ? ClassicPMList.UORListsYoung : ClassicPMList.UORLists;
                        }
                    }
				}
			}
			else
			{
				checkLists = ClassicPMList.SELists;
			}

			m_Lists = new ClassicPMList[checkLists.Length];

			for ( int i = 0; i < m_Lists.Length; ++i )
				m_Lists[i] = checkLists[i];

			for ( int i = 0; i < m_Lists.Length; ++i )
			{
				if ( m_Lists[i].Map == mobile.Map )
				{
					ClassicPMList temp = m_Lists[i];

					m_Lists[i] = m_Lists[0];
					m_Lists[0] = temp;

					break;
				}
			}

			AddPage( 0 );

			AddBackground( 0, 0, 380, 280, 5054 );

			AddButton( 10, 210, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 45, 210, 140, 25, 1011036, false, false ); // OKAY

			AddButton( 10, 235, 4005, 4007, 0, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 45, 235, 140, 25, 1011012, false, false ); // CANCEL

			AddHtmlLocalized( 5, 5, 200, 20, 1012011, false, false ); // Pick your destination:

			for ( int i = 0; i < checkLists.Length; ++i )
			{
				AddButton( 10, 35 + (i * 25), 2117, 2118, 0, GumpButtonType.Page, Array.IndexOf( m_Lists, checkLists[i] ) + 1 );
				AddHtmlLocalized( 30, 35 + (i * 25), 150, 20, checkLists[i].Number, false, false );
			}

			for ( int i = 0; i < m_Lists.Length; ++i )
				RenderPage( i, Array.IndexOf( checkLists, m_Lists[i] ) );
		}

		private void RenderPage( int index, int offset )
		{
			ClassicPMList list = m_Lists[index];

			AddPage( index + 1 );

			AddButton( 10, 35 + (offset * 25), 2117, 2118, 0, GumpButtonType.Page, index + 1 );
			AddHtmlLocalized( 30, 35 + (offset * 25), 150, 20, list.SelNumber, false, false );

			ClassicPMEntry[] entries = list.Entries;

			for ( int i = 0; i < entries.Length; ++i )
			{
				AddRadio( 200, 35 + (i * 25), 210, 211, false, (index * 100) + i );
				AddHtmlLocalized( 225, 35 + (i * 25), 150, 20, entries[i].Number, false, false );
			}
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if ( info.ButtonID == 0 ) // Cancel
				return;
			else if ( m_Mobile.Deleted || m_Moongate.Deleted || m_Mobile.Map == null )
				return;

			int[] switches = info.Switches;

			if ( switches.Length == 0 )
				return;

			int switchID = switches[0];
			int listIndex = switchID / 100;
			int listEntry = switchID % 100;

			if ( listIndex < 0 || listIndex >= m_Lists.Length )
				return;

			ClassicPMList list = m_Lists[listIndex];

			if ( listEntry < 0 || listEntry >= list.Entries.Length )
				return;

			ClassicPMEntry entry = list.Entries[listEntry];

			if ( !m_Mobile.InRange( m_Moongate.GetWorldLocation(), 1 ) || m_Mobile.Map != m_Moongate.Map )
			{
				m_Mobile.SendLocalizedMessage( 1019002 ); // You are too far away to use the gate.
			}
			else if ( m_Mobile.Player && m_Mobile.Kills >= 5 && list.Map != Map.Felucca )
			{
				m_Mobile.SendLocalizedMessage( 1019004 ); // You are not allowed to travel there.
			}
			else if ( Factions.Sigil.ExistsOn( m_Mobile ) && list.Map != Factions.Faction.Facet )
			{
				m_Mobile.SendLocalizedMessage( 1019004 ); // You are not allowed to travel there.
			}
			else if ( m_Mobile.Criminal )
			{
				m_Mobile.SendLocalizedMessage( 1005561, "", 0x22 ); // Thou'rt a criminal and cannot escape so easily.
			}
			else if ( SpellHelper.CheckCombat( m_Mobile ) )
			{
				m_Mobile.SendLocalizedMessage( 1005564, "", 0x22 ); // Wouldst thou flee during the heat of battle??
			}
			else if ( m_Mobile.Spell != null )
			{
				m_Mobile.SendLocalizedMessage( 1049616 ); // You are too busy to do that at the moment.
			}
			else if ( m_Mobile.Map == list.Map && m_Mobile.InRange( entry.Location, 1 ) )
			{
				m_Mobile.SendLocalizedMessage( 1019003 ); // You are already there.
			}
			else
			{
				BaseCreature.TeleportPets( m_Mobile, entry.Location, list.Map );

				m_Mobile.Combatant = null;
				m_Mobile.Warmode = false;
				m_Mobile.Hidden = true;

				m_Mobile.MoveToWorld( entry.Location, list.Map );

				Effects.PlaySound( entry.Location, list.Map, 0x1FE );
			}
		}
    }
    #endregion
}