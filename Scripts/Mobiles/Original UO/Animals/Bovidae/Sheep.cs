using System;
using Server.Items;
using Server.Network;
using Server.Engines.Quests;

namespace Server.Mobiles
{
    [CorpseName("a sheep corpse")]
    public class Sheep : BaseCreature, ICarvable, IScissorable
    {
        private DateTime m_NextWoolTime;
        [Constructable]
        public Sheep()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a sheep";
            Body = 0xCF;
            BaseSoundID = 0xD6;

            SetStr(19);
            SetDex(25);
            SetInt(5);

            SetHits(12);
            SetMana(0);

            SetDamage(1, 2);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);

            SetSkill(SkillName.MagicResist, 5.0);
            SetSkill(SkillName.Tactics, 6.0);
            SetSkill(SkillName.Wrestling, 5.0);

            Fame = 300;
            Karma = 0;

            VirtualArmor = 6;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 11.1;
        }

        public Sheep(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime NextWoolTime
        {
            get
            {
                return m_NextWoolTime;
            }
            set
            {
                m_NextWoolTime = value;
                Body = (DateTime.UtcNow >= m_NextWoolTime) ? 0xCF : 0xDF;
            }
        }
        public override int Meat
        {
            get
            {
                return 3;
            }
        }
        public override MeatType MeatType
        {
            get
            {
                return MeatType.LambLeg;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.FruitsAndVegies | FoodType.GrainsAndHay;
            }
        }
        public override int Wool
        {
            get
            {
                return (Body == 0xCF ? 3 : 0);
            }
        }
        public void Carve(Mobile from, Item item)
        {
            if (DateTime.UtcNow < m_NextWoolTime)
            {
                // This sheep is not yet ready to be shorn.
                PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500449, from.NetState);
                return;
            }
            else if (Controlled && ControlMaster != from)
            {
                // The sheep nimbly escapes your attempts to shear his wool.
                PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500453, from.NetState);
            }
            else
            {
                Item wool = new Wool(Map == Map.Felucca ? 2 : 1);

                if (!from.AddToBackpack(wool))
                {
                    from.SendLocalizedMessage(500451); // You would not be able to place the gathered wool in your backpack!
                    wool.Delete();
                }
                else
                {
                    from.SendLocalizedMessage(500452); // You place the gathered wool into your backpack.

                    if (from is PlayerMobile && QuestHelper.HasQuest<ShearingKnowledgeQuest>(from as PlayerMobile))
                    {
                        from.SendLocalizedMessage(1113241); // You shear some fresh, Britannian wool from the sheep.
                        from.AddToBackpack(new BritannianWool(Map == Map.Felucca ? 2 : 1));
                    }

                    NextWoolTime = DateTime.UtcNow + TimeSpan.FromHours(3.0); // TODO: Proper time delay
                }
            }
            //from.AddToBackpack(new Wool(Map == Map.Felucca ? 2 : 1));
        }

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if(Core.ML)
            {
               return false;
            }

            if (DateTime.UtcNow < m_NextWoolTime)
            {
                // This sheep is not yet ready to be shorn.
                PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500449, from.NetState);
                return false;
            }

            from.SendLocalizedMessage(500452); // You place the gathered wool into your backpack.
            from.AddToBackpack(new Wool(Map == Map.Felucca ? 2 : 1));

            NextWoolTime = DateTime.UtcNow + TimeSpan.FromHours(3.0); // TODO: Proper time delay

            return true;
        }

        public override void OnThink()
        {
            base.OnThink();
            Body = (DateTime.UtcNow >= m_NextWoolTime) ? 0xCF : 0xDF;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);

            writer.WriteDeltaTime(m_NextWoolTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 1:
                    {
                        NextWoolTime = reader.ReadDeltaTime();
                        break;
                    }
            }
        }
    }
}