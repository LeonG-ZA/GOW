using System;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TamingObjective : BaseObjective
    {
        private Type m_TameTarget;
        private string m_Name;
        private int m_Image;
        public Type   Obj   { get { return this.m_TameTarget; } set { this.m_TameTarget = value; } }
        public string Name  { get { return this.m_Name;   } set { this.m_Name = value; } }
        public int    Image { get { return this.m_Image;  } set { this.m_Image = value; } }

        public TamingObjective(Type obj, string name, int amount) : this(obj, name, amount, 0, 0)
        {
        }

        public TamingObjective(Type obj, string name, int amount, int image) : this(obj, name, amount, image, 0)
        {
        }

        public TamingObjective(Type obj, string name, int amount, int image, int seconds) : base(amount, seconds)
        {
            this.m_TameTarget = obj;
            this.m_Name = name;
            this.m_Image = image;
        }

        public override bool Update(object obj)
        {
            if (obj is Mobile)
            {
                Mobile mob = (Mobile)obj;

                if (this.IsObjective(mob))
                {
                    if (!this.Completed)
                        this.CurProgress += 1;
                    return true;
                }
            }
            return false;
        }

        public virtual bool IsObjective(Mobile mob)
        {
            if (this.m_TameTarget == null)
                return false;

            if (this.m_TameTarget.IsAssignableFrom(mob.GetType()))
                return true;
            
            return false;
        }

        public override Type Type()
        {
            return this.m_TameTarget;
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
