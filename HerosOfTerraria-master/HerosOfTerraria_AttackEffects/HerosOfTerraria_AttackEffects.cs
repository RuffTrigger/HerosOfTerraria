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
using static Terraria.GameContent.Animations.Actions;
using Microsoft.Xna.Framework;

namespace HerosOfTerraria_AttackEffects
{
    [ApiVersion(2, 1)]
    public class HerosOfTerraria_AttackEffects : TerrariaPlugin
    {
        public static Random rnd = new Random(GetSeed());
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
        public HerosOfTerraria_AttackEffects(Main game)
                : base(game)
        {
            base.Order = 1;

        }
        public override void Initialize()
        {
            ServerApi.Hooks.NetGetData.Register(this,  OnGetData);
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
                await AsyncAttackEffectsTask(args);
            }

        }
        public static Task AsyncAttackEffectsTask(GetDataEventArgs args)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var reader = new BinaryReader(new MemoryStream(args.Msg.readBuffer, args.Index, args.Length)))
                    {
                        if (args.MsgID != PacketTypes.NpcStrike) return;

                        int npcID = reader.ReadInt16();
                        NPC npc = Main.npc[npcID];
                        TSPlayer tsPlayer = TShock.Players[args.Msg.whoAmI];
                        Player tPlayer = tsPlayer?.TPlayer;
                        if (tPlayer == null) return;

                        IEntitySource source = new EntitySource_Parent(tPlayer);

                        // Read data
                        int dmg = reader.ReadInt16();
                        float knockback = reader.ReadSingle();
                        byte direction = reader.ReadByte();
                        bool critical = reader.ReadBoolean();

                        // File paths and constants
                        string playerPath = $"{PlayerFilePath}/{tsPlayer.Account.ID}";
                        int heroLevel = ReadIntFromFile(playerPath, "HeroLevel");
                        int heroDebuff = ReadIntFromFile(playerPath, "HeroDebuffs");
                        int attackBuff = ReadIntFromFile(playerPath, "HeroAttackBuff");
                        int heroDamage = ReadIntFromFile(playerPath, "HeroDamage");
                        int attackSkill = ReadIntFromFile(playerPath, "HeroAttackSkill");

                        ApplyNpcBuff(npc, heroDebuff, 5000);
                        ApplyPlayerBuff(tsPlayer, attackBuff, 300);

                        if (ShouldSpawnProjectiles())
                        {
                            CreateProjectiles(source, tPlayer.position, npc.position, dmg, critical, heroDamage, attackSkill, args.Msg.whoAmI);
                        }

                    }
                }
                catch (Exception ex)
                {
                    TShock.Log.Error(ex.ToString());
                }
            });
        }

        private static int ReadIntFromFile(string path, string key)
        {
            return int.TryParse(ReadFile(path, key)?.Trim(), out int value) ? value : 0;
        }

        private static void ApplyNpcBuff(NPC npc, int buffID, int duration)
        {
            if (buffID > 0)
            {
                npc.AddBuff(buffID, duration, true);
            }
        }

        private static void ApplyPlayerBuff(TSPlayer player, int buffID, int duration)
        {
            if (buffID > 0 && new Random().Next(1, 20) >= 19)
            {
                player.SetBuff(buffID, duration);
            }
        }

        private static bool ShouldSpawnProjectiles()
        {
            return new Random().Next(1, 3) >= 2;
        }

        private static void CreateProjectiles(IEntitySource source, Vector2 playerPosition, Vector2 npcPosition, int damage, bool critical, int heroDamage, int type, int playerId)
        {
            int totalDamage = damage + heroDamage;
            Random random = new Random();

            // Calculate the direction from the player to the NPC and normalize it
            Vector2 directionToNpc = Vector2.Normalize(npcPosition - playerPosition);

            for (int i = 0; i < (critical ? 3 : 2); i++)
            {
                // Add some random offset to the projectile's initial position for variation
                float offsetX = random.Next(-5, 5);
                float offsetY = random.Next(-5, 5);
                Vector2 spawnPosition = playerPosition + new Vector2(offsetX, offsetY);

                // Adjust the projectile's speed if needed (e.g., multiply the direction vector by a speed factor)
                float projectileSpeed = 10f; // Example speed
                Vector2 velocity = directionToNpc * projectileSpeed;
                int Bee = 181;
                int GiantBee = 566;
                if (type == Bee && ShouldSpawnProjectiles())
                {
                    // Spawn Giant Bee projectile
                    Projectile.NewProjectile(source, spawnPosition.X, spawnPosition.Y, velocity.X, velocity.Y, GiantBee, playerId, 0f);
                }
                // Spawn the projectile
                Projectile.NewProjectile(source, spawnPosition.X, spawnPosition.Y, velocity.X, velocity.Y, type, playerId, 0f);
            }
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
    }
}
