using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Network;
using Server.HolidayConfiguration;
using Server.Misc;

namespace Server.Items
{
	[FlipableAttribute(39331, 39332)]
	public class RaisedEasterBunny : Item
	{
		#region Config
        private int m_StartingMonth = HolidayConfig.EasterAprilStart;
        private int m_EndingMonth = HolidayConfig.EasterDayEnd;

		private TimeSpan m_GiftTimer = TimeSpan.FromHours(24); //Time that will expire before Bunny will give another gift. [Default: TimeSpan.FromHours(24)]
		private double m_GiftChance = 0.05;//Chance to gift a rare gift such as Chocolate Jellybeans [Min value of 0.01 to Max value of 1.00, Default: 0.05]

		private static int[] m_NormalHues = new int[] { 17, 33, 1196, 1359, 1366, 1372, 1378, }; //Default = Official Servers
		private static int[] m_RareHues = new int[] { 1151, 1157, 1175, 1283, 1289, }; //Custom Hues
		private bool AllowRareHueOnAll = false; //Do you want Rare Hue chance for all or just Rare Locations?  If true the chance to get rare hue on all is same as RareLocationChance (below).
		private double m_RareLocationChance = 0.05;//Chance that Easter Bunny will be shown as being raised in a Rare Location.

		private static string[] m_RareLocations = new string[]
		{
			"King Blackthorn's Castle", 
			"Bet-Lem Reg",	/*Ilshenar: Pixie village*/
			"Lakeshire",	/*Ilshenar: Home city of the Meer people*/
			"Mireg",		/*Ilshenar: Home city of the Meer people (native spelling)*/
			"Mistas",		/*Ilshenar: Ruined city*/
			"Montor",		/*Ilshenar: Ruined city*/
			"Reg Volom",  	/*Ilshenar: Home of the Ethereals*/
			"Ver Lor Reg",	/*Ilshenar: Gargoyle City*/
		};
		#endregion

		[Constructable]
		public RaisedEasterBunny( ) : base(Utility.RandomMinMax(0, 1) == 0 ? 39331 : 39332)
		{
			if ( Utility.RandomDouble() < m_RareLocationChance )
			{
				RareLocation = m_RareLocations[Utility.Random( m_RareLocations.Length )];
				Hue = m_RareHues[Utility.Random( m_RareHues.Length)];
			}
			else
			{
				City = m_Cities[Utility.Random( m_Cities.Length )];
				if ( AllowRareHueOnAll && Utility.RandomDouble() < m_RareLocationChance )
					Hue = m_RareHues[Utility.Random( m_RareHues.Length)];
				else
					Hue = m_NormalHues[Utility.Random( m_NormalHues.Length)];
			}
			                          	
			Weight = 1;
			LootType = LootType.Blessed;

			if (m_Year == 0)
				m_Year = HolidayConfig.m_CurrentYear;
		}

		public override void AddNameProperty( ObjectPropertyList list )
		{
			if ( City != 0 )
				list.Add( 1154711, String.Concat( "#", m_City ));	//list.Add( 1154711, (int)City ); //An Easter Bunny Raised in ~1_CITY~
			else if ( RareLocation != null )
				list.Add( 1154711, String.Format( "{0}", RareLocation ) ); //An Easter Bunny Raised in ~1_CITY~
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (m_Year == 0)
				m_Year = HolidayConfig.m_CurrentYear;
			list.Add("Easter {0}", m_Year);
		}

		public override void OnDoubleClick(Mobile from)
		{
			base.OnDoubleClick(from);

			if ( !IsChildOf( from.Backpack ) ) 
			{
				from.SendLocalizedMessage( 1042010 ); //You must have the object in your backpack to use it.
				return;
			}

			if ( DateTime.UtcNow.Month >= m_StartingMonth || DateTime.UtcNow.Month <= m_EndingMonth )
			{
				if ( DateTime.UtcNow >= m_NextEasterCandy )
				{
					m_NextEasterCandy = DateTime.UtcNow + m_GiftTimer;
					if ( m_GiftChance > Utility.RandomDouble() ) //Chance to obtain Chocolate Jellybeans
					{
						/*Note: The following can be replaced by a ChocolateJellybean constructable if you decide
						 * 		to code such item.  You could then use the cliloc for Chocolate Jellybean, but why
						 * 		worry when this works just as well with a lot less code ;)
						 */

						EasterJellyBeans ejb = new EasterJellyBeans();
						ejb.Name = "Chocolate Jellybeans";
						ejb.Hue = 1140; //The hue you want the Chocolate Jellybeans to be...
						from.AddToBackpack( ejb );
						from.SendLocalizedMessage( 1154713 ); // Ooops!  Might not want to eat that one!  *chuckles*
					}
					else
					{
						from.AddToBackpack( new MarshmallowDuckling() );
						from.SendLocalizedMessage( 1154714 ); //Here you go!  A freshly made marshmallow duckling! Don't eat too many!  *chuckles*
					}
				}
				else
					from.SendLocalizedMessage( 1154712 ); //I haven't quite finished a marshmallow duckling yet...
			}
			else
			{
				if ( m_StartingMonth == 3 && m_EndingMonth == 4 )
					from.SendLocalizedMessage( 1154716 );//The Easter Bunny only works during March & April.
				else
					from.SendMessage("The Easter Bunny does not work during this month.");
			}
		}

		private DateTime m_NextEasterCandy; //Stores the next time the Bunny will give a gift

		#region Cities
   		private static int[] m_Cities = new int[]
		{
			1012004, //Britain
			1114144, //Buccaneer's Den
			1011033, //Cove
			1011058, //Delucia
			1071432, //Heartwood
			1112572, //Holy City
			1012005, //Jhelom
			1060641, //Luna
			1012007, //Minoc
			1012003, //Moonglow
			1078098, //New Haven
			1012010, //New Magincia
			1011346, //Nu'Julem
			1076027, //Occlo
			1078341, //Old Haven
			1011057, //Papua
			1112571, //Royal City
			1074807, //Sanctuary
			1011348, //Serpent's Hold
			1012009, //Skara Brae
			1012008, //Trinsic
			1060642, //Umbra
			1011030, //Vesper
			1078263, //Wind
			1012006, //Yew
           	1072610, //Zento
		};

		private int m_City;

		[CommandProperty(AccessLevel.Developer)]
		public int City
		{
			get { return m_City; }
			set { m_City = value; InvalidateProperties(); }
		}

		private string m_RareLocation;

		[CommandProperty(AccessLevel.Developer)]
		public string RareLocation
		{
			get { return m_RareLocation; }
			set { m_RareLocation = value; InvalidateProperties(); }
		}
		#endregion

		private int m_Year;

		[CommandProperty(AccessLevel.Developer)]
		public int Year
		{
			get { return m_Year; }
			set { m_Year = value; InvalidateProperties(); }
		}

		#region Serialization
		public RaisedEasterBunny(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)1); // version
			writer.Write(m_Year);
			writer.WriteDeltaTime( (DateTime) m_NextEasterCandy );
			writer.Write(m_City);
			writer.Write(m_RareLocation);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			if (version > 0)
				m_Year = reader.ReadInt();

			m_NextEasterCandy = reader.ReadDeltaTime();
			m_City = reader.ReadInt();
			m_RareLocation = reader.ReadString();
		}
		#endregion
	}
}
