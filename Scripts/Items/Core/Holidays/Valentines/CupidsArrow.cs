using System;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class CupidsArrow : Item
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public virtual string From { get { return m_From; } set { m_From = value; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public virtual string To { get { return m_To; } set { m_To = value; } }

		private static string Unsigned = "___";

		public override int LabelNumber { get { return 1152270; } } // Cupid's Arrow 2012
		private string m_From;
		private string m_To;

		[Constructable]
		public CupidsArrow( int itemid )
			: base(itemid)
		{
			LootType = LootType.Blessed;

		}


		public override void AddNameProperty(ObjectPropertyList list)
		{
			list.Add(1152273, String.Format("{0}\t{1}", (m_To != null) ? m_To : Unsigned, (m_From != null) ? m_From : Unsigned));
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (m_To == null)
			{
				if (this.IsChildOf(from))
				{
					from.BeginTarget(10, false, TargetFlags.None, new TargetCallback(OnTarget));

					from.SendMessage( "Who would you like to target?" ); //To whom do you wish to give this card?
				}
				else
				{
					from.SendLocalizedMessage(1080063); // This must be in your backpack to use it.
				}
			}
		}

		public virtual void OnTarget(Mobile from, object targeted)
		{
			if (!Deleted)
			{
				if ( targeted is Corpse )
				{
					from.SendLocalizedMessage(1152269); //That target is dead and even Cupid's arrow won't make them love you
				}
				else if (targeted != null && targeted is Mobile)
				{
					Mobile to = targeted as Mobile;
					
//__________>>this					if( (to is PlayerMobile || to is BaseCreature) && !to.Alive )
//__________>>does					{
//__________>>not						from.SendLocalizedMessage(1152269); //That target is dead and even Cupid's arrow won't make them love you
//__________>>work						return;
//__________>>for dead bodies		}

					if ((to is PlayerMobile || to is BaseCreature) && to.Alive)
					{
						if (to != from) 
						{
							m_From = from.Name;
							m_To = to.Name;
							from.SendMessage("The arrow hits it's target. Hopefully the other person actually likes you..."); //You fill out the card. Hopefully the other person actually likes you...
							InvalidateProperties();
						}
						else
						{
							m_From = from.Name;
							m_To = to.Name;
							from.SendMessage("ok, well....good luck with that..."); //Couldnt find a cliloc here either.
							InvalidateProperties();
						}
					}
						

				}
				else
				{
					from.SendLocalizedMessage(1077488); //That's not another player!
				}
			}
		}

		public CupidsArrow(Serial serial)
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
			writer.Write((string)m_From);
			writer.Write((string)m_To);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			m_From = reader.ReadString();
			m_To = reader.ReadString();

			Utility.Intern(ref m_From);
			Utility.Intern(ref m_To);
		}
	}

	public class CupidsArrowWest : CupidsArrow
	{
		[Constructable]
		public CupidsArrowWest()
			: base(0x4F7A)
		{
		}

		public CupidsArrowWest(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class CupidsArrowSouth : CupidsArrow
	{
		[Constructable]
		public CupidsArrowSouth()
			: base(0x4F7B)
		{
		}

		public CupidsArrowSouth(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
	
	public class CupidsArrowEast : CupidsArrow
	{
		[Constructable]
		public CupidsArrowEast()
			: base(0x4F7E)
		{
		}

		public CupidsArrowEast(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class CupidsArrowNorth : CupidsArrow
	{
		[Constructable]
		public CupidsArrowNorth()
			: base(0x4F7F)
		{
		}

		public CupidsArrowNorth(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}