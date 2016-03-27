using System;
using System.Collections.Generic;
using Server.Engines.PartySystem;
using Server.Guilds;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Regions;
using Server.Spells.Fifth;
using Server.Spells.Necromancy;
using Server.Spells.Ninjitsu;
using Server.Spells.Seventh;
using Server.Targeting;
using Server.MainConfiguration;
using Server.SpellsConfiguration;
using Server.LogConsole;
using System.Collections;
using Server.Engines.ChampionSpawns;
using Server.Spells.Spellweaving;
using System.IO; 
using System.Xml; 
using System.Reflection;
using Server.Engines.ConPVP; 


namespace Server
{
    public class DefensiveSpell
    {
        public static void Nullify(Mobile from)
        {
            if (!from.CanBeginAction(typeof(DefensiveSpell)))
                new InternalTimer(from).Start();
        }

        private class InternalTimer : Timer
        {
            private readonly Mobile m_Mobile;

            public InternalTimer(Mobile m)
                : base(TimeSpan.FromMinutes(1.0))
            {
                this.m_Mobile = m;

                this.Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                this.m_Mobile.EndAction(typeof(DefensiveSpell));
            }
        }
    }
}

namespace Server.Spells
{
    public enum TravelCheckType
    {
        RecallFrom,
        RecallTo,
        GateFrom,
        GateTo,
        Mark,
        TeleportFrom,
        TeleportTo
    }

    public class SpellHelper
    {
        public static bool HasSpellMastery(Mobile m)
        {
            //Publish 71 PVP Spell damage increase cap changes. If you have GM of one school of magic and no others, you are "focused" in that school of magic and have 30% sdi cap instead of 15%.
            List<SkillName> schools = new List<SkillName>()
            {
                SkillName.Magery,
                SkillName.AnimalTaming,
                SkillName.Musicianship,
                SkillName.Mysticism,
                SkillName.Spellweaving,
                SkillName.Chivalry,
                SkillName.Necromancy,
                SkillName.Bushido,
                SkillName.Ninjitsu
            };

            bool spellMastery = false;

            foreach (SkillName skill in schools)
            {
                if (m.Skills[skill].Base >= 30.0 && spellMastery)
                    return false;
                if (m.Skills[skill].Base >= 100.0)
                    spellMastery = true;
            }
            return spellMastery;
        }

        private static readonly TimeSpan AosDamageDelay = SpellsConfig.SpellsAosDamageDelay;
        private static readonly TimeSpan OldDamageDelay = SpellsConfig.SpellsOldDamageDelay;

        public static TimeSpan GetDamageDelayForSpell(Spell sp)
        {
            if (!sp.DelayedDamage)
                return TimeSpan.Zero;

            return (Core.AOS ? AosDamageDelay : OldDamageDelay);
        }

        public static bool CheckMulti(Point3D p, Map map)
        {
            return CheckMulti(p, map, true, 0);
        }
		
        public static bool CheckMulti(Point3D p, Map map, bool houses)
        {
            return CheckMulti(p, map, houses, 0);
        }
		
        public static bool CheckMulti(Point3D p, Map map, bool houses, int housingrange)
        {
            if (map == null || map == Map.Internal)
                return false;

            Sector sector = map.GetSector(p.X, p.Y);

            for (int i = 0; i < sector.Multis.Count; ++i)
            {
                BaseMulti multi = sector.Multis[i];

                if (multi is BaseHouse)
                {
                    BaseHouse bh = (BaseHouse)multi;

                    if ((houses && bh.IsInside(p, 16)) || (housingrange > 0 && bh.InRange(p, housingrange)))
                        return true;
                }
                else if (multi.Contains(p))
                {
                    return true;
                }
            }
			
            return false;
        }

        public static void Turn(Mobile from, object to)
        {
            IPoint3D target = to as IPoint3D;

            if (target == null)
                return;

            if (target is Item)
            {
                Item item = (Item)target;

                if (item.RootParent != from)
                    from.Direction = from.GetDirectionTo(item.GetWorldLocation());
            }
            else if (from != target)
            {
                from.Direction = from.GetDirectionTo(target);
            }
        }

        private static readonly TimeSpan CombatHeatDelay = SpellsConfig.SpellsCombatHeatDelay;
        private static readonly bool RestrictTravelCombat = SpellsConfig.SpellsRestrictTravelCombat;

        public static bool CheckCombat(Mobile m)
        {
            if (!RestrictTravelCombat)
                return false;

            for (int i = 0; i < m.Aggressed.Count; ++i)
            {
                AggressorInfo info = m.Aggressed[i];

                if (info.Defender.Player && (DateTime.UtcNow - info.LastCombatTime) < CombatHeatDelay)
                    return true;
            }

            if (Core.Expansion == Expansion.AOS)
            {
                for (int i = 0; i < m.Aggressors.Count; ++i)
                {
                    AggressorInfo info = m.Aggressors[i];

                    if (info.Attacker.Player && (DateTime.UtcNow - info.LastCombatTime) < CombatHeatDelay)
                        return true;
                }
            }

            return false;
        }

        public static bool AdjustField(ref Point3D p, Map map, int height, bool mobsBlock)
        {
            if (map == null)
                return false;

            for (int offset = 0; offset < 10; ++offset)
            {
                Point3D loc = new Point3D(p.X, p.Y, p.Z - offset);

                if (map.CanFit(loc, height, true, mobsBlock))
                {
                    p = loc;
                    return true;
                }
            }

            return false;
        }
		
        public static bool CanRevealCaster(Mobile m)
        {
            if (m is BaseCreature)
            {
                BaseCreature c = (BaseCreature)m;
						
                if (!c.Controlled)
                    return true;
            }
			
            return false;
        }

