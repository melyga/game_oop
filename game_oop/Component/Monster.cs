namespace Game
{
    /// <summary>
    /// Базовый класс для всех монстров, реализующий интерфейс IEnemy.
    /// </summary>
    public abstract class Monster : IEnemy
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Armor { get; private set; }
        public int Strength { get; private set; }
        public int Level { get; private set; }
        public bool IsAlive => Health > 0;

        /// <summary>
        /// Опыт за убийство рассчитывается на основе характеристик монстра.
        /// </summary>
        public int ExpReward
        {
            get
            {
                if (IsAlive) return 0;
                double survivability = Health + (Armor * 5);
                double damageMultiplier = 1 + (Strength * 0.02);
                return (int)(survivability * damageMultiplier);
            }
        }

        /// <summary>
        /// Конструктор монстра с масштабированием характеристик по уровню.
        /// </summary>
        protected Monster(string name, int baseHp, int baseArmor, int baseStrength, int level)
        {
            Name = name;
            Level = level;

            double multiplier = 1 + (level - 1) * 0.20;
            Health = (int)(baseHp * multiplier);
            Armor = (int)(baseArmor * multiplier);
            Strength = (int)(baseStrength * multiplier);
        }

        public bool TakeDamage(int damage)
        {
            if (damage < 0) throw new ArgumentException("Урон не может быть отрицательным");
            Health -= damage;
            if (Health < 0) Health = 0;
            return !IsAlive;
        }
    }
}