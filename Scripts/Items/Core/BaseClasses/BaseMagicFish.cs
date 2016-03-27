using System;
using Server.Network;

namespace Server.Items
{
    public abstract class BaseMagicFish : Item
    {
        public BaseMagicFish(int hue)
            : base(0xDD6)
        {
            this.Hue = hue;
        }

        public BaseMagicFish(Serial serial)
            : base(serial)
        {
        }

        public virtual int Bonus
        {
            get
            {
                return 0;
            }
        }
        public virtual StatType Type
        {
            get
            {
                return StatType.Str;
            }
        }
        public override double DefaultWeight
        {
            get
            {
                return 1.0;
            }
        }
        public virtual bool Apply(Mobile from)
        {
            bool applied = Spells.SpellHelper.AddStatOffset(from, this.Type, this.Bonus, TimeSpan.FromMinutes(1.0));

            if (!applied)
                from.SendLocalizedMessage(502173); // You are already under a similar effect.

            return applied;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else if (this.Apply(from))
            {
                from.FixedEffect(0x375A, 10, 15);
                from.PlaySound(0x1E7);
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 501774); // You swallow the fish whole!
                this.Delete();
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