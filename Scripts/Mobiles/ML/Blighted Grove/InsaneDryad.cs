using System;
using Server.Engines.Plants;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an insane dryad corpse")]
    public class InsaneDryad : BaseCreature
    {
        [Constructable]
        public InsaneDryad()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an insane dryad";
            Body = 266;
            BaseSoundID = 0x57B;
            Hue = 0x487;

            SetStr(132, 149);
            SetDex(152, 168);
            SetInt(251, 280);

            SetHits(304, 321);

            SetDamage(11, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 40, 45);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.Meditation, 80.0, 90.0);
            SetSkill(SkillName.EvalInt, 70.0, 80.0);
            SetSkill(SkillName.Magery, 70.0, 80.0);
            SetSkill(SkillName.Anatomy, 0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 70.0, 80.0);
            SetSkill(SkillName.Wrestling, 70.0, 80.0);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 28; // Don't know what it should be

            if (Core.ML && Utility.RandomDouble() < .60)
                this.PackItem(Seed.RandomPeculiarSeed(1));
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
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

            this.AreaPeace();
            this.AreaUndress();
        }

        #region Area Peace
        private DateTime m_NextPeace;

        public void AreaPeace()
        {
            if (this.Combatant == null || this.Deleted || !this.Alive || this.m_NextPeace > DateTime.UtcNow || 0.1 < Utility.RandomDouble())
                return;

            TimeSpan duration = TimeSpan.FromSeconds(Utility.RandomMinMax(20, 80));

            foreach (Mobile m in this.GetMobilesInRange(this.RangePerception))
            {
                PlayerMobile p = m as PlayerMobile;

                if (this.IsValidTarget(p))
                {
                    p.PeacedUntil = DateTime.UtcNow + duration;
                    p.SendLocalizedMessage(1072065); // You gaze upon the dryad's beauty, and forget to continue battling!
                    p.FixedParticles(0x376A, 1, 20, 0x7F5, EffectLayer.Waist);
                    p.Combatant = null;
                }
            }

            this.m_NextPeace = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            this.PlaySound(0x1D3);
        }

        public bool IsValidTarget(PlayerMobile m)
        {
            if (m != null && m.PeacedUntil < DateTime.UtcNow && !m.Hidden && m.IsPlayer() && this.CanBeHarmful(m))
                return true;

            return false;
        }

        #endregion

        #region Undress
        private DateTime m_NextUndress;

        public void AreaUndress()
        {
            if (this.Combatant == null || this.Deleted || !this.Alive || this.m_NextUndress > DateTime.UtcNow || 0.005 < Utility.RandomDouble())
                return;

            foreach (Mobile m in this.GetMobilesInRange(this.RangePerception))
            {
                if (m != null && m.Player && !m.Female && !m.Hidden && m.IsPlayer() && this.CanBeHarmful(m))
                {
                    this.UndressItem(m, Layer.OuterTorso);
                    this.UndressItem(m, Layer.InnerTorso);
                    this.UndressItem(m, Layer.MiddleTorso);
                    this.UndressItem(m, Layer.Pants);
                    this.UndressItem(m, Layer.Shirt);

                    m.SendLocalizedMessage(1072197); // The dryad's beauty makes your blood race. Your clothing is too confining.
                }
            }

            this.m_NextUndress = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        public void UndressItem(Mobile m, Layer layer)
        {
            Item item = m.FindItemOnLayer(layer);

            if (item != null && item.Movable)
                m.PlaceInBackpack(item);
        }

        #endregion
		
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);		
						
            if (Utility.RandomDouble() < 0.1)
                c.DropItem(new PetParrot());	
        }

        public InsaneDryad(Serial serial)
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