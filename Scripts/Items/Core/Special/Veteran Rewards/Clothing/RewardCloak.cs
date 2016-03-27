using System;
using Server.Engines.VeteranRewards;

namespace Server.Items
{
    [Flipable]
    public class RewardCloak : BaseCloak, IRewardItem
    {
        private int m_LabelNumber;
        private bool m_IsRewardItem;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsRewardItem
        {
            get
            {
                return this.m_IsRewardItem;
            }
            set
            {
                this.m_IsRewardItem = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Number
        {
            get
            {
                return this.m_LabelNumber;
            }
            set
            {
                this.m_LabelNumber = value;
                this.InvalidateProperties();
            }
        }

        public override int LabelNumber
        {
            get
            {
                if (this.m_LabelNumber > 0)
                    return this.m_LabelNumber;

                return base.LabelNumber;
            }
        }

        public override int BasePhysicalResistance
        {
            get
            {
                return 3;
            }
        }

        public override void OnAdded(IEntity parent)
        {
            base.OnAdded(parent);

            if (parent is Mobile)
                ((Mobile)parent).VirtualArmorMod += 2;
        }

        public override void OnRemoved(IEntity parent)
        {
            base.OnRemoved(parent);

            if (parent is Mobile)
                ((Mobile)parent).VirtualArmorMod -= 2;
        }

        public override bool Dye(Mobile from, IDyeTub sender)
        {
            from.SendLocalizedMessage(sender.FailMessage);
            return false;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (Core.ML && this.m_IsRewardItem)
                list.Add(RewardSystem.GetRewardYearLabel(this, new object[] { this.Hue, this.m_LabelNumber })); // X Year Veteran Reward
        }

        public override bool CanEquip(Mobile m)
        {
            if (!base.CanEquip(m))
                return false;

            return !this.m_IsRewardItem || Engines.VeteranRewards.RewardSystem.CheckIsUsableBy(m, this, new object[] { this.Hue, this.m_LabelNumber });
        }

        [Constructable]
        public RewardCloak()
            : this(0)
        {
        }

        [Constructable]
        public RewardCloak(int hue)
            : this(hue, 0)
        {
        }

        [Constructable]
        public RewardCloak(int hue, int labelNumber)
            : base(0x1515, hue)
        {
            this.Weight = 5.0;
            this.LootType = LootType.Blessed;

            this.m_LabelNumber = labelNumber;
        }

        public RewardCloak(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)this.m_LabelNumber);
            writer.Write((bool)this.m_IsRewardItem);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 0:
                    {
                        this.m_LabelNumber = reader.ReadInt();
                        this.m_IsRewardItem = reader.ReadBool();
                        break;
                    }
            }

            if (this.Parent is Mobile)
                ((Mobile)this.Parent).VirtualArmorMod += 2;
        }
    }
}