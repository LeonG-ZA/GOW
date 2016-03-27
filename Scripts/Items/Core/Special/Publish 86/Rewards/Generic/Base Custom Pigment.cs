using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;
using Server.Misc;
//**************************************************************
//* Unit : BaseCustomPigment
//* Purpose : Generic Custom Tokuno Pigment Class
//**************************************************************
namespace Server.Items
{
    public class BaseCustomPigment : Item, IUsesRemaining
    {
        public override int LabelNumber { get { return 1070933; } }

        private int m_UsesRemaining;
        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining { get { return m_UsesRemaining; } set { m_UsesRemaining = value; } }

        [Constructable]
        public BaseCustomPigment(int s_Hue)
            : this(s_Hue, 5)
        {

        }

        [Constructable]
        public BaseCustomPigment(int s_Hue, int s_Uses)
        {
            Hue = s_Hue;
            UsesRemaining = s_Uses;
            Weight = 1;
            LootType = LootType.Blessed;
            ItemID = 3839;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1060584, m_UsesRemaining.ToString()); // uses remaining: ~1_val~
        }

        public override void OnDoubleClick(Mobile from)
        {

            if (IsAccessibleTo(from) && from.InRange(GetWorldLocation(), 3))
            {
                from.SendLocalizedMessage(1070929); // Select the artifact or enhanced magic item to dye.
                from.BeginTarget(3, false, Server.Targeting.TargetFlags.None, new TargetStateCallback(InternalCallback), this);
            }
            else
                from.SendLocalizedMessage(502436); // That is not accessible.
        }

        private void InternalCallback(Mobile from, object targeted, object state)
        {
            BaseCustomPigment pigment = (BaseCustomPigment)state;

            if (pigment.Deleted || pigment.UsesRemaining <= 0 || !from.InRange(pigment.GetWorldLocation(), 3) || !pigment.IsAccessibleTo(from))
                return;

            Item i = targeted as Item;

            if (i == null)
                from.SendLocalizedMessage(1070931); // You can only dye artifacts and enhanced magic items with this tub.
            else if (!from.InRange(i.GetWorldLocation(), 3) || !IsAccessibleTo(from))
                from.SendLocalizedMessage(502436); // That is not accessible.
            else if (i.IsLockedDown)
                from.SendLocalizedMessage(1070932); // You may not dye artifacts and enhanced magic items which are locked down.
            else if (i is PigmentsOfTokuno)
                from.SendLocalizedMessage(1042417); // You cannot dye that.
            else if (!PigmentsOfTokuno.IsValidItem(i))
            {
                from.SendLocalizedMessage(1070931); // You can only dye artifacts and enhanced magic items with this tub.	//Yes, it says tub on OSI.  Don't ask me why ;p
            }
            else
            {
                //Notes: on OSI there IS no hue check to see if it's already hued.  and no messages on successful hue either
                i.Hue = pigment.Hue;

                if (--pigment.UsesRemaining <= 0)
                    pigment.Delete();

                from.PlaySound(0x23E); // As per OSI TC1
            }
        }

        public bool ShowUsesRemaining
        {
            get { return true; }
            set { }
        }

        public BaseCustomPigment(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.WriteEncodedInt(m_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_UsesRemaining = reader.ReadEncodedInt();
        }

    }
}
