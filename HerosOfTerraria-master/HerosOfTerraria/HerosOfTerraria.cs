using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using System.Timers;
//Terraria Api's
using TerrariaApi.Server;
using Terraria;
using TShockAPI;
using TShockAPI.DB;
using Terraria.ID;


namespace HerosOfTerraria
{
    [ApiVersion(2, 1)]
    public class HerosOfTerraria : TerrariaPlugin
    {
        public static Random rnd = new Random();
        public static Color c = new Color();
        public static Color StatusColor = new Color(255, 120, 0);
        private static Timer update;
        private static Timer SendInfomationMessage;
        public static int type = 167;
        public static int fireworkcolor = rnd.Next(1, 3);
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static IDbConnection db;
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
        public static string HerosFilesPath
        {
            get { return Path.Combine(PluginPath, "" + "Heros" + ""); }
        }
        public static string PlayerFilePath
        {
            get { return Path.Combine(PluginPath, "" + "PlayerFiles" + ""); }
        }
        public HerosOfTerraria(Main game)
                : base(game)
        {
            base.Order = 1;

        }
        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
            ServerApi.Hooks.NetGreetPlayer.Register(this, OnGreet);
            ServerApi.Hooks.NetGetData.Register(this, OnGetData);
            ServerApi.Hooks.ServerLeave.Register(this, OnLeave);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
                ServerApi.Hooks.NetGreetPlayer.Deregister(this, OnGreet);
                ServerApi.Hooks.NetGetData.Deregister(this, OnGetData);
                ServerApi.Hooks.ServerLeave.Deregister(this, OnLeave);
            }
        }
        private void OnInitialize(EventArgs args)
        {
            CreateFolders();
            HeroClassSetup();
            AddCommands();
            StartTimers();
        }
        private static void OnGreet(GreetPlayerEventArgs args)
        {
            if (args == null || TShock.Players[args.Who].Account == null || TShock.Players[args.Who].TPlayer == null || TShock.Players[args.Who].IsLoggedIn == false || ReadFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroClass") == null || ReadFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroClass") == "")
            {
                return;
            }
            try
            {
                if (!Directory.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID))
                {
                    Directory.CreateDirectory(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID);

                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID + "/" + "HeroClass.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroClass", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID + "/" + "HeroLevel.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroLevel", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID + "/" + "HeroExperience.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroExperience", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID + "/" + "HeroExpMultiplier.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroExpMultiplier", "1");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID + "/" + "HeroNextLevel.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroNextLevel", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID + "/" + "HeroMaxLevel.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroMaxLevel", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID + "/" + "HeroAttackSkill.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroAttackSkill", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID + "/" + "HeroAttackBuff.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroAttackBuff", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID + "/" + "HeroDebuffs.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroDebuffs", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID + "/" + "HeroSkillpoints.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroSkillpoints", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID + "/" + "HeroDefence.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroDefence", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID + "/" + "HeroDamage.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroDamage", "0");
                    }
                }
                if (ReadFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroClass") != "0")
                {
                    TSPlayer.All.SendMessage(TShock.Players[args.Who].Account.Name
                                + " The Level "
                                + ReadFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroLevel")
                                + " "
                                + ReadFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroClass")
                                + " Has Joined The Game! ", 121, 31, 52);
                    return;
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
            }

        }
        private void OnLeave(LeaveEventArgs args)
        {
            if (args == null || TShock.Players[args.Who].IsLoggedIn == false || ReadFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroClass") == null || ReadFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroClass") == "")
            {
                return;
            }
            TSPlayer.All.SendMessage(TShock.Players[args.Who].Account.Name
                                        + " The Level "
                                        + ReadFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroLevel")
                                        + " "
                                        + ReadFile(PlayerFilePath + "/" + TShock.Players[args.Who].Account.ID, "HeroClass")
                                        + " Has left our worlde...", 121, 31, 52);
        }
        private static void OnGetData(GetDataEventArgs args)
        {
            if (args == null || TShock.Players[args.Msg.whoAmI].Account == null || TShock.Players[args.Msg.whoAmI].TPlayer == null || TShock.Players[args.Msg.whoAmI].IsLoggedIn == false || ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroClass") == null || ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroClass") == "")
            {
                return;
            }
            try
            {
                using (var reader = new BinaryReader(new MemoryStream(args.Msg.readBuffer, args.Index, args.Length)))
                {
                    Player player = players.Where(p => p.Equals(args.MsgID)).FirstOrDefault();
                    switch (args.MsgID)
                    {
                        case PacketTypes.NpcStrike:
                            Int16 npcID = reader.ReadInt16();
                            Int16 dmg = reader.ReadInt16();
                            float knockback = reader.ReadSingle();
                            byte direction = reader.ReadByte();
                            bool critical = reader.ReadBoolean();
                            //bool secondCrit = random.Next(1, 101) <= Player.Crit;
                            NPC npc = Main.npc[npcID];
                            PlayerHero.HeroClass = ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroClass").Trim();
                            PlayerHero.HeroLevel = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroLevel").Trim());
                            PlayerHero.HeroNextLevel = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroNextLevel").Trim());
                            PlayerHero.HeroExperience = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience").Trim());
                            PlayerHero.HeroExpMultiplier = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExpMultiplier").Trim());
                            PlayerHero.HeroMaxLevel = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroMaxLevel").Trim());
                            if (PlayerHero.HeroLevel == PlayerHero.HeroMaxLevel)
                            {
                                return;
                            }
                            if (npc.life - dmg <= 0)
                            {

                                if (npc.netID == NPCID.KingSlime || npc.netID == NPCID.QueenBee || npc.netID == NPCID.EyeofCthulhu || npc.netID == NPCID.EaterofWorldsHead || npc.netID == NPCID.EaterofWorldsBody || npc.netID == NPCID.EaterofWorldsTail || npc.netID == NPCID.SkeletronHead || npc.netID == NPCID.SkeletronHand || npc.netID == NPCID.BrainofCthulhu)
                                {
                                    int GainedXP = 0;
                                    int MobExp = 0;
                                    MobExp = (npc.lifeMax + 20000) * PlayerHero.HeroExpMultiplier;
                                    switch (PlayerHero.HeroLevel)
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                        case 4:
                                        case 5:
                                        case 6:
                                        case 7:
                                        case 8:
                                        case 9:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 10:
                                        case 11:
                                        case 12:
                                        case 13:
                                        case 14:
                                        case 15:
                                        case 16:
                                        case 17:
                                        case 18:
                                        case 19:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp / 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 20:
                                        case 21:
                                        case 22:
                                        case 23:
                                        case 24:
                                        case 25:
                                        case 26:
                                        case 27:
                                        case 28:
                                        case 29:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 30:
                                        case 31:
                                        case 32:
                                        case 33:
                                        case 34:
                                        case 35:
                                        case 36:
                                        case 37:
                                        case 38:
                                        case 39:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 40:
                                        case 41:
                                        case 42:
                                        case 43:
                                        case 44:
                                        case 45:
                                        case 46:
                                        case 47:
                                        case 48:
                                        case 49:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 50:
                                        case 51:
                                        case 52:
                                        case 53:
                                        case 54:
                                        case 55:
                                        case 56:
                                        case 57:
                                        case 58:
                                        case 59:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 60:
                                        case 61:
                                        case 62:
                                        case 63:
                                        case 64:
                                        case 65:
                                        case 66:
                                        case 67:
                                        case 68:
                                        case 69:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 70:
                                        case 71:
                                        case 72:
                                        case 73:
                                        case 74:
                                        case 75:
                                        case 76:
                                        case 77:
                                        case 78:
                                        case 79:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2 + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 80:
                                        case 81:
                                        case 82:
                                        case 83:
                                        case 84:
                                        case 85:
                                        case 86:
                                        case 87:
                                        case 88:
                                        case 89:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 90:
                                        case 91:
                                        case 92:
                                        case 93:
                                        case 94:
                                        case 95:
                                        case 96:
                                        case 97:
                                        case 98:
                                        case 99:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 3;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        default:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 4;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                    }
                                    return;
                                }
                                else if (npc.netID == NPCID.WallofFlesh || npc.netID == NPCID.WallofFleshEye)
                                {
                                    int GainedXP = 0;
                                    int MobExp = 0;
                                    MobExp = (npc.lifeMax + 30000) * PlayerHero.HeroExpMultiplier;
                                    switch (PlayerHero.HeroLevel)
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                        case 4:
                                        case 5:
                                        case 6:
                                        case 7:
                                        case 8:
                                        case 9:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 10:
                                        case 11:
                                        case 12:
                                        case 13:
                                        case 14:
                                        case 15:
                                        case 16:
                                        case 17:
                                        case 18:
                                        case 19:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp / 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 20:
                                        case 21:
                                        case 22:
                                        case 23:
                                        case 24:
                                        case 25:
                                        case 26:
                                        case 27:
                                        case 28:
                                        case 29:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 30:
                                        case 31:
                                        case 32:
                                        case 33:
                                        case 34:
                                        case 35:
                                        case 36:
                                        case 37:
                                        case 38:
                                        case 39:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 40:
                                        case 41:
                                        case 42:
                                        case 43:
                                        case 44:
                                        case 45:
                                        case 46:
                                        case 47:
                                        case 48:
                                        case 49:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 50:
                                        case 51:
                                        case 52:
                                        case 53:
                                        case 54:
                                        case 55:
                                        case 56:
                                        case 57:
                                        case 58:
                                        case 59:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 60:
                                        case 61:
                                        case 62:
                                        case 63:
                                        case 64:
                                        case 65:
                                        case 66:
                                        case 67:
                                        case 68:
                                        case 69:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 70:
                                        case 71:
                                        case 72:
                                        case 73:
                                        case 74:
                                        case 75:
                                        case 76:
                                        case 77:
                                        case 78:
                                        case 79:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2 + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 80:
                                        case 81:
                                        case 82:
                                        case 83:
                                        case 84:
                                        case 85:
                                        case 86:
                                        case 87:
                                        case 88:
                                        case 89:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 90:
                                        case 91:
                                        case 92:
                                        case 93:
                                        case 94:
                                        case 95:
                                        case 96:
                                        case 97:
                                        case 98:
                                        case 99:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 3;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        default:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 4;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                    }
                                    return;
                                }
                                else if (npc.netID == NPCID.Retinazer || npc.netID == NPCID.Spazmatism || npc.netID == NPCID.SkeletronPrime)
                                {
                                    int GainedXP = 0;
                                    int MobExp = 0;
                                    MobExp = (npc.lifeMax + 45000) * PlayerHero.HeroExpMultiplier;
                                    switch (PlayerHero.HeroLevel)
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                        case 4:
                                        case 5:
                                        case 6:
                                        case 7:
                                        case 8:
                                        case 9:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 10:
                                        case 11:
                                        case 12:
                                        case 13:
                                        case 14:
                                        case 15:
                                        case 16:
                                        case 17:
                                        case 18:
                                        case 19:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp / 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 20:
                                        case 21:
                                        case 22:
                                        case 23:
                                        case 24:
                                        case 25:
                                        case 26:
                                        case 27:
                                        case 28:
                                        case 29:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 30:
                                        case 31:
                                        case 32:
                                        case 33:
                                        case 34:
                                        case 35:
                                        case 36:
                                        case 37:
                                        case 38:
                                        case 39:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 40:
                                        case 41:
                                        case 42:
                                        case 43:
                                        case 44:
                                        case 45:
                                        case 46:
                                        case 47:
                                        case 48:
                                        case 49:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 50:
                                        case 51:
                                        case 52:
                                        case 53:
                                        case 54:
                                        case 55:
                                        case 56:
                                        case 57:
                                        case 58:
                                        case 59:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 60:
                                        case 61:
                                        case 62:
                                        case 63:
                                        case 64:
                                        case 65:
                                        case 66:
                                        case 67:
                                        case 68:
                                        case 69:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 70:
                                        case 71:
                                        case 72:
                                        case 73:
                                        case 74:
                                        case 75:
                                        case 76:
                                        case 77:
                                        case 78:
                                        case 79:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2 + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 80:
                                        case 81:
                                        case 82:
                                        case 83:
                                        case 84:
                                        case 85:
                                        case 86:
                                        case 87:
                                        case 88:
                                        case 89:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 90:
                                        case 91:
                                        case 92:
                                        case 93:
                                        case 94:
                                        case 95:
                                        case 96:
                                        case 97:
                                        case 98:
                                        case 99:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 3;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        default:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 4;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                    }
                                    return;
                                }
                                else if (npc.netID == NPCID.PrimeCannon || npc.netID == NPCID.PrimeSaw || npc.netID == NPCID.PrimeVice || npc.netID == NPCID.PrimeLaser)
                                {
                                    int GainedXP = 0;
                                    int MobExp = 0;
                                    MobExp = (npc.lifeMax + 50000) * PlayerHero.HeroExpMultiplier;
                                    switch (PlayerHero.HeroLevel)
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                        case 4:
                                        case 5:
                                        case 6:
                                        case 7:
                                        case 8:
                                        case 9:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 10:
                                        case 11:
                                        case 12:
                                        case 13:
                                        case 14:
                                        case 15:
                                        case 16:
                                        case 17:
                                        case 18:
                                        case 19:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp / 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 20:
                                        case 21:
                                        case 22:
                                        case 23:
                                        case 24:
                                        case 25:
                                        case 26:
                                        case 27:
                                        case 28:
                                        case 29:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 30:
                                        case 31:
                                        case 32:
                                        case 33:
                                        case 34:
                                        case 35:
                                        case 36:
                                        case 37:
                                        case 38:
                                        case 39:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 40:
                                        case 41:
                                        case 42:
                                        case 43:
                                        case 44:
                                        case 45:
                                        case 46:
                                        case 47:
                                        case 48:
                                        case 49:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 50:
                                        case 51:
                                        case 52:
                                        case 53:
                                        case 54:
                                        case 55:
                                        case 56:
                                        case 57:
                                        case 58:
                                        case 59:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 60:
                                        case 61:
                                        case 62:
                                        case 63:
                                        case 64:
                                        case 65:
                                        case 66:
                                        case 67:
                                        case 68:
                                        case 69:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 70:
                                        case 71:
                                        case 72:
                                        case 73:
                                        case 74:
                                        case 75:
                                        case 76:
                                        case 77:
                                        case 78:
                                        case 79:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2 + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 80:
                                        case 81:
                                        case 82:
                                        case 83:
                                        case 84:
                                        case 85:
                                        case 86:
                                        case 87:
                                        case 88:
                                        case 89:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 90:
                                        case 91:
                                        case 92:
                                        case 93:
                                        case 94:
                                        case 95:
                                        case 96:
                                        case 97:
                                        case 98:
                                        case 99:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 3;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        default:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 4;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                    }
                                    return;
                                }
                                else if (npc.netID == NPCID.TheDestroyer || npc.netID == NPCID.TheDestroyerBody || npc.netID == NPCID.TheDestroyerTail || npc.netID == NPCID.Golem || npc.netID == NPCID.GolemHead || npc.netID == NPCID.GolemFistLeft || npc.netID == NPCID.GolemFistRight || npc.netID == NPCID.GolemHeadFree || npc.netID == NPCID.Plantera || npc.netID == NPCID.PlanterasHook || npc.netID == NPCID.PlanterasTentacle)
                                {
                                    int GainedXP = 0;
                                    int MobExp = 0;
                                    MobExp = (npc.lifeMax + 65000) * PlayerHero.HeroExpMultiplier;
                                    switch (PlayerHero.HeroLevel)
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                        case 4:
                                        case 5:
                                        case 6:
                                        case 7:
                                        case 8:
                                        case 9:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 10:
                                        case 11:
                                        case 12:
                                        case 13:
                                        case 14:
                                        case 15:
                                        case 16:
                                        case 17:
                                        case 18:
                                        case 19:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp / 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 20:
                                        case 21:
                                        case 22:
                                        case 23:
                                        case 24:
                                        case 25:
                                        case 26:
                                        case 27:
                                        case 28:
                                        case 29:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 30:
                                        case 31:
                                        case 32:
                                        case 33:
                                        case 34:
                                        case 35:
                                        case 36:
                                        case 37:
                                        case 38:
                                        case 39:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 40:
                                        case 41:
                                        case 42:
                                        case 43:
                                        case 44:
                                        case 45:
                                        case 46:
                                        case 47:
                                        case 48:
                                        case 49:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 50:
                                        case 51:
                                        case 52:
                                        case 53:
                                        case 54:
                                        case 55:
                                        case 56:
                                        case 57:
                                        case 58:
                                        case 59:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 60:
                                        case 61:
                                        case 62:
                                        case 63:
                                        case 64:
                                        case 65:
                                        case 66:
                                        case 67:
                                        case 68:
                                        case 69:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 70:
                                        case 71:
                                        case 72:
                                        case 73:
                                        case 74:
                                        case 75:
                                        case 76:
                                        case 77:
                                        case 78:
                                        case 79:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2 + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 80:
                                        case 81:
                                        case 82:
                                        case 83:
                                        case 84:
                                        case 85:
                                        case 86:
                                        case 87:
                                        case 88:
                                        case 89:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 90:
                                        case 91:
                                        case 92:
                                        case 93:
                                        case 94:
                                        case 95:
                                        case 96:
                                        case 97:
                                        case 98:
                                        case 99:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 3;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        default:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 4;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                    }
                                    return;
                                }
                                else if (npc.netID == NPCID.MourningWood || npc.netID == NPCID.Pumpking || npc.netID == NPCID.PumpkingBlade || npc.netID == NPCID.Everscream || npc.netID == NPCID.IceQueen || npc.netID == NPCID.SantaNK1)
                                {
                                    int GainedXP = 0;
                                    int MobExp = 0;
                                    MobExp = (npc.lifeMax + 75000) * PlayerHero.HeroExpMultiplier;
                                    switch (PlayerHero.HeroLevel)
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                        case 4:
                                        case 5:
                                        case 6:
                                        case 7:
                                        case 8:
                                        case 9:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 10:
                                        case 11:
                                        case 12:
                                        case 13:
                                        case 14:
                                        case 15:
                                        case 16:
                                        case 17:
                                        case 18:
                                        case 19:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp / 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 20:
                                        case 21:
                                        case 22:
                                        case 23:
                                        case 24:
                                        case 25:
                                        case 26:
                                        case 27:
                                        case 28:
                                        case 29:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 30:
                                        case 31:
                                        case 32:
                                        case 33:
                                        case 34:
                                        case 35:
                                        case 36:
                                        case 37:
                                        case 38:
                                        case 39:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 40:
                                        case 41:
                                        case 42:
                                        case 43:
                                        case 44:
                                        case 45:
                                        case 46:
                                        case 47:
                                        case 48:
                                        case 49:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 50:
                                        case 51:
                                        case 52:
                                        case 53:
                                        case 54:
                                        case 55:
                                        case 56:
                                        case 57:
                                        case 58:
                                        case 59:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 60:
                                        case 61:
                                        case 62:
                                        case 63:
                                        case 64:
                                        case 65:
                                        case 66:
                                        case 67:
                                        case 68:
                                        case 69:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 70:
                                        case 71:
                                        case 72:
                                        case 73:
                                        case 74:
                                        case 75:
                                        case 76:
                                        case 77:
                                        case 78:
                                        case 79:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2 + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 80:
                                        case 81:
                                        case 82:
                                        case 83:
                                        case 84:
                                        case 85:
                                        case 86:
                                        case 87:
                                        case 88:
                                        case 89:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 90:
                                        case 91:
                                        case 92:
                                        case 93:
                                        case 94:
                                        case 95:
                                        case 96:
                                        case 97:
                                        case 98:
                                        case 99:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 3;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        default:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 4;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                    }
                                    return;
                                }
                                else if (npc.netID == NPCID.MartianSaucer || npc.netID == NPCID.MoonLordHead || npc.netID == NPCID.MoonLordHand || npc.netID == NPCID.MoonLordCore || npc.netID == NPCID.LunarTowerVortex || npc.netID == NPCID.CultistBoss || npc.netID == NPCID.LunarTowerStardust || npc.netID == NPCID.LunarTowerNebula || npc.netID == NPCID.LunarTowerSolar)
                                {
                                    int GainedXP = 0;
                                    int MobExp = 0;
                                    MobExp = (npc.lifeMax + 100000) * PlayerHero.HeroExpMultiplier;
                                    switch (PlayerHero.HeroLevel)
                                    {
                                        case 1:
                                        case 2:
                                        case 3:
                                        case 4:
                                        case 5:
                                        case 6:
                                        case 7:
                                        case 8:
                                        case 9:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 10:
                                        case 11:
                                        case 12:
                                        case 13:
                                        case 14:
                                        case 15:
                                        case 16:
                                        case 17:
                                        case 18:
                                        case 19:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp / 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 20:
                                        case 21:
                                        case 22:
                                        case 23:
                                        case 24:
                                        case 25:
                                        case 26:
                                        case 27:
                                        case 28:
                                        case 29:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 30:
                                        case 31:
                                        case 32:
                                        case 33:
                                        case 34:
                                        case 35:
                                        case 36:
                                        case 37:
                                        case 38:
                                        case 39:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 40:
                                        case 41:
                                        case 42:
                                        case 43:
                                        case 44:
                                        case 45:
                                        case 46:
                                        case 47:
                                        case 48:
                                        case 49:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp + PlayerHero.HeroLevel * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 50:
                                        case 51:
                                        case 52:
                                        case 53:
                                        case 54:
                                        case 55:
                                        case 56:
                                        case 57:
                                        case 58:
                                        case 59:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 60:
                                        case 61:
                                        case 62:
                                        case 63:
                                        case 64:
                                        case 65:
                                        case 66:
                                        case 67:
                                        case 68:
                                        case 69:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 70:
                                        case 71:
                                        case 72:
                                        case 73:
                                        case 74:
                                        case 75:
                                        case 76:
                                        case 77:
                                        case 78:
                                        case 79:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2 + PlayerHero.HeroLevel;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 80:
                                        case 81:
                                        case 82:
                                        case 83:
                                        case 84:
                                        case 85:
                                        case 86:
                                        case 87:
                                        case 88:
                                        case 89:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 2;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        case 90:
                                        case 91:
                                        case 92:
                                        case 93:
                                        case 94:
                                        case 95:
                                        case 96:
                                        case 97:
                                        case 98:
                                        case 99:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 3;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                        default:
                                            #region Exp And Level Handler
                                            GainedXP = MobExp * 4;
                                            if (GainedXP < 0)
                                            {
                                                GainedXP = 0;
                                            }
                                            if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                            {
                                                LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                return;
                                            }
                                            WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                            c.R = SetMsgColor();
                                            c.B = SetMsgColor();
                                            c.G = SetMsgColor();
                                            NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                + PlayerHero.HeroClass
                                                + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                + "]"
                                                + "("
                                                + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                + "%)"
                                                + "["
                                                + GainedXP
                                                + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                + 32, 0, 0, 0, 0);
                                            #endregion
                                            break;
                                    }
                                    return;
                                }
                                else
                                {
                                    int GainedXP = 0;
                                    int MobExp = 0;
                                    MobExp = npc.lifeMax * PlayerHero.HeroExpMultiplier;
                                    if (MobExp < 0)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        switch (PlayerHero.HeroLevel)
                                        {
                                            case 1:
                                            case 2:
                                            case 3:
                                            case 4:
                                            case 5:
                                            case 6:
                                            case 7:
                                            case 8:
                                            case 9:
                                                #region Exp And Level Handler
                                                GainedXP = MobExp;
                                                if (GainedXP < 0)
                                                {
                                                    GainedXP = 0;
                                                }
                                                if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                                {
                                                    LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                    return;
                                                }
                                                WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                                c.R = SetMsgColor();
                                                c.B = SetMsgColor();
                                                c.G = SetMsgColor();
                                                NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                    + PlayerHero.HeroClass
                                                    + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                    + "]"
                                                    + "("
                                                    + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                    + "%)"
                                                    + "["
                                                    + GainedXP
                                                    + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                    + 32, 0, 0, 0, 0);
                                                #endregion
                                                break;
                                            case 10:
                                            case 11:
                                            case 12:
                                            case 13:
                                            case 14:
                                            case 15:
                                            case 16:
                                            case 17:
                                            case 18:
                                            case 19:
                                                #region Exp And Level Handler
                                                GainedXP = MobExp / 2;
                                                if (GainedXP < 0)
                                                {
                                                    GainedXP = 0;
                                                }
                                                if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                                {
                                                    LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                    return;
                                                }
                                                WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                                c.R = SetMsgColor();
                                                c.B = SetMsgColor();
                                                c.G = SetMsgColor();
                                                NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                    + PlayerHero.HeroClass
                                                    + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                    + "]"
                                                    + "("
                                                    + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                    + "%)"
                                                    + "["
                                                    + GainedXP
                                                    + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                    + 32, 0, 0, 0, 0);
                                                #endregion
                                                break;
                                            case 20:
                                            case 21:
                                            case 22:
                                            case 23:
                                            case 24:
                                            case 25:
                                            case 26:
                                            case 27:
                                            case 28:
                                            case 29:
                                                #region Exp And Level Handler
                                                GainedXP = MobExp;
                                                if (GainedXP < 0)
                                                {
                                                    GainedXP = 0;
                                                }
                                                if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                                {
                                                    LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                    return;
                                                }
                                                WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                                c.R = SetMsgColor();
                                                c.B = SetMsgColor();
                                                c.G = SetMsgColor();
                                                NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                    + PlayerHero.HeroClass
                                                    + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                    + "]"
                                                    + "("
                                                    + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                    + "%)"
                                                    + "["
                                                    + GainedXP
                                                    + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                    + 32, 0, 0, 0, 0);
                                                #endregion
                                                break;
                                            case 30:
                                            case 31:
                                            case 32:
                                            case 33:
                                            case 34:
                                            case 35:
                                            case 36:
                                            case 37:
                                            case 38:
                                            case 39:
                                                #region Exp And Level Handler
                                                GainedXP = MobExp + PlayerHero.HeroLevel;
                                                if (GainedXP < 0)
                                                {
                                                    GainedXP = 0;
                                                }
                                                if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                                {
                                                    LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                    return;
                                                }
                                                WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                                c.R = SetMsgColor();
                                                c.B = SetMsgColor();
                                                c.G = SetMsgColor();
                                                NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                    + PlayerHero.HeroClass
                                                    + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                    + "]"
                                                    + "("
                                                    + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                    + "%)"
                                                    + "["
                                                    + GainedXP
                                                    + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                    + 32, 0, 0, 0, 0);
                                                #endregion
                                                break;
                                            case 40:
                                            case 41:
                                            case 42:
                                            case 43:
                                            case 44:
                                            case 45:
                                            case 46:
                                            case 47:
                                            case 48:
                                            case 49:
                                                #region Exp And Level Handler
                                                GainedXP = MobExp + PlayerHero.HeroLevel * 2;
                                                if (GainedXP < 0)
                                                {
                                                    GainedXP = 0;
                                                }
                                                if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                                {
                                                    LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                    return;
                                                }
                                                WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                                c.R = SetMsgColor();
                                                c.B = SetMsgColor();
                                                c.G = SetMsgColor();
                                                NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                    + PlayerHero.HeroClass
                                                    + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                    + "]"
                                                    + "("
                                                    + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                    + "%)"
                                                    + "["
                                                    + GainedXP
                                                    + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                    + 32, 0, 0, 0, 0);
                                                #endregion
                                                break;
                                            case 50:
                                            case 51:
                                            case 52:
                                            case 53:
                                            case 54:
                                            case 55:
                                            case 56:
                                            case 57:
                                            case 58:
                                            case 59:
                                                #region Exp And Level Handler
                                                GainedXP = MobExp;
                                                if (GainedXP < 0)
                                                {
                                                    GainedXP = 0;
                                                }
                                                if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                                {
                                                    LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                    return;
                                                }
                                                WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                                c.R = SetMsgColor();
                                                c.B = SetMsgColor();
                                                c.G = SetMsgColor();
                                                NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                    + PlayerHero.HeroClass
                                                    + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                    + "]"
                                                    + "("
                                                    + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                    + "%)"
                                                    + "["
                                                    + GainedXP
                                                    + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                    + 32, 0, 0, 0, 0);
                                                #endregion
                                                break;
                                            case 60:
                                            case 61:
                                            case 62:
                                            case 63:
                                            case 64:
                                            case 65:
                                            case 66:
                                            case 67:
                                            case 68:
                                            case 69:
                                                #region Exp And Level Handler
                                                GainedXP = MobExp * 2;
                                                if (GainedXP < 0)
                                                {
                                                    GainedXP = 0;
                                                }
                                                if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                                {
                                                    LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                    return;
                                                }
                                                WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                                c.R = SetMsgColor();
                                                c.B = SetMsgColor();
                                                c.G = SetMsgColor();
                                                NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                    + PlayerHero.HeroClass
                                                    + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                    + "]"
                                                    + "("
                                                    + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                    + "%)"
                                                    + "["
                                                    + GainedXP
                                                    + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                    + 32, 0, 0, 0, 0);
                                                #endregion
                                                break;
                                            case 70:
                                            case 71:
                                            case 72:
                                            case 73:
                                            case 74:
                                            case 75:
                                            case 76:
                                            case 77:
                                            case 78:
                                            case 79:
                                                #region Exp And Level Handler
                                                GainedXP = MobExp * 2 + PlayerHero.HeroLevel;
                                                if (GainedXP < 0)
                                                {
                                                    GainedXP = 0;
                                                }
                                                if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                                {
                                                    LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                    return;
                                                }
                                                WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                                c.R = SetMsgColor();
                                                c.B = SetMsgColor();
                                                c.G = SetMsgColor();
                                                NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                    + PlayerHero.HeroClass
                                                    + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                    + "]"
                                                    + "("
                                                    + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                    + "%)"
                                                    + "["
                                                    + GainedXP
                                                    + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                    + 32, 0, 0, 0, 0);
                                                #endregion
                                                break;
                                            case 80:
                                            case 81:
                                            case 82:
                                            case 83:
                                            case 84:
                                            case 85:
                                            case 86:
                                            case 87:
                                            case 88:
                                            case 89:
                                                #region Exp And Level Handler
                                                GainedXP = MobExp * 2;
                                                if (GainedXP < 0)
                                                {
                                                    GainedXP = 0;
                                                }
                                                if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                                {
                                                    LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                    return;
                                                }
                                                WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                                c.R = SetMsgColor();
                                                c.B = SetMsgColor();
                                                c.G = SetMsgColor();
                                                NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                    + PlayerHero.HeroClass
                                                    + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                    + "]"
                                                    + "("
                                                    + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                    + "%)"
                                                    + "["
                                                    + GainedXP
                                                    + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                    + 32, 0, 0, 0, 0);
                                                #endregion
                                                break;
                                            case 90:
                                            case 91:
                                            case 92:
                                            case 93:
                                            case 94:
                                            case 95:
                                            case 96:
                                            case 97:
                                            case 98:
                                            case 99:
                                                #region Exp And Level Handler
                                                GainedXP = MobExp * 3;
                                                if (GainedXP < 0)
                                                {
                                                    GainedXP = 0;
                                                }
                                                if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                                {
                                                    LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                    return;
                                                }
                                                WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                                c.R = SetMsgColor();
                                                c.B = SetMsgColor();
                                                c.G = SetMsgColor();
                                                NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                    + PlayerHero.HeroClass
                                                    + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                    + "]"
                                                    + "("
                                                    + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                    + "%)"
                                                    + "["
                                                    + GainedXP
                                                    + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                    + 32, 0, 0, 0, 0);
                                                #endregion
                                                break;
                                            default:
                                                #region Exp And Level Handler
                                                GainedXP = MobExp * 4;
                                                if (GainedXP < 0)
                                                {
                                                    GainedXP = 0;
                                                }
                                                if (PlayerHero.HeroLevel < PlayerHero.HeroMaxLevel && PlayerHero.HeroExperience >= PlayerHero.HeroNextLevel)
                                                {
                                                    LevelUp(TShock.Players[args.Msg.whoAmI]);
                                                    return;
                                                }
                                                WriteFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].Account.ID, "HeroExperience", Convert.ToString(PlayerHero.HeroExperience += GainedXP));
                                                c.R = SetMsgColor();
                                                c.B = SetMsgColor();
                                                c.G = SetMsgColor();
                                                NetMessage.SendData((int)119, -1, -1, Terraria.Localization.NetworkText.FromLiteral("["
                                                    + PlayerHero.HeroClass
                                                    + " " + "LvL" + " " + PlayerHero.HeroLevel
                                                    + "]"
                                                    + "("
                                                    + ((Convert.ToInt32(PlayerHero.HeroExperience += GainedXP) * 100) / Convert.ToInt32(PlayerHero.HeroNextLevel))
                                                    + "%)"
                                                    + "["
                                                    + GainedXP
                                                    + "xp!]"), (int)c.PackedValue, TShock.Players[args.Msg.whoAmI].TPlayer.position.X, TShock.Players[args.Msg.whoAmI].TPlayer.position.Y
                                                    + 32, 0, 0, 0, 0);
                                                #endregion
                                                break;
                                        }
                                        return;
                                    }
                                }
                            }
                            break;
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
        private void GameFilesCheck(object sender, ElapsedEventArgs args)
        {
            for (int i = 0; i < TShock.Players.Length; i++)
            {
                if (TShock.Players[i].Account == null || TShock.Players[i].TPlayer == null || TShock.Players[i].IsLoggedIn == false)
                {
                    return;
                }
                else
                {
                    if (!Directory.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID))
                    {
                        Directory.CreateDirectory(PlayerFilePath + "/" + TShock.Players[i].Account.ID);
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID + "/" + "HeroClass.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroClass", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID + "/" + "HeroLevel.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroLevel", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID + "/" + "HeroExperience.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroExperience", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID + "/" + "HeroExpMultiplier.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroExpMultiplier", "1");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID + "/" + "HeroNextLevel.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroNextLevel", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID + "/" + "HeroMaxLevel.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroMaxLevel", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID + "/" + "HeroAttackSkill.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroAttackSkill", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID + "/" + "HeroAttackBuff.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroAttackBuff", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID + "/" + "HeroDebuffs.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroDebuffs", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID + "/" + "HeroSkillpoints.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroSkillpoints", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID + "/" + "HeroDefence.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroDefence", "0");
                    }
                    if (!File.Exists(PlayerFilePath + "/" + TShock.Players[i].Account.ID + "/" + "HeroDamage.hotfile"))
                    {
                        WriteFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroDamage", "0");
                    }
                }
            }
        }
        private static void HeroClassSetup()
        {
            if (!File.Exists(HerosFilesPath + "/" + Heros.Class1.ClassName +  ".json"))
            {
                CreateJsonFile(HerosFilesPath, Heros.Class1.ClassName, Heros.Class1);
            }
            if (!File.Exists(HerosFilesPath + "/" + Heros.Class2.ClassName + ".json"))
            {
                CreateJsonFile(HerosFilesPath, Heros.Class2.ClassName, Heros.Class2);
            }
            if (!File.Exists(HerosFilesPath + "/" + Heros.Class3.ClassName + ".json"))
            {
                CreateJsonFile(HerosFilesPath, Heros.Class3.ClassName, Heros.Class3);
            }
            if (!File.Exists(HerosFilesPath + "/" + Heros.Class4.ClassName + ".json"))
            {
                CreateJsonFile(HerosFilesPath, Heros.Class4.ClassName, Heros.Class4);
            }
            if (!File.Exists(HerosFilesPath + "/" + Heros.Class5.ClassName + ".json"))
            {
                CreateJsonFile(HerosFilesPath, Heros.Class5.ClassName, Heros.Class5);
            }
        }
        private static void CreateFolders()
        {
            if (!Directory.Exists(PluginPath))
            {
                Directory.CreateDirectory(PluginPath);
            }
            if (!Directory.Exists(HerosFilesPath))
            {
                Directory.CreateDirectory(HerosFilesPath);
            }
            if (!Directory.Exists(PlayerFilePath))
            {
                Directory.CreateDirectory(PlayerFilePath);
            }
        }
        public static void CreateJsonFile(string FilePath, string Filename, object Input)
        {
            try
            {

                // serialize JSON directly to a file
                using (StreamWriter file = File.CreateText(FilePath + "/" + Filename + ".json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, Input);
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                //Console.Write(ex.ToString());
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
        public static void WriteFile(string FilePath, string Filename, string Data)
        {
            try
            {
                if (Filename != null || FilePath != null || Data != null)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath + "/" + Filename + ".hotfile", false))
                    {
                        sw.WriteLine(Data);
                        sw.Dispose();
                        sw.Close();
                    }
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
        public static void LevelUp(TSPlayer LevelUpPlayer)
        {
            try
            {
                if (PlayerHero.HeroLevel++ == PlayerHero.HeroMaxLevel)
                {
                    PlayerHero.HeroSkillpoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + LevelUpPlayer.Account.ID, "HeroSkillpoints"));
                    int NewSkillPoints = (PlayerHero.HeroSkillpoints + 1);
                    HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + LevelUpPlayer.Account.ID, "HeroSkillpoints", NewSkillPoints.ToString());
                    WriteFile(PlayerFilePath + "/" + LevelUpPlayer.Account.ID, "HeroLevel", PlayerHero.HeroMaxLevel.ToString());
                    WriteFile(PlayerFilePath + "/" + LevelUpPlayer.Account.ID, "HeroExperience", "99999999");
                    WriteFile(PlayerFilePath + "/" + LevelUpPlayer.Account.ID, "HeroNextLevel", "0");
                    LevelUpPlayer.SendMessage(
                    string.Format("You just turned level {0} +1 skillpoint gained No Way Dude x-x = {1}",
                    PlayerHero.HeroLevel,
                    PlayerHero.HeroNextLevel), Color.LightGreen);
                    TSPlayer.All.SendMessage(LevelUpPlayer.Account.Name
                        + " Just Turned Level "
                        + PlayerHero.HeroLevel
                        + " Holy-Shit bro.. You might need some help with your addiction to terraria!"
                        , 22, 111, 22);
                    HealOnLevelUp(LevelUpPlayer);
                    #region Level up effects
                    fireworkcolor = rnd.Next(1, 3);
                    type = 168;
                    int p1 = Projectile.NewProjectile(Projectile.GetNoneSource(), LevelUpPlayer.TPlayer.position.X, LevelUpPlayer.TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
                    Main.projectile[p1].Kill();
                    Main.projectile[p1].Kill();
                    fireworkcolor = rnd.Next(1, 3);
                    type = 169;
                    int p2 = Projectile.NewProjectile(Projectile.GetNoneSource(), LevelUpPlayer.TPlayer.position.X, LevelUpPlayer.TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
                    Main.projectile[p2].Kill();
                    Main.projectile[p2].Kill();
                    fireworkcolor = rnd.Next(1, 3);
                    type = 170;
                    int p3 = Projectile.NewProjectile(Projectile.GetNoneSource(), LevelUpPlayer.TPlayer.position.X, LevelUpPlayer.TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
                    Main.projectile[p3].Kill();
                    Main.projectile[p3].Kill();
                    fireworkcolor = rnd.Next(1, 3);
                    #endregion
                    return;
                }
                else
                {
                    PlayerHero.HeroSkillpoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + LevelUpPlayer.Account.ID, "HeroSkillpoints"));
                    int NewSkillPoints = (PlayerHero.HeroSkillpoints + 1);
                    HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + LevelUpPlayer.Account.ID, "HeroSkillpoints", NewSkillPoints.ToString());
                    PlayerHero.HeroNextLevel += (int)(Math.Pow(PlayerHero.HeroLevel, 2) * 7.4 * 40);
                    PlayerHero.HeroExperience = 0;
                    WriteFile(PlayerFilePath + "/" + LevelUpPlayer.Account.ID, "HeroLevel", PlayerHero.HeroLevel.ToString());
                    WriteFile(PlayerFilePath + "/" + LevelUpPlayer.Account.ID, "HeroExperience", PlayerHero.HeroExperience.ToString());
                    WriteFile(PlayerFilePath + "/" + LevelUpPlayer.Account.ID, "HeroNextLevel", PlayerHero.HeroNextLevel.ToString());
                    #region Level up effects
                    fireworkcolor = rnd.Next(1, 3);
                    switch (fireworkcolor)
                    {
                        case 1:
                            type = 168;
                            int p1 = Projectile.NewProjectile(Projectile.GetNoneSource(), LevelUpPlayer.TPlayer.position.X, LevelUpPlayer.TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
                            Main.projectile[p1].Kill();
                            Main.projectile[p1].Kill();
                            fireworkcolor = rnd.Next(1, 3);
                            break;
                        case 2:
                            type = 169;
                            int p2 = Projectile.NewProjectile(Projectile.GetNoneSource(), LevelUpPlayer.TPlayer.position.X, LevelUpPlayer.TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
                            Main.projectile[p2].Kill();
                            Main.projectile[p2].Kill();
                            fireworkcolor = rnd.Next(1, 3);
                            break;
                        case 3:
                            type = 170;
                            int p3 = Projectile.NewProjectile(Projectile.GetNoneSource(), LevelUpPlayer.TPlayer.position.X, LevelUpPlayer.TPlayer.position.Y - 64f, 0f, -8f, type, 0, (float)0);
                            Main.projectile[p3].Kill();
                            Main.projectile[p3].Kill();
                            fireworkcolor = rnd.Next(1, 3);
                            break;
                    }
                    #endregion
                    LevelUpPlayer.SendMessage(
                        string.Format("You just turned level {0} +1 skillpoint gained - exp until levelup = {1}",
                        PlayerHero.HeroLevel,
                        PlayerHero.HeroNextLevel), Color.LightGreen);
                    TSPlayer.All.SendMessage(LevelUpPlayer.Account.Name
                        + " Just Turned Level "
                        + PlayerHero.HeroLevel
                        + " Nice Job "
                        + PlayerHero.HeroClass
                        + "!", 22, 111, 22);
                    HealOnLevelUp(LevelUpPlayer);
                }
                //RunAsyncLevelUp(LevelUpPlayer);
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
            }
        }
        public static void HealOnLevelUp(TSPlayer HealThisPlayer)
        {
            try
            {
                using (var reader = db.QueryReader("SELECT * FROM tsCharacter WHERE Account=@0", HealThisPlayer))
                {
                    if (reader.Read())
                    {
                        int maxhp;
                        maxhp = Convert.ToInt32(reader.Get<string>("MaxHealth"));
                        HealThisPlayer.Heal(Convert.ToInt32(maxhp));
                    }
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
            }
        }
        public static byte SetMsgColor()
        {
            byte color = Convert.ToByte(rnd.Next(29, 255));
            return color;
        }
        private void SendInfomationMessageOnElapsed(object sender, ElapsedEventArgs args)
        {
            for (int i = 0; i < TShock.Players.Length; i++)
            {
                if (TShock.Players[i].Account == null || TShock.Players[i].TPlayer == null || !TShock.Players[i].IsLoggedIn)
                {
                    return;
                }
                int PlayerSkillPoints = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[i].Account.ID, "HeroSkillpoints"));
                if (PlayerSkillPoints > 0)
                {
                    TShock.Players[i].SendMessage(
                    string.Format("You have {0} Unused Skillpoints left!",
                    PlayerSkillPoints),
                    Color.LightGoldenrodYellow);
                    TShock.Players[i].SendMessage(
                    string.Format("Say /skillinfo to get more infomation"),
                    Color.LightGoldenrodYellow);
                }
            }
        }
        private void AddCommands()
        {
            Action<Command> Add = c =>
            {
                TShockAPI.Commands.ChatCommands.RemoveAll(c2 => c2.Names.Select(s => s.ToLowerInvariant()).Intersect(c.Names.Select(s => s.ToLowerInvariant())).Any());
                TShockAPI.Commands.ChatCommands.Add(c);
            };
            Add(new Command(Permissions.classinfo, Commands.ClassInfo, "classinfo")
            {
                AllowServer = true,
                HelpText = "Display infomation about Heros."
            });
            Add(new Command(Permissions.pickclass, Commands.PickClass, "pickclass")
            {
                AllowServer = false,
                HelpText = "Pick a Class please."
            });
            Add(new Command(Permissions.changeclass, Commands.ChangeClass, "changeclass")
            {
                AllowServer = false,
                HelpText = "Change Class for the cost of your hp and hp."
            });
            Add(new Command(Permissions.showexpandlevelinfo, Commands.ShowExpAndLevelInfo, "exp")
            {
                AllowServer = false,
                HelpText = "Display infomation about Your Exp and Level."
            });
            Add(new Command(Permissions.useskillpoints, Commands.UseSkillPoints, "skill")
            {
                AllowServer = false,
                HelpText = "Use 1 skill or all skillpoints"
            });
            Add(new Command(Permissions.showstats, Commands.ShowStats, "stats")
            {
                AllowServer = false,
                HelpText = "Shows Hero Stats"
            });
            Add(new Command(Permissions.skillinfo, Commands.SkillInfo, "skillinfo")
            {
                AllowServer = false,
                HelpText = "Shows Skill points infomation"
            });
            return;
        }
        private void StartTimers()
        {
            update = new Timer { Interval = 1000, AutoReset = true, Enabled = true };
            update.Elapsed += GameFilesCheck;
            SendInfomationMessage = new Timer { Interval = 300000, AutoReset = true, Enabled = true };
            SendInfomationMessage.Elapsed += SendInfomationMessageOnElapsed;
        }
    }
}
