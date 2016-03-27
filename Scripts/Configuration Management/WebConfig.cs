using System;
using Server;
using System.Text;
using System.Threading;

namespace Server.WebConfiguration
{
    class WebConfig
    {
        /// <summary>
        /// Reports Configuration Settings
        /// </summary>
        public static bool WebReportsEnabled = false;
        public static readonly string WebFtpHost = null;
        public static readonly string WebFtpUsername = null;
        public static readonly string WebFtpPassword = null;
        public static readonly string WebFtpStatsDirectory = null;
        public static readonly string WebFtpStaffDirectory = null;
        public const string WebShardTitle = "GOW";
        /// <summary>
        /// MyShard Configuration Settings
        /// </summary>
        public static bool WebMyShardEnabled = false;
        public const string WebDatabaseDriver = "{MySQL ODBC 5.2w Driver}";
        public const string WebDatabaseServer = "localhost";
        public const string WebDatabaseName = "MyShard";
        public const string WebDatabaseUserID = "username";
        public const string WebDatabasePassword = "password";
        // Should the database use transactions? This is recommended
        public static bool WebUseTransactions = true;
        // Use optimized table loading techniques? (LOAD DATA INFILE)
        public static bool WebLoadDataInFile = true;
        // This must be enabled if the database server is on a remote machine.
        public static bool WebDatabaseNonLocal = (WebDatabaseServer != "localhost");
        // Text encoding used
        public static Encoding WebEncodingIO = Encoding.ASCII;
        // Database communication is done in a separate thread. This value is the 'priority' of that thread, or, how much CPU it will try to use
        public static ThreadPriority WebDatabaseThreadPriority = ThreadPriority.BelowNormal;
        // Any character with an AccessLevel equal to or higher than this will not be displayed
        public static AccessLevel WebHiddenAccessLevel = AccessLevel.Counselor;
        // Export character database every 30 minutes
        public static TimeSpan WebCharacterUpdateInterval = TimeSpan.FromMinutes(30.0);
        // Export online list database every 5 minutes
        public static TimeSpan WebStatusUpdateInterval = TimeSpan.FromMinutes(5.0);
        public static string WebCompileConnectionString()
        {
            string connectionString = String.Format("DRIVER={0};SERVER={1};DATABASE={2};UID={3};PASSWORD={4};",
                WebDatabaseDriver, WebDatabaseServer, WebDatabaseName, WebDatabaseUserID, WebDatabasePassword);

            return connectionString;
        }
        /// <summary>
        /// Server Status Configuration Settings
        /// </summary>
        public static bool WebServerStatusEnabled = false;
        public static string WebServerStatusEmail = "YOUR@YOUR_DOMAIN.com";
        public static string WebServerStatusAdminName = "Admin";
        /// <summary>
        /// Web UO GPS Configuration Settings
        /// </summary>
        public static readonly bool WebGPSEnabled = false;
        //if you want to use another directory then where your G.O.W.exe is, change to something like
        // OutputDirectory =@"D:/var/www"
        // !!! This directory should excist !!!
        public static readonly string WebOutputDirectory = null;
        //this will enable|disable auto generation of player house locations at each world save
        //set to false to disable it
        public static readonly bool WebPlayersLGenAtSave = false;
        //this will enable auto generation of player locations at each world save
        //set to false to disable it
        public static readonly bool WebPlayersHGenAtSave = false;
    }
}