        public static void GetSurfaceTop(ref IPoint3D p)
        {
            if (p is Item)
            {
                p = ((Item)p).GetSurfaceTop();
            }
            else if (p is StaticTarget)
            {
                StaticTarget t = (StaticTarget)p;
                int z = t.Z;

                if ((t.Flags & TileFlag.Surface) == 0)
                    z -= TileData.ItemTable[t.ItemID & TileData.MaxItemValue].CalcHeight;

                p = new Point3D(t.X, t.Y, z);
            }
        }

        public static bool AddStatOffset(Mobile m, StatType type, int offset, TimeSpan duration)
        {
            if (offset > 0)
                return AddStatBonus(m, m, type, offset, duration);
            else if (offset < 0)
                return AddStatCurse(m, m, type, -offset, duration);

            return true;
        }

        public static bool AddStatBonus(Mobile caster, Mobile target, StatType type)
        {
            return AddStatBonus(caster, target, type, GetOffset(caster, target, type, false), GetDuration(caster, target));
        }

        public static bool AddStatBonus(Mobile caster, Mobile target, StatType type, int bonus, TimeSpan duration)
        {
            int offset = bonus;
            string name = String.Format("[Magic] {0} Offset", type);

            StatMod mod = target.GetStatMod(name);

            if (mod != null && mod.Offset < 0)
            {
                target.AddStatMod(new StatMod(type, name, mod.Offset + offset, duration));
                return true;
            }
            else if (mod == null || mod.Offset < offset)
            {
                target.AddStatMod(new StatMod(type, name, offset, duration));
                return true;
            }

            return false;
        }

        public static bool AddStatCurse(Mobile caster, Mobile target, StatType type)
        {
            return AddStatCurse(caster, target, type, GetOffset(caster, target, type, true), GetDuration(caster, target));
        }

        public static bool AddStatCurse(Mobile caster, Mobile target, StatType type, int curse, TimeSpan duration)
        {
            int offset = -curse;
            string name = String.Format("[Magic] {0} Offset", type);

            StatMod mod = target.GetStatMod(name);

            if (mod != null && mod.Offset > 0)
            {
                target.AddStatMod(new StatMod(type, name, mod.Offset + offset, duration));
                return true;
            }
            else if (mod == null || mod.Offset > offset)
            {
                target.AddStatMod(new StatMod(type, name, offset, duration));
                return true;
            }

            return false;
        }

        public static TimeSpan GetDuration(Mobile caster, Mobile target)
        {
            if (Core.AOS)
            {
                return TimeSpan.FromSeconds(((caster.Skills.EvalInt.Fixed / 50) + 1) * 6);
            }
            else
            {
                return TimeSpan.FromSeconds(caster.Skills[SkillName.Magery].Value * 1.2);
            }
        }

        private static bool m_DisableSkillCheck;

        public static bool DisableSkillCheck
        {
            get
            {
                return m_DisableSkillCheck;
            }
            set
            {
                m_DisableSkillCheck = value;
            }
        }

        public static double GetOffsetScalar(Mobile caster, Mobile target, bool curse)
        {
            double percent;

            if (curse)
                percent = 8 + (caster.Skills.EvalInt.Fixed / 100) - (target.Skills.MagicResist.Fixed / 100);
            else
                percent = 1 + (caster.Skills.EvalInt.Fixed / 100);

            percent *= 0.01;

            if (percent < 0)
                percent = 0;

            return percent;
        }

        public static int GetOffset(Mobile caster, Mobile target, StatType type, bool curse)
        {
            if (Core.AOS)
            {
                if (!m_DisableSkillCheck)
                {
                    caster.CheckSkill(SkillName.EvalInt, 0.0, 120.0);

                    if (curse)
                        target.CheckSkill(SkillName.MagicResist, 0.0, 120.0);
                }

                double percent = GetOffsetScalar(caster, target, curse);

                switch( type )
                {
                    case StatType.Str:
                        return (int)(target.RawStr * percent);
                    case StatType.Dex:
                        return (int)(target.RawDex * percent);
                    case StatType.Int:
                        return (int)(target.RawInt * percent);
                }
            }

            return 1 + (int)(caster.Skills[SkillName.Magery].Value * 0.1);
        }

        public static Guild GetGuildFor(Mobile m)
        {
            Guild g = m.Guild as Guild;

            if (g == null && m is BaseCreature)
            {
                BaseCreature c = (BaseCreature)m;
                m = c.ControlMaster;

                if (m != null)
                    g = m.Guild as Guild;

                if (g == null)
                {
                    m = c.SummonMaster;

                    if (m != null)
                        g = m.Guild as Guild;
                }
            }

            return g;
        }

        public static bool ValidIndirectTarget(Mobile from, Mobile to)
        {
            if (from == to)
                return true;

            if (to.Hidden && to.AccessLevel > from.AccessLevel)
                return false;

            PlayerMobile pmFrom = from as PlayerMobile;
            PlayerMobile pmTarg = to as PlayerMobile;

            if (pmFrom == null && from is BaseCreature)
            {
                BaseCreature bcFrom = (BaseCreature)from;

                if (bcFrom.Summoned)
                    pmFrom = bcFrom.SummonMaster as PlayerMobile;
            }

            if (pmTarg == null && to is BaseCreature)
            {
                BaseCreature bcTarg = (BaseCreature)to;

                if (bcTarg.Summoned)
                    pmTarg = bcTarg.SummonMaster as PlayerMobile;
            }

            if (pmFrom != null && pmTarg != null)
            {
                if (pmFrom.DuelContext != null && pmFrom.DuelContext == pmTarg.DuelContext && pmFrom.DuelContext.Started && pmFrom.DuelPlayer != null && pmTarg.DuelPlayer != null)
                    return (pmFrom.DuelPlayer.Participant != pmTarg.DuelPlayer.Participant);
            }

            Guild fromGuild = GetGuildFor(from);
            Guild toGuild = GetGuildFor(to);

            if (fromGuild != null && toGuild != null && (fromGuild == toGuild || fromGuild.IsAlly(toGuild)))
                return false;

            Party p = Party.Get(from);

            if (p != null && p.Contains(to))
                return false;

            if (to is BaseCreature)
            {
                BaseCreature c = (BaseCreature)to;

                if (c.Controlled || c.Summoned)
                {
                    if (c.ControlMaster == from || c.SummonMaster == from)
                        return false;

                    if (p != null && (p.Contains(c.ControlMaster) || p.Contains(c.SummonMaster)))
                        return false;
                }
            }

            if (from is BaseCreature)
            {
                BaseCreature c = (BaseCreature)from;

                if (c.Controlled || c.Summoned)
                {
                    if (c.ControlMaster == to || c.SummonMaster == to)
                        return false;

                    p = Party.Get(to);

                    if (p != null && (p.Contains(c.ControlMaster) || p.Contains(c.SummonMaster)))
                        return false;
                }
            }

            if (to is BaseCreature && !((BaseCreature)to).Controlled && ((BaseCreature)to).InitialInnocent)
                return true;

            int noto = Notoriety.Compute(from, to);

            return (noto != Notoriety.Innocent || from.Kills >= 5);
        }

