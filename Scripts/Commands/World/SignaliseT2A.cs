using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Items;

namespace Server.Commands
{
    public class SignPutT2A
	{
		private static int m_AddCount;
		private static int m_DelCount;

		private class SignEntryT2A
		{
			public string m_Text;
			public Point3D m_Location;
			public int m_ItemID;
			public int m_Map;

			public SignEntryT2A( string text, Point3D pt, int itemID, int mapLoc )
			{
				m_Text = text;
				m_Location = pt;
				m_ItemID = itemID;
				m_Map = mapLoc;
			}
		}

		public static void Initialize()
		{
            CommandSystem.Register("SignPutT2A", AccessLevel.Administrator, new CommandEventHandler(SignPutT2A_OnCommand));
		}

        [Usage("SignPutT2A")]
		[Description( "Places shop signs as specified in a config file." )]
        public static void SignPutT2A_OnCommand(CommandEventArgs c)
		{
			//[SignPut
			if ( c.ArgString.Length == 0 )
			{
				Parse( c.Mobile, "", false ); // Data/signs.cfg
			}
			//[SignPut SE (or ML, KR1, KR2, SA, HS1, HS2)
			else if ( Lib.IsValidExpansion( c.Arguments[0] ) == true  )
			{
				Parse( c.Mobile, c.Arguments[0], false );
			}
			//wrong use
			else
			{
				c.Mobile.SendMessage( "Usage: 'SignPut' or 'SignPut SE' (or ML, KR1, KR2, SA, HS1, HS2)" );
			}
		}
		
		public static void Parse( Mobile from, string filename, bool AddOrDel )
		{
			string ThisFile;
			
			if ( filename == null || filename == "" )
			{
				ThisFile = "Signs.cfg";
			}
			else
			{
				ThisFile = "Signs" + filename.ToUpper() + ".cfg";
			}

            List<string> line = new List<string>(Lib.ListOfLines("Data/World/Signs", ThisFile));

			if ( line.Count > 1 ) // File Exists
			{
				List<SignEntryT2A> list = new List<SignEntryT2A>();

				if ( AddOrDel == true )
				{
					from.SendMessage( "Removing signs, please wait." );
				}
				else
				{
					from.SendMessage( "Generating signs, please wait." );
				}

				for ( int i = 0; i < line.Count; ++i )
				{
					string lineA = Convert.ToString( line[i] );
					
					if ( !lineA.StartsWith("#") && Lib.IsNumber( Convert.ToString( lineA[0] ) ) && lineA != null && lineA != "" && lineA != " " ) // If not comment or blank Line
					{
						string[] split = lineA.Split( ' ' );

                        SignEntryT2A e = new SignEntryT2A(lineA.Substring(split[0].Length + 1 + split[1].Length + 1 + split[2].Length + 1 + split[3].Length + 1 + split[4].Length + 1), new Point3D(Utility.ToInt32(split[2]), Utility.ToInt32(split[3]), Utility.ToInt32(split[4])), Utility.ToInt32(split[1]), Utility.ToInt32(split[0]));

						list.Add( e );
					}
				}

				Map[] brit = new Map[]{ Map.Felucca };
				Map[] fel = new Map[]{ Map.Felucca };
				
				for ( int i = 0; i < list.Count; ++i )
				{
                    SignEntryT2A e = list[i];
					Map[] maps = null;

					switch ( e.m_Map )
					{
						case 0: maps = brit; break; // Felucca
						case 1: maps = fel; break;  // Felucca
					}

					for ( int j = 0; maps != null && j < maps.Length; ++j )
					{
						if ( AddOrDel == true )
						{
							Del_Static( e.m_ItemID, e.m_Location, maps[j] );
						}
						else
						{
							Add_Static( e.m_ItemID, e.m_Location, maps[j], e.m_Text );
							m_AddCount++;
						}
					}
				}

				if ( AddOrDel == true )
				{
					from.SendMessage( "{0} signs removed.", m_DelCount );
				}
				else
				{
					from.SendMessage( "{0} signs created.", m_AddCount  );
				}

				m_AddCount = 0;
				m_DelCount = 0;
			}
			else
			{
				from.SendMessage( "{0} not found!", line[0] ); // line[0] = path + file name
			}
		}

		private static Queue<Item> m_ToDelete = new Queue<Item>();

		public static void Add_Static( int itemID, Point3D location, Map map, string name )
		{
			Del_Static( itemID, location, map );

			Item sign;

			if ( name.StartsWith( "#" ) )
			{
				sign = new LocalizedSign( itemID, Utility.ToInt32( name.Substring( 1 ) ) );
			}
			else
			{
				sign = new Sign( itemID );
				sign.Name = name;
			}

			if ( map == Map.Malas )
			{
				if ( location.X >= 965 && location.Y >= 502 && location.X <= 1012 && location.Y <= 537 )
					sign.Hue = 0x47E;
				else if ( location.X >= 1960 && location.Y >= 1278 && location.X < 2106 && location.Y < 1413 )
					sign.Hue = 0x44E;
			}

			sign.MoveToWorld( location, map );
		}

		public static void Del_Static( int itemID, Point3D location, Map map )
		{
			IPooledEnumerable eable = map.GetItemsInRange( location, 0 );
			
			foreach ( Item item in eable )
			{
				if ( item is Sign && item.Z == location.Z && item.ItemID == itemID )
				{
					m_ToDelete.Enqueue( item );
					m_DelCount++;
				}
			}

			eable.Free();

			while ( m_ToDelete.Count > 0 )
				m_ToDelete.Dequeue().Delete();
		}
	}
	
	public class SignDelT2A
	{
        public SignDelT2A()
		{
		}

		public static void Initialize() 
		{
            CommandSystem.Register("SignDelT2A", AccessLevel.Administrator, new CommandEventHandler(SignDelT2A_OnCommand)); 
		}

        [Usage("[SignDelT2A")]
		[Description( "Removes shop signs as specified in a config file." )]
        private static void SignDelT2A_OnCommand(CommandEventArgs c)
		{
            //[SignDelT2A
			if ( c.ArgString.Length == 0 )
			{
				SignPut.Parse( c.Mobile, "", true ); // Data/signs.cfg
			}
            //[SignDelT2A SE (or ML, KR1, KR2, SA, HS1, HS2)
			else if ( Lib.IsValidExpansion( c.Arguments[0] ) == true  )
			{
				SignPut.Parse( c.Mobile, c.Arguments[0], true );
			}
			//wrong use
			else
			{
				c.Mobile.SendMessage( "Usage: 'SignDel' or 'SignDel SE' (or ML, KR1, KR2, SA, HS1, HS2)" );
			}
		}
	}
}