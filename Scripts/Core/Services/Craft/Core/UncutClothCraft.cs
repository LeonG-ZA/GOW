using System;
using Server.Items;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Engines.Craft
{
    public abstract class UnCutBoltCraft : CustomCraft
    {
        private BoltOfCloth m_Bolt;

        public BoltOfCloth BoltCloth { get { return m_Bolt; } }

        public UnCutBoltCraft(Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, int quality)
            : base(from, craftItem, craftSystem, typeRes, tool, quality)
        {
        }

        private int Verify(BoltOfCloth boltcloth)
        {
            if (!(boltcloth is BoltOfCloth))
                return 1044454;

            return 0;
        }

        private bool Acquire(object target, out int message)
        {
            BoltOfCloth boltcloth = target as BoltOfCloth;

            message = Verify(boltcloth);

            if (message > 0)
            {
                return false;
            }
            else
            {
                m_Bolt = boltcloth;
                return true;
            }
        }

        public override void EndCraftAction()
        {
            From.SendLocalizedMessage(1046438);
            From.Target = new BoltClothTarget(this);
        }

        private class BoltClothTarget : Target
        {
            private UnCutBoltCraft m_BoltCraft;

            public BoltClothTarget(UnCutBoltCraft boltCraft)
                : base(1, false, TargetFlags.None)
            {
                m_BoltCraft = boltCraft;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                int message;

                if (!(targeted is BoltOfCloth))
                {
                    Failure(1044454);
                    return;
                }

                if (m_BoltCraft.Acquire(targeted, out message))
                    m_BoltCraft.CraftItem.CompleteCraft(m_BoltCraft.Quality, false, m_BoltCraft.From, m_BoltCraft.CraftSystem, m_BoltCraft.TypeRes, m_BoltCraft.Tool, m_BoltCraft);
                else
                    Failure(message);
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if (cancelType == TargetCancelType.Canceled)
                    Failure(0);
            }

            private void Failure(int message)
            {
                Mobile from = m_BoltCraft.From;
                BaseTool tool = m_BoltCraft.Tool;

                if (tool != null && !tool.Deleted && tool.UsesRemaining > 0)
                    from.SendGump(new CraftGump(from, m_BoltCraft.CraftSystem, tool, message));
                else if (message > 0)
                    from.SendLocalizedMessage(message);
            }
        }

        public override Item CompleteCraft(out int message)
        {
            message = Verify(this.BoltCloth);

            if (message == 0)
            {
                Container pack = From.Backpack;

                List<Item> packItems = new List<Item>(pack.Items);
                int Number = 0;

                for (int i = 0; i < packItems.Count; i++)
                {
                    Item packItem = packItems[i];

                    if (packItem is BoltOfCloth)
                    {
                        Number += packItem.Amount;
                        packItem.Delete();
                    }
                }

                if (Number > 0)
                {
                    UncutCloth cut = new UncutCloth();
                    cut.Amount = Number * 50;
                    cut.Hue = 0;
                    pack.DropItem(cut);
                }
            }
            return null;
        }
    }

    [CraftItemID(0x1BFC)]
    public class UncutClothCraft : UnCutBoltCraft
    {
        public UncutClothCraft(Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, int quality)
            : base(from, craftItem, craftSystem, typeRes, tool, quality)
        {
        }
    }
}