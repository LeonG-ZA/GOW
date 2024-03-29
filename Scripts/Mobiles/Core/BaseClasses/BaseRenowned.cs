using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public abstract class BaseRenowned : BaseCreature
    {
        Dictionary<Mobile, int> m_DamageEntries;
        public BaseRenowned(AIType aiType)
            : this(aiType, FightMode.Closest)
        {
        }

        public BaseRenowned(AIType aiType, FightMode mode)
            : base(aiType, mode, 18, 1, 0.1, 0.2)
        {
        }

        public BaseRenowned(Serial serial)
            : base(serial)
        {
        }

        public abstract Type[] UniqueSAList { get; }
        public abstract Type[] SharedSAList { get; }
        public virtual bool NoGoodies
        {
            get
            {
                return false;
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

        public virtual void RegisterDamageTo(Mobile m)
        {
            if (m == null)
                return;

            foreach (DamageEntry de in m.DamageEntries)
            {
                Mobile damager = de.Damager;

                Mobile master = damager.GetDamageMaster(m);

                if (master != null)
                    damager = master;

                RegisterDamage(damager, de.DamageGiven);
            }
        }

        public void RegisterDamage(Mobile from, int amount)
        {
            if (from == null || !from.Player)
                return;

            if (m_DamageEntries.ContainsKey(from))
                m_DamageEntries[from] += amount;
            else
                m_DamageEntries.Add(from, amount);
        }

        public void AwardArtifact(Item artifact)
        {
            if (artifact == null)
                return;

            int totalDamage = 0;

            Dictionary<Mobile, int> validEntries = new Dictionary<Mobile, int>();

            foreach (KeyValuePair<Mobile, int> kvp in m_DamageEntries)
            {
                if (IsEligible(kvp.Key, artifact))
                {
                    validEntries.Add(kvp.Key, kvp.Value);
                    totalDamage += kvp.Value;
                }
            }

            int randomDamage = Utility.RandomMinMax(1, totalDamage);

            totalDamage = 0;

            foreach (KeyValuePair<Mobile, int> kvp in m_DamageEntries)
            {
                totalDamage += kvp.Value;

                if (totalDamage > randomDamage)
                {
                    GiveArtifact(kvp.Key, artifact);
                    break;
                }
            }
        }

        public void GiveArtifact(Mobile to, Item artifact)
        {
            if (to == null || artifact == null)
                return;

            Container pack = to.Backpack;

            if (pack == null || !pack.TryDropItem(to, artifact, false))
                artifact.Delete();
            else
                to.SendLocalizedMessage(1062317); // For your valor in combating the fallen beast, a special artifact has been bestowed on you.
        }

        public bool IsEligible(Mobile m, Item Artifact)
        {
            return m.Player && m.Alive && m.InRange(Location, 32) && m.Backpack != null && m.Backpack.CheckHold(m, Artifact, false);
        }

        public Item GetArtifact()
        {
            double random = Utility.RandomDouble();
            if (0.05 >= random)
                return CreateArtifact(UniqueSAList);
            else if (0.15 >= random)
                return CreateArtifact(SharedSAList);

            return null;
        }

        public Item CreateArtifact(Type[] list)
        {
            if (list.Length == 0)
                return null;

            int random = Utility.Random(list.Length);

            Type type = list[random];

            Item artifact = Loot.Construct(type);

            return artifact;
        }

        public override bool OnBeforeDeath()
        {
            if (!NoKillAwards)
            {
                if (NoGoodies)
                    return base.OnBeforeDeath();

                Map map = Map;

                if (map != null)
                {
                    for (int x = -12; x <= 12; ++x)
                    {
                        for (int y = -12; y <= 12; ++y)
                        {
                            double dist = Math.Sqrt(x * x + y * y);

                            
                        }
                    }
                }

                m_DamageEntries = new Dictionary<Mobile, int>();

                RegisterDamageTo(this);
                AwardArtifact(GetArtifact());
            }

            return base.OnBeforeDeath();
        }

        public override void OnDeath(Container c)
        {
            if (Map == Map.Felucca || Map == Map.TerMur)
            {
                //TODO: Confirm SE change or AoS one too?
                List<DamageStore> rights = BaseCreature.GetLootingRights(DamageEntries, HitsMax);
                List<Mobile> toGive = new List<Mobile>();

                for (int i = rights.Count - 1; i >= 0; --i)
                {
                    DamageStore ds = rights[i];

                    if (ds.m_HasRight)
                        toGive.Add(ds.m_Mobile);
                }
            }

            base.OnDeath(c);
        }
    }
}