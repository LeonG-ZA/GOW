using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.Multis;
using Server.Events;

namespace Server.Items
{
	[FlipableAttribute( 0x14E7, 0x14E8 )]
	public class HitchingPostAnniversary : Item, ISecurable
	{
		public override int LabelNumber { get { return 1071090; } } // Hitching Post

		private int m_UsesRemaining;
		private SecureLevel m_Level = SecureLevel.Owner;

		[CommandProperty( AccessLevel.GameMaster )]
		public SecureLevel Level
		{
			get { return m_Level; }
			set { m_Level = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set { m_UsesRemaining = value; InvalidateProperties(); }
		}

		[Constructable]
		public HitchingPostAnniversary()
			: base( 0x14E7 )
		{
			Movable = true;

			UsesRemaining = 30;
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			SetSecureLevelEntry.AddTo( from, this, list );
		}

		public HitchingPostAnniversary( Serial serial )
			: base( serial )
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1060584, m_UsesRemaining.ToString() );
		}

		private class StableEntry : ContextMenuEntry
		{
			private HitchingPostAnniversary m_Post;
			private Mobile m_From;

			public StableEntry( HitchingPostAnniversary post, Mobile from )
				: base( 6126, 12 )
			{
				m_Post = post;
				m_From = from;
			}
		}

		private class ClaimListGump : Gump
		{
			private HitchingPostAnniversary m_Post;
			private Mobile m_From;
			private List<BaseCreature> m_List;

			public ClaimListGump( HitchingPostAnniversary post, Mobile from, List<BaseCreature> list )
				: base( 50, 50 )
			{
				m_Post = post;
				m_From = from;
				m_List = list;

				from.CloseGump( typeof( ClaimListGump ) );

				AddPage( 0 );

				AddBackground( 0, 0, 325, 50 + ( list.Count * 20 ), 9250 );
				AddAlphaRegion( 5, 5, 315, 40 + ( list.Count * 20 ) );

				AddHtml( 15, 15, 275, 20, "<BASEFONT COLOR=#FFFFFF>Select a pet to retrieve from the stables:</BASEFONT>", false, false );

				for ( int i = 0; i < list.Count; ++i )
				{
					BaseCreature pet = list[i];

					if ( pet == null || pet.Deleted )
						continue;

					AddButton( 15, 39 + ( i * 20 ), 10006, 10006, i + 1, GumpButtonType.Reply, 0 );
					AddHtml( 32, 35 + ( i * 20 ), 275, 18, String.Format( "<BASEFONT COLOR=#C0C0EE>{0}</BASEFONT>", pet.Name ), false, false );
				}
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				int index = info.ButtonID - 1;

				if ( index >= 0 && index < m_List.Count )
				{
					m_Post.UsesRemaining -= 1;
					m_Post.EndClaimList( m_From, m_List[index] );
				}
			}
		}

		public static int GetMaxStabled( Mobile from )
		{
			double taming = from.Skills[SkillName.AnimalTaming].Value;
			double anlore = from.Skills[SkillName.AnimalLore].Value;
			double vetern = from.Skills[SkillName.Veterinary].Value;
			double sklsum = taming + anlore + vetern;

			int max;

			if ( sklsum >= 240.0 )
				max = 5;
			else if ( sklsum >= 200.0 )
				max = 4;
			else if ( sklsum >= 160.0 )
				max = 3;
			else
				max = 2;

			if ( taming >= 100.0 )
				max += (int) ( ( taming - 90.0 ) / 10 );

			if ( anlore >= 100.0 )
				max += (int) ( ( anlore - 90.0 ) / 10 );

			if ( vetern >= 100.0 )
				max += (int) ( ( vetern - 90.0 ) / 10 );

			return max;
		}

		private class StableTarget : Target
		{
			private HitchingPostAnniversary m_Post;

