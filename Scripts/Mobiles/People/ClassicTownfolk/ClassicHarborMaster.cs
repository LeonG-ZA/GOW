using System;
using Server.Items;
using Server;
using Server.Misc;
using Server.Targeting;
using Server.Multis;
using Server.Network;
using System.Collections;
using Server.ContextMenus;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class ClassicHarborMaster : BaseCreature
    {
        private NewBaseBoat m_Boat;
        [CommandProperty(AccessLevel.GameMaster)]
        public NewBaseBoat Boat { get { return m_Boat; } }

        [Constructable]
        public ClassicHarborMaster()
            : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            InitStats(31, 41, 51);

            SetSkill(SkillName.Mining, 36, 68);

            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();
            Blessed = true;

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Title = "the Harbor Mistress";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Title = "the Harbor Master";
            }
            AddItem(new Shirt(Utility.RandomDyedHue()));
            AddItem(new Boots());
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new QuarterStaff());

            Utility.AssignRandomHair(this);

            Container pack = new Backpack();
            pack.DropItem(new Gold(250, 300));
            pack.Movable = false;
            AddItem(pack);
        }

        public ClassicHarborMaster(Serial serial)
            : base(serial)
        {
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(Location, 4))
                return true;

            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && e.HasKeyword(0x000A) && e.Mobile.InRange(Location, 4))
            {
                e.Handled = true;
                Say(true, "I am a the dockmaster.  I dock ships for a fee.");
            }

            if (!e.Handled && e.HasKeyword(0x0009) && e.Mobile.InRange(Location, 4))
            {

                e.Handled = true;
                Say(true, "If you already gave me a ship, just use your claim ticket as you would any other deed.");
            }

            if (!e.Handled && e.HasKeyword(0x000B) && e.Mobile.InRange(Location, 4))
            {
                e.Handled = true;
                Mobile m = e.Mobile;
                Say(true, "I charge 25 gold for docking thy ship.  What ship do you want to dock?");
                m.Target = new InternalTarget(this);
            }
        }

        private class InternalTarget : Target
        {
            private Mobile m_Master;

            public InternalTarget(Mobile master)
                : base(20, false, TargetFlags.None)
            {
                m_Master = master;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is TillerMan)
                {
                    if (!m_Master.InRange(((TillerMan)target).Location, 20))
                    {
                        from.SendMessage("That is too far away.");
                        return;
                    }

                    ((TillerMan)target).Boat.BeginClassicDryDock(from, m_Master);
                }
                else
                {
                    from.SendMessage("That is not a ship!");
                }
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);//version

            // version 1 : m_Boat
            writer.Write((NewBaseBoat)m_Boat);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_Boat = reader.ReadItem() as NewBaseBoat;
                        break;
                    }
            }	
        }
    }
}

