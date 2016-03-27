using Server.ContextMenus;
using Server.Engines.Craft;
using Server.Multis;
using System;
using System.Collections.Generic;

namespace Server.Items
{

    public class SpecialVeteranAddonComponentBox : SpecialVeteranAddonComponent
    {
        private SpecialVeteranCraftAddon m_Addon;

        [Constructable]
        public SpecialVeteranAddonComponentBox(int itemID)
            : base(itemID)
        {
            m_Addon = Addon as SpecialVeteranCraftAddon;
        }

        public SpecialVeteranAddonComponentBox(Serial serial)
            : base(serial)
        {
            m_Addon = Addon as SpecialVeteranCraftAddon;
        }
        
        public override int LabelNumber
        {
            get
            {
                return 1123593;
            }
        }// crates

        public override void OnDoubleClick(Mobile from)
        {
        }

        public virtual Type[] AllowedTools
        {
            get
            {
                return new Type[] { typeof(Tongs), typeof(SmithHammer), typeof(SledgeHammer) };
            }
        }
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped != null && dropped is BaseTool)
            {
                foreach (Type t in AllowedTools)
                {
                    if (dropped.GetType().Equals(t))
                    {
                        BaseTool tool = dropped as BaseTool;
                        if (SpAddon.UsesRemaining + tool.UsesRemaining <= SpAddon.MaxUses)
                        {
                            SpAddon.UsesRemaining += tool.UsesRemaining;
                            tool.Delete();
                        }
                        else
                        {
                            int charges = SpAddon.MaxUses - SpAddon.UsesRemaining;
                            SpAddon.UsesRemaining += charges;
                            tool.UsesRemaining -= charges;
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            if (SpAddon.ShowUsesRemaining)
            {
                list.Add(1153098, SpAddon.UsesRemaining.ToString());
            }
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