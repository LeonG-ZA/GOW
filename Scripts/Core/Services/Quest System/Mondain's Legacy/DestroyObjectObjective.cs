using System;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class DestroyObjectObjective : BaseObjective
    {
        private Type m_Object;
        private string m_Name;
        private int m_Image;
        public Type   Obj   { get { return this.m_Object; } set { this.m_Object = value; } }
        public string Name  { get { return this.m_Name;   } set { this.m_Name = value; } }
        public int    Image { get { return this.m_Image;  } set { this.m_Image = value; } }

        public DestroyObjectObjective(Type obj, string name, int amount) : this(obj, name, amount, 0, 0)
        {
        }

        public DestroyObjectObjective(Type obj, string name, int amount, int image) : this(obj, name, amount, image, 0)
        {
        }

        public DestroyObjectObjective(Type obj, string name, int amount, int image, int seconds) : base(amount, seconds)
        {
            this.m_Object = obj;
            this.m_Name = name;
            this.m_Image = image;
        }

        public override bool Update(object obj)
        {
            if (obj is Item && !this.Completed)
            {
                Item object_todestroy = (Item)obj;

                if (this.IsObjective(object_todestroy))
                {
                    if (!this.Completed)
                        this.CurProgress += 1;
                    return true;
                }
            }
            return false;
        }

        public virtual bool IsObjective(Item item)
        {
            if (this.m_Object == null)
                return false;

            if (this.m_Object.IsAssignableFrom(item.GetType()))
                return true;

            return false;
        }

        public override Type Type()
        {
            return this.m_Object;
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
