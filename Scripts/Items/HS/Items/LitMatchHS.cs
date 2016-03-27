using System;

namespace Server.Items
{
    public class LitMatchHS : Item
    {
        private DateTime _TimeLit;
        private TimeSpan _Duration = TimeSpan.FromMinutes(10); //TODO verify this.. seems fairly close.

        [Constructable]
        public LitMatchHS(DateTime timeLit) : base(0xA12)
        {
            Hue = 542;
            Weight = 1.0;
            Light = LightType.Circle150;
            _TimeLit = timeLit;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if ((_TimeLit + _Duration) < DateTime.UtcNow)
            {
                from.SendLocalizedMessage(1116115); //Your match splutters and dies.
                Delete();
            }
            else if (IsChildOf(from.Backpack))
            {
                //TODO Target Method...Target cannon, check for loaded and prep'd cannon to fire
                from.SendLocalizedMessage(1116113); //Target the cannon whose fuse you wish to light.
            }
            else
                from.SendLocalizedMessage(1060640); //The item must be in your backpack to use it.
        }

        public override void OnSingleClick(Mobile from)
        {
            if ((_TimeLit + _Duration) < DateTime.UtcNow)
            {
                from.SendLocalizedMessage(1116115); //Your match splutters and dies.
                Delete();
            }
        }

        public override bool OnDragLift(Mobile from)
        {
            if ((_TimeLit + _Duration) < DateTime.UtcNow)
            {
                from.SendLocalizedMessage(1116115); //Your match splutters and dies.
                Delete();
                return false;
            }

            return true;
        }

        public LitMatchHS(Serial serial) : base(serial)
        { 
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}