        private static readonly int[] m_Offsets = new int[]
        {
            -1, -1,
            -1, 0,
            -1, 1,
            0, -1,
            0, 1,
            1, -1,
            1, 0,
            1, 1
        };

        public static void Summon(BaseCreature creature, Mobile caster, int sound, TimeSpan duration, bool scaleDuration, bool scaleStats)
        {
            Map map = caster.Map;

            if (map == null)
                return;

            double scale = 1.0 + ((caster.Skills[SkillName.Magery].Value - 100.0) / 200.0);

            if (scaleDuration)
                duration = TimeSpan.FromSeconds(duration.TotalSeconds * scale);

            if (scaleStats)
            {
                creature.RawStr = (int)(creature.RawStr * scale);
                creature.Hits = creature.HitsMax;

                creature.RawDex = (int)(creature.RawDex * scale);
                creature.Stam = creature.StamMax;

                creature.RawInt = (int)(creature.RawInt * scale);
                creature.Mana = creature.ManaMax;
            }

            Point3D p = new Point3D(caster);

            if (SpellHelper.FindValidSpawnLocation(map, ref p, true))
            {
                BaseCreature.Summon(creature, caster, p, sound, duration);
                return;
            }

            creature.Delete();
            caster.SendLocalizedMessage(501942); // That location is blocked.
        }

        public static bool FindValidSpawnLocation(Map map, ref Point3D p, bool surroundingsOnly)
        {
            if (map == null)	//sanity
                return false;

            if (!surroundingsOnly)
            {
                if (map.CanSpawnMobile(p))	//p's fine.
                {
                    p = new Point3D(p);
                    return true;
                }

                int z = map.GetAverageZ(p.X, p.Y);

                if (map.CanSpawnMobile(p.X, p.Y, z))
                {
                    p = new Point3D(p.X, p.Y, z);
                    return true;
                }
            }

            int offset = Utility.Random(8) * 2;

            for (int i = 0; i < m_Offsets.Length; i += 2)
            {
                int x = p.X + m_Offsets[(offset + i) % m_Offsets.Length];
                int y = p.Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];

                if (map.CanSpawnMobile(x, y, p.Z))
                {
                    p = new Point3D(x, y, p.Z);
                    return true;
                }
                else
                {
                    int z = map.GetAverageZ(x, y);

                    if (map.CanSpawnMobile(x, y, z))
                    {
                        p = new Point3D(x, y, z);
                        return true;
                    }
                }
            }

            return false;
        }
        /*
        public static void Configure() 
 		{ 
 			Console.Write("SpellHelper: Loading TravelRestrictions..."); 
 			if (LoadTravelRestrictions()) 
 				Console.WriteLine("done"); 
 			else 
 				Console.WriteLine("failed"); 
 		} 
 
 
 		public static bool LoadTravelRestrictions() 
 		{ 
 			string filePath = Path.Combine("Data", "TravelRestrictions.xml"); 
 
 
 			if (!File.Exists(filePath)) 
 				return false; 
 
 
 			XmlDocument x = new XmlDocument(); 
 			x.Load(filePath); 
 
 
 			try 
 			{ 
 				XmlElement e = x["TravelRestrictions"]; 
 				foreach (XmlElement r in e.GetElementsByTagName("Region")) 
 				{ 
 					if (!r.HasAttribute("Name")) 
 					{ 
 						Console.WriteLine("Warning: Missing 'Name' attribute in TravelRestrictions.xml"); 
 						continue; 
 					} 
 
 
 					string name = r.GetAttribute("Name"); 
 
 
 					if (m_TravelRestrictions.ContainsKey(name)) 
 					{ 
 						Console.WriteLine("Warning: Duplicate name '{0}' in TravelRestrictions.xml", name); 
 						continue; 
 					} 
 
 
 					if (!r.HasAttribute("Delegate")) 
 					{ 
 						Console.WriteLine("Warning: Missing 'Delegate' attribute in TravelRestrictions.xml"); 
 						continue; 
 					} 
 
 
 					string d = r.GetAttribute("Delegate"); 
 					// .NET 4.5 
 					//TravelValidator v = typeof(SpellHelper).GetMethod(d).CreateDelegate(typeof(TravelValidator)); 
 
 
 					MethodInfo m = typeof(SpellHelper).GetMethod(d); 
 					if (m == null) 
 					{ 
 						Console.WriteLine("Warning: TravelRestrictions.xml Delegate '{0}' not found in SpellHelper",d); 
 						continue; 
 					} 
 
 
 					TravelValidator v = (TravelValidator)Delegate.CreateDelegate(typeof(TravelValidator), m); 
 
 
 					TravelRules t = new TravelRules(); 
 					m_TravelRestrictions[name] = t; 
 
 
 					t.Validator = v; 
 
 
 					foreach (XmlElement rule in r) 
 					{ 
 						switch(rule.Name.ToLower()) 
 						{ 
 							case "recallfrom": t.RecallFrom = Utility.ToBoolean(rule.InnerText); break; 
 							case "recallto": t.RecallTo = Utility.ToBoolean(rule.InnerText); break; 
 							case "gatefrom": t.GateFrom = Utility.ToBoolean(rule.InnerText); break; 
 							case "gateto": t.GateTo = Utility.ToBoolean(rule.InnerText); break; 
 							case "mark": t.Mark = Utility.ToBoolean(rule.InnerText); break; 
 							case "teleportfrom": t.TeleportFrom = Utility.ToBoolean(rule.InnerText); break; 
 							case "teleportto": t.TeleportTo = Utility.ToBoolean(rule.InnerText); break; 
 							default: Console.WriteLine("Warning: Unknown element '{0}' in TravelRestrictions.xml", rule.Name); break; 
 						} 
 					} 
 				} 
 			} 
 			catch(Exception e) 
 			{ 
 				Console.WriteLine(e.ToString()); 
 				return false; 
 			} 
 
 
 			return true; 
 		} 
 
 
 		private struct TravelRules 
 		{ 
 			public TravelValidator Validator; 
 
 
 			public bool RecallFrom, RecallTo, GateFrom, GateTo, Mark, TeleportFrom, TeleportTo; 
 
 
 			public bool Allow(TravelCheckType t) 
 			{ 
 				switch(t) 
 				{ 
 					case TravelCheckType.RecallFrom: return RecallFrom; 
 					case TravelCheckType.RecallTo: return RecallTo; 
 					case TravelCheckType.GateFrom: return GateFrom; 
 					case TravelCheckType.GateTo: return GateTo; 
 					case TravelCheckType.Mark: return Mark; 
 					case TravelCheckType.TeleportFrom: return TeleportFrom; 
 					case TravelCheckType.TeleportTo: return TeleportTo; 
 					default: return false; 
 				} 
 			} 
 		} 
 
 
 		private static Dictionary<string, TravelRules> m_TravelRestrictions = new Dictionary<string, TravelRules>();
         */

