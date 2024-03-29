using System;
using Server.Engines.Plants;

namespace Server.Mobiles
{
    [CorpseName("a dryad's corpse")]
    public class Dryad : BaseCreature
    {
        public override bool InitialInnocent { get { return true; } }

        public override int GetDeathSound() { return 0x57A; }
        public override int GetAttackSound() { return 0x57B; }
        public override int GetIdleSound() { return 0x57C; }
        public override int GetAngerSound() { return 0x57D; }
        public override int GetHurtSound() { return 0x57E; }

        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }

        [Constructable]
        public Dryad()
            : base(AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4)
        {
            Name = "a dryad";
            Body = 266;

            SetStr(130, 150); // TODO: All Values are taken from Stratics and will change along with the info taken from stratics/OSI.
            SetDex(150, 170);
            SetInt(250, 275);

            SetHits(310, 320);

            SetDamage(11, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 40, 45);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.Meditation, 80, 90);
            SetSkill(SkillName.EvalInt, 70, 80);
            SetSkill(SkillName.Magery, 70, 80);
            SetSkill(SkillName.MagicResist, 110, 120);
            SetSkill(SkillName.Tactics, 70, 80);
            SetSkill(SkillName.Wrestling, 70, 80);

            Fame = 1250;
            Karma = 1250;

            PackSpellweavingScroll();

            if (Utility.RandomDouble() < .33)
            {
                PackItem(Seed.RandomPeculiarSeed(1));
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public override int Meat
        {
            get
            {
                return 1;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            AreaPeace();
            AreaUndress();
        }

        #region Area Peace
        private DateTime m_NextPeace;

        public void AreaPeace()
        {
            if (Combatant == null || Deleted || !Alive || m_NextPeace > DateTime.UtcNow || 0.1 < Utility.RandomDouble())
                return;

            TimeSpan duration = TimeSpan.FromSeconds(Utility.RandomMinMax(20, 80));

            foreach (Mobile m in GetMobilesInRange(RangePerception))
            {
                PlayerMobile p = m as PlayerMobile;

                if (IsValidTarget(p))
                {
                    p.PeacedUntil = DateTime.UtcNow + duration;
                    p.SendLocalizedMessage(1072065); // You gaze upon the dryad's beauty, and forget to continue battling!
                    p.FixedParticles(0x376A, 1, 20, 0x7F5, EffectLayer.Waist);
                    p.Combatant = null;
                }
            }

            m_NextPeace = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            PlaySound(0x1D3);
        }

        public bool IsValidTarget(PlayerMobile m)
        {
            if (m != null && m.PeacedUntil < DateTime.UtcNow && !m.Hidden && m.IsPlayer() && CanBeHarmful(m))
                return true;

            return false;
        }

        #endregion

        #region Undress
        private DateTime m_NextUndress;

        public void AreaUndress()
        {
            if (Combatant == null || Deleted || !Alive || m_NextUndress > DateTime.UtcNow || 0.005 < Utility.RandomDouble())
                return;

            foreach (Mobile m in GetMobilesInRange(RangePerception))
            {
                if (m != null && m.Player && !m.Female && !m.Hidden && m.IsPlayer() && CanBeHarmful(m))
                {
                    UndressItem(m, Layer.OuterTorso);
                    UndressItem(m, Layer.InnerTorso);
                    UndressItem(m, Layer.MiddleTorso);
                    UndressItem(m, Layer.Pants);
                    UndressItem(m, Layer.Shirt);

                    m.SendLocalizedMessage(1072197); // The dryad's beauty makes your blood race. Your clothing is too confining.
                }
            }

            m_NextUndress = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        public void UndressItem(Mobile m, Layer layer)
        {
            Item item = m.FindItemOnLayer(layer);

            if (item != null && item.Movable)
                m.PlaceInBackpack(item);
        }

        #endregion

        public Dryad(Serial serial)
            : base(serial)
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