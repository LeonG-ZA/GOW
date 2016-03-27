using System;
using Server.Items;
using Server.HolidayConfiguration;

namespace Server.Events.Halloween
{
    class HalloweenSettings
    {
        private static readonly Type[] m_GMBeggarTreats =
        {
            typeof(CreepyCake),
            typeof(PumpkinPizza),
            typeof(GrimWarning),
            typeof(HarvestWine),
            typeof(MurkyMilk),
            typeof(MrPlainsCookies),
            typeof(SkullsOnPike),
            typeof(ChairInAGhostCostume),
            typeof(ExcellentIronMaiden),
            typeof(HalloweenGuillotine),
            typeof(ColoredSmallWebs)
        };
        private static readonly Type[] m_Treats =
        {
            typeof(Lollipops),
            typeof(WrappedCandy),
            typeof(JellyBeans),
            typeof(Taffy),
            typeof(NougatSwirl)
        };
        public static DateTime StartHalloween
        {
            get
            {
                return new DateTime(HolidayConfig.m_CurrentYear, HolidayConfig.HalloweenOctoberStart, HolidayConfig.HalloweenDayStart);
            }
        }// YY MM DD
        public static DateTime FinishHalloween
        {
            get
            {
                return new DateTime(HolidayConfig.m_CurrentYear, HolidayConfig.HalloweenOctoberEnd, HolidayConfig.HalloweenDayEnd);
            }
        }
        public static Item RandomGMBeggerItem
        {
            get
            {
                return (Item)Activator.CreateInstance(m_GMBeggarTreats[Utility.Random(m_GMBeggarTreats.Length)]);
            }
        }
        public static Item RandomTreat
        {
            get
            {
                return (Item)Activator.CreateInstance(m_Treats[Utility.Random(m_Treats.Length)]) ;
            }
        }
    }
}