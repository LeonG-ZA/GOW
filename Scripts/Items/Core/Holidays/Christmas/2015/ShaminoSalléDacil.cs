using System;
using Server.Gumps;

namespace Server.Items
{
    [Flipable(0x9C14, 0x9C15)]
    public class ShaminoSalléDacil : Item
    {
        public override int LabelNumber { get { return 1156395; } }// Card of Semidar

        [Constructable]
        public ShaminoSalléDacil()
            : base(0x9C14)
        {
        }

        public ShaminoSalléDacil(Serial serial)
            : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1156398); // Shamino Sallé Dacil
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.HasGump(typeof(CardPic)))
            {
                from.CloseGump(typeof(CardPic));
                from.SendGump(new CardPic());
                return;
            }
            else
            {
                from.SendGump(new CardPic());
            }
        }

        private class CardPic : Gump
        {
            public CardPic()
                : base(0, 0)
            {
                 Closable = true;
                 Disposable = true;
                 Dragable = true;
                 Resizable = false;
                 AddImage(0, 0, 39906);
            }
        }

        //public override int Title
        //{
        //    get
         //   {
        //        return 1154355;
        //    }
       // }// Head on a spike

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