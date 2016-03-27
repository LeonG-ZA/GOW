using System;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
    public class AddonContainerComponent : Item, IChopable
    {
        private Point3D AcC_Offset;
        private BaseAddonContainer AcC_Addon;
        [Constructable]
        public AddonContainerComponent(int itemID)
            : base(itemID)
        {
            this.Movable = false;

            AddonComponent.ApplyLightTo(this);
        }

        public AddonContainerComponent(Serial serial)
            : base(serial)
        {
        }

        public virtual bool NeedsWall
        {
            get
            {
                return false;
            }
        }
        public virtual Point3D WallPosition
        {
            get
            {
                return Point3D.Zero;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public BaseAddonContainer Addon
        {
            get
            {
                return this.AcC_Addon;
            }
            set
            {
                this.AcC_Addon = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Offset
        {
            get
            {
                return this.AcC_Offset;
            }
            set
            {
                this.AcC_Offset = value;
            }
        }
        [Hue, CommandProperty(AccessLevel.GameMaster)]
        public override int Hue
        {
            get
            {
                return base.Hue;
            }
            set
            {
                base.Hue = value;

                if (this.AcC_Addon != null && this.AcC_Addon.ShareHue)
                    this.AcC_Addon.Hue = value;
            }
        }
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (this.Addon != null)
                return this.Addon.OnDragDrop(from, dropped);

            return false;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this.AcC_Addon != null)
                this.AcC_Addon.OnComponentUsed(this, from);
        }

        public override void OnLocationChange(Point3D old)
        {
            if (this.AcC_Addon != null)
                this.AcC_Addon.Location = new Point3D(this.X - this.AcC_Offset.X, this.Y - this.AcC_Offset.Y, this.Z - this.AcC_Offset.Z);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (this.AcC_Addon != null)
                this.AcC_Addon.GetContextMenuEntries(from, list);
        }

        public override void OnMapChange()
        {
            if (this.AcC_Addon != null)
                this.AcC_Addon.Map = this.Map;
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (this.AcC_Addon != null)
                this.AcC_Addon.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(this.AcC_Addon);
            writer.Write(this.AcC_Offset);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            this.AcC_Addon = reader.ReadItem() as BaseAddonContainer;
            this.AcC_Offset = reader.ReadPoint3D();

            if (this.AcC_Addon != null)
                this.AcC_Addon.OnComponentLoaded(this);

            AddonComponent.ApplyLightTo(this);
        }

        public virtual void OnChop(Mobile from)
        {
            if (this.AcC_Addon != null && from.InRange(this.GetWorldLocation(), 3))
                this.AcC_Addon.OnChop(from);
            else
                from.SendLocalizedMessage(500446); // That is too far away.
        }
    }

    public class LocalizedContainerComponent : AddonContainerComponent
    {
        private int AcC_LabelNumber;
        public LocalizedContainerComponent(int itemID, int labelNumber)
            : base(itemID)
        {
            this.AcC_LabelNumber = labelNumber;
        }

        public LocalizedContainerComponent(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                if (this.AcC_LabelNumber > 0)
                    return this.AcC_LabelNumber;

                return base.LabelNumber;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(this.AcC_LabelNumber);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            this.AcC_LabelNumber = reader.ReadInt();
        }
    }
}