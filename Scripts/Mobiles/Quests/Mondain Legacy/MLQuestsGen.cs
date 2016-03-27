using Server;
using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Gumps;
using Server.Engines.Quests;
using Server.QuestConfiguration;
using Server.Engines.Collections;

namespace Server.Items
{
    public static class MLQuestsGen
    {
        public static void Initialize()
        {
            if (QuestConfig.MLQuestGenEnabled)
            {
                Generate();
            }
        }

        public static void Generate()
        {
            //Felucca
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Jamal"), new Point3D(559, 1651, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 2, "Ioseph"), new Point3D(1354, 3754, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Aurelia"), new Point3D(1459, 3795, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 2, "Szandor"), new Point3D(1277, 3731, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Ben"), new Point3D(2467, 402, 15), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Frederic"), new Point3D(2415, 887, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Leon"), new Point3D(2918, 851, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Andros"), new Point3D(2531, 581, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Darius"), new Point3D(4310, 954, 10), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "MaulBear"), new Point3D(1730, 257, 16), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Strongroot"), new Point3D(597, 1744, 0), Map.Felucca);
            PutSpawner(new Spawner(1, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(2), 0, 12, "SosariaSap"), new Point3D(757, 1004, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Enigma"), new Point3D(1828, 961, 7), Map.Felucca);
            PutSpawner(new Spawner(1, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(90), 0, 5, "Bravehorn"), new Point3D(1193, 2467, 0), Map.Felucca);
            PutSpawner(new Spawner(5, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(30), 0, 8, "BravehornsMate"), new Point3D(1192, 2467, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Huntsman"), new Point3D(1676, 593, 16), Map.Felucca);
            PutSpawner(new Spawner(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15), 0, 10, "TimberWolf"), new Point3D(1671, 592, 16), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Nedrick"), new Point3D(2958, 3466, 15), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Sledge"), new Point3D(2673, 2129, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Patricus"), new Point3D(3007, 823, -2), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Belulah"), new Point3D(3782, 1266, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Evan"), new Point3D(1486, 1706, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Regina"), new Point3D(1422, 1621, 20), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Aeluva"), new Point3D(7064, 349, 0), Map.Felucca);
            PutSpawner(new Spawner(20, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), 0, 30, "MiniatureMushroom"), new Point3D(7015, 366, 0), Map.Felucca);
            PutSpawner(new Spawner(5, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), 0, 20, "MiniatureMushroom"), new Point3D(7081, 373, 0), Map.Felucca);
            PutSpawner(new Spawner(15, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), 0, 20, "MiniatureMushroom"), new Point3D(7052, 414, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Synaeva"), new Point3D(7064, 350, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Koole"), new Point3D(6257, 110, -10), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Brae"), new Point3D(6266, 124, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Broolol"), new Point3D(7011, 375, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Emilio"), new Point3D(1447, 1664, 10), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Thalia"), new Point3D(3675, 1322, 20), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Natalie"), new Point3D(569, 2218, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Gregorio"), new Point3D(812, 2208, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Arielle"), new Point3D(3366, 292, 9), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "AriellesBauble"), new Point3D(3400, 336, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "FrightenedDryad"), new Point3D(6500, 875, 0), Map.Felucca);
            PutSpawner(new Spawner(5, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(40), 0, 20, "SamplesOfCorruptedWater"), new Point3D(6521, 880, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Fabrizio"), new Point3D(4290, 995, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 30, 40, 0, 6, "Aminia"), new Point3D(4018, 311, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Lucius"), new Point3D(5592, 3020, 36), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 2, "Frazer"), new Point3D(6574, 181, 31), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Neil"), new Point3D(6575, 186, 32), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Lefty"), new Point3D(3792, 1105, 20), Map.Felucca);
            PutSpawner(new Spawner(1, 0, 0, 0, 2, "PrismaticCrystal"), new Point3D(6511, 80, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 0, 0, 0, 3, "CrystalFieldTele"), new Point3D(6509, 87, -4), Map.Felucca);
            PutSpawner(new Spawner(1, 30, 40, 0, 6, "DenthesJournal"), new Point3D(6526, 122, -20), Map.Felucca);
            //Sanctuary
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Beotham"), new Point3D(6285, 114, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Danoel"), new Point3D(6282, 116, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Tallinin"), new Point3D(6279, 122, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Tiana"), new Point3D(6257, 112, -10), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Oolua"), new Point3D(6250, 124, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Rollarn"), new Point3D(6244, 110, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Dallid"), new Point3D(6277, 104, -10), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Canir"), new Point3D(6274, 130, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Yellienir"), new Point3D(6257, 126, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Onallan"), new Point3D(6258, 108, -10), Map.Felucca);
            //HeartWood
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Saril"), new Point3D(7075, 376, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Cailla"), new Point3D(7075, 377, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Tamm"), new Point3D(7075, 378, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Landy"), new Point3D(7089, 390, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Alejaha"), new Point3D(7043, 387, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Mielan"), new Point3D(7063, 350, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Ciala"), new Point3D(7031, 411, 7), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Aniel"), new Point3D(7034, 412, 6), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Aulan"), new Point3D(6986, 340, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Brinnae"), new Point3D(6996, 351, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Caelas"), new Point3D(7039, 390, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Clehin"), new Point3D(7092, 390, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Cloorne"), new Point3D(7010, 364, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Salaenih"), new Point3D(7009, 362, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Vilo"), new Point3D(7029, 377, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Tholef"), new Point3D(6986, 386, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Tillanil"), new Point3D(6987, 388, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Waelian"), new Point3D(6996, 381, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Sleen"), new Point3D(6997, 381, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Unoelil"), new Point3D(7010, 388, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Anolly"), new Point3D(7009, 388, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Jusae"), new Point3D(7042, 377, 2), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Cillitha"), new Point3D(7043, 377, 2), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Lohn"), new Point3D(7062, 410, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Olla"), new Point3D(7063, 410, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Thallary"), new Point3D(7032, 439, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Ahie"), new Point3D(7033, 440, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Tyleelor"), new Point3D(7010, 364, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Athialon"), new Point3D(7011, 365, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Taellia"), new Point3D(7038, 387, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Mallew"), new Point3D(7047, 390, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Abbein"), new Point3D(7043, 390, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Vicaie"), new Point3D(7054, 390, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Jothan"), new Point3D(7056, 383, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Alethanian"), new Point3D(7056, 380, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Rebinil"), new Point3D(7089, 380, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Aluniol"), new Point3D(7089, 383, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Olaeni"), new Point3D(7080, 363, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Bolaevin"), new Point3D(7066, 351, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Aneen"), new Point3D(7053, 337, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Daelas"), new Point3D(7036, 412, 7), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Alelle"), new Point3D(7028, 406, 7), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Nillaen"), new Point3D(7061, 370, 14), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Ryal"), new Point3D(7009, 375, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Braen"), new Point3D(7081, 366, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Acob"), new Point3D(7037, 387, 0), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Calendor"), new Point3D(7062, 370, 14), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Siarra"), new Point3D(7051, 339, 0), Map.Felucca);
            //Trammel
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Jamal"), new Point3D(559, 1651, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 2, "Ioseph"), new Point3D(1354, 3754, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Aurelia"), new Point3D(1459, 3795, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 2, "Szandor"), new Point3D(1277, 3731, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Ben"), new Point3D(2467, 402, 15), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Frederic"), new Point3D(2415, 887, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Leon"), new Point3D(2918, 851, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Andros"), new Point3D(2531, 581, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Darius"), new Point3D(4310, 954, 10), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "MaulBear"), new Point3D(1730, 257, 16), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Strongroot"), new Point3D(597, 1744, 0), Map.Trammel);
            PutSpawner(new Spawner(1, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(2), 0, 12, "SosariaSap"), new Point3D(757, 1004, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Enigma"), new Point3D(1828, 961, 7), Map.Trammel);
            PutSpawner(new Spawner(1, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(90), 0, 5, "Bravehorn"), new Point3D(1193, 2467, 0), Map.Trammel);
            PutSpawner(new Spawner(5, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(30), 0, 8, "BravehornsMate"), new Point3D(1192, 2467, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Huntsman"), new Point3D(1676, 593, 16), Map.Trammel);
            PutSpawner(new Spawner(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15), 0, 10, "TimberWolf"), new Point3D(1671, 592, 16), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Nedrick"), new Point3D(2958, 3466, 15), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Sledge"), new Point3D(2673, 2129, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Patricus"), new Point3D(3007, 823, -2), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Belulah"), new Point3D(3782, 1266, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Evan"), new Point3D(1486, 1706, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Regina"), new Point3D(1362, 1622, 50), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Aeluva"), new Point3D(7064, 349, 0), Map.Trammel);
            PutSpawner(new Spawner(20, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), 0, 30, "MiniatureMushroom"), new Point3D(7015, 366, 0), Map.Trammel);
            PutSpawner(new Spawner(5, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), 0, 20, "MiniatureMushroom"), new Point3D(7081, 373, 0), Map.Trammel);
            PutSpawner(new Spawner(15, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), 0, 20, "MiniatureMushroom"), new Point3D(7052, 414, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Koole"), new Point3D(6257, 110, -10), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Synaeva"), new Point3D(7064, 350, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Brae"), new Point3D(6266, 124, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Broolol"), new Point3D(7011, 375, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Emilio"), new Point3D(1447, 1664, 10), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Thalia"), new Point3D(3675, 1322, 20), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "FrightenedDryad"), new Point3D(6500, 875, 0), Map.Trammel);
            PutSpawner(new Spawner(5, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(40), 0, 20, "SamplesOfCorruptedWater"), new Point3D(6521, 880, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Fabrizio"), new Point3D(4290, 995, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 30, 40, 0, 6, "Aminia"), new Point3D(4018, 311, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 2, "Frazer"), new Point3D(6574, 181, 31), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Neil"), new Point3D(6575, 186, 32), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Lefty"), new Point3D(3792, 1105, 20), Map.Trammel);
            PutSpawner(new Spawner(1, 0, 0, 0, 2, "PrismaticCrystal"), new Point3D(6511, 80, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 0, 0, 0, 3, "CrystalFieldTele"), new Point3D(6509, 87, -4), Map.Trammel);
            PutSpawner(new Spawner(1, 30, 40, 0, 6, "DenthesJournal"), new Point3D(6526, 122, -20), Map.Trammel);
            PutSpawner(new Spawner(1, 30, 40, 0, 14, "RedSolenMatriarch"), new Point3D(5776, 1898, -22), Map.Trammel);
            //New Haven
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Aelorn"), new Point3D(3527, 2516, 45), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Dimethro"), new Point3D(3528, 2520, 25), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Churchill"), new Point3D(3531, 2531, 20), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Robyn"), new Point3D(3535, 2531, 20), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Recaro"), new Point3D(3536, 2534, 20), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "AldenArmstrong"), new Point3D(3535, 2538, 20), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Jockles"), new Point3D(3535, 2544, 20), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "TylAriadne"), new Point3D(3525, 2556, 20), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Alefian"), new Point3D(3473, 2497, 72), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Gustar"), new Point3D(3474, 2492, 91), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Jillian"), new Point3D(3465, 2490, 71), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Kaelynna"), new Point3D(3486, 2491, 52), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Mithneral"), new Point3D(3485, 2491, 71), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Amelia"), new Point3D(3459, 2529, 53), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "AndreasVesalius"), new Point3D(3457, 2550, 35), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Avicenna"), new Point3D(3464, 2558, 35), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "SarsmeaSmythe"), new Point3D(3492, 2577, 15), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Ryuichi"), new Point3D(3422, 2520, 21), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Chiyo"), new Point3D(3420, 2516, 21), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Jun"), new Point3D(3422, 2516, 21), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Walker"), new Point3D(3429, 2518, 19), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Hamato"), new Point3D(3493, 2414, 55), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Mulcivikh"), new Point3D(3548, 2456, 15), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Morganna"), new Point3D(3547, 2463, 15), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Jacob"), new Point3D(3504, 2741, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "GeorgeHephaestus"), new Point3D(3471, 2542, 36), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Andric"), new Point3D(3742, 2582, 40), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Kashiel"), new Point3D(3744, 2586, 40), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Asandos"), new Point3D(3505, 2513, 27), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Clairesse"), new Point3D(3492, 2546, 20), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Gervis"), new Point3D(3505, 2749, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Mugg"), new Point3D(3507, 2747, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Lowel"), new Point3D(3440, 2645, 27), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Lyle"), new Point3D(3503, 2584, 14), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Nibbet"), new Point3D(3459, 2525, 53), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Norton"), new Point3D(3502, 2603, 1), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Sadrah"), new Point3D(3742, 2731, 7), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Hargrove"), new Point3D(3445, 2633, 28), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 14, "RedAmbitiousSolenQueen"), new Point3D(5790, 1983, 2), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Natalie"), new Point3D(569, 2218, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Gregorio"), new Point3D(812, 2208, 0), Map.Trammel);
            // HeartWood
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Saril"), new Point3D(7075, 376, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Cailla"), new Point3D(7075, 377, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Tamm"), new Point3D(7075, 378, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Landy"), new Point3D(7089, 390, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Alejaha"), new Point3D(7043, 387, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Mielan"), new Point3D(7063, 350, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Ciala"), new Point3D(7031, 411, 7), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Aniel"), new Point3D(7034, 412, 6), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Brinnae"), new Point3D(6996, 351, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Aulan"), new Point3D(6986, 340, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Caelas"), new Point3D(7039, 390, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Clehin"), new Point3D(7092, 390, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Cloorne"), new Point3D(7010, 364, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Salaenih"), new Point3D(7009, 362, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Vilo"), new Point3D(7029, 377, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Tholef"), new Point3D(6986, 386, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Tillanil"), new Point3D(6987, 388, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Waelian"), new Point3D(6996, 381, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Sleen"), new Point3D(6997, 381, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Unoelil"), new Point3D(7010, 388, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Anolly"), new Point3D(7009, 388, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Jusae"), new Point3D(7042, 377, 2), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Cillitha"), new Point3D(7043, 377, 2), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Lohn"), new Point3D(7062, 410, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Olla"), new Point3D(7063, 410, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Thallary"), new Point3D(7032, 439, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Ahie"), new Point3D(7033, 440, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Tyleelor"), new Point3D(7010, 364, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Athialon"), new Point3D(7011, 365, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Taellia"), new Point3D(7038, 387, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Mallew"), new Point3D(7047, 390, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Abbein"), new Point3D(7043, 390, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Vicaie"), new Point3D(7054, 390, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Jothan"), new Point3D(7056, 383, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Alethanian"), new Point3D(7056, 380, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Rebinil"), new Point3D(7089, 380, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Aluniol"), new Point3D(7089, 383, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Olaeni"), new Point3D(7080, 363, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Bolaevin"), new Point3D(7066, 351, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Aneen"), new Point3D(7053, 337, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Daelas"), new Point3D(7036, 412, 7), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Alelle"), new Point3D(7028, 406, 7), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Nillaen"), new Point3D(7061, 370, 14), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Ryal"), new Point3D(7009, 375, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Braen"), new Point3D(7081, 366, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Acob"), new Point3D(7037, 387, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Calendor"), new Point3D(7062, 370, 14), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Siarra"), new Point3D(7051, 339, 0), Map.Trammel);
            //Sanctuary
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Beotham"), new Point3D(6285, 114, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Danoel"), new Point3D(6282, 116, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Tallinin"), new Point3D(6279, 122, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Tiana"), new Point3D(6257, 112, -10), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Oolua"), new Point3D(6250, 124, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Rollarn"), new Point3D(6244, 110, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Dallid"), new Point3D(6277, 104, -10), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Canir"), new Point3D(6274, 130, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Yellienir"), new Point3D(6257, 126, 0), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "Onallan"), new Point3D(6258, 108, -10), Map.Trammel);

            //Ilshenar
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Arielle"), new Point3D(1560, 1182, -27), Map.Ilshenar);
            PutSpawner(new Spawner(6, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), 0, 20, "AriellesBauble"), new Point3D(1585, 1212, -13), Map.Ilshenar);
            PutSpawner(new Spawner(1, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30), 0, 5, "Lissbet"), new Point3D(1568, 1040, -7), Map.Ilshenar);
            PutSpawner(new Spawner(1, 5, 10, 0, 8, "GrandpaCharley"), new Point3D(1322, 1331, -14), Map.Ilshenar);
            PutSpawner(new Spawner(1, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30), 0, 3, "Sheep"), new Point3D(1308, 1324, -14), Map.Ilshenar);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Jelrice"), new Point3D(1176, 1196, -25), Map.Ilshenar);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Yorus"), new Point3D(1389, 423, -24), Map.Ilshenar);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "Petrus"), new Point3D(1420, 407, 9), Map.Ilshenar);
            PutSpawner(new Spawner(1, 5, 10, 0, 5, "AriellesBauble"), new Point3D(1595, 1209, -16), Map.Ilshenar);

            //Malas
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Kia"), new Point3D(87, 1640, 0), Map.Malas);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Nythalia"), new Point3D(91, 1639, 0), Map.Malas);
            PutSpawner(new Spawner(1, 5, 10, 0, 3, "Emerillo"), new Point3D(90, 1639, 0), Map.Malas);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Drithen"), new Point3D(1983, 1364, -80), Map.Malas);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Aernya"), new Point3D(2095, 1380, -90), Map.Malas);
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "Gorrow"), new Point3D(993, 512, -50), Map.Malas);
            PutSpawner(new Spawner(1, 5, 10, 0, 2, "Gnosos"), new Point3D(82, 1649, 0), Map.Malas);
            PutSpawner(new Spawner(1, 5, 10, 0, 2, "Cohenn"), new Point3D(88, 1645, 20), Map.Malas);

            //Tokuno
            PutSpawner(new Spawner(1, 5, 10, 0, 4, "CitadelTele"), new Point3D(1342, 768, 19), Map.Tokuno);

            //Comunity Collections
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "BritainLibraryWarriorCollection"), new Point3D(1410, 1607, 30), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "BritainLibraryArtistCollection"), new Point3D(1414, 1604, 30), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "BritainLibrarySamuraiCollection"), new Point3D(1410, 1603, 30), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "BritainLibraryMasterOfTradesCollection"), new Point3D(1415, 1590, 30), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "BritainLibraryMusicianCollection"), new Point3D(1411, 1594, 50), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "BritainLibraryAnimalTrainerCollection"), new Point3D(1416, 1595, 51), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "BritainLibraryPaladinCollection"), new Point3D(1414, 1599, 51), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "BritainLibraryNecromancerCollection"), new Point3D(1408, 1600, 50), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "BritainLibraryMageCollection"), new Point3D(1408, 1605, 50), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "BritainLibraryFisherCollection"), new Point3D(1410, 1607, 50), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "BritainLibraryTreasureHunterCollection"), new Point3D(1415, 1608, 50), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "BritainLibraryThiefCollection"), new Point3D(1415, 1602, 50), Map.Trammel);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "MoonglowZooCollection"), new Point3D(4538, 1375, 31), Map.Felucca);
            PutSpawner(new Spawner(1, 5, 10, 0, 0, "VesperMuseumCollection"), new Point3D(2925, 975, -16), Map.Felucca);
            PutDeco(new MoonglowZooCollection(), new Point3D(4538, 1375, 31), Map.Trammel);
            PutDeco(new VesperMuseumCollection(), new Point3D(2925, 975, -16), Map.Trammel);

            //Quest Decor and Teleporters
            PutDeco(new BlightedGroveTeleFel(), new Point3D(587, 1638, 0), Map.Felucca);
            PutDeco(new BlightedGroveTeleFel(), new Point3D(588, 1638, 0), Map.Felucca);
            PutDeco(new BlightedGroveTeleFel(), new Point3D(589, 1638, 0), Map.Felucca);
            PutDeco(new BlightedGroveTeleTram(), new Point3D(587, 1638, 0), Map.Trammel);
            PutDeco(new BlightedGroveTeleTram(), new Point3D(588, 1638, 0), Map.Trammel);
            PutDeco(new BlightedGroveTeleTram(), new Point3D(589, 1638, 0), Map.Trammel);
            PutDeco(new BlightedGroveTreeInTeleFel(), new Point3D(6471, 869, 21), Map.Felucca);
            PutDeco(new BlightedGroveTreeInTeleFel(), new Point3D(6472, 869, 20), Map.Felucca);
            PutDeco(new BlightedGroveTreeInTeleFel(), new Point3D(6473, 869, 20), Map.Felucca);
            PutDeco(new BlightedGroveTreeInTeleFel(), new Point3D(6474, 869, 18), Map.Felucca);
            PutDeco(new BlightedGroveTreeOutTeleFel(), new Point3D(6588, 868, 0), Map.Felucca);
            PutDeco(new BlightedGroveTreeOutTeleFel(), new Point3D(6588, 867, 0), Map.Felucca);
            PutDeco(new BlightedGroveTreeInTeleTram(), new Point3D(6471, 869, 21), Map.Trammel);
            PutDeco(new BlightedGroveTreeInTeleTram(), new Point3D(6472, 869, 20), Map.Trammel);
            PutDeco(new BlightedGroveTreeInTeleTram(), new Point3D(6473, 869, 20), Map.Trammel);
            PutDeco(new BlightedGroveTreeInTeleTram(), new Point3D(6474, 869, 18), Map.Trammel);
            PutDeco(new BlightedGroveTreeOutTeleTram(), new Point3D(6588, 868, 0), Map.Trammel);
            PutDeco(new BlightedGroveTreeOutTeleTram(), new Point3D(6588, 867, 0), Map.Trammel);
            PutDeco(new PaintedCavesTele(), new Point3D(1714, 2997, 0), Map.Felucca);
            PutDeco(new PaintedCavesTele(), new Point3D(1714, 2996, 0), Map.Felucca);
            PutDeco(new PaintedCavesTele(), new Point3D(1714, 2997, 0), Map.Trammel);
            PutDeco(new PaintedCavesTele(), new Point3D(1714, 2996, 0), Map.Trammel);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5577, 3018, 25), Map.Felucca);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5578, 3019, 25), Map.Felucca);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5578, 3018, 25), Map.Felucca);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5578, 3017, 25), Map.Felucca);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5578, 3016, 25), Map.Felucca);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5579, 3017, 25), Map.Felucca);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5579, 3018, 25), Map.Felucca);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5579, 3019, 25), Map.Felucca);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5577, 3018, 25), Map.Trammel);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5578, 3019, 25), Map.Trammel);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5578, 3018, 25), Map.Trammel);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5578, 3017, 25), Map.Trammel);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5578, 3016, 25), Map.Trammel);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5579, 3017, 25), Map.Trammel);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5579, 3018, 25), Map.Trammel);
            PutDeco(new ParoxysmusTeleFel(), new Point3D(5579, 3019, 25), Map.Trammel);
            PutDeco(new PrismOfLightTeleFel(), new Point3D(3784, 1098, 12), Map.Felucca);
            PutDeco(new PrismOfLightTeleFel(), new Point3D(3785, 1098, 12), Map.Felucca);
            PutDeco(new PrismOfLightTeleFel(), new Point3D(3784, 1098, 12), Map.Trammel);
            PutDeco(new PrismOfLightTeleFel(), new Point3D(3785, 1098, 12), Map.Trammel);
            PutDeco(new BedlamTele(), new Point3D(2067, 1371, -75), Map.Malas);
            PutDeco(new LabyrinthIslandTele(), new Point3D(1717, 1156, -95), Map.Malas);
            PutDeco(new LabyrinthTele(), new Point3D(1732, 972, -75), Map.Malas);
            PutDeco(new LabyrinthTele(), new Point3D(1734, 972, -75), Map.Malas);
            PutDeco(new TwistedWealdTele(), new Point3D(1450, 1470, -22), Map.Ilshenar);
            PutDeco(new SanctuaryTele(), new Point3D(766, 1645, 0), Map.Felucca);
            PutDeco(new SanctuaryTele(), new Point3D(766, 1646, 0), Map.Felucca);
            PutDeco(new SanctuaryTele(), new Point3D(766, 1647, 0), Map.Felucca);
            PutDeco(new SanctuaryTele(), new Point3D(766, 1645, 0), Map.Trammel);
            PutDeco(new SanctuaryTele(), new Point3D(766, 1646, 0), Map.Trammel);
            PutDeco(new SanctuaryTele(), new Point3D(766, 1647, 0), Map.Trammel);
        }

        public static void PutSpawner(Spawner s, Point3D loc, Map map)
        {
            string name = String.Format("MLQS-{0}", s.Name);

            // Auto cleanup on regeneration
            List<Item> toDelete = new List<Item>();

            foreach (Item item in map.GetItemsInRange(loc, 0))
            {
                if (item is Spawner && item.Name == name)
                {
                    toDelete.Add(item);
                }
            }

            foreach (Item item in toDelete)
            {
                item.Delete();
            }

            s.Name = name;
            s.MoveToWorld(loc, map);
        }

        public static void PutDeco(Item deco, Point3D loc, Map map)
        {
            // Auto cleanup on regeneration
            List<Item> toDelete = new List<Item>();

            foreach (Item item in map.GetItemsInRange(loc, 0))
            {
                if (item.ItemID == deco.ItemID && item.Z == loc.Z)
                {
                    toDelete.Add(item);
                }
            }

            foreach (Item item in toDelete)
            {
                item.Delete();
            }

            deco.MoveToWorld(loc, map);
        }
    }
}