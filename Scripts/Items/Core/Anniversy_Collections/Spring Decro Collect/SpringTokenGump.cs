using System; 
using System.Net; 
using Server; 
using Server.Accounting; 
using Server.Gumps; 
using Server.Items; 
using Server.Mobiles; 
using Server.Network;

namespace Server.Gumps
{
    public class SpringTokenGump : Gump
    {
        private Mobile m_Mobile;
        private Item m_Deed;


        public SpringTokenGump(Mobile from, Item deed)
            : base(30, 20)
        {
            m_Mobile = from;
            m_Deed = deed;

            Closable = true;
            Disposable = false;
            Dragable = true;
            Resizable = false;
            AddPage(1);

            AddBackground(0, 0, 300, 400, 3000);
            AddBackground(8, 8, 284, 384, 5054);

            AddLabel(40, 12, 37, "Spring Decor Rewards");

            AddLabel(52, 40, 0, "Surveyor's Scope");
            AddButton(12, 40, 4005, 4007, 1, GumpButtonType.Reply, 1);
            AddLabel(52, 60, 0, "Mongbat Dartboard");
            AddButton(12, 60, 4005, 4007, 2, GumpButtonType.Reply, 2);
            AddLabel(52, 80, 0, "Blessed Statue");
            AddButton(12, 80, 4005, 4007, 3, GumpButtonType.Reply, 3);
            AddLabel(52, 100, 0, "Carved Wooden Screen");
            AddButton(12, 100, 4005, 4007, 4, GumpButtonType.Reply, 4);
            AddLabel(52, 120, 0, "Throw Pillow");
            AddButton(12, 120, 4005, 4007, 5, GumpButtonType.Reply, 5);
            AddLabel(52, 140, 0, "Dragon Brazier");
            AddButton(12, 140, 4005, 4007, 6, GumpButtonType.Reply, 6);
            AddLabel(52, 160, 0, "Navigator's World Map");
            AddButton(12, 160, 4005, 4007, 7, GumpButtonType.Reply, 7);
            AddLabel(52, 180, 0, "Basket of Herbs");
            AddButton(12, 180, 4005, 4007, 8, GumpButtonType.Reply, 8);
            AddLabel(52, 200, 0, "Shochu");
            AddButton(12, 200, 4005, 4007, 9, GumpButtonType.Reply, 9);
            AddLabel(52, 220, 0, "Mariner's Brass Sextant");
            AddButton(12, 220, 4005, 4007, 10, GumpButtonType.Reply, 10);
            AddLabel(52, 240, 0, "Heartwood Chest (unblessed)");
            AddButton(12, 240, 4005, 4007, 11, GumpButtonType.Reply, 11);
            AddLabel(52, 260, 0, "Low Yew Table");
            AddButton(12, 260, 4005, 4007, 12, GumpButtonType.Reply, 12);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            switch (info.ButtonID)
            {
                case 0: //Close Gump 
                    {
                        from.CloseGump(typeof(SpringTokenGump));
                        break;
                    }
                case 1: // Surveyor's Scope
                    {
                        Item item = new SurveyorsScope();
                        item.LootType = LootType.Blessed;
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(SpringTokenGump));
                        m_Deed.Delete();
                        break;
                    }
                case 2: // Mongbat Dartboard
                    {
                        Item item = new MongbatDartboard();
                        item.LootType = LootType.Blessed;
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(SpringTokenGump));
                        m_Deed.Delete();
                        break;
                    }
                case 3: // Blessed Statue
                    {
                        Item item = new BlessedStatue();
                        item.LootType = LootType.Blessed;
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(SpringTokenGump));
                        m_Deed.Delete();
                        break;
                    }
                case 4: // Carved Wooden Screen
                    {
                        Item item = new CarvedWoodenScreen();
                        item.LootType = LootType.Blessed;
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(SpringTokenGump));
                        m_Deed.Delete();
                        break;
                    }
                case 5: // Throw Pillow
                    {
                        Item item = new ThrowPillow();
                        item.LootType = LootType.Blessed;
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(SpringTokenGump));
                        m_Deed.Delete();
                        break;
                    }
                case 6: // Dragon Brazier
                    {
                        Item item = new DragonBrazier();
                        item.LootType = LootType.Blessed;
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(SpringTokenGump));
                        m_Deed.Delete();
                        break;
                    }
                case 7: // Navigator's World Map
                    {
                        Item item = new NavigatorsWorldMap();
                        item.LootType = LootType.Blessed;
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(SpringTokenGump));
                        m_Deed.Delete();
                        break;
                    }
                case 8: // Basket of Herbs
                    {
                        Item item = new BasketOfHerbs();
                        item.LootType = LootType.Blessed;
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(SpringTokenGump));
                        m_Deed.Delete();
                        break;

                    }
                case 9: // Shochu
                    {
                        Item item = new Shochu();
                        item.LootType = LootType.Blessed;
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(SpringTokenGump));
                        m_Deed.Delete();
                        break;

                    }
                case 10: // Mariner's Brass Sextant
                    {
                        Item item = new MarinersBrassSextant();
                        item.LootType = LootType.Blessed;
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(SpringTokenGump));
                        m_Deed.Delete();
                        break;

                    }
                case 11: // Heartwood Chest
                    {
                        Item item = new HeartwoodChest();
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(SpringTokenGump));
                        m_Deed.Delete();
                        break;

                    }
                case 12: // Low Yew Table
                    {
                        Item item = new LowYewTable();
                        item.LootType = LootType.Blessed;
                        from.AddToBackpack(item);
                        from.CloseGump(typeof(SpringTokenGump));
                        m_Deed.Delete();
                        break;

                    }
            }
        }
    }
}
