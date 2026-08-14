namespace Game
{
    /// <summary>
    /// Абстрактный базовый класс для всех героев.
    /// Содержит общие характеристики и методы.
    /// </summary>
    public abstract class Hero
    {
        /// <summary>Имя героя (неизменяемо).</summary>
        public string Name { get; private set; }

        /// <summary>Текущее здоровье.</summary>
        public int Hp { get; protected set; }

        /// <summary>Максимальное здоровье (увеличивается с уровнем).</summary>
        public int MaxHp { get; protected set; }

        /// <summary>Броня, уменьшающая входящий физический урон.</summary>
        public int Armor { get; protected set; }

        /// <summary>Сила – базовый показатель урона.</summary>
        public int Strength { get; protected set; }

        /// <summary>Ловкость – влияет на шанс побега и другие механики.</summary>
        public int Agility { get; protected set; }

        /// <summary>Накопленный опыт.</summary>
        public int Score { get; private set; }

        /// <summary>Текущий уровень (начинается с 1).</summary>
        public int Level { get; private set; } = 1;

        /// <summary>Опыт, необходимый для следующего уровня.</summary>
        public int ExpToNextLevel => Level * 110;

        /// <summary>Критический урон в процентах.</summary>
        public float CritDamage { get; protected set; }

        /// <summary>Критический шанс в процентах.</summary>
        public float CritRate { get; protected set; }

        /// <summary>Шанс побега (зависит от ловкости, максимум 75%).</summary>
        public double EscapeChance
        {
            get
            {
                double baseChance = 25.0;
                int effectiveAgility = Math.Min(Agility, 30);
                double agilityBonus = effectiveAgility * (3.0 - (effectiveAgility - 1) * 0.1 / 2.0);
                double finalChance = baseChance + agilityBonus;
                return Math.Min(finalChance, 75.0);
            }
        }

        /// <summary>Жив ли герой.</summary>
        public bool IsAlive => Hp > 0;

        protected Random Rand = new Random();

        /// <summary>
        /// Конструктор базового героя.
        /// </summary>
        protected Hero(string name, int hp, int strength, int agility, int score, int maxHP,
                       float critDamage = 30, float critRate = 10, int armor = 0)
        {
            Name = name;
            Hp = hp;
            Strength = strength;
            Agility = agility;
            Score = score;
            MaxHp = maxHP;
            CritDamage = critDamage;
            CritRate = critRate;
            Armor = armor;
        }

        /// <summary>Получение урона от врага.</summary>
        public virtual void TakeDamage(IEnemy enemy)
        {
            if (enemy.Strength < 0)
                throw new ArgumentException("Урон не может быть отрицательным");

            Console.WriteLine($"{Name} получает {enemy.Strength} урона!");
            int damage = enemy.Strength - Armor;
            if (damage <= 0) damage = 1; // минимальный урон 1
            Hp -= damage;
            if (Hp < 0) Hp = 0;
        }

        /// <summary>Лечение героя.</summary>
        public void Heal(int amount)
        {
            if (amount < 0) throw new ArgumentException("Лечение не может быть отрицательным");
            Hp += amount;
            if (Hp > MaxHp) Hp = MaxHp;
            Console.WriteLine($"{Name} восстановил {amount} HP (теперь {Hp}/{MaxHp})");
        }

        /// <summary>Начисление опыта за убийство врага.</summary>
        protected void AwardExperience(IEnemy enemy)
        {
            int baseExp = enemy.ExpReward;
            int finalExp = baseExp;

            // Корректировка опыта в зависимости от разницы уровней
            int levelDiff = Level - enemy.Level;
            if (levelDiff > 5)
            {
                double penalty = 1.0 - (levelDiff - 5) * 0.25;
                if (penalty < 0) penalty = 0;
                finalExp = (int)(baseExp * penalty);
            }
            else if (levelDiff < -5)
            {
                double bonus = 1.0 + (Math.Abs(levelDiff) - 5) * 0.10;
                finalExp = (int)(baseExp * bonus);
            }

            AddExperience(finalExp);
        }

        private void AddExperience(int amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine($"{Name} не получил опыта (враг слишком слаб).");
                return;
            }

            Score += amount;
            Console.WriteLine($"{Name} получил {amount} опыта (всего {Score}/{ExpToNextLevel})");

            while (Score >= ExpToNextLevel)
            {
                Score -= ExpToNextLevel;
                Level++;
                LevelUp();
            }
        }

        /// <summary>
        /// Управляет достижениями героя
        /// </summary>
        public void CheckAchievements(AchievementManager manager)
        {
            int goblinKills = manager.GetKillCount(typeof(Goblin));
            if (goblinKills > 0 && goblinKills % 20 == 0)
            {
                Battle.AddLog("Истребитель гоблинов – Сила +2!");
                Strength += 2;
            }

            int mechKills = manager.GetKillCount(typeof(Mech_Golem));
            if (mechKills > 0 && mechKills % 10 == 0)
            {
                Battle.AddLog("Покоритель механизмов – Ловкость +5 и Макс. здоровье +10!");
                Agility += 5;
                MaxHp += 10;
            }

            int orcKills = manager.GetKillCount(typeof(Orc));
            if (orcKills > 0 && orcKills % 8 == 0)
            {
                Battle.AddLog("Орк победитель – Сила +3 и Броня +5!");
                Strength += 3;
                Armor += 5;
            }

            int trollKills = manager.GetKillCount(typeof(Troll));
            if (trollKills > 0 && trollKills % 5 == 0)
            {
                Battle.AddLog("Тролль охотник – Крит. шанс +2 и Крит. урон +10!");
                CritDamage += 10;
                CritRate += 2;
            }

            int dragonKills = manager.GetKillCount(typeof(Dragon));
            if (dragonKills > 0 && dragonKills % 2 == 0)
            {
                Battle.AddLog("Драконоборец – Макс. здоровье +20 и Сила +5!");
                Armor += 5;
                MaxHp += 20;
            }

            int totalKills = manager.GetTotalKills();
            if (totalKills > 0 && totalKills % 20 == 0)
            {
                Battle.AddLog("Победитель – Броня +1 и Сила +1!");
                Strength += 1;
                Armor += 1;
            }
        }

        /// <summary>Расчёт базового урона с учётом критического попадания.</summary>
        protected virtual int CalculateDamage()
        {
            if (Rand.NextDouble() * 100 < CritRate)
                return (int)(Strength * (CritDamage / 100f));
            return Strength;
        }

        /// <summary>Расчёт итогового урона с учётом брони врага (по умолчанию вычитается полностью).</summary>
        protected virtual int CalculateFinalDamage(int rawDamage, IEnemy enemy)
        {
            int final = rawDamage - enemy.Armor;
            return final < 0 ? 0 : final;
        }

        /// <summary>Повышение уровня – увеличивает характеристики.</summary>
        protected virtual void LevelUp()
        {
            Console.WriteLine($"Поздравляем! {Name} достиг {Level} уровня!");
            Hp = MaxHp;
        }

        /// <summary>Атака врага (реализуется в наследниках).</summary>
        public abstract int Attack(IEnemy enemy);

        /// <summary>Название класса (для отображения).</summary>
        public abstract string ClassName { get; }
    }
}