using System;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Items
{
	public class HalloweenCandy2  : BaseFood
	{
		private static Dictionary<Mobile, HalloweenCandy2Timer> m_ToothAches;

		public static Dictionary<Mobile, HalloweenCandy2Timer> ToothAches
		{
			get { return m_ToothAches; }
			set { m_ToothAches = value; }
		}

		public static void Initialize()
		{
			m_ToothAches = new Dictionary<Mobile, HalloweenCandy2Timer>();
		}

		public class HalloweenCandy2Timer : Timer
		{
			private int m_Eaten;
			private Mobile m_Eater;

			public Mobile Eater { get { return m_Eater; } }
			public int Eaten { get { return m_Eaten; } set { m_Eaten = value; } }

			public HalloweenCandy2Timer( Mobile eater ) 
				: base( TimeSpan.FromSeconds( 30 ), TimeSpan.FromSeconds( 30 ) )
			{
				m_Eater = eater;
				Priority = TimerPriority.FiveSeconds;
				Start();
			}

			protected override void OnTick()
			{
				Eaten--;

				if ( Eater == null || Eater.Deleted ||  Eaten <= 0 )
				{
					Stop();
					ToothAches.Remove( Eater );
				}
				else if ( Eater.Map != Map.Internal && Eater.Alive )
				{
					if( Eaten > 60 )
					{
						Eater.Say( 1077388  + Utility.Random(5) ); // ARRGH! My tooth hurts sooo much!, etc.

						if( Utility.RandomBool() )
						{
							Eater.Animate( 32, 5, 1, true, false, 0 );
						}
					}
					else if ( Eaten == 60 )
					{
						Eater.SendLocalizedMessage( 1077393 ); // The extreme pain in your teeth subsides.
					}
				}
			}
		}

		[Constructable]
		public HalloweenCandy2() : base( 0x469D )
		{
			ItemID = 0x469D + Utility.Random(2);
			Stackable = false;
			LootType=LootType.Blessed;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( IsChildOf( from.Backpack ) || from.InRange(this, 1) )
			{
				from.PlaySound( 0x3a + Utility.Random(3) ); 
				from.Animate( 34, 5, 1, true, false, 0 );

				if ( !ToothAches.ContainsKey( from ) )
				{
					ToothAches.Add( from, new HalloweenCandy2Timer( from ) );
				}

				ToothAches[from].Eaten += 32;

				from.SendLocalizedMessage( 1077387 ); // You feel as if you could eat as much as you wanted!
				Delete();
			}
		}

		public HalloweenCandy2( Serial serial ) : base( serial )
		{
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
	}
}

