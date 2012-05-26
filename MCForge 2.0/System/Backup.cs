using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCForge.Utils;
using MCForge.World;
using System.IO;
using System.Timers;
using MCForge.Utils.Settings;

namespace MCForge.Core {
    public class Backup {

        internal static string DateFormat {
            get { return _lastTime.ToString( "dd-MM-yyyy" ); }
        }

        private static DateTime _lastTime;
        private static Timer _timer;

        public static void BackupAll ( ) {
            CheckDirs();

            BackupLevels();
            BackupLogs();
            BackupGroups();
        }

        public static void BackupLevels ( ) {
            CheckDirs();

            foreach ( var level in Level.Levels ) {
                if ( !level.BackupLevel )
                    continue;

                BackupLevel( level );
            }
        }

        public static void BackupGroups ( ) {
            CheckDirs();
        }

        public static void BackupLogs ( ) {
            CheckDirs();

            if ( File.Exists( FileUtils.LogsPath + Logger.DateFormat ) )
                File.Copy( FileUtils.LogsPath + Logger.DateFormat, FileUtils.BackupsPath + DateFormat + "-backup/" +  DateTime.Now.ToString("hhmmss")+ "logs.log" );
        }

        public static void BackupLevel ( Level s ) {
            CheckDirs();

            if ( File.Exists( FileUtils.LevelsPath + s.Name + ".lvl" ) ) {
                File.Copy( FileUtils.LevelsPath + s.Name + ".lvl", "levels/" + DateTime.Now.ToString("hhmmss")+ s.Name + ".lvl" );
            }
        }

        public static void CheckDirs ( ) {
            if ( _lastTime.Day != DateTime.Now.Day )
                return;

            _lastTime = DateTime.Now;
            FileUtils.CreateDirIfNotExist( FileUtils.BackupsPath + DateFormat + "-backup" );
        }

        public static void StartBackup ( ) {
            if ( !ServerSettings.GetSettingBoolean( "BackupFiles" ) )
                return;

            BackupAll();

            _timer = new Timer( ServerSettings.GetSettingInt( "BackupInterval" ) );
            _timer.Elapsed += ( obj, e) => {
                BackupAll();
            };
        }
        static Backup ( ) {
            _lastTime = DateTime.Now;
            FileUtils.CreateDirIfNotExist( FileUtils.BackupsPath + DateFormat + "-backup" );

        }
    }
}