        private delegate bool TravelValidator(Map map, Point3D loc);

        private static readonly TravelValidator[] m_Validators = new TravelValidator[]
        {
            new TravelValidator(IsFeluccaT2A),
            new TravelValidator(IsKhaldun),
            new TravelValidator(IsIlshenar),
            new TravelValidator(IsTrammelWind),
            new TravelValidator(IsFeluccaWind),
            new TravelValidator(IsFeluccaDungeon),
            new TravelValidator(IsTrammelSolenHive),
            new TravelValidator(IsFeluccaSolenHive),
            new TravelValidator(IsCrystalCave),
            new TravelValidator(IsDoomGauntlet),
            new TravelValidator(IsDoomFerry),
            new TravelValidator(IsSafeZone),
            new TravelValidator(IsFactionStronghold),
            new TravelValidator(IsChampionSpawn),
            new TravelValidator(IsGuardianRoom),
            new TravelValidator(IsTokunoDungeon),
            new TravelValidator(IsLampRoom),
            new TravelValidator(IsHeartwood),
            new TravelValidator(IsMLDungeon),
            new TravelValidator(IsSADungeon),
			new TravelValidator(IsTombOfKings),
			new TravelValidator(IsMazeOfDeath),
			new TravelValidator(IsSAEntrance),
        };

        private static bool[,] m_Rules = new bool[,]
			{
					/*T2A(Fel)	Khaldun, Ilshenar	    Wind(Tram),	Wind(Fel),	Dungeons(Fel),	Solen(Tram),	Solen(Fel), CrystalCave(Malas),	Gauntlet(Malas),	Gauntlet(Ferry),  SafeZone,	  Stronghold,		ChampionSpawn,	Guardian Room, 	Tokuno Dungeon, 	Lamp Room   Heartwood  		ML Dungeons		SA Dungeons		Tomb of Kings	Maze of Death	SA Entrance */
/* Recall From */	{ false,	false,   true, 		    true, 		false, 		false, 			true, 			false, 		false, 				false, 				false, 			  true,       true, 			false,			false, 			true, 				false, 		false,          false,			true,			true,			false,			false },
/* Recall To */		{ false, 	false,   false, 		false, 		false, 		false, 			false, 			false, 		false, 				false, 				false, 			  false,	  false, 			false,			false, 			false, 				false, 		false,          false,			false,			false,			false,			false }, 
/* Gate From */		{ false, 	false,   false, 		false, 		false, 		false, 			false, 			false, 		false, 				false, 				false, 			  false,	  false, 			false,			false, 			false, 				false, 		false,          false,			false,			false,			false,			false }, 
/* Gate To */		{ false, 	false,   false, 		false, 		false, 		false, 			false, 			false, 		false, 				false, 				false, 			  false,	  false, 			false,			false, 			false, 				false, 		false,          false,			false,			false,			false,			false },
/* Mark In */		{ false, 	false,   false, 		false, 		false, 		false, 			false, 			false, 		false, 				false, 				false, 			  false,	  false, 			false,			false, 			false, 				false, 		false,          false,			false,			false,			false,			false },
/* Tele From */		{ true, 	true,    true, 		    true, 		true, 		true, 			true, 			true, 		false, 				true, 				true, 			  true,	      false, 			true,			false, 			true, 				true, 		false,          false, 			true,			false,			false,			false }, 
/* Tele To */		{ true, 	true,    true, 		    true, 		true, 		true, 			true, 			true, 		false, 				true, 				false,			  false,	  false, 			true,			false, 			true, 				false, 		false,          false,			true,			false,			false,			false },
             };



        public static void SendInvalidMessage(Mobile caster, TravelCheckType type)
        {
            if (type == TravelCheckType.RecallTo || type == TravelCheckType.GateTo)
            {
                caster.SendLocalizedMessage(1019004); // You are not allowed to travel there.
            }
            else if (type == TravelCheckType.TeleportTo)
            {
                caster.SendLocalizedMessage(501035); // You cannot teleport from here to the destination.
            }
            else
                caster.SendLocalizedMessage(501802); // Thy spell doth not appear to work...
        }

