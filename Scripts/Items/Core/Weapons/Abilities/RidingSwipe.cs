using System;
using Server.Items;
using Server.Mobiles;
using Server.Spells.Ninjitsu;

namespace Server.Items
{
	/// <summary>
	/// If you are on foot, dismounts your opponent and damage the ethereal's rider or the
	/// living mount(which must be healed before ridden again). If you are mounted, damages
	/// and stuns the mounted opponent.
	/// </summary>
	public class RidingSwipe : WeaponAbility
	{
		public RidingSwipe()
		{
		}

        public override int BaseMana { get { return !Core.SA ? 30 : 25; } }

		public override bool RequiresSE { get { return true; } }

		public override bool CheckSkills( Mobile from )
		{
			if( GetSkill( from, SkillName.Bushido ) < 50.0 )
			{
				from.SendLocalizedMessage( 1070768, "50" ); // You need ~1_SKILL_REQUIREMENT~ Bushido skill to perform that attack!
				return false;
			}

			return base.CheckSkills( from );
		}

		public override void OnHit( Mobile attacker, Mobile defender, int damage )
		{
            if (!defender.Mounted && !AnimalForm.UnderTransformation(defender) && !defender.Flying)
			{
                attacker.SendLocalizedMessage(1060848); // This attack only works on mounted or flying targets
				ClearCurrentAbility( attacker );
				return;
			}

			if( !Validate( attacker ) || !CheckMana( attacker, true ) )
				return;

			ClearCurrentAbility( attacker );

            if (!attacker.Mounted)
            {
                defender.FixedParticles(0x376A, 9, 32, 0x13AF, 0, 0, EffectLayer.RightFoot);

                Mobile mount = defender.Mount as Mobile;

                if (mount != null)
                {
                    BaseMount.Dismount(defender);
                }
                else if (defender.Flying)
                {
                    defender.Flying = false;
                }
                else
                {
                    AnimalForm.RemoveContext(defender, true);
                }

                if (mount != null)	//Ethy mounts don't take damage
                {
                    int amount = 10 + (int)(10.0 * (attacker.Skills[SkillName.Bushido].Value - 50.0) / 70.0 + 5);

                    ItemAttributes.Damage(mount, null, amount, 100, 0, 0, 0, 0);	//The mount just takes damage, there's no flagging as if it was attacking the mount directly

                    //TODO: Mount prevention until mount healed
                }
            }
            else
            {
                int amount = 10 + (int)(10.0 * (attacker.Skills[SkillName.Bushido].Value - 50.0) / 70.0 + 5);

                ItemAttributes.Damage(defender, attacker, amount, 100, 0, 0, 0, 0);

                ParalyzingBlow pb = new ParalyzingBlow();
                pb.IsBladeweaveAttack = true;
                pb.OnHit(attacker, defender, damage);
                /*
                int amount = 10 + (int)(10.0 * (attacker.Skills[SkillName.Bushido].Value - 50.0) / 70.0 + 5);

                ItemAttributes.Damage(defender, attacker, amount, 100, 0, 0, 0, 0);

                if (Server.Items.ParalyzingBlow.IsInmune(defender))	//Does it still do damage?
                {
                    attacker.SendLocalizedMessage(1070804); // Your target resists paralysis.
                    defender.SendLocalizedMessage(1070813); // You resist paralysis.
                }
                else
                {
                    defender.Paralyze(TimeSpan.FromSeconds(3.0));
                    Server.Items.ParalyzingBlow.BeginInmunity(defender, Server.Items.ParalyzingBlow.FreezeDelayDuration);
                }
                 */
            }
		}
	}
}