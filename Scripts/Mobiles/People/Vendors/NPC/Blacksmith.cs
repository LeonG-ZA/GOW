using System;
using System.Collections.Generic;
using Server.Engines.BulkOrders;
using Server.CharConfiguration;

namespace Server.Mobiles
{
    public class Blacksmith : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos
        {
            get
            {
                return m_SBInfos;
            }
        }

        public override NpcGuild NpcGuild
        {
            get
            {
                return NpcGuild.BlacksmithsGuild;
            }
        }

        [Constructable]
        public Blacksmith()
            : base("the blacksmith")
        {
            SetSkill(SkillName.ArmsLore, 36.0, 68.0);
            SetSkill(SkillName.Blacksmith, 65.0, 88.0);
            SetSkill(SkillName.Fencing, 60.0, 83.0);
            SetSkill(SkillName.Macing, 61.0, 93.0);
            SetSkill(SkillName.Swords, 60.0, 83.0);
            SetSkill(SkillName.Tactics, 60.0, 83.0);
            SetSkill(SkillName.Parry, 61.0, 93.0);
        }

        public override void InitSBInfo()
        {
            /*m_SBInfos.Add( new SBSmithTools() );
            m_SBInfos.Add( new SBMetalShields() );
            m_SBInfos.Add( new SBWoodenShields() );
            m_SBInfos.Add( new SBPlateArmor() );
            m_SBInfos.Add( new SBHelmetArmor() );
            m_SBInfos.Add( new SBChainmailArmor() );
            m_SBInfos.Add( new SBRingmailArmor() );
            m_SBInfos.Add( new SBAxeWeapon() );
            m_SBInfos.Add( new SBPoleArmWeapon() );
            m_SBInfos.Add( new SBRangedWeapon() );
            m_SBInfos.Add( new SBKnifeWeapon() );
            m_SBInfos.Add( new SBMaceWeapon() );
            m_SBInfos.Add( new SBSpearForkWeapon() );
            m_SBInfos.Add( new SBSwordWeapon() );*/
            if (IsTerMurVendor)
            {
                m_SBInfos.Add(new SBTerMurBlacksmith());
            }
            else
            {
                m_SBInfos.Add(new SBBlacksmith());

                if (IsTokunoVendor)
                {
                    m_SBInfos.Add(new SBSEArmor());
                    m_SBInfos.Add(new SBSEWeapons());
                }
            }
        }

        public override VendorShoeType ShoeType
        {
            get
            {
                return VendorShoeType.None;
            }
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            Item item = (Utility.RandomBool() ? null : new Server.Items.RingmailChest());

            if (item != null && !EquipItem(item))
            {
                item.Delete();
                item = null;
            }

            if (item == null)
                AddItem(new Server.Items.FullApron());

            AddItem(new Server.Items.Bascinet());
            AddItem(new Server.Items.SmithHammer());
        }

        #region Bulk Orders
        public override Item CreateBulkOrder(Mobile from, bool fromContextMenu)
        {
            if (CharConfig.CharBODPub74Enabled)
            {
                PlayerMobile pm = from as PlayerMobile;
                if ((pm != null && pm.NextSmithBulkOrder74 < DateTime.UtcNow && (fromContextMenu || 0.2 > Utility.RandomDouble())) || pm.AccessLevel > AccessLevel.Player)
                {
                    double theirSkill = pm.Skills[SkillName.Blacksmith].Base;

                    if (pm.NextSmithBulkOrder74 < DateTime.UtcNow.Subtract(TimeSpan.FromHours(12)))
                    {
                        pm.NextSmithBulkOrder74 = DateTime.UtcNow.Subtract(TimeSpan.FromHours(6));
                    }
                    else if (pm.NextSmithBulkOrder74 < DateTime.UtcNow.Subtract(TimeSpan.FromHours(6)))
                    {
                        pm.NextSmithBulkOrder74 = pm.NextSmithBulkOrder74.Add(TimeSpan.FromHours(6));
                    }
                    else if (pm.NextSmithBulkOrder74 < DateTime.UtcNow)
                    {
                        pm.NextSmithBulkOrder74 = DateTime.UtcNow.Add(TimeSpan.FromHours(6));
                    }

                    if (theirSkill >= 70.1 && ((theirSkill - 40.0) / 300.0) > Utility.RandomDouble())
                        return new LargeSmithBOD();

                    return SmallSmithBOD.CreateRandomFor(from);
                }
            }
            else
            {
                PlayerMobile pm = from as PlayerMobile;
                if (pm != null && pm.NextSmithBulkOrder == TimeSpan.Zero && (fromContextMenu || 0.2 > Utility.RandomDouble()))
                {
                    double theirSkill = pm.Skills[SkillName.Blacksmith].Base;

                    if (theirSkill >= 70.1)
                        pm.NextSmithBulkOrder = TimeSpan.FromHours(6.0);
                    else if (theirSkill >= 50.1)
                        pm.NextSmithBulkOrder = TimeSpan.FromHours(2.0);
                    else
                        pm.NextSmithBulkOrder = TimeSpan.FromHours(1.0);

                    if (theirSkill >= 70.1 && ((theirSkill - 40.0) / 300.0) > Utility.RandomDouble())
                        return new LargeSmithBOD();

                    return SmallSmithBOD.CreateRandomFor(from);
                }
            }

            return null;
        }

        public override bool IsValidBulkOrder(Item item)
        {
            return (item is SmallSmithBOD || item is LargeSmithBOD);
        }

        public override bool SupportsBulkOrders(Mobile from)
        {
            return (from is PlayerMobile && from.Skills[SkillName.Blacksmith].Base > 0);
        }

        public override TimeSpan GetNextBulkOrder(Mobile from)
        {
            if (from is PlayerMobile)
                return ((PlayerMobile)from).NextSmithBulkOrder;

            return TimeSpan.Zero;
        }

        public override DateTime GetNextBulkOrder74(Mobile from)
        {
            if (from is PlayerMobile)
                return ((PlayerMobile)from).NextSmithBulkOrder74;

            return DateTime.MinValue;
        }

        public override void OnSuccessfulBulkOrderReceive(Mobile from)
        {
            if (CharConfig.CharBODPub74Enabled)
            {
                if (Core.SE && from is PlayerMobile)
                {
                    PlayerMobile pm = from as PlayerMobile;
                    DateTime next = pm.NextSmithBulkOrder74.Subtract(TimeSpan.FromHours(6));
                    if (next < DateTime.UtcNow.Subtract(TimeSpan.FromHours(12)))
                    {
                        next = DateTime.UtcNow.Subtract(TimeSpan.FromHours(12));
                    }
                    pm.NextSmithBulkOrder74 = next;
                }
            }
            else
            {
                if (Core.SE && from is PlayerMobile)
                    ((PlayerMobile)from).NextSmithBulkOrder = TimeSpan.Zero;
            }
        }

        #endregion

        public Blacksmith(Serial serial)
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
}