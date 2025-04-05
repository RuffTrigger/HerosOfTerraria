using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Data;
//Terraria Api's
using TerrariaApi.Server;
using Terraria;
using TShockAPI;
using System.Security.Cryptography;
using Terraria.DataStructures;
using Terraria.ID;

namespace HerosOfTerraria_DefenceEffect
{
    [ApiVersion(2, 1)]
    public class HerosOfTerraria_DefenceEffect : TerrariaPlugin
    {
        public static Random rnd = new Random(GetSeed());
        public static int type = 496;
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static string PlayerFilePath
        {
            get { return Path.Combine(PluginPath, "" + "PlayerFiles" + ""); }
        }
        public static List<Player> players = new List<Player>();
        public override string Name { get { return "" + "Heros Of Terraria" + ""; } }
        public override string Author { get { return "Ruff Trigger"; } }
        public override Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }
        public static string PluginPath
        {
            get { return Path.Combine(ServerApi.ServerPluginsDirectoryPath, "" + "HerosOfTerraria" + ""); }
        }
        public HerosOfTerraria_DefenceEffect(Main game)
                : base(game)
        {
            base.Order = 1;

        }
        public override void Initialize()
        {
            ServerApi.Hooks.NetGetData.Register(this, OnGetData);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.NetGetData.Deregister(this, OnGetData);
            }
        }
        public async static void OnGetData(GetDataEventArgs args)
        {
            if (args == null || TShock.Players[args.Msg.whoAmI].Account == null || TShock.Players[args.Msg.whoAmI].TPlayer == null || !TShock.Players[args.Msg.whoAmI].IsLoggedIn)
            {
                return;
            }
            else
            {
                await AsyncDefenceEffectsTask(args);
            }
        }
        public static Task AsyncDefenceEffectsTask(GetDataEventArgs args)
        {
            return Task.Run(() =>
            {
                using (var reader = new BinaryReader(new MemoryStream(args.Msg.readBuffer, args.Index, args.Length)))
                {
                    Player player = players.Where(p => p.Equals(args.MsgID)).FirstOrDefault();
                    switch (args.MsgID)
                    {
                        case PacketTypes.PlayerHurtV2:
                            try
                            {
                                TSPlayer tsPlayer = TShock.Players[args.Msg.whoAmI];
                                Player tPlayer = tsPlayer?.TPlayer;
                                string playerPath = $"{PlayerFilePath}/{tsPlayer.Account.ID}";
                                int Bee = 181;
                                int attackSkill = ReadIntFromFile(playerPath, "HeroAttackSkill");
                                if (attackSkill == Bee)
                                {
                                    int Rage = 115;
                                    if (new Random().Next(1, 20) >= 19)
                                    {
                                        tsPlayer.SetBuff(Rage, 300);
                                    }
                                }
                                type = 645;
                                byte IsPVP = reader.ReadByte();
                                int PlayerHeroDefence = (Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroDefence").Trim()));

                                int p1 = Projectile.NewProjectile(new EntitySource_DebugCommand(), TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
                                // Since we can't override defense, we'll have to mitigate through healing :(
                                if (/*IsPVP == 1 ||*/ PlayerHeroDefence == 0)
                                {
                                    return;
                                }
                                else
                                {
                                    if (PlayerHeroDefence > 0)
                                    {
                                        TShock.Players[args.Msg.whoAmI].Heal((PlayerHeroDefence * 2));
                                        Main.projectile[p1].Kill();
                                        return;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                TShock.Log.Error(ex.ToString());
                                Console.Write(ex.ToString());
                                return;
                            }
                            break;
                    }
                }
            });
        }
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
        public static string ReadFile(string FilePath, string Filename)
        {
            try
            {
                string Result = string.Empty;
                if (Filename != null || FilePath != null)
                {
                    var StreamReaders = new List<StreamReader>();
                    StreamReaders.Add(new StreamReader(FilePath + "/" + Filename + ".hotfile"));
                    Parallel.ForEach(StreamReaders, sr => { Result = sr.ReadLine(); sr.Dispose(); sr.Close(); });
                    return Result.Replace(" ", "").Trim();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
                return null;
            }
        }
        private static void ApplyPlayerBuff(TSPlayer player, int buffID, int duration)
        {
            if (buffID > 0 && new Random().Next(1, 20) >= 19)
            {
                player.SetBuff(buffID, duration);
            }
        }
        private static int ReadIntFromFile(string path, string key)
        {
            return int.TryParse(ReadFile(path, key)?.Trim(), out int value) ? value : 0;
        }

    }
}
