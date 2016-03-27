using System;
using System.Collections;
using Server.Commands;
using Server.Gumps;
using Server.Network;

namespace Server.Commands 
{
    public class CreateWorldXML
    {
        public CreateWorldXML()
        {
        }

        public static void Initialize() 
        { 
            CommandSystem.Register("Createworldxml", AccessLevel.Administrator, new CommandEventHandler(CreateXML_OnCommand)); 
        }

        [Usage("[createworldxml")]
        [Description("Create world with xml files and menu.")]
        private static void CreateXML_OnCommand(CommandEventArgs e)
        {
            e.Mobile.CloseGump(typeof(CreateWorldXMLGump));
            e.Mobile.SendGump(new CreateWorldXMLGump(e));
        }
    }
}

namespace Server.Gumps
{
    public class CreateWorldXMLGump : Gump
    {
        private readonly CommandEventArgs m_CommandEventArgs;
        public CreateWorldXMLGump(CommandEventArgs e)
            : base(50,50)
        {
            this.m_CommandEventArgs = e;
            this.Closable = true;
            this.Dragable = true;

            this.AddPage(1);

            //fundo cinza
            //x, y, largura, altura, item
            this.AddBackground(0, 0, 240, 625, 5054);
            //----------
            this.AddLabel(40, 2, 200, "CREATE WORLD GUMP");
            //fundo branco
            //x, y, largura, altura, item
            this.AddImageTiled(10, 20, 220, 560, 3004);
            //----------
            this.AddLabel(20, 26, 246, "Generate Moongen ");
            this.AddLabel(20, 51, 246, "Generate Door");
            this.AddLabel(20, 76, 246, "Decorate World");
            this.AddLabel(20, 101, 200, "Generate Sign");
            this.AddLabel(20, 126, 246, "Generate Teleport");
            this.AddLabel(20, 151, 246, "Generate Gauntlet");
            this.AddLabel(20, 176, 246, "Generate Champions");
            this.AddLabel(20, 201, 200, "Generate Khaldun");
            this.AddLabel(20, 226, 246, "Generate Doom");
            this.AddLabel(20, 251, 200, "Generate StealArties");
            this.AddLabel(20, 276, 200, "Generate SHTel");
            this.AddLabel(20, 301, 200, "Generate SecretLoc");
            this.AddLabel(20, 326, 200, "Generate Factions");
            this.AddLabel(20, 351, 246, "Decorate Mondain's");
            this.AddLabel(20, 376, 246, "Decorate Stygian");
            this.AddLabel(20, 401, 246, "GenPrimevalLichLever");
            this.AddLabel(20, 426, 221, "Spawn Felucca");
            this.AddLabel(20, 451, 221, "Spawn Trammel");
            this.AddLabel(20, 476, 221, "Spawn Ilshenar");
            this.AddLabel(20, 501, 221, "Spawn Malas");
            this.AddLabel(20, 526, 221, "Spawn Tokuno");
            this.AddLabel(20, 551, 221, "Spawn TerMur");
            //Options
            //Options
            this.AddCheck(180, 23, 210, 211, true, 101);
            this.AddCheck(180, 48, 210, 211, true, 102);
            this.AddCheck(180, 73, 210, 211, true, 103);
            this.AddCheck(180, 98, 210, 211, true, 104);
            this.AddCheck(180, 123, 210, 211, true, 105);
            this.AddCheck(180, 148, 210, 211, true, 106);
            this.AddCheck(180, 173, 210, 211, true, 107);
            this.AddCheck(180, 198, 210, 211, true, 108);
            this.AddCheck(180, 223, 210, 211, true, 109);
            this.AddCheck(180, 248, 210, 211, true, 110);
            this.AddCheck(180, 273, 210, 211, true, 111);
            this.AddCheck(180, 298, 210, 211, true, 112);
            this.AddCheck(180, 323, 210, 211, true, 116);
            this.AddCheck(180, 348, 210, 211, true, 113);
            this.AddCheck(180, 373, 210, 211, true, 114);
            this.AddCheck(180, 398, 210, 211, true, 115);
            this.AddCheck(180, 423, 210, 211, true, 117);
            this.AddCheck(180, 448, 210, 211, true, 118);
            this.AddCheck(180, 473, 210, 211, true, 119);
            this.AddCheck(180, 498, 210, 211, true, 120);
            this.AddCheck(180, 523, 210, 211, true, 121);
            this.AddCheck(180, 548, 210, 211, true, 122);
            //Ok, Cancel (x, y, ?, ?, ?)
            this.AddButton(60, 600, 247, 249, 1, GumpButtonType.Reply, 0);
            this.AddButton(130, 600, 241, 243, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState state, RelayInfo info) 
        { 
            Mobile from = state.Mobile; 

            switch( info.ButtonID ) 
            { 
                case 0: // Closed or Cancel
                    {
                        return;
                    }

                default: 
                    { 
                        // Make sure that the OK, button was pressed
                        if (info.ButtonID == 1)
                        {
                            // Get the array of switches selected
                            ArrayList Selections = new ArrayList(info.Switches);
                            string prefix = Server.Commands.CommandSystem.Prefix;

                            from.Say("CREATING WORLD...");

                            // Now use any selected command
                            if (Selections.Contains(101) == true)
                            {
                                from.Say("Generating Moongates...");
                                CommandSystem.Handle(from, String.Format("{0}Moongen", prefix));
                            }

                            if (Selections.Contains(102) == true)
                            {
                                from.Say("Generating Doors...");
                                CommandSystem.Handle(from, String.Format("{0}DoorGen", prefix));
                            }

                            if (Selections.Contains(103) == true)
                            {
                                from.Say("Decorating World...");
                                CommandSystem.Handle(from, String.Format("{0}Decorate", prefix));
                            }

                            if (Selections.Contains(104) == true)
                            {
                                from.Say("Generating Signs...");
                                CommandSystem.Handle(from, String.Format("{0}SignGen", prefix));
                            }

                            if (Selections.Contains(105) == true)
                            {
                                from.Say("Generating Teleporters...");
                                CommandSystem.Handle(from, String.Format("{0}TelGen", prefix));
                            }

                            if (Selections.Contains(106) == true)
                            {
                                from.Say("Generating Gauntlet Spawners...");
                                CommandSystem.Handle(from, String.Format("{0}GenGauntlet", prefix));
                            }

                            if (Selections.Contains(107) == true)
                            {
                                // champions message in champions script
                                CommandSystem.Handle(from, String.Format("{0}GenChampions", prefix));
                            }

                            if (Selections.Contains(108) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}GenKhaldun", prefix));
                            }

                            if (Selections.Contains(109) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}GenLeverPuzzle", prefix));
                                from.Say("Doom Lamp Puzzle Generated!");
                            }

                            if (Selections.Contains(110) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}GenStealArties", prefix));
                                from.Say("Stealable Artifacts Generated!");
                            }

                            if (Selections.Contains(111) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}SHTelGen", prefix));
                            }

                            if (Selections.Contains(112) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}SecretLocGen", prefix));
                            }

