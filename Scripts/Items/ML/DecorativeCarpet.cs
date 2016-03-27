// Decorative Carpet -  v1.1 ( 19/12/2011 )
// 
// Now with workaround to let people drop stuff on carpets, fixed loss of color with pigments
//
// Source:  http://uo.stratics.com/php-bin/show_content.php?content=31094

using System;
using Server;
using Server.Regions;
using Server.Multis;

namespace Server.Items
{
    public class DecorativeCarpet : Item, IDyable, ITokunoDyable // and natural dyes
    {
        private const bool OSIStyle = false; // Change to true if you want 100% OSI compliant
        private bool highlighted = false;

        public override int LabelNumber { get { return 1101072; } } // a decorative carpet

        public bool Dye(Mobile from, IDyeTub sender)
        {
            if (Deleted) return false;

            Hue = sender.DyedHue;
            CheckHighlight();

            return true;
        }

        public int TrueHue { get { return base.Hue; } }

        [Hue, CommandProperty(AccessLevel.GameMaster)]
        public override int Hue
        {
            get
            {
                return (highlighted ? 53 : base.Hue);
            }
            set
            {
                base.Hue = value;
            }
        }

        [Constructable]
        public DecorativeCarpet(int itemId)
            : base(itemId)
        {
            Weight = 1.0;
            Stackable = true;
        }

        [Constructable]
        public DecorativeCarpet()
            : this(0x56B8 + Utility.Random(61)) // 0xABD olds
        {
        }

        public override void OnAfterDuped(Item newItem)
        {
            ((DecorativeCarpet)newItem).Hue = TrueHue;
        }

        public override bool StackWith(Mobile from, Item dropped, bool playSound)
        {
            if (dropped is DecorativeCarpet && ((DecorativeCarpet)dropped).TrueHue == TrueHue && Movable)
                return base.StackWith(from, dropped, playSound);

            return false;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (CheckHighlight()) list.Add(1113267); // (Double Click to Lockdown)
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Region is HouseRegion)
            {
                HouseRegion region = (HouseRegion)from.Region;

                if (region.House.IsInside(this) && Parent == null)
                {
                    if (region.House.IsCoOwner(from))
                    {
                        if (!Movable || IsOnSurface()) ToggleLockdown();
                    }
                    else
                    {
                        if (Movable) from.SendLocalizedMessage(1113268); // Only the house owner may lock this down.
                    }
                }
            }
        }

        public void ToggleLockdown()
        {
            Movable = !Movable;
            if (!OSIStyle) ItemID = AlternateItemID(ItemID); // With this we can drop object on the carpets
            CheckHighlight();
            InvalidateProperties();
        }

        public override void OnLocationChange(Point3D oldLocation)
        {
            base.OnLocationChange(oldLocation);

            CheckHighlight();
        }

        private bool CheckHighlight()
        {
            highlighted = Movable && IsOnSurface();
            return highlighted;
        }

        private bool IsOnSurface()
        {
            if (Parent != null) return false;
            if (BaseHouse.FindHouseAt(this) == null) return false;
            if (Amount != 1) return false;

            StaticTile[] tiles = Map.Tiles.GetStaticTiles(X, Y, true);
            for (int i = 0; i < tiles.Length; ++i)
            {
                StaticTile tile = tiles[i];
                ItemData id = TileData.ItemTable[tile.ID & TileData.MaxItemValue];

                if (id.Surface && tile.Z == Z) return true;
            }
            return false;
        }

        private static int AlternateItemID(int itemID)
        {
            /*
            Conversion Table:
            0x56B8            0x0ABE
            0x56B9            0x0ABD
            0x56BA            0x0AC0
            0x56BB            0x0ABF
            0x56BD-0x56C0    0x0AC2-0x0AC5    diff:(4BFB)
            0x56C1-0x56C4    0x0AF6-0x0AF9    diff:(4BCB)
            0x56C5            0x0AC8
            0x56C6            0x0AC7
            0x56C7            0x0AC6
            0x56C8-0x56E1    0x0AC9-0x0AE2    diff:(4BFF)
            0x56E2            0x0AEB
            0x56E3-0x56EA    0x0AE3-0x0AEA    diff:(4C00)
            0x56EB-0x56F4    0x0AEC-0x0AF5    diff:(4BFF)
            */
            if (itemID >= 0x56B8) switch (itemID)
                {
                    case 0x56B8: return 0x0ABE;
                    case 0x56B9: return 0x0ABD;
                    case 0x56BA: return 0x0AC0;
                    case 0x56BB: return 0x0ABF;
                    case 0x56C5: return 0x0AC8;
                    case 0x56C6: return 0x0AC7;
                    case 0x56C7: return 0x0AC6;
                    case 0x56E2: return 0x0AEB;
                    default:
                        {
                            if (itemID >= 0x56BD && itemID <= 0x56C0) return itemID - 0x4BFB;
                            else if (itemID >= 0x56C1 && itemID <= 0x56C4) return itemID - 0x4BCB;
                            else if (itemID >= 0x56E3 && itemID <= 0x56EA) return itemID - 0x4C00;
                            else if ((itemID >= 0x56C8 && itemID <= 0x56E1) || (itemID >= 0x56EB && itemID <= 0x56F4)) return itemID - 0x4BFF;
                            break;
                        }
                }
            else switch (itemID)
                {
                    case 0x0ABE: return 0x56B8;
                    case 0x0ABD: return 0x56B9;
                    case 0x0AC0: return 0x56BA;
                    case 0x0ABF: return 0x56BB;
                    case 0x0AC8: return 0x56C5;
                    case 0x0AC7: return 0x56C6;
                    case 0x0AC6: return 0x56C7;
                    case 0x0AEB: return 0x56E2;
                    default:
                        {
                            if (itemID >= 0x0AC2 && itemID <= 0x0AC5) return itemID + 0x4BFB;
                            else if (itemID >= 0x0AF6 && itemID <= 0x0AF9) return itemID + 0x4BCB;
                            else if (itemID >= 0x0AE3 && itemID <= 0x0AEA) return itemID + 0x4C00;
                            else if ((itemID >= 0x0AC9 && itemID <= 0x0AE2) || (itemID >= 0x0AEC && itemID <= 0x0AF5)) return itemID + 0x4BFF;
                            break;
                        }
                }

            return itemID;
        }

        public DecorativeCarpet(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version


        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            switch (version)
            {
                case 0: reader.ReadInt(); break;
            }
        }
    }
}