using System;

namespace Server.Mobiles
{
    [CorpseName("a Desert Scorpion corpse")]
    public class Desert_Scorpion : BaseCreature
    {
        [Constructable]
        public Desert_Scorpion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "a Desert Scorpion";
            Body = 717;

            SetStr(300, 310);
            SetDex(270, 280);
            SetInt(200, 250);

            SetHits(280, 330);

            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            SetResistance(ResistanceType.Physical, 25, 30);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 40, 45);

            SetSkill(SkillName.MagicResist, 50.1, 65.0);
            SetSkill(SkillName.Poisoning, 95.1, 105.0);
            SetSkill(SkillName.Tactics, 70.1, 90.0);
            SetSkill(SkillName.Wrestling, 50.1, 60.0);

            Fame = 3500;
            Karma = -3500;

            ControlSlots = 1;
        }

        public Desert_Scorpion(Serial serial)
            : base(serial)
        {
        }

        public override bool IsScaredOfScaryThings
        {
            get
            {
                return false;
            }
        }
        public override bool IsScaryToPets
        {
            get
            {
                return true;
            }
        }
        public override bool IsBondable
        {
            get
            {
                return false;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Meat;
            }
        }
        public override bool AutoDispel
        {
            get
            {
                return !Controlled;
            }
        }
        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
        public override bool DeleteOnRelease
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
                return !Core.AOS || Controlled;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager, 2);
        }

        public override int GetAngerSound()
        {
            return 541;
        }

        public override int GetIdleSound()
        {
            if (!Controlled)
                return 542;

            return base.GetIdleSound();
        }

        public override int GetDeathSound()
        {
            if (!Controlled)
                return 545;

            return base.GetDeathSound();
        }

        public override int GetAttackSound()
        {
            return 562;
        }

        public override int GetHurtSound()
        {
            if (Controlled)
                return 320;

            return base.GetHurtSound();
        }

        /*
        public override void OnGaveMeleeAttack( Mobile defender )
        {
        base.OnGaveMeleeAttack( defender );

        if ( !m_Stunning && 0.3 > Utility.RandomDouble() )
        {
        m_Stunning = true;

        defender.Animate( 21, 6, 1, true, false, 0 );
        PlaySound( 0xEE );
        defender.LocalOverheadMessage( MessageType.Regular, 0x3B2, false, "You have been stunned by a colossal blow!" );

        BaseWeapon weapon = Weapon as BaseWeapon;
        if ( weapon != null )
        weapon.OnHit( this, defender );

        if ( defender.Alive )
        {
        defender.Frozen = true;
        Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerStateCallback( Recover_Callback ), defender );
        }
        }
        }
        */
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            Mobile master = GetMaster();

            if (master != null && master.Player && master.Map == Map && master.InRange(Location, 20))
            {
                if (master.Mana >= amount)
                {
                    master.Mana -= amount;
                }
                else
                {
                    amount -= master.Mana;
                    master.Mana = 0;
                    master.Damage(amount);
                }
            }

            base.OnDamage(amount, from, willKill);
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
    }
}