using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Spells;

namespace Server.Items
{
    public class BaseSpellScroll : Item, ICommodity
    {
        private int I_SpellID;
        public BaseSpellScroll(Serial serial)
            : base(serial)
        {
        }

        [Constructable]
        public BaseSpellScroll(int spellID, int itemID)
            : this(spellID, itemID, 1)
        {
        }

        [Constructable]
        public BaseSpellScroll(int spellID, int itemID, int amount)
            : base(itemID)
        {
            this.Stackable = true;
            this.Weight = 1.0;
            this.Amount = amount;

            this.I_SpellID = spellID;
        }

        public int SpellID
        {
            get
            {
                return this.I_SpellID;
            }
        }
        int ICommodity.DescriptionNumber
        {
            get
            {
                return this.LabelNumber;
            }
        }
        bool ICommodity.IsDeedable
        {
            get
            {
                return (Core.ML);
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)this.I_SpellID);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 0:
                    {
                        this.I_SpellID = reader.ReadInt();

                        break;
                    }
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive && this.Movable)
                list.Add(new ContextMenus.AddToSpellbookEntry());
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Multis.DesignContext.Check(from))
                return; // They are customizing

            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            Spell spell = SpellRegistry.NewSpell(this.I_SpellID, from, this);

            if (spell != null)
                spell.Cast();
            else
                from.SendLocalizedMessage(502345); // This spell has been temporarily disabled.
        }
    }
}