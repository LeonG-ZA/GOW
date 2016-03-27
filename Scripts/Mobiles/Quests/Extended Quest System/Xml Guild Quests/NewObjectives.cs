using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Engines.Quests
{
    public class GuildCraftObjective : BaseObjective
    {
        private Type m_Obtain;
        private string m_Name;
        private int m_Image;
        private bool m_Exceptional;
        private CraftResource m_Resource;

        public Type Obtain
        {
            get { return m_Obtain; }
            set { m_Obtain = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public int Image
        {
            get { return m_Image; }
            set { m_Image = value; }
        }

        public bool Exceptional
        {
            get { return m_Exceptional; }
            set { m_Exceptional = value; }
        }

        public CraftResource Resource
        {
            get { return m_Resource; }
            set { m_Resource = value; }
        }

        public GuildCraftObjective(Type obtain, string name, int amount)
            : this(obtain, name, amount, 0, 0, false, CraftResource.None)
        {
        }

        public GuildCraftObjective(Type obtain, string name, int amount, int image)
            : this(obtain, name, amount, image, 0, false, CraftResource.None)
        {
        }

        public GuildCraftObjective(Type obtain, string name, int amount, int image, bool exceptional, CraftResource resource)
            : this(obtain, name, amount, image, 0, exceptional, resource)
        {
        }

        public GuildCraftObjective(Type obtain, string name, int amount, int image, int seconds, bool exceptional, CraftResource resource)
            : base(amount, seconds)
        {
            m_Obtain = obtain;
            m_Name = name;
            m_Image = image;
            m_Exceptional = exceptional;
            m_Resource = resource;
        }

        public override bool Update(object obj)
        {
            if (obj is Item)
            {
                Item obtained = (Item)obj;

                if (IsObjective(obtained))
                {
                    if (Completed)
                    {
                        Quest.Owner.SendMessage("You do not need any more of that item for the order.");
                        return false;
                    }
                    else if (!obtained.QuestItem)
                    {
                        CurProgress += obtained.Amount;

                        obtained.QuestItem = true;
                        obtained.Delete();
                        //Quest.Owner.SendLocalizedMessage(1072353); // You set the item to Quest Item status
                        Quest.Owner.SendMessage("You add the item to your order.");
                    }

                    return true;
                }
            }

            return false;
        }

        public virtual bool IsObjective(Item item)
        {
            if (m_Obtain == null)
                return false;

            if (m_Obtain.IsAssignableFrom(item.GetType()))
            {
                if (m_Exceptional)
                {
                    if (item is BaseWeapon)
                    {
                        BaseWeapon weapon = (BaseWeapon)item;

                        if (weapon.Quality != WeaponQuality.Exceptional)
                            return false;
                    }
                    else if (item is BaseArmor)
                    {
                        BaseArmor armor = (BaseArmor)item;

                        if (armor.Quality != ArmorQuality.Exceptional)
                            return false;
                    }
                    else if (item is BaseClothing)
                    {
                        BaseClothing clothing = (BaseClothing)item;

                        if (clothing.Quality != ClothingQuality.Exceptional)
                            return false;
                    }
                }

                if (m_Resource != CraftResource.None)
                {
                    if (item is BaseWeapon)
                    {
                        BaseWeapon weapon = (BaseWeapon)item;

                        if (weapon.Resource != m_Resource)
                            return false;
                    }
                    else if (item is BaseArmor)
                    {
                        BaseArmor armor = (BaseArmor)item;

                        if (armor.Resource != m_Resource)
                            return false;
                    }
                    else if (item is BaseJewel)
                    {
                        BaseJewel jewel = (BaseJewel)item;

                        if (jewel.Resource != m_Resource)
                            return false;
                    }
                }
                return true;
            }

            return false;
        }

        public override Type Type()
        {
            return m_Obtain;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
