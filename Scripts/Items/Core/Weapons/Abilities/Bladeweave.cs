using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
    /// <summary>
    /// Warriors becomes one with their weapon, allowing it to guide their hand. The effects of this attack are unpredictable, but effective.
    /// </summary>
    public class Bladeweave : WeaponAbility
    {
        public Bladeweave()
        {
        }

        public override int BaseMana { get { return 30; } }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!Validate(attacker) || !CheckMana(attacker, true))
            {
                return;
            }

            attacker.PlaySound(0x5BC); // Bladeweave sound effect
            attacker.FixedParticles(0x376A, 1, 20, 0x7F5, 0x960, 0x3, EffectLayer.Waist); // TODO: Revisar efecto

            switch (Utility.RandomMinMax(1, 7))
            {
                case 1:
                    {
                        attacker.SendLocalizedMessage(1072841, "Paralyzing Blow"); // You weave your blade to execute a ~1_attack~.
                        ParalyzingBlow at = new ParalyzingBlow();
                        at.IsBladeweaveAttack = true;
                        at.OnHit(attacker, defender, damage);
                        break;
                    }
                case 2:
                    {
                        attacker.SendLocalizedMessage(1072841, "Bleed Attack"); // You weave your blade to execute a ~1_attack~.
                        BleedAttack at = new BleedAttack();
                        at.IsBladeweaveAttack = true;
                        at.OnHit(attacker, defender, damage);
                        break;
                    }
                case 3:
                    {
                        attacker.SendLocalizedMessage(1072841, "Double Strike"); // You weave your blade to execute a ~1_attack~.
                        DoubleStrike at = new DoubleStrike();
                        at.IsBladeweaveAttack = true;
                        at.OnHit(attacker, defender, damage);
                        break;
                    }
                case 4:
                    {
                        attacker.SendLocalizedMessage(1072841, "Feint"); // You weave your blade to execute a ~1_attack~.
                        Feint at = new Feint();
                        at.IsBladeweaveAttack = true;
                        at.OnHit(attacker, defender, damage);
                        break;
                    }
                case 5:
                    {
                        attacker.SendLocalizedMessage(1072841, "Mortal Strike"); // You weave your blade to execute a ~1_attack~.
                        MortalStrike at = new MortalStrike();
                        at.IsBladeweaveAttack = true;
                        at.OnHit(attacker, defender, damage);
                        break;
                    }
                case 6:
                    {
                        attacker.SendLocalizedMessage(1072841, "block"); // You weave your blade to execute a ~1_attack~.
                        Block at = new Block();
                        at.IsBladeweaveAttack = true;
                        at.OnHit(attacker, defender, damage);
                        break;
                    }
                case 7:
                    {
                        attacker.SendLocalizedMessage(1072841, "Crushing Blow"); // You weave your blade to execute a ~1_attack~.
                        CrushingBlow at = new CrushingBlow();
                        at.IsBladeweaveAttack = true;
                        at.OnHit(attacker, defender, damage);
                        break;
                    }
            }
        }
    }
    /*
	public class Bladeweave : WeaponAbility
	{
		private class BladeWeaveRedirect
		{
			public WeaponAbility NewAbility;
			public int ClilocEntry;

			public BladeWeaveRedirect(WeaponAbility ability, int cliloc)
			{
				NewAbility = ability;
				ClilocEntry = cliloc;
			}
		}

		private static Dictionary<Mobile, BladeWeaveRedirect> m_NewAttack = new Dictionary<Mobile, BladeWeaveRedirect>();

		public static bool BladeWeaving(Mobile attacker, out WeaponAbility a)
		{
			BladeWeaveRedirect bwr;
			bool success = m_NewAttack.TryGetValue(attacker, out bwr);
			if (success)
				a = bwr.NewAbility;
			else
				a = null;

			return success;
		}

		public Bladeweave()
		{
		}

        public override int BaseMana { get { return !Core.SA ? 30 : 15; } }

		public override bool OnBeforeSwing(Mobile attacker, Mobile defender)
		{
			if (!Validate(attacker) || !CheckMana(attacker, false))
				return false;

			switch (Utility.Random(7))
			{
				case 0:
					m_NewAttack[attacker] = new BladeWeaveRedirect(WeaponAbility.ArmorIgnore, 1028838);
					break;
				case 1:
					m_NewAttack[attacker] = new BladeWeaveRedirect(WeaponAbility.BleedAttack, 1028839);
					break;
				case 2:
					m_NewAttack[attacker] = new BladeWeaveRedirect(WeaponAbility.ConcussionBlow, 1028840);
					break;
				case 3:
					m_NewAttack[attacker] = new BladeWeaveRedirect(WeaponAbility.CrushingBlow, 1028841);
					break;
				case 4:
					m_NewAttack[attacker] = new BladeWeaveRedirect(WeaponAbility.DoubleStrike, 1028844);
					break;
				case 5:
					m_NewAttack[attacker] = new BladeWeaveRedirect(WeaponAbility.MortalStrike, 1028846);
					break;
				case 6:
					m_NewAttack[attacker] = new BladeWeaveRedirect(WeaponAbility.ParalyzingBlow, 1028848);
					break;
				default:
					// should never happen
					return false;
			}


			return ((BladeWeaveRedirect)m_NewAttack[attacker]).NewAbility.OnBeforeSwing(attacker, defender, false);
		}

		public override bool OnBeforeDamage(Mobile attacker, Mobile defender)
		{
			BladeWeaveRedirect bwr;
			if (m_NewAttack.TryGetValue(attacker, out bwr))
				return bwr.NewAbility.OnBeforeDamage(attacker, defender);
			else
				return base.OnBeforeDamage(attacker, defender);
		}

		public override void OnHit(Mobile attacker, Mobile defender, int damage)
		{
			if (CheckMana(attacker, true))
			{
				BladeWeaveRedirect bwr;
				if (m_NewAttack.TryGetValue(attacker, out bwr))
				{
					attacker.SendLocalizedMessage(1072841, "#" + bwr.ClilocEntry.ToString());
					bwr.NewAbility.OnHit(attacker, defender, damage);
				}
				else
					base.OnHit(attacker, defender, damage);

				m_NewAttack.Remove(attacker);
				ClearCurrentAbility(attacker);
			}
		}

		public override void OnMiss(Mobile attacker, Mobile defender)
		{
			BladeWeaveRedirect bwr;
			if (m_NewAttack.TryGetValue(attacker, out bwr))
				bwr.NewAbility.OnMiss(attacker, defender);
			else
				base.OnMiss(attacker, defender);

			m_NewAttack.Remove(attacker);
		}
	}
     */
}