			public StableTarget( HitchingPostAnniversary post )
				: base( 12, false, TargetFlags.None )
			{
				m_Post = post;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is BaseCreature )
					m_Post.EndStable( from, (BaseCreature) targeted );
				else if ( targeted == from )
					from.SendLocalizedMessage( 502672 ); // HA HA HA! Sorry, I am not an inn.
				else
					from.SendLocalizedMessage( 1048053 ); // You can't stable that!
			}
		}

		public void BeginClaimList( Mobile from )
		{
			if ( Deleted || !from.CheckAlive() )
				return;

			if ( UsesRemaining <= 0 )
				from.SendLocalizedMessage( 1071151 ); //Hitching rope is insufficient. You have to supply it.
			else
			{
				List<BaseCreature> list = new List<BaseCreature>();

				for ( int i = 0; i < from.Stabled.Count; ++i )
				{
					BaseCreature pet = from.Stabled[i] as BaseCreature;

					if ( pet == null || pet.Deleted )
					{
						pet.IsStabled = false;
						from.Stabled.RemoveAt( i );
						--i;
						continue;
					}

					list.Add( pet );
				}

				if ( list.Count > 0 )
					from.SendGump( new ClaimListGump( this, from, list ) );
				else
					from.SendLocalizedMessage( 502671 ); // But I have no animals stabled with me at the moment!
			}
		}

		public void EndClaimList( Mobile from, BaseCreature pet )
		{
			if ( pet == null || pet.Deleted || from.Map != this.Map || !from.InRange( this, 14 ) || !from.Stabled.Contains( pet ) || !from.CheckAlive() )
				return;

			if ( ( from.Followers + pet.ControlSlots ) <= from.FollowersMax )
			{
				pet.SetControlMaster( from );

				if ( pet.Summoned )
					pet.SummonMaster = from;

				pet.ControlTarget = from;
				pet.ControlOrder = OrderType.Follow;

				pet.MoveToWorld( from.Location, from.Map );

				pet.IsStabled = false;
				from.Stabled.Remove( pet );

				from.SendLocalizedMessage( 1042559 ); // Here you go... and good day to you!
			}
			else
			{
				from.SendLocalizedMessage( 1049612, pet.Name ); // ~1_NAME~ remained in the stables because you have too many followers.
			}
		}

		public void BeginStable( Mobile from )
		{
			if ( Deleted || !from.CheckAlive() )
				return;

			if ( UsesRemaining <= 0 )
				from.SendLocalizedMessage( 1071151 ); //Hitching rope is insufficient. You have to supply it.

			else if ( from.Stabled.Count >= GetMaxStabled( from ) )
			{
				from.SendLocalizedMessage( 1042565 ); // You have too many pets in the stables!
			}
			else
			{
				from.SendLocalizedMessage( 1042558 ); /* I charge 30 gold per pet for a real week's stable time.
										 * I will withdraw it from thy bank account.
										 * Which animal wouldst thou like to stable here?
										 */

				from.Target = new StableTarget( this );
			}
		}

		public void EndStable( Mobile from, BaseCreature pet )
		{
			if ( Deleted || !from.CheckAlive() )
				return;

			if ( !pet.Controlled || pet.ControlMaster != from )
			{
				from.SendLocalizedMessage( 1042562 ); // You do not own that pet!
			}
			else if ( pet.IsDeadPet )
			{
				from.SendLocalizedMessage( 1049668 ); // Living pets only, please.
			}
			else if ( pet.Summoned )
			{
				from.SendLocalizedMessage( 502673 ); // I can not stable summoned creatures.
			}
			else if ( pet.Body.IsHuman )
			{
				from.SendLocalizedMessage( 502672 ); // HA HA HA! Sorry, I am not an inn.
			}
			else if ( ( pet is PackLlama || pet is PackHorse || pet is Beetle ) && ( pet.Backpack != null && pet.Backpack.Items.Count > 0 ) )
			{
				from.SendLocalizedMessage( 1042563 ); // You need to unload your pet.
			}
			else if ( pet.Combatant != null && pet.InRange( pet.Combatant, 12 ) && pet.Map == pet.Combatant.Map )
			{
				from.SendLocalizedMessage( 1042564 ); // I'm sorry.  Your pet seems to be busy.
			}
			else if ( from.Stabled.Count >= GetMaxStabled( from ) )
			{
				from.SendLocalizedMessage( 1042565 ); // You have too many pets in the stables!
			}
			else
			{
				Container bank = from.FindBankNoCreate();

				if ( bank != null && bank.ConsumeTotal( typeof( Gold ), 30 ) )
				{
					pet.ControlTarget = null;
					pet.ControlOrder = OrderType.Stay;
					pet.Internalize();

					pet.SetControlMaster( null );
					pet.SummonMaster = null;

					pet.IsStabled = true;
                    //pet.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully happy
                    pet.Loyalty = PetLoyalty.WonderfullyHappy; // Wonderfully happy

					from.Stabled.Add( pet );

					UsesRemaining -= 1;

					from.SendLocalizedMessage( 502679 ); // Very well, thy pet is stabled. Thou mayst recover it by saying 'claim' to me. In one real world week, I shall sell it off if it is not claimed!
				}
				else
				{
					from.SendLocalizedMessage( 502677 ); // But thou hast not the funds in thy bank account!
				}
			}
		}

		public void Claim( Mobile from )
		{
			if ( m_UsesRemaining <= 0 )
				from.SendLocalizedMessage( 1071151 ); // Hitching rope is insufficient. You have to supply it.
			else
			{
				if ( Deleted || !from.CheckAlive() )
					return;

				bool claimed = false;
				int stabled = 0;

				for ( int i = 0; i < from.Stabled.Count; ++i )
				{
					BaseCreature pet = from.Stabled[i] as BaseCreature;

					if ( pet == null || pet.Deleted )
					{
						pet.IsStabled = false;
						from.Stabled.RemoveAt( i );
						--i;
						continue;
					}

					++stabled;

					if ( ( from.Followers + pet.ControlSlots ) <= from.FollowersMax )
					{
						pet.SetControlMaster( from );

						if ( pet.Summoned )
							pet.SummonMaster = from;

						pet.ControlTarget = from;
						pet.ControlOrder = OrderType.Follow;

						pet.MoveToWorld( from.Location, from.Map );

						pet.IsStabled = false;
                        //pet.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully Happy
                        pet.Loyalty = PetLoyalty.WonderfullyHappy; // Wonderfully happy

						from.Stabled.RemoveAt( i );
						--i;

						claimed = true;
					}
					else
					{
						from.SendLocalizedMessage( 1049612, pet.Name ); // ~1_NAME~ remained in the stables because you have too many followers.
					}
				}

				if ( claimed )
				{
					from.SendLocalizedMessage( 1042559 ); // Here you go... and good day to you!
					UsesRemaining -= 1;
				}

				else if ( stabled == 0 )
					from.SendLocalizedMessage( 502671 ); // But I have no animals stabled with me at the moment!
			}
		}

		public bool IsOwner( Mobile mob )
		{
            BaseHouse house = BaseHouse.FindHouseAt(this);

			return ( house != null && house.IsOwner( mob ) );
		}
		public bool CheckAccess( Mobile m )
		{
			if ( !IsLockedDown || m.AccessLevel >= AccessLevel.GameMaster )
				return true;

            BaseHouse house = BaseHouse.FindHouseAt(this);

			if ( house != null && ( house.Public ? house.IsBanned( m ) : !house.HasAccess( m ) ) )
				return false;

			return ( house != null && house.HasSecureAccess( m, m_Level ) );
		}

		public override bool HandlesOnSpeech { get { return true; } }

		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( CheckAccess( e.Mobile ) && IsLockedDown )
			{
				if ( !e.Handled && e.HasKeyword( 0x0008 ) )
				{
					e.Handled = true;
					BeginStable( e.Mobile );
				}
				else if ( !e.Handled && e.HasKeyword( 0x0009 ) )
				{
					e.Handled = true;

					if ( !Insensitive.Equals( e.Speech, "claim" ) )
						BeginClaimList( e.Mobile );
					else
						Claim( e.Mobile );
				}
				else
				{
					base.OnSpeech( e );
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Level );
			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Level = (SecureLevel) reader.ReadInt();
			m_UsesRemaining = reader.ReadInt();
		}
	}
}