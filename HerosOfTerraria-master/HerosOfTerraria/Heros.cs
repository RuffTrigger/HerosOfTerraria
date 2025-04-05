namespace HerosOfTerraria
{

    public class Heros
    {
        public string ClassName { get; set; }
        public int Level { get; set; }
        public int MaxLevel { get; set; }
        public int Experience { get; set; }
        public int ExpMultiplier { get; set; }
        public int NextLevel { get; set; }
        public int Skillpoints { get; set; }
        public int Defence { get; set; }
        public int Damage { get; set; }
        public int Mana { get; set; }
        public int Heath { get; set; }
        public string Description { get; set; }
        public string AttackSkill { get; set; }
        public string AttackBuff { get; set; }
        public string Debuffs { get; set; }
        public string ClassType { get; set; }
        public static Heros Class1 = new Heros
        {
            ClassName = "Diabolist",
            Level = 1,
            MaxLevel = 99,
            Experience = 0,
            ExpMultiplier = 1,
            NextLevel = 1000,
            Skillpoints = 0,
            Defence = 0,
            Damage = 0,
            Description = @"The personification and archetype of evil. Attacks inflicts Burning debuff and exploding on attack.",
            AttackSkill = "711",
            AttackBuff = "179",
            Debuffs = "67",
            ClassType = "Undead"
        };
        public static Heros Class2 = new Heros
        {
            ClassName = "Gigazapper",
            Level = 1,
            MaxLevel = 99,
            Experience = 0,
            ExpMultiplier = 1,
            NextLevel = 1000,
            Skillpoints = 0,
            Defence = 0,
            Damage = 0,
            Description = @"Likes big guns! attacks inflicts the Confused debuff.",
            AttackSkill = "440",
            AttackBuff = "112",
            Debuffs = "77",
            ClassType = "Martian"
        };
        public static Heros Class3 = new Heros
        {
            ClassName = "Reaper",
            Level = 1,
            MaxLevel = 99,
            Experience = 0,
            ExpMultiplier = 1,
            NextLevel = 1000,
            Skillpoints = 0,
            Defence = 0,
            Damage = 0,
            Description = @"From the shadows. Fires Shadowflame Skull and inflicts the Shadowflame and Slow debuff on attack, Mana regen buff.",
            AttackSkill = "585",
            AttackBuff = "178",
            Debuffs = "44",
            ClassType = "Undead"
        };
        public static Heros Class4 = new Heros
        {
            ClassName = "Selenian",
            Level = 1,
            MaxLevel = 99,
            Experience = 0,
            ExpMultiplier = 1,
            NextLevel = 1000,
            Skillpoints = 0,
            Defence = 0,
            Damage = 0,
            Description = @"Spawned by the Solar Pillar. Certain damage will be reflected. Attacks inflicts Phantom Phoenix and the Ichor debuff.",
            AttackSkill = "706",
            AttackBuff = "14",
            Debuffs = "69",
            ClassType = "Solar"
        };
        public static Heros Class5 = new Heros
        {
            ClassName = "Beekeeper",
            Level = 1,
            MaxLevel = 99,
            Experience = 0,
            ExpMultiplier = 1,
            NextLevel = 1000,
            Skillpoints = 0,
            Defence = 0,
            Damage = 0,
            Description = @"The Hive needs it's protector. Sugar Rush for the nerves, Spawns bees, attacks inflicts Weak on foes",
            AttackSkill = "181",
            AttackBuff = "192",
            Debuffs = "33",
            ClassType = "Bee"
        };
    }
    public class PlayerHero
    {
        public static string HeroClass { get; set; }
        public static int HeroLevel { get; set; }
        public static int HeroMaxLevel { get; set; }
        public static int HeroExperience { get; set; }
        public static int HeroExpMultiplier { get; set; }
        public static int HeroNextLevel { get; set; }
        public static int HeroSkillpoints { get; set; }
        public static int HeroDefence { get; set; }
        public static int HeroDamage { get; set; }
        public static int HeroMana { get; set; }
        public static int HeroHeath { get; set; }
        public static string HeroAttackSkill { get; set; }
        public static string HeroAttackBuff { get; set; }
        public static string HeroDebuffs { get; set; }
        public PlayerHero()
        {
            HeroClass = PlayerHero.HeroClass;
            HeroLevel = PlayerHero.HeroLevel;
            HeroMaxLevel = PlayerHero.HeroMaxLevel;
            HeroExperience = PlayerHero.HeroExperience;
            HeroExpMultiplier = PlayerHero.HeroExpMultiplier;
            HeroNextLevel = PlayerHero.HeroNextLevel;
            HeroSkillpoints = PlayerHero.HeroSkillpoints;
            HeroDefence = PlayerHero.HeroDefence;
            HeroDamage = PlayerHero.HeroDamage;
            HeroMana = PlayerHero.HeroMana;
            HeroHeath = PlayerHero.HeroHeath;
            HeroAttackSkill = PlayerHero.HeroAttackSkill;
            HeroAttackBuff = PlayerHero.HeroAttackBuff;
            HeroDebuffs = PlayerHero.HeroDebuffs;
        }
    }
}
