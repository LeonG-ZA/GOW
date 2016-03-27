using Server.LogConsole;
using System;
using System.Collections.Generic;
using System.IO;

namespace Server
{
	public static class Teleporters
	{
        public static bool GOWTeleportSystemEnabled = GOWLoggingConfig.TeleportSystemEnabled;
		public static Teleport[] m_Teleporters;
		public static string[] m_Paths;

		public static void Initialize()
		{
			if (Teleporters.GOWTeleportSystemEnabled)
			{
				Teleporters.LoadPaths();
			}
		}
		private static void LoadPaths()
		{
			List<string> list = new List<string>();
			if (list != null)
			{
				string path = Path.Combine(Core.BaseDirectory, "Data/World/Teleporters/teleporters.cfg");
				if (File.Exists(path))
				{
					using (StreamReader streamReader = new StreamReader(path))
					{
						string text;
						while ((text = streamReader.ReadLine()) != null)
						{
							if (text.Length != 0 && !text.StartsWith("#"))
							{
								try
								{
									string item = Path.Combine(Core.BaseDirectory, string.Format("Data/World/Teleporters/{0}.cfg", text.Trim()));
									list.Add(item);
								}
								catch
								{
								}
							}
						}
					}
					Teleporters.m_Paths = list.ToArray();
				}
			}
			Teleporters.LoadLocations();
		}
		private static void LoadLocations()
		{
			List<Teleport> list = new List<Teleport>();
			if (list == null)
			{
				return;
			}
			string[] paths = Teleporters.m_Paths;
			for (int i = 0; i < paths.Length; i++)
			{
				string text = paths[i];
				if (File.Exists(text))
				{
					using (StreamReader streamReader = new StreamReader(text))
					{
						string text2;
						while ((text2 = streamReader.ReadLine()) != null)
						{
							if (text2.Length != 0 && !text2.StartsWith("#"))
							{
								try
								{
									string[] array = text2.Split(new char[]
									{
										'|'
									});
									string[] array2 = text2.Split(new char[]
									{
										'|'
									});
									int x = Convert.ToInt32(array[0]);
									int y = Convert.ToInt32(array[1]);
									int z = Convert.ToInt32(array[2]);
									int index = Convert.ToInt32(array[3]);
									int x2 = Convert.ToInt32(array[4]);
									int y2 = Convert.ToInt32(array[5]);
									int z2 = Convert.ToInt32(array[6]);
									int index2 = Convert.ToInt32(array[7]);
									Map mapLoc = Map.AllMaps[index];
									Map mapDest = Map.AllMaps[index2];
									Point3D pointLoc = new Point3D(x, y, z);
									Point3D pointDest = new Point3D(x2, y2, z2);
									Teleport teleport = new Teleport(pointLoc, mapLoc, pointDest, mapDest);
									if (array2[8] == "true")
									{
										teleport.Active = true;
									}
									else
									{
										teleport.Active = false;
									}
									if (array2[9] == "true")
									{
										teleport.Creatures = true;
									}
									else
									{
										teleport.Creatures = false;
									}
									if (array2[10] == "true")
									{
										teleport.CombatCheck = true;
									}
									else
									{
										teleport.CombatCheck = false;
									}
									if (array2[11] == "true")
									{
										teleport.CriminalCheck = true;
									}
									else
									{
										teleport.CriminalCheck = false;
									}
									teleport.SourceEffect = Utility.ToInt32(array2[12]);
									teleport.DestEffect = Utility.ToInt32(array2[13]);
									teleport.SoundID = Utility.ToInt32(array[14]);
									teleport.Delay = TimeSpan.FromSeconds((double)Convert.ToInt32(array[15]));
									if (array2[16] == "true")
									{
										teleport.Oneway = true;
									}
									else
									{
										teleport.Oneway = false;
									}
									if (array2[17] == "true")
									{
										teleport.Bonded = true;
									}
									else
									{
										teleport.Bonded = false;
									}
									list.Add(teleport);
								}
								catch
								{
									ErrorLog.WriteLine("ERROR: Bad teleport location entry in ({0})", new object[]
									{
										text
									});
									ErrorLog.WriteLine("");
								}
							}
						}
					}
				}
				Teleporters.m_Teleporters = list.ToArray();
			}
		}
	}

