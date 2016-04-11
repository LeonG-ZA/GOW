using System;
using Server.Items;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Engines.Craft
{
    public abstract class AllClothCraft : CustomCraft
    {
        private BaseCloth m_AllCloth;

        public BaseCloth AllCloth { get { return m_AllCloth; } }

        public AllClothCraft(Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, int quality)
            : base(from, craftItem, craftSystem, typeRes, tool, quality)
        {
        }

        private int Verify(BaseCloth mistcloth)
        {
            if (!(mistcloth is BaseCloth))
            {
                return 1044456;
            }

            return 0;
        }

        private bool Acquire(object target, out int message)
        {
            BaseCloth allcloth = target as BaseCloth;

            message = Verify(allcloth);

            if (message > 0)
            {
                return false;
            }
            else
            {
                m_AllCloth = allcloth;
                return true;
            }
        }

        public override void EndCraftAction()
        {
            From.SendLocalizedMessage(1074794);
            From.Target = new AllClothTarget(this);
        }

        private class AllClothTarget : Target
        {
            private AllClothCraft m_AllCraft;

            public AllClothTarget(AllClothCraft allclothCraft)
                : base(1, false, TargetFlags.None)
            {
                m_AllCraft = allclothCraft;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                int message;

                if (!(targeted is BaseCloth))
                {
                    Failure(500294);
                    return;
                }

                if (m_AllCraft.Acquire(targeted, out message))
                {
                    m_AllCraft.CraftItem.CompleteCraft(m_AllCraft.Quality, false, m_AllCraft.From, m_AllCraft.CraftSystem, m_AllCraft.TypeRes, m_AllCraft.Tool, m_AllCraft);
                }
                else
                {
                    Failure(message);
                }
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if (cancelType == TargetCancelType.Canceled)
                    Failure(0);
            }

            private void Failure(int message)
            {
                Mobile from = m_AllCraft.From;
                BaseTool tool = m_AllCraft.Tool;

                if (tool != null && !tool.Deleted && tool.UsesRemaining > 0)
                {
                    from.SendGump(new CraftGump(from, m_AllCraft.CraftSystem, tool, message));
                }
                else if (message > 0)
                {
                    from.SendLocalizedMessage(message);
                }
            }
        }

        public override Item CompleteCraft(out int message)
        {
            message = Verify(this.AllCloth);

            if (message == 0)
            {
                Container pack = From.Backpack;

                List<Item> packItems = new List<Item>(pack.Items);
                int Number;

                for (int j = 0; j < 3000; ++j)
                {
                    Number = 0;

                    for (int i = 0; i < packItems.Count; i++)
                    {
                        Item packItem = packItems[i];

                        if (packItem is OilCloth)
                        {
                            packItem.Hue = 0;
                        }

                        if (packItem is BaseCloth && packItem.Hue == j)
                        {
                            Number += packItem.Amount;
                            packItem.Delete();
                        }
                    }

                    if (Number > 0)
                    {
                        UncutCloth cut = new UncutCloth();
                        cut.Hue = j;
                        cut.Amount = Number;
                        pack.DropItem(cut);
                    }
                }
            }
            return null;
        }
    }

    [CraftItemID(0x1BFC)]
    public class AllClothCrafting : AllClothCraft
    {
        public AllClothCrafting(Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, int quality)
            : base(from, craftItem, craftSystem, typeRes, tool, quality)
        {
        }
    }
}