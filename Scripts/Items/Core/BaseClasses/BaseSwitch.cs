using System;
using Server.Network;

namespace Server.Items
{
    public class BaseSwitch : Item
    {
        private int I_TurnOn;
        private int I_TurnOff;
        private int I_LocMessageA;
        private int I_LocMessageB;
        private bool I_Used;
        private bool I_Working;
        [Constructable]
        public BaseSwitch(int TurnOff, int TurnOn, int LocMessageA, int LocMessageB, bool Working)
            : base(TurnOff)
        {
            this.Movable = false;
            this.I_TurnOn = TurnOn;
            this.I_TurnOff = TurnOff;
            this.I_LocMessageA = LocMessageA;
            this.I_LocMessageB = LocMessageB;
            this.I_Used = false;
            this.I_Working = Working;
        }

        [Constructable]
        public BaseSwitch(int TurnOff, int TurnOn)
            : base(TurnOff)
        {
            this.Movable = false;
            this.I_TurnOn = TurnOn;
            this.I_TurnOff = TurnOff;
            this.I_LocMessageA = 0;
            this.I_LocMessageB = 0;
            this.I_Used = false;
            this.I_Working = false;
        }

        public BaseSwitch(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile m)
        {
            if (!m.InRange(this, 2))
            {
                m.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }
            else
            {
                int MessageA = 0;
				
                if (this.I_LocMessageA == 0)
                    MessageA = 500357 + Utility.Random(5);
                else
                    MessageA = this.I_LocMessageA;
				
                int MessageB = 0;
				
                if (this.I_LocMessageB == 0)
                    MessageB = 500357 + Utility.Random(5);
                else
                    MessageB = this.I_LocMessageB;

                /*
                500357 - If this lever ever did anything, it doesn't do it anymore.
                500358 - The lever feels loose, and you realize it no longer controls anything.
                500359 - You flip the lever and think you hear something, but realize it was just your imagination.
                500360 - The lever flips without effort, doing nothing.
                */
					
                if (this.ItemID == this.I_TurnOff && this.I_Used == false)
                {
                    this.ItemID = this.I_TurnOn;
                    this.I_Used = true;
                    Effects.PlaySound(this.Location, this.Map, 0x3E8);
					
                    m.LocalOverheadMessage(MessageType.Regular, 0, MessageA); //Message received when it is turned on by first time.

                    //This call to another method to do something special, so you don't need
                    //to override OnDoubleClick and rewrite this section again.
                    if (this.I_Working == true)
                    {
                        this.DoSomethingSpecial(m);
                    }
					
                    //Refresh time of two minutes, equal to RaiseSwith
                    Timer.DelayCall(TimeSpan.FromMinutes(2.0), delegate()
                    {
                        this.ItemID = this.I_TurnOff;
                        this.I_Used = false;
                    });
                }
                else if (this.ItemID == this.I_TurnOff && this.I_Used == true)
                {
                    this.ItemID = this.I_TurnOn;
                    Effects.PlaySound(this.Location, this.Map, 0x3E8);
                    m.LocalOverheadMessage(MessageType.Regular, 0, MessageB); //Message received after click it again until the refresh.
                }
                else //TurnOn and I_Used true
                {
                    this.ItemID = this.I_TurnOff;
                    Effects.PlaySound(this.Location, this.Map, 0x3E8);
                    m.LocalOverheadMessage(MessageType.Regular, 0, MessageB); //Message received after click it again until the refresh.
                }
            }
        }

        public virtual void DoSomethingSpecial(Mobile from)
        {
            from.LocalOverheadMessage(MessageType.Regular, 0, 1116629); //It does Nothing!
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(this.I_TurnOn);
            writer.Write(this.I_TurnOff);
            writer.Write(this.I_LocMessageA);
            writer.Write(this.I_LocMessageB);
            writer.Write(this.I_Working);
            writer.Write(this.I_Used);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            this.I_TurnOn = reader.ReadInt();
            this.I_TurnOff = reader.ReadInt();
            this.I_LocMessageA = reader.ReadInt();
            this.I_LocMessageB = reader.ReadInt();
            this.I_Working = reader.ReadBool();
            this.I_Used = reader.ReadBool();
            this.I_Used = false;
            this.ItemID = this.I_TurnOff;
        }
    }
}