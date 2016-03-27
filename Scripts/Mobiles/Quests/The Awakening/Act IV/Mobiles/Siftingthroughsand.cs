using Server.Items;
using Server.Network;
using System;

namespace Server.Mobiles
{
    public class SiftingThroughSand : Mobile
    {

        public virtual bool IsInvulnerable { get { return true; } }
        private DateTime m_Spoken;
        [Constructable]
        public SiftingThroughSand()
        {
            this.m_Spoken = DateTime.UtcNow;
            InitStats(91, 91, 91);

            Name = NameList.RandomName("male");
            Title = "the provisioner";
            InitOutfit();

            Body = 0x190;
            Hue = 1111;

            Blessed = true;
            CantWalk = true;

            Container pack = new Backpack();
            pack.Movable = false;
            AddItem(pack);
        }

        public SiftingThroughSand(Serial serial)
            : base(serial)
        {
        }

        public virtual void InitOutfit()
        {
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new ShortPants(Utility.RandomNeutralHue()));
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new Pickaxe());

            Utility.AssignRandomHair(this);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.Alive && m is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)m;

                int range = 5;

                if (range >= 0 && this.InRange(m, range) && !this.InRange(oldLocation, range) && DateTime.UtcNow >= this.m_Spoken + TimeSpan.FromSeconds(30))
                {
                    this.Say(1152492 + Utility.Random(5));
                    this.m_Spoken = DateTime.UtcNow;
                }
            }
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            Mobile m = from; PlayerMobile mobile = m as PlayerMobile;

            if (mobile != null)
            {
                // item to be dropped
                if (dropped is Gold)
                {
                    if (dropped.Amount < 1000)
                    {
                        PublicOverheadMessage(MessageType.Regular, 1153, 1152498);// Your payment seems to be a little light there. I need 1000 gold pieces! 
                        return false;
                    }
                    else if (dropped.Amount > 1000)
                    {
                        PublicOverheadMessage(MessageType.Regular, 1153, 1152499);// I appreciate the generous payment, but I need 1000 gold pieces, no more, no less.
                        return false;
                    }

                    dropped.Delete();
                    PublicOverheadMessage(MessageType.Regular, 1153, 1152500);//Many thanks for your patronage! 
                    return true;
                }
                else if (dropped is Item)
                {
                    PrivateOverheadMessage(MessageType.Regular, 1153, 1152497, mobile.NetState);//What is this? Some kind of joke? I deal in hard coin only!
                    return false;
                }
            }
            return false;
        }
    }
}
