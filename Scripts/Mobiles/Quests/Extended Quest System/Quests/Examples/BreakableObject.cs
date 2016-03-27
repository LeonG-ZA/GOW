using System;
using Server;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Items
{
    //This is a base clase for running any object you want a player to destroy by double clicking the object.
    //Use the QuestObjective [DestroyObjectObjective]
    public class BreakableObject : Item
    {
        public virtual int BreakSound { get { return 0x11F; } }

        public BreakableObject(int itemId) : base ( itemId )
        {
            Movable = false;
        }

        public override void OnDoubleClick(Mobile from) 
        {
            //If the player has the quest, and clicked this object that matches the quest.
            //Destroy this object.
            if (ExtendedQuestHelper.CheckDestroyObject((PlayerMobile)from, this))
            {
                from.PlaySound(BreakSound);
                from.SendMessage("You destroy " + this.Name + "!");
                this.Delete();
            }
            else
            {
                from.SendMessage("nothing happens...");
            }
        }

        public BreakableObject(Serial serial) : base(serial)
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
