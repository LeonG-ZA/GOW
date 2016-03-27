using System;
using System.Collections;
using Server.Engines.ChampionSpawns;
using Server.Items;
using Server.Commands;
using System.Collections.Generic;
using Server.Targeting;
using Server.Spells;
using Server.Engines.Doom;

namespace Server.Mobiles
{
    public class CoraSorceress : BaseCreature
    {
		private Timer m_Timer;
		
        private DateTime m_NextDiscordTime;
        [Constructable]
        public CoraSorceress() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "Cora the Sorceress";
            this.Body = 0x191;
			this.HairItemID = 8262;
			this.HairHue = 52;
			this.Hue = 33810;

            this.SetStr(500, 600);
            this.SetDex(130, 140);
            this.SetInt(1000, 1200);

            this.SetHits(50000);
            this.SetStam(130, 140);
            this.SetMana(4500, 5500);

            this.SetDamage(17, 21);

            this.SetDamageType(ResistanceType.Physical, 20);
            this.SetDamageType(ResistanceType.Fire, 20);
            this.SetDamageType(ResistanceType.Cold, 20);
            this.SetDamageType(ResistanceType.Energy, 20);
            this.SetDamageType(ResistanceType.Poison, 20);

            this.SetResistance(ResistanceType.Physical, 20);
            this.SetResistance(ResistanceType.Fire, 20);
            this.SetResistance(ResistanceType.Cold, 20);
            this.SetResistance(ResistanceType.Poison, 20);
            this.SetResistance(ResistanceType.Energy, 20);

            this.SetSkill(SkillName.EvalInt, 90, 120.0);
            this.SetSkill(SkillName.Magery, 90, 120.0);
            this.SetSkill(SkillName.Meditation, 100, 120.0);
            this.SetSkill(SkillName.Necromancy, 120.0);
            this.SetSkill(SkillName.SpiritSpeak, 120.0);
            this.SetSkill(SkillName.MagicResist, 120, 140.0);
            this.SetSkill(SkillName.Tactics, 90, 120);
            this.SetSkill(SkillName.Wrestling, 100, 120);

            this.Fame = 22500;
            this.Karma = -22500;

            this.VirtualArmor = 80;
			
			StuddedBustierArms chest = new StuddedBustierArms();
			chest.Movable = false;
			chest.Hue = 1913;
			
			ChainLegs skirt = new ChainLegs();
			skirt.Movable = false;
			skirt.Hue = 1913;
			
			LeatherGloves gloves = new LeatherGloves();
			gloves.Movable = false;
			gloves.Hue = 2022;
			
			ThighBoots tigh = new ThighBoots();
			tigh.Movable = false;
			tigh.Hue = 2022;
			
			WildStaff staff = new WildStaff();
			staff.Movable = false;
			staff.Hue = 1927;
			
			AddItem( chest );
			AddItem( skirt );
			AddItem( gloves );
			AddItem( tigh );
			AddItem( staff );
        }	
		
		public class EarthquakeTimer : Timer
        {
            public Mobile m;
            public int inc;
            public int ehue;
            public int fstart;
            public int fdir;
            public EarthquakeTimer(DateTime time, Mobile from, int hue, int start, int dir)
                : base(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1))
            {
                this.Priority = TimerPriority.FiftyMS;
                this.m = from;               
                this.fstart = start;
                this.fdir = dir;
                this.inc = start;
            }