        public static bool CheckTravel(Mobile caster, TravelCheckType type)
        {
            return CheckTravel(caster, caster.Map, caster.Location, type);
        }

        public static bool CheckTravel(Map map, Point3D loc, TravelCheckType type)
        {
            return CheckTravel(null, map, loc, type);
        }

        private static Mobile m_TravelCaster;
        private static TravelCheckType m_TravelType;

        public static bool CheckTravel(Mobile caster, Map map, Point3D loc, TravelCheckType type)
        {  
            if (IsInvalid(map, loc)) // null, internal, out of bounds
            {
                if (caster != null)
                    SendInvalidMessage(caster, type);

                return false;
            }

            if (caster != null && caster.IsPlayer() && caster.Region.IsPartOf(typeof(Regions.Jail)))
            {
                caster.SendLocalizedMessage(1114345); // You'll need a better jailbreak plan than that!
                return false;
            }

            // Always allow monsters to teleport
            if (caster is BaseCreature && (type == TravelCheckType.TeleportTo || type == TravelCheckType.TeleportFrom))
            {
                BaseCreature bc = (BaseCreature)caster;

                if (!bc.Controlled && !bc.Summoned)
                    return true;
            }

            int v = (int)type;
            bool isValid = true;

            m_TravelCaster = caster;
            m_TravelType = type;

            if (m_TravelCaster != null && m_TravelCaster.Region != null)
            {
                if (m_TravelCaster.Region.IsPartOf("Blighted Grove") && loc.Z < -10)
                    isValid = false;
            }

            /*foreach(TravelRules r in m_TravelRestrictions.Values) 
 			{ 
 				isValid = (r.Allow(type) || !r.Validator(map, loc)); 
 				if (!isValid) 
 					break; 
 			} 
             */

            for (int i = 0; isValid && i < m_Validators.Length; ++i)
                isValid = (m_Rules[v, i] || !m_Validators[i](map, loc));

            if (!isValid && caster != null)
                SendInvalidMessage(caster, type);

            return isValid;
        }

        public static bool IsWindLoc(Point3D loc)
        {
            int x = loc.X, y = loc.Y;

            return (x >= 5120 && y >= 0 && x < 5376 && y < 256);
        }

        public static bool IsFeluccaWind(Map map, Point3D loc)
        {
            return (map == Map.Felucca && IsWindLoc(loc));
        }

        public static bool IsTrammelWind(Map map, Point3D loc)
        {
            return (map == Map.Trammel && IsWindLoc(loc));
        }

        public static bool IsIlshenar(Map map, Point3D loc)
        {
            return (map == Map.Ilshenar);
        }

        public static bool IsSolenHiveLoc(Point3D loc)
        {
            int x = loc.X, y = loc.Y;

            return (x >= 5640 && y >= 1776 && x < 5935 && y < 2039);
        }

        public static bool IsTrammelSolenHive(Map map, Point3D loc)
        {
            return (map == Map.Trammel && IsSolenHiveLoc(loc));
        }

        public static bool IsFeluccaSolenHive(Map map, Point3D loc)
        {
            return (map == Map.Felucca && IsSolenHiveLoc(loc));
        }

        public static bool IsFeluccaT2A(Map map, Point3D loc)
        {
            int x = loc.X, y = loc.Y;

            return (map == Map.Felucca && x >= 5120 && y >= 2304 && x < 6144 && y < 4096);
        }

        public static bool IsAnyT2A(Map map, Point3D loc)
        {
            int x = loc.X, y = loc.Y;

            return ((map == Map.Trammel || map == Map.Felucca) && x >= 5120 && y >= 2304 && x < 6144 && y < 4096);
        }

        public static bool IsFeluccaDungeon(Map map, Point3D loc)
        {
            Region region = Region.Find(loc, map);
            return (region.IsPartOf(typeof(DungeonRegion)) && region.Map == Map.Felucca);
        }

        public static bool IsKhaldun(Map map, Point3D loc)
        {
            return (Region.Find(loc, map).Name == "Khaldun");
        }

        public static bool IsCrystalCave(Map map, Point3D loc)
        {
            if (map != Map.Malas || loc.Z >= -80)
                return false;

            int x = loc.X, y = loc.Y;

            return (x >= 1182 && y >= 437 && x < 1211 && y < 470) ||
                   (x >= 1156 && y >= 470 && x < 1211 && y < 503) ||
                   (x >= 1176 && y >= 503 && x < 1208 && y < 509) ||
                   (x >= 1188 && y >= 509 && x < 1201 && y < 513);
        }

        public static bool IsSafeZone(Map map, Point3D loc)
        {
            if (Region.Find(loc, map).IsPartOf(typeof(Engines.ConPVP.SafeZone)))
            {
                if (m_TravelType == TravelCheckType.TeleportTo || m_TravelType == TravelCheckType.TeleportFrom)
                {
                    PlayerMobile pm = m_TravelCaster as PlayerMobile;

                    if (pm != null && pm.DuelPlayer != null && !pm.DuelPlayer.Eliminated)
                        return true;
                }

                return true;
            }

            return false;
        }

        public static bool IsFactionStronghold(Map map, Point3D loc)
        {
            /*// Teleporting is allowed, but only for faction members
            if ( !Core.AOS && m_TravelCaster != null && (m_TravelType == TravelCheckType.TeleportTo || m_TravelType == TravelCheckType.TeleportFrom) )
            {
            if ( Factions.Faction.Find( m_TravelCaster, true, true ) != null )
            return false;
            }*/
            return (Region.Find(loc, map).IsPartOf(typeof(Factions.StrongholdRegion)));
        }

        public static bool IsChampionSpawn(Map map, Point3D loc)
        {
            return (Region.Find(loc, map).IsPartOf(typeof(ChampionSpawnRegion)));
        }

