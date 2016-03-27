using System;
using System.IO;
using System.Text;
using System.Collections;
using Server;
using Server.Network;
using Server.Guilds;
using Server.Mobiles;
using Server.WebConfiguration;

namespace Server.Misc
{
	public class CharStatusPage : Timer
	{
		public static void Initialize()
		{
            if (WebConfig.WebServerStatusEnabled)
            {
                new CharStatusPage().Start();
            }
		}

		public CharStatusPage() : base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 2.0 ) )
		{
			Priority = TimerPriority.FiveSeconds;
		}

		private static string Encode( string input )
		{
			StringBuilder sb = new StringBuilder( input );

			sb.Replace( "&", "&amp;" );
			sb.Replace( "<", "&lt;" );
			sb.Replace( ">", "&gt;" );
			sb.Replace( "\"", "&quot;" );
			sb.Replace( "'", "&apos;" );

			return sb.ToString();
		}

		private string Pagename;
		protected override void OnTick()
		{
			foreach ( NetState state in NetState.Instances )
			{
				Mobile m = state.Mobile;
				if ( !Directory.Exists( "Web" ) )
				Directory.CreateDirectory( "Web" );

				
				if ( m != null && m.AccessLevel < AccessLevel.GameMaster )
				{
					Pagename =  m.Name;
					Pagename = Pagename.Trim();
					string serial = m.Serial.Value.ToString();

					using ( StreamWriter op = new StreamWriter( "Web/Players/" + Pagename + serial + ".html" ) )
					{
						op.WriteLine( "<html>" );
						op.WriteLine( "   <head>" );
                     				op.WriteLine( "      <table border=\"6\" cellpadding=\"4\" width=\"100%\"><tr>" );
                     				op.WriteLine( "      <tr><td width=\"33%\"></td><td width=\"33%\"></td></tr>" ); 
                     				op.WriteLine( "      </table><br>" );
						op.WriteLine( "      <title>Character Status</title>");
						op.WriteLine( "   <body bgcolor=\"black\">" );
						op.Write( "<font color=\"red\">" );
						op.WriteLine( "      <h1><p align=\"center\">" + Encode( m.Name ) + "'s Skill Status</h1>" );
						op.WriteLine( "      <table width=\"100%\">" );
						op.WriteLine( "         <tr>" );
		     				op.WriteLine( "      <table width=\"100%\">" );
                     				op.WriteLine( "      <tr><td width=\"33%\"></td><td width=\"33%\"></td></tr>" ); 
                     				op.WriteLine( "      </table><br>" );
                     				op.WriteLine( "      <table border=\"6\" cellpadding=\"4\" width=\"100%\"><tr>" );;
						op.WriteLine( "            <td bgcolor=\"red\"><font color=\"white\">Skill</font></td><td bgcolor=\"red\"><font color=\"white\">Status</font></td><td bgcolor=\"red\"><font color=\"white\">Skill Points</font></td>" );
						op.WriteLine( "         </tr>" );

						for ( int i = 0; i < m.Skills.Length; ++i )
						{
							Skill sk = m.Skills[i];

							if ( sk.BaseFixedPoint >= 500 )
							{
								string skname = "";
								switch ( i )
								{
									case 0: skname = "Alchemy"; break;
									case 1: skname = "Anatomy"; break;
									case 2: skname = "Animal Lore"; break;
									case 3: skname = "Item Identification"; break;
									case 4: skname = "Arms Lore"; break;
									case 5: skname = "Parrying"; break;
									case 6: skname = "Begging"; break;
									case 7: skname = "Blacksmithy"; break;
									case 8: skname = "Bowcraft/Fletching"; break;
									case 9: skname = "Peacemaking"; break;
									case 10: skname = "Camping"; break;
									case 11: skname = "Carpentry"; break;
									case 12: skname = "Cartography"; break;
									case 13: skname = "Cooking"; break;
									case 14: skname = "Detect Hidden"; break;
									case 15: skname = "Discordance"; break;
									case 16: skname = "Evaluating Intelligence"; break;
									case 17: skname = "Healing"; break;
									case 18: skname = "Fishing"; break;
									case 19: skname = "Forensic Evaluation"; break;
									case 20: skname = "Herding"; break;
									case 21: skname = "Hiding"; break;
									case 22: skname = "Provercation"; break;
									case 23: skname = "Inscription"; break;
									case 24: skname = "Lockpicking"; break;
									case 25: skname = "Magery"; break;
									case 26: skname = "Spells"; break;
									case 27: skname = "Tactics"; break;
									case 28: skname = "Snooping"; break;
									case 29: skname = "Musicianship"; break;
									case 30: skname = "Poisoning"; break;
									case 31: skname = "Archery"; break;
									case 32: skname = "Spirit Speak"; break;
									case 33: skname = "Stealing"; break;
									case 34: skname = "Tailoring"; break;
									case 35: skname = "Animal Taming"; break;
									case 36: skname = "Taste Identification"; break;
									case 37: skname = "Tinkering"; break;
									case 38: skname = "Tracking"; break;
									case 39: skname = "Veterinary"; break;
									case 40: skname = "Swordsmanship"; break;
									case 41: skname = "Mace Fighting"; break;
									case 42: skname = "Fencing"; break;
									case 43: skname = "Wrestling"; break;
									case 44: skname = "Lumberjacking"; break;
									case 45: skname = "Mining"; break;
									case 46: skname = "Meditation"; break;
									case 47: skname = "Stealth"; break;
									case 48: skname = "Remove Trap"; break;
									case 49: skname = "Necromancy"; break;
									case 50: skname = "Focus"; break;
									case 51: skname = "Chivalry"; break;
									case 52: skname = "Bushido"; break;
									case 53: skname = "Ninjitsu"; break;
									case 54: skname = "Spellweaving"; break;
									case 55: skname = "Throwing"; break;
									case 56: skname = "Mysticism"; break;
									case 57: skname = "Imbuing"; break;
								}

								string skvalue = "";
								if ( m.Skills[i].Value >= 120 )
									skvalue = "<font color=\"cyan\">Legandary";
								else if ( m.Skills[i].Value >= 110 )
									skvalue = "<font color=\"darkblue\">Elder";
								else if ( m.Skills[i].Value >= 100 )
									skvalue = "<font color=\"green\">Grandmaster";
								else if ( m.Skills[i].Value >= 90 )
									skvalue = "<font color=\"orangered\">Master";
								else if ( m.Skills[i].Value >= 80 )
									skvalue = "<font color=\"palegreen\">Adept";
								else if ( m.Skills[i].Value >= 70 )
									skvalue = "<font color=\"purple\">Expert";
								else if ( m.Skills[i].Value >= 60 )
									skvalue = "<font color=\"darkorange\">Journeyman";
								else if ( m.Skills[i].Value >= 50 )
									skvalue = "<font color=\"yellow\">Aprentice";
							
								op.WriteLine( "<td><font color=\"red\">" + skname + "</td><td>" + skvalue + "</td><td>" );


								op.Write( "<font color=\"gold\">" );
								op.Write( m.Skills[i].Value );
								op.WriteLine( "</td></tr>" );

							}
						}

						op.WriteLine( "         <tr>" );
						op.WriteLine( "      </table>" );
						op.WriteLine( "</h1><h1  align=\"center\"><img border=\"0\" src=\"logo.gif\">" );
            					op.WriteLine( " <META HTTP-EQUIV=\"REFRESH\" CONTENT=\"10\"> " );
						op.WriteLine( "   </body>" );
						op.WriteLine( "</html>" );
					}
				}
				else
					Pagename = "";
			}
		}
	}
}