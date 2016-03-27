#region References
using System;
using System.Collections.Generic;

using CustomsFramework;

using Server.Guilds;
#endregion

namespace Server
{
	public class StandardSaveStrategy : SaveStrategy
	{
		public static SaveOption SaveType = SaveOption.Normal;
		private readonly Queue<Item> _decayQueue;
		private bool _permitBackgroundWrite;

		public StandardSaveStrategy()
		{
			_decayQueue = new Queue<Item>();
		}

		public enum SaveOption
		{
			Normal,
			Threaded
		}

		public override string Name { get { return "Standard"; } }
		protected bool PermitBackgroundWrite { get { return _permitBackgroundWrite; } set { _permitBackgroundWrite = value; } }
		protected bool UseSequentialWriters { get { return (SaveType == SaveOption.Normal || !_permitBackgroundWrite); } }

		public override void Save(SaveMetrics metrics, bool permitBackgroundWrite)
		{
			_permitBackgroundWrite = permitBackgroundWrite;

			SaveMobiles(metrics);
			SaveItems(metrics);
			SaveGuilds(metrics);
			SaveData(metrics);

			if (permitBackgroundWrite && UseSequentialWriters)
			{
				//If we're permitted to write in the background, but we don't anyways, then notify.
				World.NotifyDiskWriteComplete();
			}
		}

		public override void ProcessDecay()
		{
			while (_decayQueue.Count > 0)
			{
				Item item = _decayQueue.Dequeue();

				if (item.OnDecay())
				{
					item.Delete();
				}
			}
		}

		protected void SaveMobiles(SaveMetrics metrics)
		{
			Dictionary<Serial, Mobile> mobiles = World.Mobiles;

			GenericWriter idx;
			GenericWriter tdb;
			GenericWriter bin;

			if (UseSequentialWriters)
			{
				idx = new BinaryFileWriter(World.MobileIndexPath, false);
				tdb = new BinaryFileWriter(World.MobileTypesPath, false);
				bin = new BinaryFileWriter(World.MobileDataPath, true);
			}
			else
			{
				idx = new AsyncWriter(World.MobileIndexPath, false);
				tdb = new AsyncWriter(World.MobileTypesPath, false);
				bin = new AsyncWriter(World.MobileDataPath, true);
			}

			idx.Write(mobiles.Count);
			foreach (Mobile m in mobiles.Values)
			{
				long start = bin.Position;

				idx.Write(m._TypeRef);
				idx.Write(m.Serial);
				idx.Write(start);

				m.Serialize(bin);

				if (metrics != null)
				{
					metrics.OnMobileSaved((int)(bin.Position - start));
				}

				idx.Write((int)(bin.Position - start));

				m.FreeCache();
			}

			tdb.Write(World.m_MobileTypes.Count);

			foreach (Type t in World.m_MobileTypes)
			{
				tdb.Write(t.FullName);
			}

			idx.Close();
			tdb.Close();
			bin.Close();
		}

		protected void SaveItems(SaveMetrics metrics)
		{
			Dictionary<Serial, Item> items = World.Items;

			GenericWriter idx;
			GenericWriter tdb;
			GenericWriter bin;

			if (UseSequentialWriters)
			{
				idx = new BinaryFileWriter(World.ItemIndexPath, false);
				tdb = new BinaryFileWriter(World.ItemTypesPath, false);
				bin = new BinaryFileWriter(World.ItemDataPath, true);
			}
			else
			{
				idx = new AsyncWriter(World.ItemIndexPath, false);
				tdb = new AsyncWriter(World.ItemTypesPath, false);
				bin = new AsyncWriter(World.ItemDataPath, true);
			}

			idx.Write(items.Count);

            DateTime n = DateTime.UtcNow;

			foreach (Item item in items.Values)
			{
                if (item.Decays && item.Parent == null && item.Map != Map.Internal && (item.LastMoved + item.DecayTime) <= n)
				{
					_decayQueue.Enqueue(item);
				}

				long start = bin.Position;

				idx.Write(item.m_TypeRef);
				idx.Write(item.Serial);
				idx.Write(start);

				item.Serialize(bin);

				if (metrics != null)
				{
					metrics.OnItemSaved((int)(bin.Position - start));
				}

				idx.Write((int)(bin.Position - start));

				item.FreeCache();
			}

			tdb.Write(World.m_ItemTypes.Count);
			foreach (Type t in World.m_ItemTypes)
			{
				tdb.Write(t.FullName);
			}

			idx.Close();
			tdb.Close();
			bin.Close();
		}

		protected void SaveGuilds(SaveMetrics metrics)
		{
			GenericWriter idx;
			GenericWriter bin;

			if (UseSequentialWriters)
			{
				idx = new BinaryFileWriter(World.GuildIndexPath, false);
				bin = new BinaryFileWriter(World.GuildDataPath, true);
			}
			else
			{
				idx = new AsyncWriter(World.GuildIndexPath, false);
				bin = new AsyncWriter(World.GuildDataPath, true);
			}

			idx.Write(BaseGuild.List.Count);
			foreach (BaseGuild guild in BaseGuild.List.Values)
			{
				long start = bin.Position;

				idx.Write(0); //guilds have no typeid
				idx.Write(guild.Id);
				idx.Write(start);

				guild.Serialize(bin);

				if (metrics != null)
				{
					metrics.OnGuildSaved((int)(bin.Position - start));
				}

				idx.Write((int)(bin.Position - start));
			}

			idx.Close();
			bin.Close();
		}

		protected void SaveData(SaveMetrics metrics)
		{
			Dictionary<CustomSerial, SaveData> data = World.Data;

			GenericWriter indexWriter;
			GenericWriter typeWriter;
			GenericWriter dataWriter;

			if (UseSequentialWriters)
			{
				indexWriter = new BinaryFileWriter(World.DataIndexPath, false);
				typeWriter = new BinaryFileWriter(World.DataTypesPath, false);
				dataWriter = new BinaryFileWriter(World.DataBinaryPath, true);
			}
			else
			{
				indexWriter = new AsyncWriter(World.DataIndexPath, false);
				typeWriter = new AsyncWriter(World.DataTypesPath, false);
				dataWriter = new AsyncWriter(World.DataBinaryPath, true);
			}

			indexWriter.Write(data.Count);

			foreach (SaveData saveData in data.Values)
			{
				long start = dataWriter.Position;

				indexWriter.Write(saveData._TypeID);
				indexWriter.Write(saveData.Serial);
				indexWriter.Write(start);

				saveData.Serialize(dataWriter);

				if (metrics != null)
				{
					metrics.OnDataSaved((int)(dataWriter.Position - start));
				}

				indexWriter.Write((int)(dataWriter.Position - start));
			}

			typeWriter.Write(World._DataTypes.Count);

			foreach (Type t in World._DataTypes)
			{
				typeWriter.Write(t.FullName);
			}

			indexWriter.Close();
			typeWriter.Close();
			dataWriter.Close();
		}
	}
}