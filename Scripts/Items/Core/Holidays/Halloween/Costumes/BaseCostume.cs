using System;

namespace Server.Items
{
    [Flipable(0x19BC, 0x19BD)]
    public class BaseCostume : BaseShield, IDyable
    {
        private int _Hue = -1;
        private bool _SaveDisplayGuildTitle = true;
        private int _SaveHueMod = -1;
        private int _SaveNameHue = -1;
        public bool _Transformed;
        private Mobile _Wearer;

        public BaseCostume() : base(0x19BC)
        {
            CostumeBody = 0;
            Name = "Generic Costume";
            Resource = CraftResource.None;
            Attributes.SpellChanneling = 1;
            Layer = Layer.FirstValid;
            Weight = 3.0;
        }

        public BaseCostume(Serial serial) : base(serial)
        {
            CostumeBody = 0;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Transformed
        {
            get { return _Transformed; }
            set { _Transformed = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int CostumeBody { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int CostumeHue
        {
            get { return _Hue; }
            set { _Hue = value; }
        }

        public virtual bool Dye(Mobile from, IDyeTub sender)
        {
            if (Deleted)
                return false;

            if (RootParent is Mobile && @from != RootParent)
                return false;

            Hue = sender.DyedHue;
            return true;
        }

        private void EquipCostume(Mobile from)
        {
            _Wearer = from;
            from.SendMessage("You put on your spooky costume!");

            _SaveNameHue = from.NameHue;
            _SaveDisplayGuildTitle = from.DisplayGuildTitle;
            _SaveHueMod = from.HueMod;
            from.BodyMod = CostumeBody;
            from.NameHue = 39;
            from.HueMod = _Hue;
            from.DisplayGuildTitle = false;
            Transformed = true;
        }

        private void UnEquipCostume(Mobile from)
        {
            from.SendMessage("You dicide to quit being so spooky.");

            from.BodyMod = 0;
            from.NameHue = _SaveNameHue;
            from.HueMod = _SaveHueMod;
            from.DisplayGuildTitle = _SaveDisplayGuildTitle;
            Transformed = false;
        }

        public override void OnAdded(IEntity parent)
        {
            if (parent is Mobile) _Wearer = (Mobile) parent;
            base.OnAdded(parent);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Parent != from)
            {
                from.SendMessage("The costume must be equiped to be used.");
            }

            else if (@from.Mounted)
            {
                from.SendMessage("You cannot be mounted while wearing your costume!");
            }

            else if (from.BodyMod != 0 && !Transformed)
            {
                from.SendMessage("You are already costumed!");
            }

            else if (Transformed == false)
            {
                EquipCostume(from);
            }
            else
            {
                UnEquipCostume(from);
            }
        }

        public override void OnRemoved(IEntity o)
        {
            if (Transformed) UnEquipCostume(_Wearer);
            _Wearer = null;

            if (o is Mobile && ((Mobile) o).Kills >= 5)
            {
                ((Mobile) o).Criminal = true;
            }

            if (o is Mobile && ((Mobile) o).GuildTitle != null)
            {
                ((Mobile) o).DisplayGuildTitle = _SaveDisplayGuildTitle;
            }

            base.OnRemoved(o);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1);
            writer.Write(CostumeBody);
            writer.Write(_Hue);
            writer.Write(_SaveNameHue);
            writer.Write(_SaveDisplayGuildTitle);
            writer.Write(_SaveHueMod);

            if (_Wearer == null)
                writer.Write(Serial.MinusOne.Value);
            else
                writer.Write(_Wearer.Serial.Value);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();

            if (version == 1)
            {
                CostumeBody = reader.ReadInt();
                _Hue = reader.ReadInt();
                _SaveNameHue = reader.ReadInt();
                _SaveDisplayGuildTitle = reader.ReadBool();
                _SaveHueMod = reader.ReadInt();
                Serial WearerSerial = reader.ReadInt();

                if (WearerSerial.IsMobile)
                    _Wearer = World.FindMobile(WearerSerial);

                else
                    _Wearer = null;
            }
        }
    }
}