            protected override void OnTick()
            {				
                this.inc = this.inc + this.fdir;				

                Effects.SendLocationEffect(new Point3D(this.m.X + this.inc, this.m.Y, this.m.Z), this.m.Map, 0x1B07, 17, 0, 0);
                Effects.SendLocationEffect(new Point3D(this.m.X - this.inc, this.m.Y, this.m.Z), this.m.Map, 0x1B07, 17, 0, 0);
                Effects.SendLocationEffect(new Point3D(this.m.X, this.m.Y + this.inc, this.m.Z), this.m.Map, 0x1B08, 17, 0, 0);
                Effects.SendLocationEffect(new Point3D(this.m.X, this.m.Y - this.inc, this.m.Z), this.m.Map, 0x1B08, 17, 0, 0);
                Effects.SendLocationEffect(new Point3D(this.m.X + this.inc, this.m.Y - this.inc, this.m.Z), this.m.Map, 0x1B07, 17, 0, 0);
                Effects.SendLocationEffect(new Point3D(this.m.X - this.inc, this.m.Y + this.inc, this.m.Z), this.m.Map, 0x1B07, 17, 0, 0);

                if ((this.fdir == 1 && this.inc >= (this.fstart + 5)) || (this.fdir == -1 && this.inc < 0))
                {
                    if (this.fdir == -1)
                    {
                        this.m.Hidden = !this.m.Hidden;
                    }
                    this.Stop();
                }
            }
        }
		
		public void Effect()
        { 		
			this.m_Timer = new EarthquakeTimer(DateTime.UtcNow, this, 0, 0, 1);
			this.m_Timer.Start();
        }
		
		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if (m_Timer == null)
			{
				Effects.SendLocationEffect(new Point3D(this.X + 1, this.Y, this.Z), m.Map, 0x1B07, 17, 0, 0);
				Effects.SendLocationEffect(new Point3D(this.X - 1, this.Y, this.Z), m.Map, 0x1B07, 17, 0, 0);
				Effects.SendLocationEffect(new Point3D(this.X, this.Y + 1, this.Z), m.Map, 0x1B08, 17, 0, 0);
				Effects.SendLocationEffect(new Point3D(this.X, this.Y - 1, this.Z), m.Map, 0x1B08, 17, 0, 0);
				Effects.SendLocationEffect(new Point3D(this.X, this.Y, this.Z), this.Map, 0x1B07, 17, 0, 0);
				m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( 20.0 ), TimeSpan.FromSeconds( 20.0 ), new TimerCallback( Effect ) );
			}
		}	
		
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		
        public CoraSorceress(Serial serial)
            : base(serial)
        {
        }

        public virtual bool CanDiscord
        {
            get
            {
                return true;
            }
        }
        public virtual int DiscordDuration
        {
            get
            {
                return 20;
            }
        }
        public virtual int DiscordMinDelay
        {
            get
            {
                return 5;
            }
        }
        public virtual int DiscordMaxDelay
        {
            get
            {
                return 22;
            }
        }
        public virtual double DiscordModifier
        {
            get
            {
                return 0.28;
            }
        }
        public virtual int PerceptionRange
        {
            get
            {
                return 8;
            }
        }
             
        public override bool AlwaysMurderer
        {
            get
            {
                return true;
            }
        }
        public override bool BardImmune
        {
            get
            {
                return !Core.SE;
            }
        }
        public override bool Unprovokable
        {
            get
            {
                return Core.SE;
            }
        }
        public override bool Uncalmable
        {
            get
            {
                return Core.SE;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override bool ShowFameTitle
        {
            get
            {
                return false;
            }
        }
        public override bool ClickTitle
        {
            get
            {
                return false;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.UltraRich, 3);
            this.AddLoot(LootPack.Meager);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (!Summoned && !NoKillAwards)
                DoomArtifactGiver.CheckArtifactGiving(this);
        }

        public void ChangeCombatant()
        {
            this.ForceReacquire();
            this.BeginFlee(TimeSpan.FromSeconds(2.5));
        }

        public override int GetIdleSound()
        {
            return 0x622;
        }

        public override int GetAngerSound()
        {
            return 0x61F;
        }

        public override int GetDeathSound()
        {
            return 0x620;
        }

        public override int GetHurtSound()
        {
            return 0x621;
        }

        public override void OnThink()
        {
            if (this.CanDiscord && this.m_NextDiscordTime <= DateTime.UtcNow)
            {
                Mobile target = this.Combatant;

                if (target != null && target.InRange(this, this.PerceptionRange) && this.CanBeHarmful(target))
                    this.Discord(target);
            }
        }

        public void Discord(Mobile target)
        {
            if (Utility.RandomDouble() < 0.9)
            {
                target.AddSkillMod(new TimedSkillMod(SkillName.Magery, true, this.Combatant.Skills.Magery.Base * this.DiscordModifier * -1, TimeSpan.FromSeconds(this.DiscordDuration)));
                target.AddSkillMod(new TimedSkillMod(SkillName.Necromancy, true, this.Combatant.Skills.Necromancy.Base * this.DiscordModifier * -1, TimeSpan.FromSeconds(this.DiscordDuration)));
                target.AddSkillMod(new TimedSkillMod(SkillName.Tactics, true, this.Combatant.Skills.Tactics.Base * this.DiscordModifier * -1, TimeSpan.FromSeconds(this.DiscordDuration)));
                target.AddSkillMod(new TimedSkillMod(SkillName.Swords, true, this.Combatant.Skills.Swords.Base * this.DiscordModifier * -1, TimeSpan.FromSeconds(this.DiscordDuration)));
                target.AddSkillMod(new TimedSkillMod(SkillName.Meditation, true, this.Combatant.Skills.Meditation.Base * this.DiscordModifier * -1, TimeSpan.FromSeconds(this.DiscordDuration)));
                target.AddSkillMod(new TimedSkillMod(SkillName.Focus, true, this.Combatant.Skills.Focus.Base * this.DiscordModifier * -1, TimeSpan.FromSeconds(this.DiscordDuration)));
                target.AddSkillMod(new TimedSkillMod(SkillName.Chivalry, true, this.Combatant.Skills.Chivalry.Base * this.DiscordModifier * -1, TimeSpan.FromSeconds(this.DiscordDuration)));
                target.AddSkillMod(new TimedSkillMod(SkillName.Wrestling, true, this.Combatant.Skills.Wrestling.Base * this.DiscordModifier * -1, TimeSpan.FromSeconds(this.DiscordDuration)));
                target.AddSkillMod(new TimedSkillMod(SkillName.Spellweaving, true, this.Combatant.Skills.Spellweaving.Base * this.DiscordModifier * -1, TimeSpan.FromSeconds(this.DiscordDuration)));

                Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), (int)this.DiscordDuration, new TimerStateCallback(Animate), target);

                target.SendMessage("The Cora touch weakens all of your fighting skills!");
                target.PlaySound(0x458);////
            }
            else
            {
                target.SendMessage("The Cora barely misses touching you, saving you from harm!"); 
                target.PlaySound(0x458);/////
            }

            this.m_NextDiscordTime = DateTime.UtcNow + TimeSpan.FromSeconds(this.DiscordMinDelay + Utility.RandomDouble() * this.DiscordMaxDelay);
        }        

        public override void AlterDamageScalarFrom(Mobile caster, ref double scalar)
        {			
			if (0.3 >= Utility.RandomDouble())
                new ManaDrainArea( this ).Cast();
        }
		
		private static readonly Dictionary<Mobile, Timer> m_Table = new Dictionary<Mobile, Timer>();
		private static readonly Dictionary<Mobile, Timer> m_Table2 = new Dictionary<Mobile, Timer>();
		
		
		private class ManaDrainArea : Spell
		{
			private static SpellInfo m_Info = new SpellInfo( "Mana Drain Area", "Ort Rel Ylem", 230 );
			
			private Mobile m_Mobile;

			 public ManaDrainArea(Mobile caster)
            : base(caster, null, m_Info)
			{
				m_Mobile = caster;		
			}
			
			public override TimeSpan CastDelayBase
			{
				get
				{
					return TimeSpan.FromSeconds( 5.0 );
				}
			}
			
			public override TimeSpan GetCastRecovery()
			{
				return TimeSpan.Zero;
			}

			public override double CastDelayFastScalar { get { return 0; } }
			
			public override int GetMana()
			{
				return 0;
			}

			public override bool ConsumeReagents()
			{
				return true;
			}

			public override bool CheckFizzle()
			{
				return true;
			}	
			
			public override void OnCast()
			{
				List<Mobile> targets = new List<Mobile>();

				foreach (Mobile m in Map.AllMaps[m_Mobile.Map.MapID].GetMobilesInRange(m_Mobile.Location, 10))
				{		
					if (!m_Table2.ContainsKey(m) && m != m_Mobile)					
						targets.Add(m);				
				}	

				for (int i = 0; i < targets.Count; i++)
				{
					Mobile m = (Mobile)targets[i];
					
					SpellHelper.Turn(m_Mobile, m);				

					if (m.Spell != null)
						m.Spell.OnCasterHurt();

					m.Paralyzed = false;
					m.RevealingAction();	
						
					int toDrain = 40 + (int)(GetDamageSkill(m_Mobile) - GetResistSkill(m));

					if (toDrain < 0)
						toDrain = 0;
					else if (toDrain > m.Mana)
						toDrain = m.Mana;

					if (m_Table2.ContainsKey(m))
						toDrain = 0;

					m.FixedParticles(0x3789, 10, 25, 5032, EffectLayer.Head);
					m.PlaySound(0x1F8);

					if (toDrain > 0)
					{
						m.Mana -= toDrain;

						m_Table2[m] = Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(AosDelay_Callback), new object[] { m, toDrain });
					}
					
					this.HarmfulSpell(m);
				}

				FinishSequence();
			}
			
			private void AosDelay_Callback(object state)
			{
				object[] states = (object[])state;

				Mobile m = (Mobile)states[0];
				int mana = (int)states[1];

				if (m.Alive && !m.IsDeadBondedPet)
				{
					m.Mana += mana;

					m.FixedEffect(0x3779, 10, 25);
					m.PlaySound(0x28E);
				}

				m_Table2.Remove(m);
			}
		}
		
		public virtual double GetDamageSkill(Mobile m)
		{
			return m.Skills[SkillName.EvalInt].Value;
		}

		public virtual double GetResistSkill(Mobile m)
		{
			return m.Skills[SkillName.MagicResist].Value;
		}
		
		private void AosDelay_Callback(object state)
        {
            object[] states = (object[])state;

            Mobile m = (Mobile)states[0];
            int mana = (int)states[1];

            if (m.Alive && !m.IsDeadBondedPet)
            {
                m.Mana += mana;

                m.FixedEffect(0x3779, 10, 25);
                m.PlaySound(0x28E);
            }

            m_Table.Remove(m);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.8 >= Utility.RandomDouble())
                this.Lightning();
			
			if (0.3 >= Utility.RandomDouble())
                 new ManaDrainArea( this ).Cast();
        }

        public void Lightning()
        {
            Map map = this.Map;

            if (map == null)
                return;

            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(15))
            {
                if (m == this || !this.CanBeHarmful(m))
                    continue;

                if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
                    targets.Add(m);
                else if (m.Player)
                    targets.Add(m);
            }

            this.PlaySound(0x2A);

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                double damage = m.Hits * 0.6;

                if (damage < 100.0)
                    damage = 100.0;
                else if (damage > 200.0)
                    damage = 200.0;

                this.DoHarmful(m);

                ItemAttributes.Damage(m, this, (int)damage, 0, 0, 100, 0, 0);

                if (m.Alive && m.Body.IsHuman && !m.Mounted)
                    m.Animate(20, 7, 1, true, false, 0); // take hit
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (0.3 >= Utility.RandomDouble())
                 new ManaDrainArea( this ).Cast();
        }

        public override void OnDamagedBySpell(Mobile attacker)
        {
            if (this.Map != null && attacker != this && 0.05 > Utility.RandomDouble())
            {
                this.Combatant = attacker;
                this.Map = attacker.Map;
                this.Location = attacker.Location;

                switch (Utility.Random(4))
                {
                    case 0:
                        attacker.Location = new Point3D(6974, 994,-15);
                        break;
                    case 1:
                        attacker.Location = new Point3D(6977, 1025, -15);
                        break;
                    case 2:
                        attacker.Location = new Point3D(7020, 1027, -15);
                        break;
                    case 3:
                        attacker.Location = new Point3D(6999, 978, -15);
                        break;
                }
                attacker.SendMessage("You are teleported away, the Cora is Taunting you!");
                ItemAttributes.Damage(attacker, Utility.RandomMinMax(50, 65), 0, 100, 0, 0, 0);
                attacker.MoveToWorld(attacker.Location, attacker.Map);
                attacker.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                attacker.PlaySound(0x1FE);
            }

            base.OnDamagedBySpell(attacker);

            this.DoCounter(attacker);
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

        private void Animate(object state)
        {
            if (state is Mobile)
            {
                Mobile mob = (Mobile)state;

                mob.FixedEffect(0x376A, 1, 32);
            }
        }

        private void DoCounter(Mobile attacker)
        {
            if (this.Map == null)
                return;

            if (attacker is BaseCreature && ((BaseCreature)attacker).BardProvoked)
                return;

            if (0.20 > Utility.RandomDouble())
            {
                Mobile target = null;

                if (attacker is BaseCreature)
                {
                    Mobile m = ((BaseCreature)attacker).GetMaster();

                    if (m != null)
                        target = m;
                }

                if (target == null || !target.InRange(this, 15))
                    target = attacker;

                this.Animate(10, 4, 1, true, false, 0);

                ArrayList targets = new ArrayList();

                foreach (Mobile m in target.GetMobilesInRange(8))
                {
                    if (m == this || !this.CanBeHarmful(m))
                        continue;

                    if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
                        targets.Add(m);
                    else if (m.Player && m.Alive)
                        targets.Add(m);
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    this.DoHarmful(m);

                    ItemAttributes.Damage(m, this, Utility.RandomMinMax(35, 45), true, 0, 0, 100, 0, 0);

                    m.FixedParticles(0x36E4, 1, 10, 0x1F78, 0x47F, 0, (EffectLayer)255);
                    m.ApplyPoison(this, Poison.Lethal);
                }
            }
        }
    }
}