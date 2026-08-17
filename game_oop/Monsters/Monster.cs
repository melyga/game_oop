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

        public int Armor => _armor;
        private int _armor;

        public int Power => _power;
        private int _power;

        public int HP => _hp;
        private int _hp;
        public bool IsAlive => _hp > 0;
        public Guid Id { get; protected set; } = Guid.NewGuid();

        public Monster(string name, int level, int baseHP, int basePower, int baseArmor,
                       MonsterRarity rarity = MonsterRarity.Normal,
                       float hpMultiplier = 1.0f, float dmgMultiplier = 1.0f)
        {
            Name = name;
            Level = Math.Max(1, level);
            Rarity = rarity;

            CalculateStats(hpMultiplier, dmgMultiplier, baseHP, basePower, baseArmor);
        }

        private void CalculateStats(float hpMult, float dmgMult, int hp, int power, int armor)
        {
            // Коэффициенты здоровья в зависимости от редкости
            float rarityHpBonus = Rarity switch
            {
                MonsterRarity.Normal => 1.0f,
                MonsterRarity.Elite => 1.8f,
                MonsterRarity.Boss => 3.5f,
                _ => 1.0f
            };

            // Мягкий прирост HP: +15% от базового HP и +10 ед. за каждый уровень
            double baseHp = hp * (1.0 + 0.15 * (Level - 1)) + (10 * (Level - 1));
            _hp = (int)(baseHp * hpMult * rarityHpBonus);

            // Множитель урона за редкость
            float rarityDmgBonus = Rarity switch
            {
                MonsterRarity.Normal => 1.0f,
                MonsterRarity.Elite => 1.3f,
                MonsterRarity.Boss => 2.0f,
                _ => 1.0f
            };

            // Линейный прирост урона
            double baseDmg = power + ((Level - 1) * 2.5);
            _power = (int)(baseDmg * dmgMult * rarityDmgBonus);

            // Броня растет умеренно, без прямого умножения на Rarity
            _armor = armor + (int)((Level - 1) * 1.5f) + (int)Rarity;
        }

        /// <summary>
        /// Расчет опыта за убийство монстра
        /// </summary>
        public int CalculateExpReward(int playerLevel)
        {
            int monsterLevel = this.Level; // Фикс: используем уровень текущего монстра
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

            // Опыт за редкость
            double rarityXpMult = Rarity switch
            {
                MonsterRarity.Normal => 1.0,
                MonsterRarity.Elite => 1.5,
                MonsterRarity.Boss => 3.0,
                _ => 1.0
            };

            expReward *= rarityXpMult;

            return Math.Max(1, (int)Math.Round(expReward));
        }

        private int GetLevelDifference(int playerLevel)
        {
            if (playerLevel <= 5) return 5;
            if (playerLevel <= 9) return 6;
            if (playerLevel <= 11) return 7;
            if (playerLevel <= 15) return 8;
            if (playerLevel <= 19) return 9;
            if (playerLevel <= 39) return 9 + (playerLevel - 20) / 10;
            return 12;
        }

        public abstract int Attack(IEnemy enemy);

        public virtual int TakeDamage(int damage)
        {
            if (damage <= 0 || _hp <= 0)
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