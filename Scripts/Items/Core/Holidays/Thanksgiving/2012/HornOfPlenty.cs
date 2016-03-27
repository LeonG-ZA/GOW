using Server.Mobiles;
using Server.Multis;
using System;

namespace Server.Items
{
    [FlipableAttribute(0x4BD9, 0x4BDA)]
    public class HornOfPlenty : Item
    {
        int m_charges;

        [Constructable]
        public HornOfPlenty()
            : base(0x4BD9)
        {
            m_charges = 10;
            Name = "Horn of Plenty";
        }

        public HornOfPlenty(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber { get { return 1153503; } }//Horn of Plenty

        public override void OnDoubleClick(Mobile from)
        {
            if (m_charges > 0)
            {
                double random = Utility.RandomDouble();
                if (0.09 >= random) //Roasting Pig On A Spit
                {
                    from.AddToBackpack(new RoastingPigDeed("1/1/2001"));
                    m_charges -= 1;
                } 
                else if (0.25 >= random) //turkey
                {
                    Map map = from.Map;

                    if (map == null)
                        return;
                    
                    BaseCreature turkey;
                    int msg;

                    random = Utility.RandomDouble();
                    if (0.1 >= random)
                    {
                        turkey = new MisterGobbles();
                        msg = 1153511;
                    }
                    else
                    {
                        turkey = new Turkey();
                        msg = 1153512;
                    }
                    bool validLocation = false;
                    Point3D loc = from.Location;

                    if (BaseHouse.FindHouseAt(loc, map, 0) == null)
                    {
                        for (int j = 0; !validLocation && j < 10; ++j)
                        {
                            int x = this.X + Utility.Random(3) - 1;
                            int y = this.Y + Utility.Random(3) - 1;
                            int z = map.GetAverageZ(x, y);

                            if (validLocation = map.CanFit(x, y, this.Z, 3, false, false))
                                loc = new Point3D(x, y, this.Z);
                            else if (validLocation = map.CanFit(x, y, z, 3, false, false))
                                loc = new Point3D(x, y, z);
                        }
                    }

                    turkey.MoveToWorld(loc, map);
                    from.LocalOverheadMessage(Network.MessageType.Emote, 53, msg);
                }
                else if (0.33 >= random) //turkey
                {
                    from.Sleep(TimeSpan.FromSeconds(10));
                    from.LocalOverheadMessage(Network.MessageType.Emote, 53, 1153513); //* ZzzzZzzzZzzzZ *
                }
                else
                {
                    m_charges -= 1;
                    switch (Utility.Random(4))
                    {
                        case 0: from.AddToBackpack(new SweetPotatoPie()); break;
                        case 1: from.AddToBackpack(new MashedSweetPotatoes()); break;
                        case 2: from.AddToBackpack(new BasketOfRolls()); break;
                        case 3: from.AddToBackpack(new TurkeyPlatter()); break;
                    }
                }
                this.InvalidateProperties();
            }
            else
            {
                from.SendLocalizedMessage(502412); //There are no charges left on that item.
            }
            if (m_charges < 10)
                Timer.DelayCall(TimeSpan.FromDays(1), new TimerCallback(Respawn));
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060741, this.m_charges.ToString()); // charges: ~1_val~
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            this.LabelTo(from, 1060741, this.m_charges.ToString()); // charges: ~1_val~
        }
        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_charges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_charges = reader.ReadInt();
        }

        private void Respawn()
        {
            if ((DateTime.UtcNow.Month == 11) && (this.m_charges < 10))
            {
                this.m_charges += 1;
                this.InvalidateProperties();
            }
            if (m_charges < 10)
                Timer.DelayCall(TimeSpan.FromDays(1), new TimerCallback(Respawn));
        }
    }
}