                            if (Selections.Contains(113) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}DecorateML", prefix));
                            }
                                                 
                            if (Selections.Contains(114) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}DecorateSA", prefix));
                                from.Say("Stygian Abyss Decorate!");
                            }

                            if (Selections.Contains(115) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}GenLichPuzzle", prefix));
                                from.Say("Primeval Lich Puzzle Generated!");
                            }

                            if (Selections.Contains(116) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}GenerateFactions", prefix));
                            }

                            if (Selections.Contains(117) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}XmlLoad Data\\World\\XMLSpawns\\felucca.xml", prefix));
                                from.Say("Felucca spawned!");
                            }

                            if (Selections.Contains(118) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}XmlLoad Data\\World\\XMLSpawns\\trammel.xml", prefix));
                                from.Say("Trammel spawned!");
                            }

                            if (Selections.Contains(119) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}XmlLoad Data\\World\\XMLSpawns\\ilshenar.xml", prefix));
                                from.Say("Ilshenar spawned!");
                            }

                            if (Selections.Contains(120) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}XmlLoad Data\\World\\XMLSpawns\\malas.xml", prefix));
                                from.Say("Malas spawned!");
                            }

                            if (Selections.Contains(121) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}XmlLoad Data\\World\\XMLSpawns\\tokuno.xml", prefix));
                                from.Say("Tokuno spawned!");
                            }

                            if (Selections.Contains(122) == true)
                            {
                                CommandSystem.Handle(from, String.Format("{0}XmlLoad Data\\World\\XMLSpawns\\termur.xml", prefix));
                                from.Say("TerMur spawned!");
                            }
                        }

                        from.Say("World generation completed!");

                        break;
                    }
            }
        }
    }
}