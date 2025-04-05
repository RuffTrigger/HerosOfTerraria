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

namespace HerosOfTerraria_AttackEffects
{
    [ApiVersion(2, 1)]
    public class HerosOfTerraria_AttackEffects : TerrariaPlugin
    {
        public static Random rnd = new Random();
        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();
        public static string PlayerFilePath
        {
            get { return Path.Combine(PluginPath, "" + "PlayerFiles" + ""); }
        }
        public static List<Player> players = new List<Player>();
        public override string Name { get { return "" + "Heros Of Terraria" + ""; } }
        public override string Author { get { return "Hi5AMidget"; } }
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

            ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
            ServerApi.Hooks.NetGetData.Register(this, OnGetData);



        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
                ServerApi.Hooks.NetGetData.Deregister(this, OnGetData);


            }
        }
        private void OnInitialize(EventArgs args)
        {

        }
        public static void OnGetData(GetDataEventArgs args)
        {
            if (args == null || TShock.Players[args.Msg.whoAmI].User == null || TShock.Players[args.Msg.whoAmI].TPlayer == null || TShock.Players[args.Msg.whoAmI].IsLoggedIn == false)
            {
                return;
            }
            using (var reader = new BinaryReader(new MemoryStream(args.Msg.readBuffer, args.Index, args.Length)))
            {
                Player player = players.Where(p => p.Equals(args.MsgID)).FirstOrDefault();
                switch (args.MsgID)
                {
                    case PacketTypes.ConnectRequest:
                        break;
                    case PacketTypes.Disconnect:
                        break;
                    case PacketTypes.ContinueConnecting:
                        break;
                    case PacketTypes.PlayerInfo:
                        break;
                    case PacketTypes.PlayerSlot:
                        break;
                    case PacketTypes.ContinueConnecting2:
                        break;
                    case PacketTypes.WorldInfo:
                        break;
                    case PacketTypes.TileGetSection:
                        break;
                    case PacketTypes.Status:
                        break;
                    case PacketTypes.TileSendSection:
                        break;
                    case PacketTypes.TileFrameSection:
                        break;
                    case PacketTypes.PlayerSpawn:
                        break;
                    case PacketTypes.PlayerUpdate:
                        break;
                    case PacketTypes.PlayerActive:
                        break;
                    case PacketTypes.PlayerHp:
                        break;
                    case PacketTypes.Tile:
                        break;
                    case PacketTypes.TimeSet:
                        break;
                    case PacketTypes.DoorUse:
                        break;
                    case PacketTypes.TileSendSquare:
                        break;
                    case PacketTypes.ItemDrop:
                        break;
                    case PacketTypes.ItemOwner:
                        break;
                    case PacketTypes.NpcUpdate:
                        break;
                    case PacketTypes.NpcItemStrike:
                        break;
                    case PacketTypes.PlayerDamage:
                        break;
                    case PacketTypes.ProjectileNew:
                        break;
                    case PacketTypes.NpcStrike:
                        Int16 npcID = reader.ReadInt16();
                        Int16 dmg = reader.ReadInt16();
                        float knockback = reader.ReadSingle();
                        byte direction = reader.ReadByte();
                        bool critical = reader.ReadBoolean();
                        //bool secondCrit = random.Next(1, 101) <= Player.Crit;
                        NPC npc = Main.npc[npcID];
                        #region Adding Class Attack Effects
                        string PlayerClass = ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroClass").ToString().Trim();
                        //TShock.Players[args.Msg.whoAmI].SendData(PacketTypes.NpcStrike, "", npcID, (float)(dmg+ Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroLevel").Trim())), (float)knockback, (float)direction, critical ? 1 : 0);
                        switch (PlayerClass)
                        {
                            default:
                                break;
                            case "Diabolist":
                                npc.AddBuff(Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroDebuffs").Trim()), 5000, true);
                                if (rndNext(1, 11) >= 10)
                                {
                                    int PlayerLevel = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroLevel").Trim());
                                    int type = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroAttackSkill").Trim());
                                    int p2 = Projectile.NewProjectile(npc.position.X, npc.position.Y, 0f, dmg / 3, type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);

                                    //This is for explosives //Main.projectile[p2].Kill();
                                    //This is for spawns//Projectile.NewProjectile(npc.position.X, npc.position.Y, 0f, dmg, type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);

                                    Projectile.NewProjectile(npc.position.X + +-rndNext(1, 5), npc.position.Y + +-rndNext(1, 5), 0f, ((dmg + PlayerLevel) / 2), type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);
                                }
                                if (TShock.Players[args.Msg.whoAmI].TPlayer.buffTime.Length >= 300)
                                {
                                    break;
                                }
                                else
                                {
                                    if (getrandom.Next(1, 20) >= 19)
                                    {



                                        TShock.Players[args.Msg.whoAmI].SetBuff(Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroAttackBuff").Trim()), 300);
                                    }

                                }
                                break;
                            case "Gigazapper":
                                npc.AddBuff(Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroDebuffs").Trim()), 5000, true);
                                if (rndNext(1, 11) >= 10)
                                {
                                    int PlayerLevel = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroLevel").Trim());
                                    int type = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroAttackSkill").Trim());
                                    int p2 = Projectile.NewProjectile(npc.position.X, npc.position.Y, 0f, dmg / 3, type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);

                                    //This is for explosives //Main.projectile[p2].Kill();
                                    //This is for spawns//Projectile.NewProjectile(npc.position.X, npc.position.Y, 0f, dmg, type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);
                                    Projectile.NewProjectile(npc.position.X + +-rndNext(1, 5), npc.position.Y + +-rndNext(1, 5), 0f, ((dmg + PlayerLevel) / 2), type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);
                                }
                                if (TShock.Players[args.Msg.whoAmI].TPlayer.buffTime.Length >= 300)
                                {
                                    break;
                                }
                                else
                                {
                                    if (getrandom.Next(1, 20) >= 19)
                                    {



                                        TShock.Players[args.Msg.whoAmI].SetBuff(Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroAttackBuff").Trim()), 300);
                                    }

                                }
                                break;
                            case "Reaper":
                                npc.AddBuff(Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroDebuffs").Trim()), 5000, true);
                                if (rndNext(1, 11) >= 10)
                                {
                                    int PlayerLevel = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroLevel").Trim());
                                    int type = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroAttackSkill").Trim());
                                    int p2 = Projectile.NewProjectile(npc.position.X, npc.position.Y, 0f, dmg / 3, type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);

                                    //This is for explosives //Main.projectile[p2].Kill();
                                    //This is for spawns//Projectile.NewProjectile(npc.position.X, npc.position.Y, 0f, dmg, type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);
                                    Projectile.NewProjectile(npc.position.X + +-rndNext(1, 5), npc.position.Y + +-rndNext(1, 5), 0f, ((dmg + PlayerLevel) / 2), type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);
                                }
                                if (TShock.Players[args.Msg.whoAmI].TPlayer.buffTime.Length >= 300)
                                {
                                    break;
                                }
                                else
                                {
                                    if (getrandom.Next(1, 20) >= 19)
                                    {



                                        TShock.Players[args.Msg.whoAmI].SetBuff(Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroAttackBuff").Trim()), 300);
                                    }

                                }
                                break;
                            case "Selenian":
                                npc.AddBuff(Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroDebuffs").Trim()), 5000, true);
                                if (rndNext(1, 11) >= 10)
                                {
                                    int PlayerLevel = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroLevel").Trim());
                                    int type = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroAttackSkill").Trim());
                                    int p2 = Projectile.NewProjectile(npc.position.X, npc.position.Y, 0f, dmg / 3, type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);

                                    //This is for explosives //Main.projectile[p2].Kill();
                                    //This is for spawns//Projectile.NewProjectile(npc.position.X, npc.position.Y, 0f, dmg, type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);
                                    Projectile.NewProjectile(npc.position.X + +-rndNext(1, 5), npc.position.Y + +-rndNext(1, 5), 0f, ((dmg + PlayerLevel) / 2), type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);
                                }
                                if (TShock.Players[args.Msg.whoAmI].TPlayer.buffTime.Length >= 300)
                                {
                                    break;
                                }
                                else
                                {
                                    if (getrandom.Next(1, 20) >= 19)
                                    {



                                        TShock.Players[args.Msg.whoAmI].SetBuff(Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroAttackBuff").Trim()), 300);
                                    }

                                }
                                break;
                            case "QueenBee":
                                npc.AddBuff(Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroDebuffs").Trim()), 5000, true);
                                if (rndNext(1, 26) >= 25)
                                {
                                    int PlayerLevel = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroLevel").Trim());
                                    int type = Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroAttackSkill").Trim());
                                    int p2 = Projectile.NewProjectile(npc.position.X + +-rndNext(1, 5), npc.position.Y + +-rndNext(1, 5), 0f, ((dmg + PlayerLevel) / 2), type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);

                                    //This is for explosives //Main.projectile[p2].Kill();
                                    //This is for spawns//Projectile.NewProjectile(npc.position.X, npc.position.Y, 0f, dmg, type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);
                                    //Projectile.NewProjectile(npc.position.X + +-rndNext(1, 5), npc.position.Y + +-rndNext(1, 5), 0f, ((dmg + PlayerLevel) / 2), type, TShock.Players[args.Msg.whoAmI].User.ID, (float)0);
                                    Main.projectile[p2].Kill();
                                }
                                if (TShock.Players[args.Msg.whoAmI].TPlayer.buffTime.Length >= 300)
                                {
                                    break;
                                }
                                else
                                {
                                    if (getrandom.Next(1, 20) >= 19)
                                    {



                                        TShock.Players[args.Msg.whoAmI].SetBuff(Convert.ToInt32(ReadFile(PlayerFilePath + "/" + TShock.Players[args.Msg.whoAmI].User.ID, "HeroAttackBuff").Trim()), 300);
                                    }

                                }
                                break;
                        }
                        #endregion
                        break;
                    case PacketTypes.ProjectileDestroy:
                        break;
                    case PacketTypes.TogglePvp:
                        break;
                    case PacketTypes.ChestGetContents:
                        break;
                    case PacketTypes.ChestItem:
                        break;
                    case PacketTypes.ChestOpen:
                        break;
                    case PacketTypes.TileKill:
                        break;
                    case PacketTypes.EffectHeal:
                        break;
                    case PacketTypes.Zones:
                        break;
                    case PacketTypes.PasswordRequired:
                        break;
                    case PacketTypes.PasswordSend:
                        break;
                    case PacketTypes.RemoveItemOwner:
                        break;
                    case PacketTypes.NpcTalk:
                        break;
                    case PacketTypes.PlayerAnimation:
                        break;
                    case PacketTypes.PlayerMana:
                        break;
                    case PacketTypes.EffectMana:
                        break;
                    case PacketTypes.PlayerKillMe:
                        break;
                    case PacketTypes.PlayerTeam:
                        break;
                    case PacketTypes.SignRead:
                        break;
                    case PacketTypes.SignNew:
                        break;
                    case PacketTypes.LiquidSet:
                        break;
                    case PacketTypes.PlayerSpawnSelf:
                        break;
                    case PacketTypes.PlayerBuff:
                        break;
                    case PacketTypes.NpcSpecial:
                        break;
                    case PacketTypes.ChestUnlock:
                        break;
                    case PacketTypes.NpcAddBuff:
                        break;
                    case PacketTypes.NpcUpdateBuff:
                        break;
                    case PacketTypes.PlayerAddBuff:
                        break;
                    case PacketTypes.UpdateNPCName:
                        break;
                    case PacketTypes.UpdateGoodEvil:
                        break;
                    case PacketTypes.PlayHarp:
                        break;
                    case PacketTypes.HitSwitch:
                        break;
                    case PacketTypes.UpdateNPCHome:
                        break;
                    case PacketTypes.SpawnBossorInvasion:
                        break;
                    case PacketTypes.PlayerDodge:
                        break;
                    case PacketTypes.PaintTile:
                        break;
                    case PacketTypes.PaintWall:
                        break;
                    case PacketTypes.Teleport:
                        break;
                    case PacketTypes.PlayerHealOther:
                        break;
                    case PacketTypes.Placeholder:
                        break;
                    case PacketTypes.ClientUUID:
                        break;
                    case PacketTypes.ChestName:
                        break;
                    case PacketTypes.CatchNPC:
                        break;
                    case PacketTypes.ReleaseNPC:
                        break;
                    case PacketTypes.TravellingMerchantInventory:
                        break;
                    case PacketTypes.TeleportationPotion:
                        break;
                    case PacketTypes.AnglerQuest:
                        break;
                    case PacketTypes.CompleteAnglerQuest:
                        break;
                    case PacketTypes.NumberOfAnglerQuestsCompleted:
                        break;
                    case PacketTypes.CreateTemporaryAnimation:
                        break;
                    case PacketTypes.ReportInvasionProgress:
                        break;
                    case PacketTypes.PlaceObject:
                        break;
                    case PacketTypes.SyncPlayerChestIndex:
                        break;
                    case PacketTypes.CreateCombatText:
                        break;
                    case PacketTypes.LoadNetModule:
                        break;
                    case PacketTypes.NpcKillCount:
                        break;
                    case PacketTypes.PlayerStealth:
                        break;
                    case PacketTypes.ForceItemIntoNearestChest:
                        break;
                    case PacketTypes.UpdateTileEntity:
                        break;
                    case PacketTypes.PlaceTileEntity:
                        break;
                    case PacketTypes.TweakItem:
                        break;
                    case PacketTypes.PlaceItemFrame:
                        break;
                    case PacketTypes.UpdateItemDrop:
                        break;
                    case PacketTypes.EmoteBubble:
                        break;
                    case PacketTypes.SyncExtraValue:
                        break;
                    case PacketTypes.SocialHandshake:
                        break;
                    case PacketTypes.Deprecated:
                        break;
                    case PacketTypes.KillPortal:
                        break;
                    case PacketTypes.PlayerTeleportPortal:
                        break;
                    case PacketTypes.NotifyPlayerNpcKilled:
                        break;
                    case PacketTypes.NotifyPlayerOfEvent:
                        break;
                    case PacketTypes.UpdateMinionTarget:
                        break;
                    case PacketTypes.NpcTeleportPortal:
                        break;
                    case PacketTypes.UpdateShieldStrengths:
                        break;
                    case PacketTypes.NebulaLevelUp:
                        break;
                    case PacketTypes.MoonLordCountdown:
                        break;
                    case PacketTypes.NpcShopItem:
                        break;
                    case PacketTypes.GemLockToggle:
                        break;
                    case PacketTypes.PoofOfSmoke:
                        break;
                    case PacketTypes.SmartTextMessage:
                        break;
                    case PacketTypes.WiredCannonShot:
                        break;
                    case PacketTypes.MassWireOperation:
                        break;
                    case PacketTypes.MassWireOperationPay:
                        break;
                    case PacketTypes.ToggleParty:
                        break;
                    case PacketTypes.TreeGrowFX:
                        break;
                    case PacketTypes.CrystalInvasionStart:
                        break;
                    case PacketTypes.CrystalInvasionWipeAll:
                        break;
                    case PacketTypes.MinionAttackTargetUpdate:
                        break;
                    case PacketTypes.CrystalInvasionSendWaitTime:
                        break;
                    case PacketTypes.PlayerHurtV2:
                        break;
                    case PacketTypes.PlayerDeathV2:
                        break;
                }

            }
        }
        public static int rndNext(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next(min, max);
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
                    StreamReaders.Add(new StreamReader(FilePath + "/" + Filename + ".json"));
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
