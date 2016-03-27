using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Server;
using Server.Engines.Craft;
using Server.Engines.Plants;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;

namespace Server.Items
{
    [Flipable(0xDFC, 0xDFD)]
    public class Clippers : Item, IUsesRemaining, ICraftable
    {
        public override int LabelNumber { get { return 1112117; } } // clippers

        private Mobile _Crafter;
        private ToolQuality _Quality;

        private int _UsesRemaining;
        private bool _CutReeds;
        private bool _ShowUsesRemaining;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter
        {
            get { return _Crafter; }
            set
            {
                _Crafter = value;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ToolQuality Quality
        {
            get { return _Quality; }
            set
            {
                UnscaleUses();

                _Quality = value;

                ScaleUses();
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ShowUsesRemaining
        {
            get { return _ShowUsesRemaining; }
            set
            {
                _ShowUsesRemaining = value;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return _UsesRemaining; }
            set
            {
                _UsesRemaining = value;
                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CutReeds
        {
            get { return _CutReeds; }
            set { _CutReeds = value; }
        }

        [Constructable]
        public Clippers()
            : base(0x0DFC)
        {
            Weight = 1.0;
            Hue = 1168;
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendLocalizedMessage(1112118); // What plant do you wish to use these clippers on?
            from.Target = new ClippersTarget(this);
        }

        public void ScaleUses()
        {
            _UsesRemaining = (_UsesRemaining * GetUsesScalar()) / 100;
            InvalidateProperties();
        }

        public void UnscaleUses()
        {
            _UsesRemaining = (_UsesRemaining * 100) / GetUsesScalar();
        }

        public int GetUsesScalar()
        {
            return _Quality == ToolQuality.Exceptional ? 200 : 100;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            //Makers mark not displayed on OSI
            if (_Crafter != null)
            {
                list.Add(1050043, _Crafter.RawName); // crafted by ~1_NAME~
            }

            if (_Quality == ToolQuality.Exceptional)
            {
                list.Add(1060636); // exceptional
            }

            list.Add(1060584, _UsesRemaining.ToString(CultureInfo.InvariantCulture)); // uses remaining: ~1_val~
        }

        public Clippers(Serial serial)
            : base(serial)
        {
        }

        #region ICraftable Members
        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, IUsesRemaining tool, CraftItem craftItem, int resHue)
        //public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
        {
            Quality = (ToolQuality)quality;

            if (makersMark)
            {
                Crafter = from;
            }

            return quality;
        }
        #endregion

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            const int ver = 2;

            writer.Write(ver); // version

            switch (ver)
            {
                case 2:
                    writer.Write(_ShowUsesRemaining);
                    goto case 1;
                case 1:
                    writer.Write(_UsesRemaining);
                    goto case 0;
                case 0:
                    break;
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    _ShowUsesRemaining = reader.ReadBool();
                    goto case 1;
                case 1:
                    _UsesRemaining = reader.ReadInt();
                    goto case 0;
                case 0:
                    {
                        if (version < 2)
                        {
                            ShowUsesRemaining = true;
                        }
                    }
                    break;
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive)
            {
                list.Add(new SetClipModeEntry(this, true, 1112283, !_CutReeds)); // Set to cut reeds
                list.Add(new SetClipModeEntry(this, false, 1112282, _CutReeds)); // Set to clip plants
            }
        }

        private class SetClipModeEntry : ContextMenuEntry
        {
            private Clippers m_Clippers;
            private bool m_EnableCutReeds;

            public SetClipModeEntry(Clippers clippers, bool enableCutReeds, int number, bool enabled)
                : base(number)
            {
                m_Clippers = clippers;
                m_EnableCutReeds = enableCutReeds;

                if (!enabled)
                    Flags |= CMEFlags.Disabled;
            }

            public override void OnClick()
            {
                if (m_Clippers.Deleted)
                    return;

                Mobile from = Owner.From;

                if (m_Clippers.CutReeds && m_EnableCutReeds)
                    from.SendLocalizedMessage(1112287); // You are already set to cut reeds!
                else if (!m_Clippers.CutReeds && !m_EnableCutReeds)
                    from.SendLocalizedMessage(1112284); // You are already set to make plant clippings!
                else
                {
                    m_Clippers.CutReeds = m_EnableCutReeds;

                    from.SendLocalizedMessage(m_EnableCutReeds
                            ? 1112286 // You are now set to cut reeds.
                            : 1112285 // You are now set to cut plant clippings.
                        );
                }
            }
        }

        private class ClippersTarget : Target
        {
            private Clippers m_Clippers;

            public ClippersTarget(Clippers clippers)
                : base(1, false, TargetFlags.None)
            {
                m_Clippers = clippers;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Clippers.Deleted)
                    return;

                if (targeted is PlantItem)
                {
                    PlantItem plant = (PlantItem)targeted;

                    if (!plant.IsChildOf(from.Backpack))
                        from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack.
                    if (plant.PlantStatus != PlantStatus.DecorativePlant)
                        from.SendLocalizedMessage(1112119); // You may only use these clippers on decorative plants.
                    else
                    {
                        // TODO (SA): ¿Se puede podar cualquier tipo de planta?

                        Item item;
                        if (m_Clippers.CutReeds)
                            item = new DryReeds(plant.PlantHue);
                        else
                            item = new PlantClippings(plant.PlantHue);

                        plant.Delete();

                        if (!from.PlaceInBackpack(item))
                            item.MoveToWorld(from.Location, from.Map);

                        from.SendLocalizedMessage(1112120); // You cut the plant into small pieces and place them in your backpack.

                        m_Clippers.UsesRemaining--;

                        if (m_Clippers.UsesRemaining <= 0)
                        {
                            m_Clippers.Delete();
                            from.SendLocalizedMessage(1112126); // Your clippers break as you use up the last charge.
                        }
                    }
                }
                else
                    from.SendLocalizedMessage(1112119); // You may only use these clippers on decorative plants.
            }
        }
    }
}