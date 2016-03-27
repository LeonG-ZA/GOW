using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public static class SetItemsHelper
    {
        public static Item GetRandomSetItem()
        {
            Item result = null;
            //Choose between common or rare
            if (5 > Utility.Random(100))
            {
                //Rare item
                switch (Utility.RandomMinMax(1, 16))
                {
                    case 1:
                        result = new GreymistChest(); break;
                    case 2:
                        result = new GreymistLegs(); break;
                    case 3:
                        result = new AssassinGloves(); break;
                    case 4:
                        result = new AssassinLegs(); break;
                    case 5:
                        result = new HunterChest(); break;
                    case 6:
                        result = new HunterArms(); break;
                    case 7:
                        result = new LeafweaveChest(); break;
                    case 8:
                        result = new LeafweavePauldrons(); break;
                    case 9:
                        result = new MyrmidonGorget(); break;
                    case 10:
                        result = new MyrmidonChest(); break;
                    case 11:
                        result = new MyrmidonBascinet(); break;
                    case 12:
                        result = new DeathBoneHelm(); break;
                    case 13:
                        result = new DeathArms(); break;
                    case 14:
                        result = new PaladinChest(); break;
                    case 15:
                        result = new PaladinLegs(); break;
                    case 16:
                        result = new PaladinHelm(); break;
                }

            }
            else
            {
                //Common item
                switch (Utility.RandomMinMax(1, 21))
                {
                    case 1:
                        result = new GreymistArms(); break;
                    case 2:
                        result = new GreymistGloves(); break;
                    case 3:
                        result = new AssassinArms(); break;
                    case 4:
                        result = new AssassinChest(); break;
                    case 5:
                        result = new HunterLegs(); break;
                    case 6:
                        result = new HunterGloves(); break;
                    case 7:
                        result = new LeafweaveLegs(); break;
                    case 8:
                        result = new LeafweaveGloves(); break;
                    case 9:
                        result = new MyrmidonArms(); break;
                    case 10:
                        result = new MyrmidonLegs(); break;
                    case 11:
                        result = new MyrmidonGloves(); break;
                    case 12:
                        result = new DeathLegs(); break;
                    case 13:
                        result = new DeathChest(); break;
                    case 14:
                        result = new DeathGloves(); break;
                    case 15:
                        result = new PaladinArms(); break;
                    case 16:
                        result = new PaladinGloves(); break;
                    case 17:
                        result = new PaladinGorget(); break;
                    case 18:
                        result = new Evocaricus(); break;
                    case 19:
                        result = new MalekisHonor(); break;
                    case 20:
                        result = new Feathernock(); break;
                    case 21:
                        result = new Swiftflight(); break;
                }
            }
            return result;
        }
    }
}