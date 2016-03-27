using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Commands
{
	public class GOWSpawnerExporter
	{
		public const bool Enabled = true;

		public static void Initialize()
		{
            CommandSystem.Register("GOWSpawnerExporter", AccessLevel.Administrator, new CommandEventHandler(GOWSpawnerExporter_OnCommand));
            CommandSystem.Register("GSE", AccessLevel.Administrator, new CommandEventHandler(GOWSpawnerExporter_OnCommand));
		}

 		public static int ConvertToInt( TimeSpan ts )
 		{
 			return ( ( ts.Hours * 60 ) + ts.Minutes + (ts.Seconds/60) );
 		}

        [Usage("GOWSpawnerExporter")]
		[Aliases( "GSE" )]
		[Description( "Convert G.O.W Spawners to Prime Spawners." )]
        public static void GOWSpawnerExporter_OnCommand(CommandEventArgs e)
		{
			Map map = e.Mobile.Map;
			List<Item> list = new List<Item>();

			if ( !Directory.Exists( @".\Data\World\Spawns\" ) )
                Directory.CreateDirectory(@".\Data\World\Spawns\");

            using (StreamWriter op = new StreamWriter(String.Format(@".\Data\World\Spawns\{0}-exported.map", map)))
			{

				if ( map == null || map == Map.Internal )
				{
					e.Mobile.SendMessage( "You may not run that command here." );
					return;
				}

				e.Mobile.SendMessage( "Converting Spawners..." );

				op.WriteLine( "#######################################" );
                op.WriteLine(" ## Converted By GOWSpawnerExporter ##");
				op.WriteLine( "#######################################" );

				foreach ( Item item in World.Items.Values )
				{
					if ( item.Map == map && item.Parent == null && item is Spawner )
						list.Add( item );
				}

				foreach ( Spawner spawner in list )
				{
					string mapfinal = "";

					string walkrange = "";

					if(map == Map.Maps[0])
					{
						mapfinal = "1";
					}
					else if(map == Map.Maps[1])
					{
						mapfinal = "2";
					}
					else if(map == Map.Maps[2])
					{
						mapfinal = "3";
					}
					else if(map == Map.Maps[3])
					{
						mapfinal = "4";
					}
					else if(map == Map.Maps[4])
					{
						mapfinal = "5";
					}
					else
					{
						mapfinal = "6";
					}

					if( spawner.WalkingRange == -1 )
					{
						walkrange = spawner.HomeRange.ToString();
					}
					else
					{
						walkrange = spawner.WalkingRange.ToString();
					}
					
					int MinDelay = ConvertToInt(spawner.MinDelay);

					if (MinDelay < 1)
					{
						MinDelay = 1;
					}

					int MaxDelay = ConvertToInt(spawner.MaxDelay);

					if (MaxDelay < MinDelay)
					{
						MaxDelay = MinDelay;
					}
					
					string towrite = "*|";

					if( spawner.SpawnNames.Count > 0 )
					{
						towrite = "*|" + spawner.SpawnNames[0];

						for ( int i = 1; i < spawner.SpawnNames.Count; ++i )
						{
							towrite = towrite + ":" + spawner.SpawnNames[i].ToString();
						}
					}
					
					if ( spawner.SpawnNames.Count > 0 && spawner.Running == true )
					{
                        op.WriteLine("{0}||||||{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|1|{9}|0|0|0|0|0", towrite, spawner.X, spawner.Y, spawner.Z, mapfinal, MinDelay, MaxDelay, walkrange, spawner.HomeRange, spawner.SpawnMax);
					}
					
					if( spawner.SpawnNames.Count == 0 )
					{
                        op.WriteLine("## Void: {0}||||||{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|1|{9}|0|0|0|0|0", towrite, spawner.X, spawner.Y, spawner.Z, mapfinal, MinDelay, MaxDelay, walkrange, spawner.HomeRange, spawner.SpawnMax);
					}
					
					if( spawner.SpawnNames.Count > 0 && spawner.Running == false )
					{
                        op.WriteLine("## Inactive: {0}||||||{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|1|{9}|0|0|0|0|0", towrite, spawner.X, spawner.Y, spawner.Z, mapfinal, MinDelay, MaxDelay, walkrange, spawner.HomeRange, spawner.SpawnMax);
					}
				}
				e.Mobile.SendMessage( String.Format( "You exported {0} Shard Spawner{1} from this facet.", list.Count, list.Count == 1 ? "" : "s" ) );
			}
		}
	}
}