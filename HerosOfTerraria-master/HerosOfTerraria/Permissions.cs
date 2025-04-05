using System.ComponentModel;

namespace HerosOfTerraria
{
    public static class Permissions
    {
        [Description("Gives infomation about Heros and its abilities.")]
        public static readonly string classinfo = "HerosOfTerraria.classinfo";
        [Description("Allows the players to pick a Class.")]
        public static readonly string pickclass = "HerosOfTerraria.pickclass";
        [Description("Allows players to change there Class.")]
        public static readonly string changeclass = "HerosOfTerraria.changeclass";
        [Description("Allows the players to gain exp and check level and exp status")]
        public static readonly string showexpandlevelinfo = "HerosOfTerraria.exp";
        [Description("Allows the players to gain exp and check level and exp status")]
        public static readonly string useskillpoints = "HerosOfTerraria.useskillpoints";
        [Description("Allows the players to gain exp and check level and exp status")]
        public static readonly string showstats = "HerosOfTerraria.showstats";
        [Description("Allows the players to gain exp and check level and exp status")]
        public static readonly string skillinfo = "HerosOfTerraria.skillinfo";
    }
}
