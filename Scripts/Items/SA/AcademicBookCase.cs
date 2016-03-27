using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using Server.ContextMenus;
using Server.Engines.Craft;
using Server.Targeting;
using Server.Gumps;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{

    [Furniture]
    [Flipable(0xA9D, 0xA9E)]
    public class AcademicBookCase : BaseContainer
    {

        public override int LabelNumber { get { return 1071213; } } // academic bookcase

        [Constructable]
        public AcademicBookCase()
            : base(0xA9D)
        {
            Weight = 1.0;
        }

        public AcademicBookCase(Serial serial)
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

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            Mobile m = from;
            PlayerMobile mobile = m as PlayerMobile;

            if (!from.InRange(this.GetWorldLocation(), 1))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1076766);//That is too far away.
                return false;
                 }
                 else 
                 {

                if (dropped is BaseBook)// || dropped is Spellbook || dropped is Runebook)
                {
                    if (Items.Count >= 0)
                    {
                        this.DropItem(dropped);
                        from.SendSound(this.GetDroppedSound(dropped), this.GetWorldLocation());
                    }

                    if (ItemID == 0xA9D)
                    {
                        ItemID = 0xa98;
                        this.DropItem(dropped);
                        from.SendSound(this.GetDroppedSound(dropped), this.GetWorldLocation());
                    }

                    if (ItemID == 0xA9E)
                    {
                        ItemID = 0xa99;
                        this.DropItem(dropped);
                        from.SendSound(this.GetDroppedSound(dropped), this.GetWorldLocation());
                    }
                    return true;
                }
                 else 
                {
                    from.SendMessage(89, "Only books can be placed in an academic bookcase.", mobile.NetState);
                }
            }
             return false;
            }

       public override bool OnDragDropInto(Mobile from, Item item, Point3D p, byte gridloc)
        {
            Mobile m = from;
            PlayerMobile mobile = m as PlayerMobile;

            if (!from.InRange(this.GetWorldLocation(), 1))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1076766);//That is too far away.
                return false;
                 }
                 else 
                 {

                if (item is BaseBook)// || item is Spellbook || item is Runebook)
                {
                    if (Items.Count >= 0)
                    {
                        item.Location = new Point3D(p.X, p.Y, 0);
                        this.AddItem(item);
                        from.SendSound(this.GetDroppedSound(item), this.GetWorldLocation());
                    }

                    if (ItemID == 0xA9D)
                    {
                        ItemID = 0xa98;
                        item.Location = new Point3D(p.X, p.Y, 0);
                        this.AddItem(item);
                        from.SendSound(this.GetDroppedSound(item), this.GetWorldLocation());
                    }

                    if (ItemID == 0xA9E)
                    {
                        ItemID = 0xa99;
                        item.Location = new Point3D(p.X, p.Y, 0);
                        this.AddItem(item);
                        from.SendSound(this.GetDroppedSound(item), this.GetWorldLocation());
                    }
                    return true;
                }
                 else 
                {
                    from.SendMessage(89, "Only books can be placed in an academic bookcase.", mobile.NetState);
                }
            }
             return false;
            }
                    
       public override void OnItemRemoved(Item item)
        {
            if (Items.Count == 0)

                 if ( ItemID == 0xA98 )
            {
                      ItemID = 0xA9D;
                }

             else if ( ItemID == 0xA99 )
            {
                   ItemID = 0xa9E;
                }

            base.OnItemRemoved(item);
                   
        }
    }
}
            

  