    public class Teleport
    {
        private bool m_Active;
        private bool m_Creatures;
        private bool m_CombatCheck;
        private bool m_CriminalCheck;
        private bool m_Oneway;
        private bool m_Bonded;
        private Point3D m_PointLoc;
        private Map m_MapLoc;
        private Point3D m_PointDest;
        private Map m_MapDest;
        private int m_SourceEffect;
        private int m_DestEffect;
        private int m_SoundID;
        private TimeSpan m_Delay;
        private static readonly TimeSpan CombatHeatDelay = TimeSpan.FromSeconds(30.0);

        public int SourceEffect
        {
            get
            {
                return this.m_SourceEffect;
            }
            set
            {
                this.m_SourceEffect = value;
            }
        }
        public int DestEffect
        {
            get
            {
                return this.m_DestEffect;
            }
            set
            {
                this.m_DestEffect = value;
            }
        }
        public int SoundID
        {
            get
            {
                return this.m_SoundID;
            }
            set
            {
                this.m_SoundID = value;
            }
        }
        public TimeSpan Delay
        {
            get
            {
                return this.m_Delay;
            }
            set
            {
                this.m_Delay = value;
            }
        }
        public bool Active
        {
            get
            {
                return this.m_Active;
            }
            set
            {
                this.m_Active = value;
            }
        }
        public Point3D PointLoc
        {
            get
            {
                return this.m_PointLoc;
            }
            set
            {
                this.m_PointLoc = value;
            }
        }
        public Map MapLoc
        {
            get
            {
                return this.m_MapLoc;
            }
            set
            {
                this.m_MapLoc = value;
            }
        }
        public Point3D PointDest
        {
            get
            {
                return this.m_PointDest;
            }
            set
            {
                this.m_PointDest = value;
            }
        }
        public Map MapDest
        {
            get
            {
                return this.m_MapDest;
            }
            set
            {
                this.m_MapDest = value;
            }
        }
        public bool Creatures
        {
            get
            {
                return this.m_Creatures;
            }
            set
            {
                this.m_Creatures = value;
            }
        }
        public bool CombatCheck
        {
            get
            {
                return this.m_CombatCheck;
            }
            set
            {
                this.m_CombatCheck = value;
            }
        }
        public bool CriminalCheck
        {
            get
            {
                return this.m_CriminalCheck;
            }
            set
            {
                this.m_CriminalCheck = value;
            }
        }
        public bool Oneway
        {
            get
            {
                return this.m_Oneway;
            }
            set
            {
                this.m_Oneway = value;
            }
        }
        public bool Bonded
        {
            get
            {
                return this.m_Bonded;
            }
            set
            {
                this.m_Bonded = value;
            }
        }
        public Teleport(Point3D pointLoc, Map mapLoc, Point3D pointDest, Map mapDest)
        {
            this.m_Active = true;
            this.m_PointLoc = pointLoc;
            this.m_MapLoc = mapLoc;
            this.m_PointDest = pointDest;
            this.m_MapDest = mapDest;
            this.m_Creatures = false;
            this.m_CombatCheck = false;
            this.m_CriminalCheck = false;
            this.m_Oneway = false;
            this.m_Bonded = false;
        }
        public static bool CheckCombat(Mobile m)
        {
            for (int i = 0; i < m.Aggressed.Count; i++)
            {
                AggressorInfo aggressorInfo = m.Aggressed[i];
                if (aggressorInfo.Defender.Player && DateTime.UtcNow - aggressorInfo.LastCombatTime < Teleport.CombatHeatDelay)
                {
                    return true;
                }
            }
            if (Core.Expansion == Expansion.AOS)
            {
                for (int j = 0; j < m.Aggressors.Count; j++)
                {
                    AggressorInfo aggressorInfo2 = m.Aggressors[j];
                    if (aggressorInfo2.Attacker.Player && DateTime.UtcNow - aggressorInfo2.LastCombatTime < Teleport.CombatHeatDelay)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public virtual bool CanTeleport(Mobile m)
        {
            if (!this.m_Creatures && !m.Player)
            {
                return false;
            }
            if (this.m_CriminalCheck && m.Criminal)
            {
                m.SendLocalizedMessage(1005561, "", 34);
                return false;
            }
            if (this.m_CombatCheck && Teleport.CheckCombat(m))
            {
                m.SendLocalizedMessage(1005564, "", 34);
                return false;
            }
            return true;
        }
        public virtual void DoTeleport(Mobile m)
        {
            Map map = this.m_MapLoc;
            if (map == null || map == Map.Internal)
            {
                map = m.Map;
            }
            Map map2 = this.m_MapDest;
            if (map2 == null || map2 == Map.Internal)
            {
                map2 = m.Map;
            }
            Point3D point3D = this.m_PointDest;
            if (point3D == Point3D.Zero)
            {
                point3D = m.Location;
            }
            Teleport.TeleportPets(m, point3D, map2, this.m_Bonded);
            bool flag = !m.Hidden || m.IsPlayer();
            if (this.SourceEffect > 0 && flag)
            {
                Effects.SendLocationEffect(m.Location, map, this.SourceEffect, 35);
            }
            if (this.SoundID > 0 && flag)
            {
                Effects.PlaySound(point3D, map, this.SoundID);
            }
            m.MoveToWorld(point3D, map2);
            if (this.DestEffect > 0 && flag)
            {
                Effects.SendLocationEffect(m.Location, m.Map, this.DestEffect, 35);
            }
            if (this.SoundID > 0 && flag)
            {
                Effects.PlaySound(m.Location, m.Map, this.SoundID);
            }
        }
        public virtual void DoTeleportReturn(Mobile m)
        {
            Map map = this.m_MapDest;
            if (map == null || map == Map.Internal)
            {
                map = m.Map;
            }
            Map map2 = this.m_MapLoc;
            if (map2 == null || map2 == Map.Internal)
            {
                map2 = m.Map;
            }
            Point3D point3D = this.m_PointLoc;
            if (point3D == Point3D.Zero)
            {
                point3D = m.Location;
            }

            Teleport.TeleportPets(m, point3D, map2, this.m_Bonded);
            bool flag = !m.Hidden || m.IsPlayer();
            if (this.SourceEffect > 0 && flag)
            {
                Effects.SendLocationEffect(m.Location, m.Map, this.SourceEffect, 35);
            }
            if (this.SoundID > 0 && flag)
            {
                Effects.PlaySound(m.Location, m.Map, this.SoundID);
            }

            m.MoveToWorld(point3D, map2);

            if (this.DestEffect > 0 && flag)
            {
                Effects.SendLocationEffect(m.Location, m.Map, this.DestEffect, 35);
            }
            if (this.SoundID > 0 && flag)
            {
                Effects.PlaySound(m.Location, m.Map, this.SoundID);
            }
        }
        public bool OnMoveOver(Mobile m)
        {
            if (m.Holding != null)
            {
                m.SendLocalizedMessage(1071955);
                return true;
            }
            if (!this.m_Active || !this.CanTeleport(m))
            {
                return true;
            }
            if (m.Holding != null)
            {
                m.SendLocalizedMessage(1071955);
                return true;
            }
            if (this.m_Delay == TimeSpan.Zero)
            {
                this.DoTeleport(m);
            }
            else
            {
                Timer.DelayCall<Mobile>(this.m_Delay, new TimerStateCallback<Mobile>(this.DoTeleport), m);
            }
            return false;
        }
        public bool OnMoveOverReturn(Mobile m)
        {
            if (this.m_Oneway)
            {
                return true;
            }
            if (m.Holding != null)
            {
                m.SendLocalizedMessage(1071955);
                return true;
            }
            if (!this.m_Active || !this.CanTeleport(m))
            {
                return true;
            }
            if (m.Holding != null)
            {
                m.SendLocalizedMessage(1071955);
                return true;
            }
            if (this.m_Delay == TimeSpan.Zero)
            {
                this.DoTeleportReturn(m);
            }
            else
            {
                Timer.DelayCall<Mobile>(this.m_Delay, new TimerStateCallback<Mobile>(this.DoTeleport), m);
            }
            return false;
        }
        public static void TeleportPets(Mobile master, Point3D loc, Map map, bool onlyBonded)
        {
            List<Mobile> list = new List<Mobile>();
            foreach (Mobile current in master.GetMobilesInRange(3))
            {
                if ((!onlyBonded || current.IsPetBonded) && current.IsControlled && current.IsControlMaster(master) && current.IsControlOrderWithMe)
                {
                    list.Add(current);
                }
            }
            foreach (Mobile current2 in list)
            {
                current2.MoveToWorld(loc, map);
            }
        }
    }
}
