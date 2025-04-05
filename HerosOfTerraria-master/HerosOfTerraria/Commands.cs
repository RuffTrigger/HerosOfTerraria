using System;
using System.IO;
using Newtonsoft.Json;
//Terraria Api's
using TShockAPI;
// Database Plugins
using TShockAPI.DB;

namespace HerosOfTerraria
{
    public static class Commands
    {
        public static Int32 DefaultHP = 100;
        public static Int32 DefaultMAXHP = 100;
        public static Int32 DefaultMP = 100;
        public static Int32 DefaultMAXMP = 20;
        public static void ClassInfo(CommandArgs args)
        {
            if (!args.Player.IsLoggedIn)
            {
                args.Player.SendErrorMessage("Please Login first!");
                return;
            }
            if (args.Player.Account == null)
            {
                args.Player.SendErrorMessage("You need to register before you can use this command!");
                return;
            }
            try
            {
                if (args.Parameters.Count == 0 || args.Parameters.Count > 1)
                {
                    args.Player.SendInfoMessage("Class Info");
                    args.Player.SendErrorMessage("Say /pickclass and classname or number.");
                    args.Player.SendErrorMessage("Say /classinfo 1 or /classinfo 2 or /classinfo 3 or /classinfo 4 for more infomation.");
                    return;
                }
                switch (args.Parameters[0])
                {
                    case "1":
                        using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class1.ClassName + ".json"))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            Heros Class1 = (Heros)serializer.Deserialize(file, typeof(Heros));
                            args.Player.SendInfoMessage("Hero Name =" + Class1.ClassName);
                            args.Player.SendInfoMessage(Class1.Description);
                            args.Player.SendInfoMessage("Maxlevel =" + Class1.MaxLevel.ToString());
                            args.Player.SendInfoMessage("Attack Skill = Betsy's Wrath");
                            args.Player.SendInfoMessage("Attack Buff = Damage Booster 15%");
                            args.Player.SendInfoMessage("Attack Debuff = Burning");
                        }
                        break;
                    case "2":
                        using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class2.ClassName + ".json"))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            Heros Class2 = (Heros)serializer.Deserialize(file, typeof(Heros));
                            args.Player.SendInfoMessage("Hero Name =" + Class2.ClassName);
                            args.Player.SendInfoMessage(Class2.Description);
                            args.Player.SendInfoMessage("Maxlevel =" + Class2.MaxLevel.ToString());
                            args.Player.SendInfoMessage("Attack Skill = Laser");
                            args.Player.SendInfoMessage("Attack Buff = Ammo Reservation");
                            args.Player.SendInfoMessage("Attack Debuff = Confused");
                        }
                        break;
                    case "3":
                        using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class3.ClassName + ".json"))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            Heros Class3 = (Heros)serializer.Deserialize(file, typeof(Heros));
                            args.Player.SendInfoMessage("Hero Name =" + Class3.ClassName);
                            args.Player.SendInfoMessage(Class3.Description);
                            args.Player.SendInfoMessage("Maxlevel =" + Class3.MaxLevel.ToString());
                            args.Player.SendInfoMessage("Attack Skill = Shadowflame Skull");
                            args.Player.SendInfoMessage("Attack Buff = Mana Regeneration");
                            args.Player.SendInfoMessage("Attack Debuff = Slow");
                        }
                        break;
                    case "4":
                        using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class4.ClassName + ".json"))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            Heros Selenian = (Heros)serializer.Deserialize(file, typeof(Heros));
                            args.Player.SendInfoMessage("Hero Name =" + Selenian.ClassName);
                            args.Player.SendInfoMessage(Selenian.Description);
                            args.Player.SendInfoMessage("Maxlevel =" + Selenian.MaxLevel.ToString());
                            args.Player.SendInfoMessage("Attack Skill = Solar Eruption");
                            args.Player.SendInfoMessage("Attack Buff = Thorns");
                            args.Player.SendInfoMessage("Attack Debuff = Ichor");
                        }
                        break;
                    case "5":
                        using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class5.ClassName + ".json"))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            Heros Beekeeper = (Heros)serializer.Deserialize(file, typeof(Heros));
                            args.Player.SendInfoMessage("Hero Name =" + Beekeeper.ClassName);
                            args.Player.SendInfoMessage(Beekeeper.Description);
                            args.Player.SendInfoMessage("Maxlevel =" + Beekeeper.MaxLevel.ToString());
                            args.Player.SendInfoMessage("Attack Skill = Bees");
                            args.Player.SendInfoMessage("Attack Buff = Sugar Rush");
                            args.Player.SendInfoMessage("Attack Debuff = Weak");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
            }
        }
        public static void PickClass(CommandArgs args)
        {
            if (!args.Player.IsLoggedIn)
            {
                args.Player.SendErrorMessage("Please Login first!");
                return;
            }
            if (args.Player.Account == null)
            {
                args.Player.SendErrorMessage("You need to register before you can use this command!");
                return;
            }
            try
            {
                if (args.Parameters.Count == 0 || args.Parameters.Count > 1)
                {
                    args.Player.SendErrorMessage("Invalid syntax: - say /classinfo for more infomation.");
                    args.Player.SendErrorMessage("Say /classinfo 1 or /classinfo 2 or /classinfo 3 or /classinfo 4");
                    return;
                }
                else
                {
                    if (HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class1.ClassName || HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class2.ClassName || HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class3.ClassName || HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class4.ClassName)
                    {
                        args.Player.SendErrorMessage("You allready picked The Class=" + HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() + "!");
                        args.Player.SendErrorMessage("You can change your Class by using /changeclass");
                        return;
                    }
                    switch (HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString())
                    {
                        default:
                            switch (args.Parameters[0])
                            {
                                case "1":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class1.ClassName + ".json"))
                                    {
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class1 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class1.ClassName.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroLevel", Class1.Level.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroExperience", Class1.Experience.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroNextLevel", Class1.NextLevel.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroMaxLevel", Class1.MaxLevel.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class1.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class1.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class1.Debuffs.ToString());
                                        args.Player.SendInfoMessage("You have picked the heros = " + Class1.ClassName + " .. Enjoy!");
                                    }
                                    break;
                                case "2":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class2.ClassName + ".json"))
                                    {
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class2 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class2.ClassName.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroLevel", Class2.Level.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroExperience", Class2.Experience.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroNextLevel", Class2.NextLevel.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroMaxLevel", Class2.MaxLevel.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class2.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class2.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class2.Debuffs.ToString());
                                        args.Player.SendInfoMessage("You have picked the heros = " + Class2.ClassName + " .. Enjoy!");
                                    }
                                    break;
                                case "3":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class3.ClassName + ".json"))
                                    {
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class3 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class3.ClassName.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroLevel", Class3.Level.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroExperience", Class3.Experience.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroNextLevel", Class3.NextLevel.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroMaxLevel", Class3.MaxLevel.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class3.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class3.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class3.Debuffs.ToString());
                                        args.Player.SendInfoMessage("You have picked the heros = " + Class3.ClassName + " .. Enjoy!");
                                    }
                                    break;
                                case "4":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class4.ClassName + ".json"))
                                    {
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class4 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class4.ClassName.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroLevel", Class4.Level.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroExperience", Class4.Experience.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroNextLevel", Class4.NextLevel.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroMaxLevel", Class4.MaxLevel.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class4.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class4.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class4.Debuffs.ToString());
                                        args.Player.SendInfoMessage("You have picked the heros = " + Class4.ClassName + " .. Enjoy!");
                                    }
                                    break;
                                case "5":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class5.ClassName + ".json"))
                                    {
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class5 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class5.ClassName.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroLevel", Class5.Level.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroExperience", Class5.Experience.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroNextLevel", Class5.NextLevel.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroMaxLevel", Class5.MaxLevel.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class5.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class5.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class5.Debuffs.ToString());
                                        args.Player.SendInfoMessage("You have picked the heros = " + Class5.ClassName + " .. Enjoy!");
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
            }
        }
        public static void ChangeClass(CommandArgs args)
        {
            if (!args.Player.IsLoggedIn)
            {
                args.Player.SendErrorMessage("Please Login first!");
                return;
            }
            if (args.Player.Account == null)
            {
                args.Player.SendErrorMessage("You need to register before you can use this command!");
                return;
            }
            try
            {
                if (args.Parameters.Count < 1)
                {
                    args.Player.SendErrorMessage("W A R N I N G! - Changing Class will reset your hp and mp to default and will reset your buffs");
                    args.Player.SendErrorMessage("Changing Class requires 400+ HP and 200+ MP");
                    args.Player.SendInfoMessage("1=" + Heros.Class1.ClassName + " 2=" + Heros.Class2.ClassName + " 3=" + Heros.Class3.ClassName + " 4 =" + Heros.Class4.ClassName + " 5 =" + Heros.Class5.ClassName);
                    args.Player.SendInfoMessage("Good Luck!");
                    return;
                }
                else
                {
                    int UserHP;
                    int UserMP;
                    bool UserHasMaxHpAndLife = false;
                    #region Changing Class Failed
                    if (HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class1.ClassName && args.Parameters[0].ToString() == "1")
                    {
                        args.Player.SendErrorMessage("You already chosen " + Heros.Class1.ClassName);
                        return;
                    }
                    if (HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class2.ClassName && args.Parameters[0].ToString() == "2")
                    {
                        args.Player.SendErrorMessage("You already chosen " + Heros.Class2.ClassName);
                        return;
                    }
                    if (HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class3.ClassName && args.Parameters[0].ToString() == "3")
                    {
                        args.Player.SendErrorMessage("You already chosen " + Heros.Class3.ClassName);
                        return;
                    }
                    if (HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class4.ClassName && args.Parameters[0].ToString() == "4")
                    {
                        args.Player.SendErrorMessage("You already chosen " + Heros.Class4.ClassName);
                        return;
                    }
                    #endregion
                    #region Changing Class
                    if (HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class1.ClassName && args.Parameters[0].ToString() != "1")
                    {
                        using (var reader_HP_MP = TShock.DB.QueryReader("SELECT * FROM tsCharacter WHERE Account=@0", args.Player.Account.ID))
                        {
                            if (reader_HP_MP.Read())
                            {
                                UserHP = reader_HP_MP.Get<Int32>("MaxHealth");
                                UserMP = reader_HP_MP.Get<Int32>("MaxMana");
                                if (UserHP >= 400 && UserMP >= 200)
                                {
                                    UserHasMaxHpAndLife = true;
                                }
                                else
                                {
                                    UserHasMaxHpAndLife = false;
                                    args.Player.SendErrorMessage("Changing Class requires - 400+ HP and 200+ MP");
                                    args.Player.SendErrorMessage("You have -" + " " + UserHP.ToString() + " " + "HP" + " " + "and" + " " + UserMP.ToString() + " " + "MP" + " ");
                                    return;
                                }
                            }
                        }
                        if (UserHasMaxHpAndLife)
                        {
                            switch (args.Parameters[0])
                            {
                                case "2":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class2.ClassName + ".json"))
                                    {
                                        args.Player.SaveServerCharacter();
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class2 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class2.ClassName);
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class2.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class2.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class2.Debuffs.ToString());
                                        args.Player.Kick("You Change Hero Class Too = " + Class2.ClassName.ToString() + " " + "REJOIN!", true, true, null, false);
                                        TShock.DB.Query("UPDATE tsCharacter SET Health=@0, MaxHealth=@1, Mana=@2, MaxMana=@3 WHERE Account=@4;", DefaultHP, DefaultMAXHP, DefaultMP, DefaultMAXMP, args.Player.Account.ID);
                                    }
                                    break;
                                case "3":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class3.ClassName + ".json"))
                                    {
                                        args.Player.SaveServerCharacter();
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class3 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class3.ClassName);
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class3.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class3.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class3.Debuffs.ToString());
                                        args.Player.Kick("You Change Hero Class Too = " + Class3.ClassName.ToString() + " " + "REJOIN!", true, true, null, false);
                                        TShock.DB.Query("UPDATE tsCharacter SET Health=@0, MaxHealth=@1, Mana=@2, MaxMana=@3 WHERE Account=@4;", DefaultHP, DefaultMAXHP, DefaultMP, DefaultMAXMP, args.Player.Account.ID);
                                    }
                                    break;
                                case "4":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class4.ClassName + ".json"))
                                    {
                                        args.Player.SaveServerCharacter();
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class4 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class4.ClassName);
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class4.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class4.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class4.Debuffs.ToString());
                                        args.Player.Kick("You Change Hero Class Too = " + Class4.ClassName.ToString() + " " + "REJOIN!", true, true, null, false);
                                        TShock.DB.Query("UPDATE tsCharacter SET Health=@0, MaxHealth=@1, Mana=@2, MaxMana=@3 WHERE Account=@4;", DefaultHP, DefaultMAXHP, DefaultMP, DefaultMAXMP, args.Player.Account.ID);
                                    }
                                    break;
                            }
                        }
                        return;
                    }
                    if (HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class2.ClassName && args.Parameters[0].ToString() != "2")
                    {
                        using (var reader_HP_MP = TShock.DB.QueryReader("SELECT * FROM tsCharacter WHERE Account=@0", args.Player.Account.ID))
                        {
                            if (reader_HP_MP.Read())
                            {
                                UserHP = reader_HP_MP.Get<Int32>("MaxHealth");
                                UserMP = reader_HP_MP.Get<Int32>("MaxMana");
                                if (UserHP >= 400 && UserMP >= 200)
                                {
                                    UserHasMaxHpAndLife = true;
                                }
                                else
                                {
                                    UserHasMaxHpAndLife = false;
                                    args.Player.SendErrorMessage("Changing Class requires - 400+ HP and 200+ MP");
                                    args.Player.SendErrorMessage("You have -" + " " + UserHP.ToString() + " " + "HP" + " " + "and" + " " + UserMP.ToString() + " " + "MP" + " ");
                                    return;
                                }
                            }
                        }
                        if (UserHasMaxHpAndLife)
                        {
                            switch (args.Parameters[0])
                            {
                                case "1":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class1.ClassName + ".json"))
                                    {
                                        args.Player.SaveServerCharacter();
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class1 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class1.ClassName);
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class1.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class1.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class1.Debuffs.ToString());
                                        args.Player.Kick("You Change Hero Class Too = " + Class1.ClassName.ToString() + " " + "REJOIN!", true, true, null, false);
                                        TShock.DB.Query("UPDATE tsCharacter SET Health=@0, MaxHealth=@1, Mana=@2, MaxMana=@3 WHERE Account=@4;", DefaultHP, DefaultMAXHP, DefaultMP, DefaultMAXMP, args.Player.Account.ID);
                                    }
                                    break;
                                case "3":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class3.ClassName + ".json"))
                                    {
                                        args.Player.SaveServerCharacter();
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class3 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class3.ClassName);
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class3.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class3.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class3.Debuffs.ToString());
                                        args.Player.Kick("You Change Hero Class Too = " + Class3.ClassName.ToString() + " " + "REJOIN!", true, true, null, false);
                                        TShock.DB.Query("UPDATE tsCharacter SET Health=@0, MaxHealth=@1, Mana=@2, MaxMana=@3 WHERE Account=@4;", DefaultHP, DefaultMAXHP, DefaultMP, DefaultMAXMP, args.Player.Account.ID);
                                    }
                                    break;
                                case "4":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class4.ClassName + ".json"))
                                    {
                                        args.Player.SaveServerCharacter();
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class4 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class4.ClassName);
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class4.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class4.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class4.Debuffs.ToString());
                                        args.Player.Kick("You Change Hero Class Too = " + Class4.ClassName.ToString() + " " + "REJOIN!", true, true, null, false);
                                        TShock.DB.Query("UPDATE tsCharacter SET Health=@0, MaxHealth=@1, Mana=@2, MaxMana=@3 WHERE Account=@4;", DefaultHP, DefaultMAXHP, DefaultMP, DefaultMAXMP, args.Player.Account.ID);
                                    }
                                    break;
                            }
                        }
                        return;
                    }
                    if (HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class3.ClassName && args.Parameters[0].ToString() != "3")
                    {
                        using (var reader_HP_MP = TShock.DB.QueryReader("SELECT * FROM tsCharacter WHERE Account=@0", args.Player.Account.ID))
                        {
                            if (reader_HP_MP.Read())
                            {
                                UserHP = reader_HP_MP.Get<Int32>("MaxHealth");
                                UserMP = reader_HP_MP.Get<Int32>("MaxMana");
                                if (UserHP >= 400 && UserMP >= 200)
                                {
                                    UserHasMaxHpAndLife = true;
                                }
                                else
                                {
                                    UserHasMaxHpAndLife = false;
                                    args.Player.SendErrorMessage("Changing Class requires - 400+ HP and 200+ MP");
                                    args.Player.SendErrorMessage("You have -" + " " + UserHP.ToString() + " " + "HP" + " " + "and" + " " + UserMP.ToString() + " " + "MP" + " ");
                                    return;
                                }
                            }
                        }
                        if (UserHasMaxHpAndLife)
                        {
                            switch (args.Parameters[0])
                            {
                                case "1":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class1.ClassName + ".json"))
                                    {
                                        args.Player.SaveServerCharacter();
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class1 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class1.ClassName);
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class1.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class1.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class1.Debuffs.ToString());
                                        args.Player.Kick("You Change Hero Class Too = " + Class1.ClassName.ToString() + " " + "REJOIN!", true, true, null, false);
                                        TShock.DB.Query("UPDATE tsCharacter SET Health=@0, MaxHealth=@1, Mana=@2, MaxMana=@3 WHERE Account=@4;", DefaultHP, DefaultMAXHP, DefaultMP, DefaultMAXMP, args.Player.Account.ID);
                                    }
                                    break;
                                case "2":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class3.ClassName + ".json"))
                                    {
                                        args.Player.SaveServerCharacter();
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class2 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class2.ClassName);
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class2.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class2.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class2.Debuffs.ToString());
                                        args.Player.Kick("You Change Hero Class Too = " + Class2.ClassName.ToString() + " " + "REJOIN!", true, true, null, false);
                                        TShock.DB.Query("UPDATE tsCharacter SET Health=@0, MaxHealth=@1, Mana=@2, MaxMana=@3 WHERE Account=@4;", DefaultHP, DefaultMAXHP, DefaultMP, DefaultMAXMP, args.Player.Account.ID);
                                    }
                                    break;
                                case "4":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class4.ClassName + ".json"))
                                    {
                                        args.Player.SaveServerCharacter();
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class4 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class4.ClassName);
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class4.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class4.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class4.Debuffs.ToString());
                                        args.Player.Kick("You Change Hero Class Too = " + Class4.ClassName.ToString() + " " + "REJOIN!", true, true, null, false);
                                        TShock.DB.Query("UPDATE tsCharacter SET Health=@0, MaxHealth=@1, Mana=@2, MaxMana=@3 WHERE Account=@4;", DefaultHP, DefaultMAXHP, DefaultMP, DefaultMAXMP, args.Player.Account.ID);
                                    }
                                    break;
                            }
                        }
                        return;
                    }
                    if (HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class4.ClassName && args.Parameters[0].ToString() != "4")
                    {
                        using (var reader_HP_MP = TShock.DB.QueryReader("SELECT * FROM tsCharacter WHERE Account=@0", args.Player.Account.ID))
                        {
                            if (reader_HP_MP.Read())
                            {
                                UserHP = reader_HP_MP.Get<Int32>("MaxHealth");
                                UserMP = reader_HP_MP.Get<Int32>("MaxMana");
                                if (UserHP >= 400 && UserMP >= 200)
                                {
                                    UserHasMaxHpAndLife = true;
                                }
                                else
                                {
                                    UserHasMaxHpAndLife = false;
                                    args.Player.SendErrorMessage("Changing Class requires - 400+ HP and 200+ MP");
                                    args.Player.SendErrorMessage("You have -" + " " + UserHP.ToString() + " " + "HP" + " " + "and" + " " + UserMP.ToString() + " " + "MP" + " ");
                                    return;
                                }
                            }
                        }
                        if (UserHasMaxHpAndLife)
                        {
                            switch (args.Parameters[0])
                            {
                                case "1":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class1.ClassName + ".json"))
                                    {
                                        args.Player.SaveServerCharacter();
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class1 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class1.ClassName);
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class1.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class1.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class1.Debuffs.ToString());
                                        args.Player.Kick("You Change Hero Class Too = " + Class1.ClassName.ToString() + " " + "REJOIN!", true, true, null, false);
                                        TShock.DB.Query("UPDATE tsCharacter SET Health=@0, MaxHealth=@1, Mana=@2, MaxMana=@3 WHERE Account=@4;", DefaultHP, DefaultMAXHP, DefaultMP, DefaultMAXMP, args.Player.Account.ID);
                                    }
                                    break;
                                case "2":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class3.ClassName + ".json"))
                                    {
                                        args.Player.SaveServerCharacter();
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class2 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class2.ClassName);
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class2.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class2.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class2.Debuffs.ToString());
                                        args.Player.Kick("You Change Hero Class Too = " + Class2.ClassName.ToString() + " " + "REJOIN!", true, true, null, false);
                                        TShock.DB.Query("UPDATE tsCharacter SET Health=@0, MaxHealth=@1, Mana=@2, MaxMana=@3 WHERE Account=@4;", DefaultHP, DefaultMAXHP, DefaultMP, DefaultMAXMP, args.Player.Account.ID);
                                    }
                                    break;
                                case "3":
                                    using (StreamReader file = File.OpenText(HerosOfTerraria.HerosFilesPath + "/" + Heros.Class3.ClassName + ".json"))
                                    {args.Player.SaveServerCharacter();
                                        JsonSerializer serializer = new JsonSerializer();
                                        Heros Class3 = (Heros)serializer.Deserialize(file, typeof(Heros));
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass", Class3.ClassName);
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackSkill", Class3.AttackSkill.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroAttackBuff", Class3.AttackBuff.ToString());
                                        HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDebuffs", Class3.Debuffs.ToString());
                                        args.Player.Kick("You Change Hero Class Too = " + Class3.ClassName.ToString() + " " + "REJOIN!", true, true, null, false);
                                        TShock.DB.Query("UPDATE tsCharacter SET Health=@0, MaxHealth=@1, Mana=@2, MaxMana=@3 WHERE Account=@4;", DefaultHP, DefaultMAXHP, DefaultMP, DefaultMAXMP, args.Player.Account.ID);
                                    }
                                    break;
                            }
                        }
                        return;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
            }
        }
        public static void ShowExpAndLevelInfo(CommandArgs args)
        {
            if (!args.Player.IsLoggedIn)
            {
                args.Player.SendErrorMessage("Please Login first!");
                return;
            }
            if (args.Player.Account == null)
            {
                args.Player.SendErrorMessage("You need to register before you can use this command!");
                return;
            }
            try
            {
                if (HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == null)
                {
                    args.Player.SendErrorMessage("You need to pick a class first! say /classinfo");
                    return;
                }
                if (HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class1.ClassName || HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class2.ClassName || HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class3.ClassName || HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class4.ClassName || HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass").ToString() == Heros.Class5.ClassName)
                {
                    args.Player.SendInfoMessage(string.Format("{0} Level {1}" + " " + "{2}", args.Player.Account.Name, HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroLevel"), HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroClass")));
                    args.Player.SendInfoMessage(string.Format("Experience: {0:n0}/{1:n0} ({2:n2}%)", HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroExperience"), HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroNextLevel"), Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroExperience")) * 100 / Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroNextLevel"))));
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
            }
        }
        public static void UseSkillPoints(CommandArgs args)
        {
            if (!args.Player.IsLoggedIn)
            {
                args.Player.SendErrorMessage("Please Login first!");
                return;
            }
            if (args.Player.Account == null)
            {
                args.Player.SendErrorMessage("You need to register before you can use this command!");
                return;
            }
            try
            {
                if (args.Parameters.Count == 0 || args.Parameters.Count > 1)
                {
                    args.Player.SendErrorMessage("Invalid syntax: - say /skillinfo for more infomation.");
                    args.Player.SendErrorMessage("Say /skill def or /skill dmg");
                    return;
                }
                else
                {
                    switch (args.Parameters[0])
                    {
                        case "def":
                            PlayerHero.HeroSkillpoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints"));
                            PlayerHero.HeroDefence = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDefence"));
                            if (PlayerHero.HeroSkillpoints <= 0)
                            {
                                args.Player.SendErrorMessage("Sorry you dont have any skillpoints left! Level up to gain more :)");
                            }
                            else
                            {
                                int NewSkillPoints = (PlayerHero.HeroSkillpoints - 1);
                                int NewDefence = (PlayerHero.HeroDefence + 1);
                                HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints", NewSkillPoints.ToString());
                                HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDefence", NewDefence.ToString());
                                int TotalSkillPoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints"));
                                int TotalDamage = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDamage"));
                                int TotalDefence = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDefence"));
                                args.Player.SendErrorMessage("You have added 1 to Your Defence! You have {0} skillpoints left!", TotalSkillPoints.ToString().Trim());
                                args.Player.SendInfoMessage("Your stats are now Defence ={0} and Damage ={1}", TotalDefence.ToString().Trim(), TotalDamage.ToString().Trim());
                            }
                            break;
                        case "dmg":
                            PlayerHero.HeroSkillpoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints"));
                            PlayerHero.HeroDamage = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDamage"));
                            if (PlayerHero.HeroSkillpoints <= 0)
                            {
                                args.Player.SendErrorMessage("Sorry you dont have any skillpoints left! Level up to gain more :)");
                            }
                            else
                            {
                                int NewSkillPoints = (PlayerHero.HeroSkillpoints - 1);
                                int NewHeroDamage = (PlayerHero.HeroDamage + 1);
                                HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints", NewSkillPoints.ToString());
                                HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDamage", NewHeroDamage.ToString());
                                int TotalSkillPoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints"));
                                int TotalDamage = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDamage"));
                                int TotalDefence = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDefence")) - 1;
                                args.Player.SendErrorMessage("You have added 1 to Your Damage! You have {0} skillpoints left!", TotalSkillPoints.ToString().Trim());
                                args.Player.SendInfoMessage("Your stats are now Defence ={0} and Damage ={1}", TotalDefence.ToString().Trim(), TotalDamage.ToString().Trim());
                            }
                            break;
                        case "exp":
                            PlayerHero.HeroSkillpoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints"));
                            PlayerHero.HeroExpMultiplier = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroExpMultiplier"));
                            if (PlayerHero.HeroSkillpoints <= 0 || PlayerHero.HeroSkillpoints <= 19)
                            {
                                args.Player.SendErrorMessage("Sorry you dont have enough skillpoints left! Level up to gain more :)");
                            }
                            else if (PlayerHero.HeroExpMultiplier == 3)
                            {
                                args.Player.SendErrorMessage("Skill is maxed!");
                            }
                            else
                            {
                                int NewSkillPoints = (PlayerHero.HeroSkillpoints - 20);
                                int HeroExpMultiplier = (PlayerHero.HeroExpMultiplier + 1);
                                HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints", NewSkillPoints.ToString());
                                HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroExpMultiplier", HeroExpMultiplier.ToString());
                                int TotalSkillPoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints"));
                                int TotalDamage = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDamage"));
                                int TotalDefence = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDefence"));
                                int TotalExpMultiplier = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroExpMultiplier")) - 1;
                                args.Player.SendErrorMessage("You have added 1 to Your ExpMultiplier! You have {0} skillpoints left!", TotalSkillPoints.ToString().Trim());
                                args.Player.SendInfoMessage("Your stats are now Defence ={0} Damage ={1} ExpMultiplier ={2}", TotalDefence.ToString().Trim(), TotalDamage.ToString().Trim(), TotalExpMultiplier.ToString().Trim());
                            }
                            break;
                        case "defall":
                            PlayerHero.HeroSkillpoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints"));
                            PlayerHero.HeroDefence = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDefence"));
                            if (PlayerHero.HeroSkillpoints <= 0)
                            {
                                args.Player.SendErrorMessage("Sorry you dont have any skillpoints left! Level up to gain more :)");
                            }
                            else
                            {

                                int NewDefence = (PlayerHero.HeroDefence + PlayerHero.HeroSkillpoints);
                                HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDefence", NewDefence.ToString());
                                int NewSkillPoints = 0;
                                HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints", NewSkillPoints.ToString());
                                int TotalSkillPoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints"));
                                int TotalDamage = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDamage"));
                                int TotalDefence = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDefence"));
                                args.Player.SendErrorMessage("You have added all skillpoints to Your Defence! You have {0} skillpoints left!", TotalSkillPoints.ToString().Trim());
                                args.Player.SendInfoMessage("Your stats are now Defence ={0} and Damage ={1}", TotalDefence.ToString().Trim(), TotalDamage.ToString().Trim());
                            }
                            break;
                        case "dmgall":
                            PlayerHero.HeroSkillpoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints"));
                            PlayerHero.HeroDamage = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDamage"));
                            if (PlayerHero.HeroSkillpoints <= 0)
                            {
                                args.Player.SendErrorMessage("Sorry you dont have any skillpoints left! Level up to gain more :)");
                            }
                            else
                            {
                                int NewHeroDamage = (PlayerHero.HeroDamage + PlayerHero.HeroSkillpoints);
                                HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDamage", NewHeroDamage.ToString());
                                int NewSkillPoints = 0;
                                HerosOfTerraria.WriteFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints", NewSkillPoints.ToString());
                                int TotalSkillPoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints"));
                                int TotalDamage = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDamage"));
                                int TotalDefence = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDefence"));
                                args.Player.SendErrorMessage("You have added all you skillpoints to Your Damage! You have {0} skillpoints left!", TotalSkillPoints.ToString().Trim());
                                args.Player.SendInfoMessage("Your stats are now Defence ={0} and Damage ={1}", TotalDefence.ToString().Trim(), TotalDamage.ToString().Trim());
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
            }
        }
        public static void ShowStats(CommandArgs args)
        {
            if (!args.Player.IsLoggedIn)
            {
                args.Player.SendErrorMessage("Please Login first!");
                return;
            }
            if (args.Player.Account == null)
            {
                args.Player.SendErrorMessage("You need to register before you can use this command!");
                return;
            }
            try
            {
                PlayerHero.HeroSkillpoints = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroSkillpoints"));
                PlayerHero.HeroDefence = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDefence"));
                PlayerHero.HeroDamage = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroDamage"));
                PlayerHero.HeroExpMultiplier = Convert.ToInt32(HerosOfTerraria.ReadFile(HerosOfTerraria.PlayerFilePath + "/" + args.Player.Account.ID, "HeroExpMultiplier")) - 1;
                args.Player.SendInfoMessage("Hero Stats!");
                args.Player.SendErrorMessage("##########");
                args.Player.SendInfoMessage("HeroSkillpoints={0}", PlayerHero.HeroSkillpoints.ToString().Trim());
                args.Player.SendInfoMessage("HeroDefence={0}", PlayerHero.HeroDefence.ToString().Trim());
                args.Player.SendInfoMessage("HeroDamage={0}", PlayerHero.HeroDamage.ToString().Trim());
                args.Player.SendInfoMessage("HeroExpMultiplier={0} (MAX LVL is 2!)", PlayerHero.HeroExpMultiplier.ToString().Trim());
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
            }
        }
        public static void SkillInfo(CommandArgs args)
        {
            if (!args.Player.IsLoggedIn)
            {
                args.Player.SendErrorMessage("Please Login first!");
                return;
            }
            if (args.Player.Account == null)
            {
                args.Player.SendErrorMessage("You need to register before you can use this command!");
                return;
            }
            try
            {
                args.Player.SendInfoMessage("Skill Infomation.");
                args.Player.SendErrorMessage("##########");
                args.Player.SendInfoMessage("HeroDefence will make you stronger against monster/bosses!");
                args.Player.SendInfoMessage("HeroDamage will add extra damage to your attack skill! Each skillpoints in damage will multiply attack skill damage by 3");
                args.Player.SendInfoMessage("HeroExpMultiplier will Multiply gained experience by amount. Costs 20 points per level (MAX LVL is 2!)");
                args.Player.SendInfoMessage("/skill def or /skill defall /skill dmg or /skill dmgall  /skill exp or /skill expall");
            }
            catch (Exception ex)
            {
                TShock.Log.Error(ex.ToString());
                Console.Write(ex.ToString());
            }
        }
    }
}