        public static bool IsDoomFerry(Map map, Point3D loc)
        {
            if (map != Map.Malas)
                return false;

            int x = loc.X, y = loc.Y;

            if (x >= 426 && y >= 314 && x <= 430 && y <= 331)
                return true;

            if (x >= 406 && y >= 247 && x <= 410 && y <= 264)
                return true;

            return false;
        }

        public static bool IsTokunoDungeon(Map map, Point3D loc)
        {
            //The tokuno dungeons are really inside malas
            if (map != Map.Malas)
                return false;

            int x = loc.X, y = loc.Y, z = loc.Z;

            bool r1 = (x >= 0 && y >= 0 && x <= 128 && y <= 128);
            bool r2 = (x >= 45 && y >= 320 && x < 195 && y < 710);

            return (r1 || r2);
        }

        public static bool IsDoomGauntlet(Map map, Point3D loc)
        {
            if (map != Map.Malas)
                return false;

            int x = loc.X - 256, y = loc.Y - 304;

            return (x >= 0 && y >= 0 && x < 256 && y < 256);
        }

        public static bool IsLampRoom(Map map, Point3D loc)
        {
            if (map != Map.Malas)
                return false;

            int x = loc.X, y = loc.Y;

            return (x >= 465 && y >= 92 && x < 474 && y < 102);
        }

        public static bool IsGuardianRoom(Map map, Point3D loc)
        {
            if (map != Map.Malas)
                return false;

            int x = loc.X, y = loc.Y;

            return (x >= 356 && y >= 5 && x < 375 && y < 25);
        }

        public static bool IsHeartwood(Map map, Point3D loc)
        {
            int x = loc.X, y = loc.Y;

            return (map == Map.Trammel || map == Map.Felucca) && (x >= 6911 && y >= 254 && x < 7167 && y < 511);
        }

        public static bool IsMLDungeon(Map map, Point3D loc)
        {
            return MondainsLegacy.IsMLRegion(Region.Find(loc, map));
        }

        public static bool IsTombOfKings(Map map, Point3D loc)
        {
            return (Region.Find(loc, map).IsPartOf(typeof(TombOfKingsRegion)));
        }

        public static bool IsMazeOfDeath(Map map, Point3D loc)
        {
            return (Region.Find(loc, map).IsPartOf(typeof(MazeOfDeathRegion)));
        }

        public static bool IsSAEntrance(Map map, Point3D loc)
        {
            int x = loc.X, y = loc.Y;
            return (map == Map.TerMur) && (x >= 1122 && y >= 1067 && x <= 1144 && y <= 1086);
        }

        public static bool IsSADungeon(Map map, Point3D loc)
        {
            if (map != Map.TerMur)
                return false;

            Region region = Region.Find(loc, map);
            return (region.IsPartOf(typeof(DungeonRegion)) && !region.IsPartOf(typeof(TombOfKingsRegion)));
        }

        public static bool IsInvalid(Map map, Point3D loc)
        {
            if (map == null || map == Map.Internal)
                return true;

            int x = loc.X, y = loc.Y;

            return (x < 0 || y < 0 || x >= map.Width || y >= map.Height);
        }

        //towns
        public static bool IsTown(IPoint3D loc, Mobile caster)
        {
            if (loc is Item)
                loc = ((Item)loc).GetWorldLocation();

            return IsTown(new Point3D(loc), caster);
        }

        public static bool IsTown(Point3D loc, Mobile caster)
        {
            Map map = caster.Map;

            if (map == null)
                return false;

            SafeZone sz = (SafeZone)Region.Find(loc, map).GetRegion(typeof(Engines.ConPVP.SafeZone));

            if (sz != null)
            {
                PlayerMobile pm = (PlayerMobile)caster;

                if (pm == null || pm.DuelContext == null || !pm.DuelContext.Started || pm.DuelPlayer == null || pm.DuelPlayer.Eliminated)
                    return true;
            }

            GuardedRegion reg = (GuardedRegion)Region.Find(loc, map).GetRegion(typeof(GuardedRegion));

            return (reg != null && !reg.IsDisabled());
        }

        public static bool CheckTown(IPoint3D loc, Mobile caster)
        {
            if (loc is Item)
                loc = ((Item)loc).GetWorldLocation();

            return CheckTown(new Point3D(loc), caster);
        }

        public static bool CheckTown(Point3D loc, Mobile caster)
        {
            if (IsTown(loc, caster))
            {
                caster.SendLocalizedMessage(500946); // You cannot cast this in town!
                return false;
            }

            return true;
        }

        //magic reflection
        public static void CheckReflect(int circle, Mobile caster, ref Mobile target)
        {
            CheckReflect(circle, ref caster, ref target);
        }

        public static void CheckReflect(int circle, ref Mobile caster, ref Mobile target)
        {
            if (target.MagicDamageAbsorb > 0)
            {
                ++circle;

                target.MagicDamageAbsorb -= circle;

                // This order isn't very intuitive, but you have to nullify reflect before target gets switched

                bool reflect = (target.MagicDamageAbsorb >= 0);

                if (target is BaseCreature)
                    ((BaseCreature)target).CheckReflect(caster, ref reflect);

                if (target.MagicDamageAbsorb <= 0)
                {
                    target.MagicDamageAbsorb = 0;
                    DefensiveSpell.Nullify(target);
                }

                if (reflect)
                {
                    target.FixedEffect(0x37B9, 10, 5);

                    Mobile temp = caster;
                    caster = target;
                    target = temp;
                }
            }
            else if (target is BaseCreature)
            {
                bool reflect = false;

                ((BaseCreature)target).CheckReflect(caster, ref reflect);

                if (reflect)
                {
                    target.FixedEffect(0x37B9, 10, 5);

                    Mobile temp = caster;
                    caster = target;
                    target = temp;
                }
            }
        }

