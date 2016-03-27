using System;
using Server.Gumps;
using Server.Multis;
using Server.Targeting;

namespace Server.Items
{
    interface IEngravable
    {
        string EngravedText { get; set; }
    }

    public class BaseEngravingTool : Item, IUsesRemaining
    { 
        private int I_UsesRemaining;
        [Constructable]
        public BaseEngravingTool(int itemID)
            : this(itemID, 30)
        {
        }

        [Constructable]
        public BaseEngravingTool(int itemID, int uses)
            : base(itemID)
        {
            Weight = 1.0;
            Hue = 0x48D;
			
            I_UsesRemaining = uses;
        }

        public BaseEngravingTool(Serial serial)
            : base(serial)
        {
        }

        public virtual Type[] Engraves
        {
            get
            {
                return null;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get
            {
                return this.I_UsesRemaining;
            }
            set
            {
                this.I_UsesRemaining = value;
                this.InvalidateProperties();
            }
        }
        public virtual bool ShowUsesRemaining
        { 
            get
            {
                return true;
            }
            set
            {
            }
        }
        public bool CheckItem(Item item)
        {
            if (this.Engraves == null || item == null)
                return false;
				
            Type type = item.GetType();
				
            for (int i = 0; i < this.Engraves.Length; i ++)
            { 
                if (type == this.Engraves[i] || type.IsSubclassOf(this.Engraves[i]))
                    return true;
            }
			
            return false;
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
			
            if (!from.NetState.SupportsExpansion(Expansion.ML))
            {
                from.SendLocalizedMessage(1072791); // You must upgrade to Mondain's Legacy in order to use that item.				
                return;
            }
            if (IsChildOf(from.Backpack) || Parent == from)
            {
                if (this.I_UsesRemaining > 0)
                {
                    from.SendLocalizedMessage(1072357); // Select an object to engrave.
                    from.Target = new EngraveTarget(this);
                }
                else
                {
                    from.SendLocalizedMessage(1042544); // This item is out of charges.
                }
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
			
            if (this.ShowUsesRemaining)
                list.Add(1060584, this.I_UsesRemaining.ToString()); // uses remaining: ~1_val~			
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
			
            writer.Write((int)0); // version
			
            writer.Write((int)this.I_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
			
            int version = reader.ReadInt();
			
            this.I_UsesRemaining = reader.ReadInt();
        }

        private class EngraveTarget : Target
        {
            private readonly BaseEngravingTool I_Tool;
            public EngraveTarget(BaseEngravingTool tool)
                : base(2, true, TargetFlags.None)
            {
                this.I_Tool = tool;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (this.I_Tool == null || this.I_Tool.Deleted)
                    return;

                if (targeted is Item)
                {
                    Item item = (Item)targeted;
					
                    if (this.IsValid(item, from))
                    {
                        if (item is IEngravable && this.I_Tool.CheckItem(item))
                        {
                            from.CloseGump(typeof(EngraveGump));
                            from.SendGump(new EngraveGump(this.I_Tool, item));
                        }
                        else
                            from.SendLocalizedMessage(1072309); // The selected item cannot be engraved by this engraving tool.
                    }
                    else
                        from.SendLocalizedMessage(1072310); // The selected item is not accessible to engrave.
                }
                else
                    from.SendLocalizedMessage(1072309); // The selected item cannot be engraved by this engraving tool.
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                from.SendLocalizedMessage(1072310); // The selected item is not accessible to engrave.
            }

            private bool IsValid(Item item, Mobile m)
            {
                if (BaseHouse.CheckAccessible(m, item))
                    return true;
                else if (item.Movable && !item.IsLockedDown && !item.IsSecure)
                    return true;
					
                return false;				
            }
        }

        private class EngraveGump : Gump
        {
            private readonly BaseEngravingTool I_Tool;
            private readonly Item I_Target;
            public EngraveGump(BaseEngravingTool tool, Item target)
                : base(0, 0)
            {
                this.I_Tool = tool;
                this.I_Target = target;

                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddBackground(50, 50, 400, 300, 0xA28);

                AddPage(0);

                AddHtmlLocalized(50, 70, 400, 20, 1072359, 0x0, false, false); // <CENTER>Engraving Tool</CENTER>
                AddHtmlLocalized(75, 95, 350, 145, 1076229, 0x0, true, true); // Please enter the text to add to the selected object. Leave the text area blank to remove any existing text.  Removing text does not use a charge.
                AddButton(125, 300, 0x81A, 0x81B, (int)Buttons.Okay, GumpButtonType.Reply, 0);
                AddButton(320, 300, 0x819, 0x818, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
                AddImageTiled(75, 245, 350, 40, 0xDB0);
                AddImageTiled(76, 245, 350, 2, 0x23C5);
                AddImageTiled(75, 245, 2, 40, 0x23C3);
                AddImageTiled(75, 285, 350, 2, 0x23C5);
                AddImageTiled(425, 245, 2, 42, 0x23C3);

                AddTextEntry(78, 246, 343, 37, 0xA28, (int)Buttons.Text, "");//15, "", 64);
			/*
                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;
			
                this.AddPage(0);
				
                this.AddBackground(50, 50, 400, 300, 0xA28);
                this.AddHtmlLocalized(50, 70, 400, 20, 1072359, 0x0, false, false);
                this.AddHtmlLocalized(75, 95, 350, 145, 1072360, 0x0, true, true);				
				
                this.AddButton(125, 300, 0x81A, 0x81B, (int)Buttons.Okay, GumpButtonType.Reply, 0);
                this.AddButton(320, 300, 0x819, 0x818, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
				
                this.AddImageTiled(75, 245, 350, 40, 0xDB0);
                this.AddImageTiled(76, 245, 350, 2, 0x23C5);
                this.AddImageTiled(75, 245, 2, 40, 0x23C3);
                this.AddImageTiled(75, 285, 350, 2, 0x23C5);
                this.AddImageTiled(425, 245, 2, 42, 0x23C3);
				
                this.AddTextEntry(78, 245, 345, 40, 0x0, (int)Buttons.Text, "");
             */
            }

            private enum Buttons
            {
                Cancel,
                Okay,
                Text
            }
            public override void OnResponse(Server.Network.NetState state, RelayInfo info)
            { 
                if (this.I_Tool == null || this.I_Tool.Deleted || this.I_Target == null || this.I_Target.Deleted)
                    return;
			
                if (info.ButtonID == (int)Buttons.Okay)
                {
                    TextRelay relay = info.GetTextEntry((int)Buttons.Text);
					
                    if (relay != null)
                    {
                        if (relay.Text == null || relay.Text.Equals(""))
                        {
                            ((IEngravable)this.I_Target).EngravedText = null;
                            state.Mobile.SendLocalizedMessage(1072362); // You remove the engraving from the object.
                        }
                        else
                        {
                            if (relay.Text.Length > 40)
                                ((IEngravable)this.I_Target).EngravedText = relay.Text.Substring(0, 40);
                            else
                                ((IEngravable)this.I_Target).EngravedText = relay.Text;
						
                            state.Mobile.SendLocalizedMessage(1072361); // You engraved the object.	

                            this.I_Target.InvalidateProperties();						
                            this.I_Tool.UsesRemaining -= 1;
                            this.I_Tool.InvalidateProperties();
						
                            if (this.I_Tool.UsesRemaining < 1)
                            {
                                this.I_Tool.Delete();
                                state.Mobile.SendLocalizedMessage(1044038); // You have worn out your tool!
                            }
                        }
                    }
                }
            }
        }
    }
}