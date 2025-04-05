using System;
using System.Timers;
using System.IO;
using System.Reflection;
//Terraria Api's
using TerrariaApi.Server;
using Terraria;
using TShockAPI;
// Database Plugins
using System.Collections.Generic;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HerosOfTerraria_BackupSystem
{
    [ApiVersion(2, 1)]
    public class HerosOfTerraria_BackupSystem : TerrariaPlugin
    {
        public static Random rnd = new Random();
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static string PluginPath
        {
            get { return Path.Combine(ServerApi.ServerPluginsDirectoryPath, "" + "HerosOfTerraria" + ""); }
        }
        public static string PlayerFilePath
        {
            get { return Path.Combine(PluginPath, "" + "PlayerFiles" + ""); }
        }
        public static string BackupFilePath
        {
            get { return Path.Combine(PluginPath, "" + "backups" + ""); }
        }
        public static List<Player> players = new List<Player>();
        public override string Name { get { return "" + "Heros Of Terraria - Backup System" + ""; } }
        public override string Author { get { return "Ruff Trigger"; } }
        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }
        private static Timer update;
        public HerosOfTerraria_BackupSystem(Main game)
            : base(game)
        {
            base.Order = 1;

        }
        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
            }
        }
        private void OnInitialize(EventArgs args)
        {
            update = new Timer { Interval = 12000 + rndNext(1000, 5000), AutoReset = true, Enabled = true };
            update.Elapsed += OnElapsed;
        }
        private void OnElapsed(object sender, ElapsedEventArgs args)
        {
            try
            {
                update.Interval = 900000 + rndNext(10000, 100000);
                float milliseconds = Convert.ToUInt32(update.Interval);
                float seconds = milliseconds / 1000;
                float minutes = seconds / 60;
                string MinutesInput = minutes.ToString();
                string SecondsInput = seconds.ToString();
                int MinutesIndex = MinutesInput.LastIndexOf(",");
                int SecondsIndex = SecondsInput.LastIndexOf(",");

                if (MinutesIndex > 0)
                {
                    MinutesInput = MinutesInput.Substring(0, MinutesIndex); // or index + 1 to keep slash
                                                                            //SecondsIndex = SecondsInput.Substring(0, SecondsIndex); // or index + 1 to keep slash
                    Console.Write("New Backup interval was set to = " + MinutesInput + " " + "minutes" + " " + "and" + " " + SecondsIndex + " " + "seconds" + Environment.NewLine);
                }
                else
                {
                    Console.Write("New Backup interval was set to = " + MinutesInput + " " + "minutes" + " " + "and" + " " + SecondsIndex + " " + "seconds" + Environment.NewLine);
                }
                if (!Directory.Exists(BackupFilePath))
                {
                    Directory.CreateDirectory(BackupFilePath);
                    RunAsyncBackupTask();
                }
                else
                {
                    RunAsyncBackupTask();
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
                return;
            }

        }
        /// <summary>
        /// Async Backup of files
        /// </summary>
        /// <returns></returns>
        public static Task AsyncBackupTask()
        {
            return Task.Run(() =>
            {
                ZipFiles();
            });
        }
        public static async void CallProcess()
        {
            await AsyncBackupTask();
        }
        public static void RunAsyncBackupTask()
        {
            CallProcess();
        }
        private static void CopyPlayerData()
        {
            for (int i = 0; i < TShock.Players.Length; i++)
            {
                if (TShock.Players[i].Account == null || TShock.Players[i].TPlayer == null || !TShock.Players[i].IsLoggedIn)
                {
                    return;
                }
                if (TShock.Players[i] != null || TShock.Players.Length != 0)
                {
                    string sourceDirectory = PlayerFilePath + "/" + TShock.Players[i].Account.ID;
                    string targetDirectory = BackupFilePath + "/" + TShock.Players[i].Account.ID;
                    if (!Directory.Exists(targetDirectory))
                    {
                        Directory.CreateDirectory(targetDirectory);
                    }
                    Copy(sourceDirectory, targetDirectory);
                }
            }
        }
        private static void Copy(string sourceDirectory, string targetDirectory)
        {
            try
            {
                DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
                DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);
                if (diSource != null || diTarget != null)
                {
                    CopyAll(diSource, diTarget);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
                return;
            }

        }
        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            try
            {
                // Copy each file into the new directory.
                foreach (FileInfo fi in source.GetFiles())
                {
                    //Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                    fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                }
                // Copy each subdirectory using recursion.
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                        target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyAll(diSourceSubDir, nextTargetSubDir);
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
                return;
            }
        }
        private static void ZipFiles()
        {
            try
            {
                DateTime now = DateTime.Now;
                string Hour = now.Hour.ToString();
                string Minute = now.Minute.ToString();
                string Day = now.Day.ToString();
                string Month = now.Month.ToString();
                string Year = now.Year.ToString();
                string startPath = PlayerFilePath;
                string zipPath = BackupFilePath + "/";
                string zipName = "backup" + "_" + "[Time]" + "_" + Hour + @"-" + Minute + "_" + "[Date]" + "_" + Day + "_" + Month + "_" + "[Year]" + "_" + Year + ".zip";
                int fileCount = Directory.GetFiles(zipPath, "*.zip", SearchOption.AllDirectories).Length; // Will Retrieve count of files zip extension in directry and sub directries
                if (fileCount >= 100)
                {
                    DirectoryInfo di = new DirectoryInfo(zipPath);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    if (File.Exists(zipPath + zipName)) { return; }
                    else
                    {

                        ZipFile.CreateFromDirectory(startPath, zipPath + zipName, CompressionLevel.Fastest, true);
                    }
                }
                else
                {
                    if (File.Exists(zipPath + zipName)) { return; }
                    else
                    {
                        ZipFile.CreateFromDirectory(startPath, zipPath + zipName, CompressionLevel.Fastest, true);
                    }
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
                return;
            }
        }
        /// <summary>
        /// Returns a random number
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int rndNext(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(min, max);
            }
        }
        public static int GetSeed()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var intBytes = new byte[4];
                rng.GetBytes(intBytes);
                return BitConverter.ToInt32(intBytes, 0);
            }
        }
    }
}