        public static void CheckSummonLimits(BaseCreature creature)
        {
            ArrayList creatures = new ArrayList();

            int limit = 6; // 6 creatures
            int range = 5; // per 5x5 area

            var eable = creature.GetMobilesInRange(range);

            foreach (Mobile mobile in eable)
            {
                if (mobile != null && mobile.GetType() == creature.GetType())
                    creatures.Add(mobile);
            }

            int amount = 0;

            if (creatures.Count > limit)
                amount = creatures.Count - limit;

            while (amount > 0)
            {
                for (int i = 0; i < creatures.Count; i++)
                {
                    Mobile m = creatures[i] as Mobile;

                    if (m != null && ((BaseCreature)m).Summoned)
                    {
                        if (Utility.RandomBool() && amount > 0)
                        {
                            m.Delete();
                            amount--;
                        }
                    }
                }
            }
        }

        public static void Damage(Spell spell, Mobile target, double damage)
        {
            TimeSpan ts = GetDamageDelayForSpell(spell);

            Damage(spell, ts, target, spell.Caster, damage);
        }

        public static void Damage(TimeSpan delay, Mobile target, double damage)
        {
            Damage(delay, target, null, damage);
        }

        public static void Damage(TimeSpan delay, Mobile target, Mobile from, double damage)
        {
            Damage(null, delay, target, from, damage);
        }

        public static void Damage(Spell spell, TimeSpan delay, Mobile target, Mobile from, double damage)
        {
            int iDamage = (int)damage;

            if (delay == TimeSpan.Zero)
            {
                if (from is BaseCreature)
                    ((BaseCreature)from).AlterSpellDamageTo(target, ref iDamage);

                if (target is BaseCreature)
                    ((BaseCreature)target).AlterSpellDamageFrom(from, ref iDamage);

                target.Damage(iDamage, from);
            }
            else
            {
                new SpellDamageTimer(spell, target, from, iDamage, delay).Start();
            }

            if (target is BaseCreature && from != null && delay == TimeSpan.Zero)
            {
                BaseCreature c = (BaseCreature)target;

                c.OnHarmfulSpell(from);
                c.OnDamagedBySpell(from);
            }
        }

        public static void Damage(Spell spell, Mobile target, double damage, int phys, int fire, int cold, int pois, int nrgy)
        {
            TimeSpan ts = GetDamageDelayForSpell(spell);

            Damage(spell, ts, target, spell.Caster, damage, phys, fire, cold, pois, nrgy, DFAlgorithm.Standard);
        }

        public static void Damage(Spell spell, Mobile target, double damage, int phys, int fire, int cold, int pois, int nrgy, DFAlgorithm dfa)
        {
            TimeSpan ts = GetDamageDelayForSpell(spell);

            Damage(spell, ts, target, spell.Caster, damage, phys, fire, cold, pois, nrgy, dfa);
        }

        public static void Damage(TimeSpan delay, Mobile target, double damage, int phys, int fire, int cold, int pois, int nrgy)
        {
            Damage(delay, target, null, damage, phys, fire, cold, pois, nrgy);
        }

        public static void Damage(TimeSpan delay, Mobile target, Mobile from, double damage, int phys, int fire, int cold, int pois, int nrgy)
        {
            Damage(delay, target, from, damage, phys, fire, cold, pois, nrgy, DFAlgorithm.Standard);
        }

        public static void Damage(TimeSpan delay, Mobile target, Mobile from, double damage, int phys, int fire, int cold, int pois, int nrgy, DFAlgorithm dfa)
        {
            Damage(null, delay, target, from, damage, phys, fire, cold, pois, nrgy, dfa);
        }

        public static void Damage(Spell spell, TimeSpan delay, Mobile target, Mobile from, double damage, int phys, int fire, int cold, int pois, int nrgy, DFAlgorithm dfa)
        {
            int iDamage = (int)damage;

            if (delay == TimeSpan.Zero)
            {
                if (from is BaseCreature)
                    ((BaseCreature)from).AlterSpellDamageTo(target, ref iDamage);

                if (target is BaseCreature)
                    ((BaseCreature)target).AlterSpellDamageFrom(from, ref iDamage);

                WeightOverloading.DFA = dfa;

                int damageGiven = ItemAttributes.Damage(target, from, iDamage, phys, fire, cold, pois, nrgy);

                if (from != null) // sanity check
                {
                    DoLeech(damageGiven, from, target);
                }

                WeightOverloading.DFA = DFAlgorithm.Standard;
            }
            else
            {
                new SpellDamageTimerAOS(spell, target, from, iDamage, phys, fire, cold, pois, nrgy, delay, dfa).Start();
            }

            if (target is BaseCreature && from != null && delay == TimeSpan.Zero)
            {
                BaseCreature c = (BaseCreature)target;

                c.OnHarmfulSpell(from);
                c.OnDamagedBySpell(from);
            }
        }

        public static void DoLeech(int damageGiven, Mobile from, Mobile target)
        {
            Server.Spells.Necromancy.TransformationSpell.TransformContext context = TransformationSpell.GetContext(from);

            if (context != null && context.Type == typeof(WraithFormSpell))
            {
                int wraithLeech = (5 + (int)((15 * from.Skills.SpiritSpeak.Value) / 100)); // Wraith form gives 5-20% mana leech
                int manaLeech = Math.Min(target.Mana, ItemAttributes.Scale(damageGiven, wraithLeech));

                if (manaLeech != 0)
                {
                    // Mana leeched by the Wraith Form spell is actually stolen, not just leeched.
                    target.Mana -= manaLeech;
                    from.Mana += manaLeech;
                    from.PlaySound(0x44D);
                }
            }
            else if (context != null && context.Type == typeof(VampiricEmbraceSpell))
            {
                if (target is BaseCreature && ((BaseCreature)target).TaintedLifeAura)
                {
                    ItemAttributes.Damage(from, target, ItemAttributes.Scale(damageGiven, 20), false, 0, 0, 0, 0, 0, 0, 100, false, false, false);
                    from.SendLocalizedMessage(1116778); //The tainted life force energy damages you as your body tries to absorb it.
                }
                else
                {
                    from.Hits += ItemAttributes.Scale(damageGiven, 20);
                    from.PlaySound(0x44D);
                }
            }
        }

