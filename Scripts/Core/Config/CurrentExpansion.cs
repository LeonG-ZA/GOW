using Server.Network;
using Server.MainConfiguration;
using Server.CharConfiguration;
using Server.Accounting;

namespace Server
{
	public class CurrentExpansion
	{
        private static readonly Expansion Expansion = MainConfig.MainExpansion;

		public static void Configure()
		{
			Core.Expansion = Expansion;

            if (CharConfig.CharInsuranceEnabled)
            {
                Mobile.InsuranceEnabled = Core.AOS;
            }

            ObjectPropertyList.Enabled = Core.AOS;
            Mobile.VisibleDamageType = Core.AOS ? VisibleDamageType.Related : VisibleDamageType.None;
            Mobile.GuildClickMessage = !Core.AOS;
            Mobile.AsciiClickMessage = !Core.AOS;
            AccountGold.Enabled = Core.TOL;
            AccountGold.ConvertOnTrade = false;
            AccountGold.ConvertOnBank = true;
            VirtualCheck.UseEditGump = true;

            if (Core.AOS)
			{
				ItemAttributes.DisableStatInfluences();

				if (ObjectPropertyList.Enabled)
				{
					PacketHandlers.SingleClickProps = true; // single click for everything is overriden to check object property list
				}

				Mobile.ActionDelay = 1000;
				Mobile.AosStatusHandler = ItemAttributes.GetStatus;
			}
		}
	}
}