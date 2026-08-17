using Game.Heros;

namespace Game.Monsters
{
    public enum MonsterRarity
    {
        Normal = 1,
        Elite = 2,
        Boss = 5
    }

    public abstract class Monster : IEnemy
    {
        public string Name { get; protected set; }
        public int Level { get; protected set; }
        public MonsterRarity Rarity { get; protected set; }

        // Характеристики
        public int Armor => _armor;
        private int _armor = 0;

        public int Power => _power;
        private int _power;

        public int HP => _hp;
        private int _hp;
        public bool IsAlive => _hp > 0;
        public Guid Id { get; protected set; } = Guid.NewGuid();

        public Monster(string name, int level, int baseHP, int basePower, int baseArmor, MonsterRarity rarity = MonsterRarity.Normal,
                       float hpMultiplier = 1.0f, float dmgMultiplier = 1.0f)
        {
            Name = name;
            Level = Math.Max(1, level);
            Rarity = rarity;

            // Расчет характеристик при спавне
            CalculateStats(hpMultiplier, dmgMultiplier, baseHP, basePower, baseArmor);
        }

        /// <summary>
        /// Формула усиления монстра с ростом уровня
        /// </summary>
        private void CalculateStats(float hpMult, float dmgMult, int hp, int power, int armor)
        {
            double baseHp = hp * Math.Pow(Level, 1.25) + (15 * Level);
            _hp = (int)(baseHp * hpMult * (int)Rarity);

            double baseDmg = power + (Level * 3.5) + Math.Pow(Level, 1.05);
            double rarityDmgBonus = Rarity == MonsterRarity.Boss ? 2.5 : (int)Rarity;
            _power = (int)(baseDmg * dmgMult * rarityDmgBonus);

            _armor = (int)(Level * armor * (int)Rarity);
        }

        /// <summary>
        /// Расчет опыта за убийство монстра
        /// </summary>
        public int CalculateExpReward(Hero hero)
        {
            int playerLevel = hero.Progress.Level;
            int monsterLevel = Level;

            int grayDifference = GetLevelDifference(playerLevel);

            if (monsterLevel <= playerLevel - grayDifference)
            {
                return 0;
            }

            double baseXP = (playerLevel * 5) + 35;

            double expReward;

            if (monsterLevel > playerLevel)
            {
                int levelDiff = Math.Min(monsterLevel - playerLevel, 4);
                expReward = baseXP * (1.0 + (levelDiff * 0.05));
            }
            else if (monsterLevel == playerLevel)
            {
                expReward = baseXP;
            }
            else
            {
                int levelDiff = playerLevel - monsterLevel;
                expReward = baseXP * (1.0 - ((double)levelDiff / grayDifference));
            }

            expReward *= (int)Rarity;

            return Math.Max(1, (int)Math.Round(expReward));
        }

        /// <summary>
        /// Определяет порог уровня в зависимости от уровня игрока
        /// </summary>
        private int GetLevelDifference(int playerLevel)
        {
            if (playerLevel <= 5) return 5;
            if (playerLevel <= 9) return 6;
            if (playerLevel <= 11) return 7;
            if (playerLevel <= 15) return 8;
            if (playerLevel <= 19) return 9;
            if (playerLevel <= 39) return 9 + (playerLevel - 20) / 10;
            return 12; // Для 40+ уровней
        }

        public abstract int Attack(IEnemy enemy);

        public virtual int TakeDamage(int damage)
        {
            if (damage <= 0 && _hp == 0)
                return 0;

            int realDamage = damage - Armor;
            if (realDamage <= 0)
                realDamage = 1;

            _hp -= realDamage;
            if (_hp < 0)
                _hp = 0;
            return realDamage;
        }
    }
}