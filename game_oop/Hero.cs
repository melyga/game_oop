namespace Game
{
    public abstract class Hero
    {
        /// <summary>
        /// Имя героя, которое задается при создании экземпляра класса и не может быть изменено после этого
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Текущее здоровье героя, которое может изменяться в процессе игры (например, при получении урона или лечении)
        /// </summary>
        public int HP { get; private set; }

        /// <summary>
        /// Максимальное здоровье героя, которое может изменяться в процессе игры
        /// </summary>
        public int MaxHP { get; private set; }

        /// <summary>
        /// Броня героя, которая уменьшает получаемый урон.
        /// </summary>
        public int Armor { get; private set; }

        /// <summary>
        /// Сила героя, которая определяет базовый урон, наносимый героем.
        /// </summary>
        public int Strength { get; private set; }

        /// <summary>
        /// Ловкость героя, которая может влиять на скорость атаки, шанс уклонения .
        /// </summary>
        public int Agility { get; private set; }

        /// <summary>
        /// Опыт героя, который может увеличиваться в процессе игры и использоваться для повышения уровня героя.
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Сколько опыта нужно на повышение уровня героя.
        /// </summary>
        public int ExpToNextLevel => Level * 110;

        /// <summary>
        /// Уровень героя, который может увеличиваться при накоплении определенного количества опыта. Уровень может влиять на характеристики героя.
        /// </summary>
        public int Level { get; protected set; } = 1;

        /// <summary>
        /// Критический урон героя, который определяет дополнительный урон, наносимый при критическом попадании. Значение задается в процентах.
        /// </summary>
        public float CritDamage { get; private set; }

        /// <summary>
        /// Критический шанс героя, который определяет вероятность критического попадания. Значение задается в процентах.
        /// </summary>
        public float CritRate { get; private set; }

        public bool IsAlive => HP > 0;

        protected Random Rand = new Random();

        public Hero(string name, int hp, int strength, int agility, int score, int maxHP, float critDamage = 30, float critRate = 10, int armor = 0)
        {
            Name = name;
            HP = hp;
            Strength = strength;
            Agility = agility;
            Score = score;
            MaxHP = maxHP;
            CritDamage = critDamage;
            CritRate = critRate;
            Armor = armor;
        }

        /// <summary>
        /// Наносит урон герою
        /// </summary>
        public void TakeDamage(Monster monster)
        {
            if (monster.Strength < 0)
                throw new ArgumentException("Урон не может быть отрицательным");

            Console.WriteLine($"Герой {Name} получает урон в {monster.Strength} единиц здоровья!");
            Console.WriteLine();
            if (monster.Strength - Armor <= 0)
                HP -= 1; // Если броня полностью поглощает урон, наносим минимальный урон в 1 единицу

            HP -= monster.Strength - Armor;
            if (HP < 0)
                HP = 0;
        }

        /// <summary>
        /// Лечит героя
        /// </summary>
        public void Heal(int heal)
        {
            if (heal < 0)
                throw new ArgumentException("Лечение не может быть отрицательным");

            HP += heal;

            if (HP > MaxHP)
            {
                Console.WriteLine($"Лечение в {heal} единиц превысило максимальное значение здоровья. HP: {HP} => HP: {MaxHP}");
                Console.WriteLine();
                HP = MaxHP;
            }
            else
            { 
                Console.WriteLine($"Герой {Name} вылечился на {heal} единиц здоровья!");
                Console.WriteLine();
            }                
        }

        /// <summary>
        /// Добавляет опыт герою в зависимости в разнице уровней. Если опыт превышает порог, уровень повышается.
        /// </summary>
        protected void AwardExperience(Monster monster)
        {
            int baseExp = monster.ExpReward;
            int finalExp = baseExp;

            // Вычисляем чистую разницу уровней
            int levelDifference = Level - monster.Level;

            if (levelDifference > 5)
            {
                int effectiveDifference = levelDifference - 5;

                // Уменьшаем опыт на 25% за каждый уровень сверх порога
                double penaltyMultiplier = 1.0 - (effectiveDifference * 0.25);

                // Если разница 9 уровней и более — опыт обнуляется
                if (penaltyMultiplier < 0) penaltyMultiplier = 0;

                finalExp = (int)(baseExp * penaltyMultiplier);
            } 
            else if (levelDifference < -5)
            {
                int effectiveDifference = Math.Abs(levelDifference) - 5;

                // Увеличиваем опыт на 10% за каждый уровень сверх порога (опасная охота!)
                double bonusMultiplier = 1.0 + (effectiveDifference * 0.10);

                finalExp = (int)(baseExp * bonusMultiplier);
            }
            else
            {
                // Базовый опыт монстра
                finalExp = baseExp;
            }

            AddExperience(finalExp);
        }

        private void AddExperience(int amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine($"{Name} уничтожил слишком слабого врага и не получил опыта.");
                return;
            }

            Score += amount;
            Console.WriteLine($"{Name} получил {amount} опыта (Текущий: {Score}/{ExpToNextLevel})");

            while (Score >= ExpToNextLevel)
            {
                Score -= ExpToNextLevel;
                Level++;
                LevelUp();
            }
        }


        /// <summary>
        /// Считает урон c критическим попаданием
        /// </summary>
        protected virtual int CalculateDamage()
        {
            double roll = Rand.NextDouble() * 100;

            if (roll < CritRate)
            {
                return (int)(Strength * (CritDamage / 100f));
            }

            return Strength;
        }

        /// <summary>
        /// Рассчитывает урон с учетом брони монстра. По умолчанию броня учитывается полностью.
        /// </summary>
        protected virtual int CalculateFinalDamage(int rawDamage, Monster monster)
        {
            int finalDamage = rawDamage - monster.Armor;
            return finalDamage < 0 ? 0 : finalDamage;
        }

        /// <summary>
        /// Базовая логика повышения уровня. Увеличивает общие для всех параметры.
        /// </summary>
        protected virtual void LevelUp()
        {
            Console.WriteLine($"ПОЗДРАВЛЯЕМ! {Name} поднял уровень до {Level}!");

            MaxHP += 15;
            HP = MaxHP;

            Strength += 2;
            Armor += 1;
        }

        /// <summary>
        /// Выводит информацию о герое
        /// </summary>
        public void DisplayHeroStats()
        {
            string alive = IsAlive ? "жив" : "мертв";
            Console.WriteLine("*------------------------------------*");
            Console.WriteLine($"Имя героя:              {Name}");
            Console.WriteLine($"Здоровье:               {HP}");
            Console.WriteLine($"Сила:                   {Strength}");
            Console.WriteLine($"Ловкость:               {Agility}");
            Console.WriteLine($"Опыт:                   {Score}");
            Console.WriteLine($"Опыта до нового уровня: {ExpToNextLevel}");
            Console.WriteLine($"Уровень:                {Level}");
            Console.WriteLine($"Герой: {alive}");
            Console.WriteLine("*-----------------------------------*");
        }

        public abstract int Attack(Monster monster);

        public abstract string ClassName { get; }
    }
}
