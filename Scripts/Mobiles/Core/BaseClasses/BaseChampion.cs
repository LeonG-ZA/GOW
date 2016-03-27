using System;
using System.Collections.Generic;
using Server.Engines.ChampionSpawns;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
    public abstract class BaseChampion : BaseCreature
    {
        public override bool CanMoveOverObstacles { get { return true; } }
        public override bool CanDestroyObstacles { get { return true; } }

        public BaseChampion(AIType aiType)
            : this(aiType, FightMode.Closest)
        {
        }

        public BaseChampion(AIType aiType, FightMode mode)
            : base(aiType, mode, 18, 1, 0.1, 0.2)
        {
        }

        public BaseChampion(Serial serial)
            : base(serial)
        {
        }

        public abstract ChampionSkullType SkullType { get; }
        public abstract Type[] UniqueList { get; }
        public abstract Type[] SharedList { get; }
        public abstract Type[] DecorativeList { get; }
        public abstract MonsterStatuetteType[] StatueTypes { get; }

        public virtual bool NoGoodies
        {
            get
            {
                return false;
            }
        }

        public static void GivePowerScrollTo(Mobile m, PowerScroll ps)
        {
            if (ps == null || m == null)	//sanity
            {
                return;
            }

            m.SendLocalizedMessage(1049524); // You have received a scroll of power!

            if (!Core.SE || m.Alive)
            {
                m.AddToBackpack(ps);
            }
            else
            {
                if (m.Corpse != null && !m.Corpse.Deleted)
                {
                    m.Corpse.DropItem(ps);
                }
                else
                {
                    m.AddToBackpack(ps);
                }
            }

            if (m is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)m;

                for (int j = 0; j < pm.JusticeProtectors.Count; ++j)
                {
                    Mobile prot = pm.JusticeProtectors[j];

                    if (prot.Map != m.Map || prot.Kills >= 5 || prot.Criminal || !JusticeVirtue.CheckMapRegion(m, prot))
                    {
                        continue;
                    }

                    int chance = 0;

                    switch (VirtueHelper.GetLevel(prot, VirtueName.Justice))
                    {
                        case VirtueLevel.Seeker: chance = 60; break;
                        case VirtueLevel.Follower: chance = 80; break;
                        case VirtueLevel.Knight: chance = 100; break;
                    }

                    if (chance > Utility.Random(100))
                    {
                        PowerScroll powerScroll = new PowerScroll(ps.Skill, ps.Value);

                        prot.SendLocalizedMessage(1049368); // You have been rewarded for your dedication to Justice!

                        if (!Core.SE || prot.Alive)
                        {
                            prot.AddToBackpack(powerScroll);
                        }
                        else
                        {
                            if (prot.Corpse != null && !prot.Corpse.Deleted)
                            {
                                prot.Corpse.DropItem(powerScroll);
                            }
                            else
                            {
                                prot.AddToBackpack(powerScroll);
                            }
                        }
                    }
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public Item GetArtifact()
        {
            double random = Utility.RandomDouble();
            if (0.05 >= random)
            {
                return this.CreateArtifact(this.UniqueList);
            }
            else if (0.15 >= random)
            {
                return this.CreateArtifact(this.SharedList);
            }
            else if (0.30 >= random)
            {
                return this.CreateArtifact(this.DecorativeList);
            }
            return null;
        }

        public Item CreateArtifact(Type[] list)
        {
            if (list.Length == 0)
            {
                return null;
            }

            int random = Utility.Random(list.Length);

            Type type = list[random];

            Item artifact = Loot.Construct(type);

            if (artifact is MonsterStatuette && this.StatueTypes.Length > 0)
            {
                ((MonsterStatuette)artifact).Type = this.StatueTypes[Utility.Random(this.StatueTypes.Length)];
                ((MonsterStatuette)artifact).LootType = LootType.Regular;
            }

            return artifact;
        }

        public void GiveRewards()
        {
            //List<Mobile> toGive = new List<Mobile>();
            ArrayList toGive = new ArrayList();
            List<DamageStore> rights = BaseCreature.GetLootingRights(this.DamageEntries, this.HitsMax);

            for (int i = rights.Count - 1; i >= 0; --i)
            {
                DamageStore ds = rights[i];

                if (ds.m_HasRight)
                {
                    toGive.Add(ds.m_Mobile);
                }
            }

            if (toGive.Count == 0)
            {
                return;
            }

            // Randomize
            for (int i = 0; i < toGive.Count; ++i)
            {
                int rand = Utility.Random(toGive.Count);
                object hold = toGive[i];
                toGive[i] = toGive[rand];
                toGive[rand] = hold;
            }

            GivePowerScrolls(toGive);
            GiveValor(toGive);
            GiveTitles(toGive);
        }

        public void GivePowerScrolls(ArrayList toGive)
        {
            if (NoKillAwards)
            {
                return;
            }

            if (Map != Map.Felucca)
            {
                return;
            }

            for (int i = 0; i < toGive.Count; i++)
            {
                Mobile m = (Mobile)toGive[i];

                if (!(m.Map == this.Map && m.InRange(this, 90)) || (!m.Alive && m.Corpse == null))
                {
                    toGive.Remove(m);
                }
            }

            if (toGive.Count == 0)
            {
                return;
            }

            for (int i = 0; i < 6; ++i)
            {
                int level;
                double random = Utility.RandomDouble();

                /* Powerscroll type chance:
                 * - 80% chance to get a 110 powerscroll
                 * - 15% chance to get a 115 powerscroll
                 * - 5% chance to get a 120 powerscroll
                 */

                if (0.05 >= random)
                {
                    level = 20;
                }
                else if (0.2 >= random)
                {
                    level = 15;
                }
                else
                {
                    level = 10;
                }

                Mobile m = (Mobile)toGive[i % toGive.Count];

                PowerScroll ps = PowerScroll.CreateRandomNoCraft(level, level);

                m.SendLocalizedMessage(1049524); // You have received a scroll of power!

                if (m.Alive)
                {
                    m.AddToBackpack(ps);
                }
                else
                {
                    Container corpse = m.Corpse;

                    if (corpse != null)
                    {
                        corpse.DropItem(ps);
                    }
                }

                if (m is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile)m;

                    for (int j = 0; j < pm.JusticeProtectors.Count; ++j)
                    {
                        Mobile prot = (Mobile)pm.JusticeProtectors[j];

                        if (prot.Map != m.Map || prot.Kills >= 5 || prot.Criminal || !JusticeVirtue.CheckMapRegion(m, prot))
                        {
                            continue;
                        }

                        int chance = 0;

                        switch (VirtueHelper.GetLevel(prot, VirtueName.Justice))
                        {
                            case VirtueLevel.Seeker: chance = 60; break;
                            case VirtueLevel.Follower: chance = 80; break;
                            case VirtueLevel.Knight: chance = 100; break;
                        }

                        if (chance > Utility.Random(100))
                        {
                            PowerScroll pps = PowerScroll.CreateRandomNoCraft(level, level);
                            prot.SendLocalizedMessage(1049368); // You have been rewarded for your dedication to Justice!
                            prot.AddToBackpack(pps);
                        }
                    }
                }
            }
        }

        public void GiveValor(ArrayList toGive)
        {
            for (int i = 0; i < toGive.Count; ++i)
            {
                Mobile m = (Mobile)toGive[i];

                if (!(m is PlayerMobile))
                {
                    continue;
                }

                bool gainedPath = false;

                int pointsToGain = 800;

                if (VirtueHelper.Award(m, VirtueName.Valor, pointsToGain, ref gainedPath))
                {
                    if (gainedPath)
                    {
                        m.SendLocalizedMessage(1054032); // You have gained a path in Valor!
                    }
                    else
                    {
                        m.SendLocalizedMessage(1054030); // You have gained in Valor!
                    }

                    // No delay on Valor gains
                }
                else
                {
                    m.SendLocalizedMessage(1054031); // You have achieved the highest path in Valor and can no longer gain any further.
                }
            }
        }

        public void GiveTitles(ArrayList toGive)
        {
            // TODO: verify scores for killing champions

            for (int i = 0; i < toGive.Count; ++i)
            {
                PlayerMobile pm = (PlayerMobile)toGive[i % toGive.Count];

                switch (SkullType)
                {
                    case ChampionSkullType.Power:
                        pm.ChampionTiers[0] += 2;
                        pm.SuperChampionTiers[1]++;
                        break;
                    case ChampionSkullType.Enlightenment:
                        pm.ChampionTiers[1] += 2;
                        pm.SuperChampionTiers[2]++;
                        break;
                    case ChampionSkullType.Venom:
                        pm.ChampionTiers[2] += 2;
                        pm.SuperChampionTiers[3]++;
                        break;
                    case ChampionSkullType.Pain:
                        pm.ChampionTiers[3] += 2;
                        pm.SuperChampionTiers[4]++;
                        break;
                    case ChampionSkullType.Greed:
                        pm.ChampionTiers[4] += 2;
                        pm.SuperChampionTiers[5]++;
                        break;
                    case ChampionSkullType.Death:
                        pm.ChampionTiers[5] += 2;
                        pm.SuperChampionTiers[6]++;
                        break;
                }

                if (this is Serado)
                {
                    pm.ChampionTiers[6] += 2;
                    pm.SuperChampionTiers[7]++;
                }

                if (this is Ilhenir)
                {
                    pm.ChampionTiers[7] += 2;
                    pm.SuperChampionTiers[8]++;
                }

                if (this is Twaulo)
                {
                    pm.ChampionTiers[8] += 2;
                    pm.SuperChampionTiers[9]++;
                }

                if (this is PrimevalLich)
                {
                    pm.ChampionTiers[9] += 2;
                    pm.SuperChampionTiers[10]++;
                }

                if (this is AbyssalInfernal)
                {
                    pm.ChampionTiers[10] += 2;
                    pm.SuperChampionTiers[11]++;
                }
            }
        }

        public override bool OnBeforeDeath()
        {
            GiveRewards();

            if (!this.NoKillAwards)
            {
                if (this.NoGoodies)
                {
                    return base.OnBeforeDeath();
                }

                DoForChamp(Location, Map);
            }

            return base.OnBeforeDeath();
        }

        public static readonly int Goldpiles = 50;
        public static readonly int GoldminAmount = 2500;
        public static readonly int GoldmaxAmount = 7500;

        public static void DoForChamp(Point3D center, Map map)
        {
            Do(center, map, Goldpiles, GoldminAmount, GoldmaxAmount);
        }

        public static void Do(Point3D center, Map map, int piles, int minAmount, int maxAmount)
        {
            new GoodiesTimer(center, map, piles, minAmount, maxAmount).Start();
        }

        public override void OnDeath(Container c)
        {
            if (this.Map == Map.Felucca)
            {
                //TODO: Confirm SE change or AoS one too?
                List<DamageStore> rights = BaseCreature.GetLootingRights(this.DamageEntries, this.HitsMax);
                List<Mobile> toGive = new List<Mobile>();

                for (int i = rights.Count - 1; i >= 0; --i)
                {
                    DamageStore ds = rights[i];

                    if (ds.m_HasRight)
                    {
                        toGive.Add(ds.m_Mobile);
                    }
                }

                if (toGive.Count > 0)
                {
                    toGive[Utility.Random(toGive.Count)].AddToBackpack(new ChampionSkull(this.SkullType));
                }
                else
                {
                    c.DropItem(new ChampionSkull(this.SkullType));
                }
            }

            base.OnDeath(c);
        }

        private PowerScroll CreateRandomPowerScroll()
        {
            int level;
            double random = Utility.RandomDouble();

            if (0.05 >= random)
            {
                level = 20;
            }
            else if (0.4 >= random)
            {
                level = 15;
            }
            else
            {
                level = 10;
            }

            return PowerScroll.CreateRandomNoCraft(level, level);
        }

        private class GoodiesTimer : Timer
        {
            private readonly Map m_Map;
            private readonly Point3D m_Location;
            private readonly int m_PilesMax;
            private int m_PilesDone = 0;
            private readonly int m_MinAmount;
            private readonly int m_MaxAmount;
            public GoodiesTimer(Point3D center, Map map, int piles, int minAmount, int maxAmount)
                : base(TimeSpan.FromSeconds(0.25d), TimeSpan.FromSeconds(0.25d))
            {
                m_Location = center;
                m_Map = map;
                m_PilesMax = piles;
                m_MinAmount = minAmount;
                m_MaxAmount = maxAmount;
            }

            protected override void OnTick()
            {
                if (m_PilesDone >= m_PilesMax)
                {
                    Stop();
                    return;
                }

                Point3D p = FindGoldLocation(m_Map, m_Location, m_PilesMax / 8);
                Gold g = new Gold(m_MinAmount, m_MaxAmount);
                g.DropToWorld(p, this.m_Map);
                switch (Utility.Random(3))
                {
                    case 0: // Fire column
                        {
                            Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
                            Effects.PlaySound(g, g.Map, 0x208);
                            break;
                        }
                    case 1: // Explosion
                        {
                            Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
                            Effects.PlaySound(g, g.Map, 0x307);
                            break;
                        }
                    case 2: // Ball of fire
                        {
                            Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36FE, 10, 10, 5052);
                            break;
                        }
                }
                ++m_PilesDone;
            }
            private static Point3D FindGoldLocation(Map map, Point3D center, int range)
            {
                int cx = center.X;
                int cy = center.Y;

                for (int i = 0; i < 20; ++i)
                {
                    int x = cx + Utility.Random(range * 2) - range;
                    int y = cy + Utility.Random(range * 2) - range;
                    if ((cx - x) * (cx - x) + (cy - y) * (cy - y) > range * range)
                    {
                        continue;
                    }

                    int z = map.GetAverageZ(x, y);
                    if (!map.CanFit(x, y, z, 6, false, false))
                    {
                        continue;
                    }

                    int topZ = z;
                    foreach (Item item in map.GetItemsInRange(new Point3D(x, y, z), 0))
                    {
                        topZ = Math.Max(topZ, item.Z + item.ItemData.CalcHeight);
                    }
                    return new Point3D(x, y, topZ);
                }
                return center;
            }
        }
    }
}