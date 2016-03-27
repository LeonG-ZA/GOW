using System;
using Server.Targeting;

namespace Server.Items
{
    public abstract class BaseCookableFood : Item
    {
        private int m_CookingLevel;

        [CommandProperty(AccessLevel.GameMaster)]
        public int CookingLevel
        {
            get
            {
                return this.m_CookingLevel;
            }
            set
            {
                this.m_CookingLevel = value;
            }
        }

        public BaseCookableFood(int itemID, int cookingLevel)
            : base(itemID)
        {
            this.m_CookingLevel = cookingLevel;
        }

        public BaseCookableFood(Serial serial)
            : base(serial)
        {
        }

        public abstract BaseFood Cook();

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
            // Version 1
            writer.Write((int)this.m_CookingLevel);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 1:
                    {
                        this.m_CookingLevel = reader.ReadInt();

                        break;
                    }
            }
        }

        #if false
		public override void OnDoubleClick( Mobile from )
		{
			if ( !Movable )
				return;

			from.Target = new InternalTarget( this );
		}
        #endif

        public static bool IsHeatSource(object targeted)
        {
            int itemID;

            if (targeted is Item)
                itemID = ((Item)targeted).ItemID;
            else if (targeted is StaticTarget)
                itemID = ((StaticTarget)targeted).ItemID;
            else
                return false;

            if (itemID >= 0xDE3 && itemID <= 0xDE9)
                return true; // Campfire
            else if (itemID >= 0x461 && itemID <= 0x48E)
                return true; // Sandstone oven/fireplace
            else if (itemID >= 0x92B && itemID <= 0x96C)
                return true; // Stone oven/fireplace
            else if (itemID == 0xFAC)
                return true; // Firepit
            else if (itemID >= 0x184A && itemID <= 0x184C)
                return true; // Heating stand (left)
            else if (itemID >= 0x184E && itemID <= 0x1850)
                return true; // Heating stand (right)
            else if (itemID >= 0x398C && itemID <= 0x399F)
                return true; // Fire field

            return false;
        }

        private class CookableFoodTarget : Target
        {
            private readonly BaseCookableFood m_Item;

            public CookableFoodTarget(BaseCookableFood item)
                : base(1, false, TargetFlags.None)
            {
                this.m_Item = item;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (this.m_Item.Deleted)
                    return;

                if (BaseCookableFood.IsHeatSource(targeted))
                {
                    if (from.BeginAction(typeof(BaseCookableFood)))
                    {
                        from.PlaySound(0x225);

                        this.m_Item.Consume();

                        CookableFoodTimer t = new CookableFoodTimer(from, targeted as IPoint3D, from.Map, this.m_Item);
                        t.Start();
                    }
                    else
                    {
                        from.SendLocalizedMessage(500119); // You must wait to perform another action
                    }
                }
            }

            private class CookableFoodTimer : Timer
            {
                private readonly Mobile m_From;
                private readonly IPoint3D m_Point;
                private readonly Map m_Map;
                private readonly BaseCookableFood m_CookableFood;

                public CookableFoodTimer(Mobile from, IPoint3D p, Map map, BaseCookableFood cookableFood)
                    : base(TimeSpan.FromSeconds(5.0))
                {
                    this.m_From = from;
                    this.m_Point = p;
                    this.m_Map = map;
                    this.m_CookableFood = cookableFood;
                }

                protected override void OnTick()
                {
                    this.m_From.EndAction(typeof(BaseCookableFood));

                    if (this.m_From.Map != this.m_Map || (this.m_Point != null && this.m_From.GetDistanceToSqrt(this.m_Point) > 3))
                    {
                        this.m_From.SendLocalizedMessage(500686); // You burn the food to a crisp! It's ruined.
                        return;
                    }

                    if (this.m_From.CheckSkill(SkillName.Cooking, this.m_CookableFood.CookingLevel, 100))
                    {
                        BaseFood cookedFood = this.m_CookableFood.Cook();

                        if (this.m_From.AddToBackpack(cookedFood))
                            this.m_From.PlaySound(0x57);
                    }
                    else
                    {
                        this.m_From.SendLocalizedMessage(500686); // You burn the food to a crisp! It's ruined.
                    }
                }
            }
        }
    }
}