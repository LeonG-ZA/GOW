using System;
using System.Collections;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
    public class CrystalofShame : Item
    {
        [Constructable]
        public CrystalofShame() : this(1)
        {
        }

        [Constructable]
        public CrystalofShame(int amount) : base(0x400B)
        {
            this.Weight = 1.0;
            this.Hue = 2959;
            this.Stackable = true;
            this.Amount = amount;
        }

        public CrystalofShame(Serial serial) : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1151624;
            }
        }//Crystal of Shame

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                PlayerMobile pm = from as PlayerMobile;
                if (pm.ShamePoints == 30)
                {
                    this.Consume();
                }
                else
                {
                    pm.ShamePoints += 1;
                    this.Consume();
                }
            }
            else
            {
                from.SendLocalizedMessage(1054107);//This item must be in your backpack.
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}