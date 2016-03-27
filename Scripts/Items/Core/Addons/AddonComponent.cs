using System;

namespace Server.Items
{
    [Server.Engines.Craft.Anvil]
    public class AnvilComponent : AddonComponent
    {
        [Constructable]
        public AnvilComponent(int itemID) : base(itemID)
        {
        }

        public AnvilComponent(Serial serial)
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

    [Server.Engines.Craft.Forge]
    public class ForgeComponent : AddonComponent
    {
        [Constructable]
        public ForgeComponent(int itemID) : base(itemID)
        {
        }

        public ForgeComponent(Serial serial)
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

    public class LocalizedAddonComponent : AddonComponent
    {
        private int AC_LabelNumber;
        [Constructable]
        public LocalizedAddonComponent(int itemID, int labelNumber) : base(itemID)
        {
            this.AC_LabelNumber = labelNumber;
        }

        public LocalizedAddonComponent(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Number
        {
            get
            {
                return this.AC_LabelNumber;
            }
            set
            {
                this.AC_LabelNumber = value;
                this.InvalidateProperties();
            }
        }
        public override int LabelNumber
        {
            get
            {
                return this.AC_LabelNumber;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)this.AC_LabelNumber);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 0:
                    {
                        this.AC_LabelNumber = reader.ReadInt();
                        break;
                    }
            }
        }
    }

    public class AddonComponent : Item, IChopable
    {
        private static readonly LightEntry[] AC_Entries = new LightEntry[]
        {
            new LightEntry(LightType.WestSmall, 1122, 1123, 1124, 1141, 1142, 1143, 1144, 1145, 1146, 2347, 2359, 2360, 2361, 2362, 2363, 2364, 2387, 2388, 2389, 2390, 2391, 2392),
            new LightEntry(LightType.NorthSmall, 1131, 1133, 1134, 1147, 1148, 1149, 1150, 1151, 1152, 2352, 2373, 2374, 2375, 2376, 2377, 2378, 2401, 2402, 2403, 2404, 2405, 2406),
            new LightEntry(LightType.Circle300, 6526, 6538, 6571),
            new LightEntry(LightType.Circle150, 5703, 6587)
        };
        private Point3D AC_Offset;
        private BaseAddon AC_Addon;
        [Constructable]
        public AddonComponent(int itemID) : base(itemID)
        {
            this.Movable = false;
            ApplyLightTo(this);
        }

        public AddonComponent(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseAddon Addon
        {
            get
            {
                return this.AC_Addon;
            }
            set
            {
                this.AC_Addon = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Offset
        {
            get
            {
                return this.AC_Offset;
            }
            set
            {
                this.AC_Offset = value;
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

                if (this.AC_Addon != null && this.AC_Addon.ShareHue)
                    this.AC_Addon.Hue = value;
            }
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
        public static void ApplyLightTo(Item item)
        {
            if ((item.ItemData.Flags & TileFlag.LightSource) == 0)
                return; // not a light source

            int itemID = item.ItemID;

            for (int i = 0; i < AC_Entries.Length; ++i)
            {
                LightEntry entry = AC_Entries[i];
                int[] toMatch = entry.AC_ItemIDs;
                bool contains = false;

                for (int j = 0; !contains && j < toMatch.Length; ++j)
                    contains = (itemID == toMatch[j]);

                if (contains)
                {
                    item.Light = entry.AC_Light;
                    return;
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this.AC_Addon != null)
                this.AC_Addon.OnComponentUsed(this, from);
        }

        public void OnChop(Mobile from)
        {
            if (this.AC_Addon != null && from.InRange(this.GetWorldLocation(), 3))
                this.AC_Addon.OnChop(from);
            else
                from.SendLocalizedMessage(500446); // That is too far away.
        }

        public override void OnLocationChange(Point3D old)
        {
            if (this.AC_Addon != null)
                this.AC_Addon.Location = new Point3D(this.X - this.AC_Offset.X, this.Y - this.AC_Offset.Y, this.Z - this.AC_Offset.Z);
        }

        public override void OnMapChange()
        {
            if (this.AC_Addon != null)
                this.AC_Addon.Map = this.Map;
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (this.AC_Addon != null)
                this.AC_Addon.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write(this.AC_Addon);
            writer.Write(this.AC_Offset);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 1:
                case 0:
                    {
                        this.AC_Addon = reader.ReadItem() as BaseAddon;
                        this.AC_Offset = reader.ReadPoint3D();

                        if (this.AC_Addon != null)
                            this.AC_Addon.OnComponentLoaded(this);

                        ApplyLightTo(this);

                        break;
                    }
            }

            if (version < 1 && this.Weight == 0)
                this.Weight = -1;
        }

        private class LightEntry
        {
            public readonly LightType AC_Light;
            public readonly int[] AC_ItemIDs;
            public LightEntry(LightType light, params int[] itemIDs)
            {
                this.AC_Light = light;
                this.AC_ItemIDs = itemIDs;
            }
        }
    }
}