        public static void Heal(int amount, Mobile target, Mobile from)
        {
            Heal(amount, target, from, true);
        }

        public static void Heal(int amount, Mobile target, Mobile from, bool message)
        {
            //TODO: All Healing *spells* go through ArcaneEmpowerment
            target.Heal(amount, from, message);
        }

        private static SkillName[] m_MainSkills = new SkillName[]
			{
				SkillName.Magery,
				SkillName.Necromancy,
				SkillName.Mysticism,
				SkillName.Ninjitsu,
				SkillName.Bushido,
				SkillName.AnimalTaming,
				SkillName.Musicianship,
				SkillName.Chivalry,
				SkillName.Spellweaving
			};

        public static bool IsFocusSkillSpec(Mobile m)
        {
            int total = 0;

            for (int i = 0; i < m_MainSkills.Length; i++)
            {
                if (m.Skills[m_MainSkills[i]].Value >= 30.0)
                    total++;
            }

            return total <= 1;
        }

        public static int GetSpellDamage(Mobile m, bool applyCap)
        {
            int bonus = 0;

            bonus += ArcaneEmpowermentSpell.GetSDIBonus(m);

            bonus += ReaperFormSpell.GetSDIBonus(m);

            if (GrapesOfWrath.IsUnderInfluence(m))
                bonus += 15;

            //if (PsychicAttack.UnderEffect(m))
                //bonus -= 4;

            if (m is PlayerMobile)
            {
                PlayerMobile pm = m as PlayerMobile;

                bonus += AosAttributes.GetValue(m, AosAttribute.SpellDamage);

                if (applyCap)
                {
                    int cap = IsFocusSkillSpec(m) ? 30 : 15;

                    if (bonus > cap)
                        bonus = cap;
                }

                if (pm.Berserk)
                    bonus += 3 * (int)(((float)(m.HitsMax - m.Hits) / m.HitsMax) * 5.0);
            }

            return bonus;
        }

        private class SpellDamageTimer : Timer
        {
            private readonly Mobile m_Target;

            private readonly Mobile m_From;

            private int m_Damage;
            private readonly Spell m_Spell;

            public SpellDamageTimer(Spell s, Mobile target, Mobile from, int damage, TimeSpan delay)
                : base(delay)
            {
                this.m_Target = target;
                this.m_From = from;
                this.m_Damage = damage;
                this.m_Spell = s;

                if (this.m_Spell != null && this.m_Spell.DelayedDamage && !this.m_Spell.DelayedDamageStacking)
                    this.m_Spell.StartDelayedDamageContext(target, this);

                this.Priority = TimerPriority.TwentyFiveMS;
            }

            protected override void OnTick()
            {
                if (this.m_From is BaseCreature)
                    ((BaseCreature)this.m_From).AlterSpellDamageTo(this.m_Target, ref this.m_Damage);

                if (this.m_Target is BaseCreature)
                    ((BaseCreature)this.m_Target).AlterSpellDamageFrom(this.m_From, ref this.m_Damage);

                this.m_Target.Damage(this.m_Damage);
                if (this.m_Spell != null)
                    this.m_Spell.RemoveDelayedDamageContext(this.m_Target);
            }
        }

        private class SpellDamageTimerAOS : Timer
        {
            private readonly Mobile m_Target;
            private readonly Mobile m_From;
            private int m_Damage;
            private readonly int m_Phys;
            private readonly int m_Fire;
            private readonly int m_Cold;
            private readonly int m_Pois;
            private readonly int m_Nrgy;
            private readonly DFAlgorithm m_DFA;
            private readonly Spell m_Spell;

            public SpellDamageTimerAOS(Spell s, Mobile target, Mobile from, int damage, int phys, int fire, int cold, int pois, int nrgy, TimeSpan delay, DFAlgorithm dfa)
                : base(delay)
            {
                this.m_Target = target;
                this.m_From = from;
                this.m_Damage = damage;
                this.m_Phys = phys;
                this.m_Fire = fire;
                this.m_Cold = cold;
                this.m_Pois = pois;
                this.m_Nrgy = nrgy;
                this.m_DFA = dfa;
                this.m_Spell = s;
                if (this.m_Spell != null && this.m_Spell.DelayedDamage && !this.m_Spell.DelayedDamageStacking)
                    this.m_Spell.StartDelayedDamageContext(target, this);

                this.Priority = TimerPriority.TwentyFiveMS;
            }

            protected override void OnTick()
            {
                if (this.m_From is BaseCreature && this.m_Target != null)
                    ((BaseCreature)this.m_From).AlterSpellDamageTo(this.m_Target, ref this.m_Damage);

                if (this.m_Target is BaseCreature && this.m_From != null)
                    ((BaseCreature)this.m_Target).AlterSpellDamageFrom(this.m_From, ref this.m_Damage);

                WeightOverloading.DFA = this.m_DFA;

                int damageGiven = ItemAttributes.Damage(this.m_Target, this.m_From, this.m_Damage, this.m_Phys, this.m_Fire, this.m_Cold, this.m_Pois, this.m_Nrgy);

                if (this.m_From != null) // sanity check
                {
                    DoLeech(damageGiven, this.m_From, this.m_Target);
                }

                WeightOverloading.DFA = DFAlgorithm.Standard;

                if (this.m_Target is BaseCreature && this.m_From != null)
                {
                    BaseCreature c = (BaseCreature)this.m_Target;

                    c.OnHarmfulSpell(this.m_From);
                    c.OnDamagedBySpell(this.m_From);
                }

                if (this.m_Spell != null)
                    this.m_Spell.RemoveDelayedDamageContext(this.m_Target);
            }
        }
    }
}