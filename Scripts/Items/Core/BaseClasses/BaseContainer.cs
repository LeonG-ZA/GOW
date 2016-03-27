using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
    public abstract class BaseContainer : Container
    {
        public BaseContainer(int itemID)
            : base(itemID)
        {
        }

        public BaseContainer(Serial serial)
            : base(serial)
        {
        }

        public override int DefaultMaxWeight
        {
            get
            {
                if (this.IsSecure)
                    return 0;

                return base.DefaultMaxWeight;
            }
        }
        public override bool IsAccessibleTo(Mobile m)
        {
            if (!BaseHouse.CheckAccessible(m, this))
                return false;

            return base.IsAccessibleTo(m);
        }

        public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
        {
            if (this.IsSecure && !BaseHouse.CheckHold(m, this, item, message, checkItems, plusItems, plusWeight))
                return false;

            return base.CheckHold(m, item, message, checkItems, plusItems, plusWeight);
        }

        public override bool CheckItemUse(Mobile from, Item item)
        {
            if (this.IsDecoContainer && item is BaseBook)
                return true;

            return base.CheckItemUse(from, item);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            SetSecureLevelEntry.AddTo(from, this, list);
        }

        public override bool TryDropItem(Mobile from, Item dropped, bool sendFullMessage)
        {
            if (!this.CheckHold(from, dropped, sendFullMessage, true))
                return false;

            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house != null && house.IsLockedDown(this))
            {
                if (dropped is VendorRentalContract || (dropped is Container && ((Container)dropped).FindItemByType(typeof(VendorRentalContract)) != null))
                {
                    from.SendLocalizedMessage(1062492); // You cannot place a rental contract in a locked down container.
                    return false;
                }

                if (!house.LockDown(from, dropped, false))
                    return false;
            }

            List<Item> list = this.Items;

            for (int i = 0; i < list.Count; ++i)
            {
                Item item = list[i];

                if (!(item is Container) && item.StackWith(from, dropped, false))
                    return true;
            }

            this.DropItem(dropped);

            ItemFlags.SetTaken(dropped, true);

            return true;
        }

        public override bool OnDragDropInto(Mobile from, Item item, Point3D p, byte gridloc)
        {
            if (!this.CheckHold(from, item, true, true))
                return false;

            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house != null && house.IsLockedDown(this))
            {
                if (item is VendorRentalContract || (item is Container && ((Container)item).FindItemByType(typeof(VendorRentalContract)) != null))
                {
                    from.SendLocalizedMessage(1062492); // You cannot place a rental contract in a locked down container.
                    return false;
                }

                if (!house.LockDown(from, item, false))
                    return false;
            }

            item.Location = new Point3D(p.X, p.Y, 0);
            #region Enhance Client
            item.SetGridLocation(gridloc, this);
            #endregion
            this.AddItem(item);

            from.SendSound(this.GetDroppedSound(item), this.GetWorldLocation());

            ItemFlags.SetTaken(item, true);

            return true;
        }

        public override void UpdateTotal(Item sender, TotalType type, int delta)
        {
            base.UpdateTotal(sender, type, delta);

            if (type == TotalType.Weight && this.RootParent is Mobile)
                ((Mobile)this.RootParent).InvalidateProperties();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.IsStaff() || from.InRange(this.GetWorldLocation(), 2) || this.RootParent is PlayerVendor)
                this.Open(from);
            else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
        }

        public virtual void Open(Mobile from)
        {
            this.DisplayTo(from);
        }

        /* Note: base class insertion; we cannot serialize anything here */
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
