namespace Game
{
    public class Monster
    {
        public string Name { get; private set; } // Имя монстра
        public int HP { get; private set; } // Текущее здоровье монстра
        public int Armor { get; private set; } // Броня монстра
        public int Strength { get; private set; } // Сила монстрта
        public int Level { get; private set; } // Уровень монстра

        public Guid Id { get; private init; } // Уникальный идентификатор монстра (используется на данный момент для механики разбойника)

        public bool IsAlive => HP > 0; // Проверка, жив ли монстр (если здоровье больше 0)

        /// <summary>
        /// Подсчет опыта за убийство монстра
        /// </summary>
        public int ExpReward
        {
            get
            {
                if (IsAlive)
                    return 0;

                double survivability = HP + (Armor * 5);

                double damageMultiplier = 1 + (Strength * 0.02);

                return (int)(survivability * damageMultiplier);
            }
        }

        public Monster(string name, int hp, int armor, int strenght, int level = 1)
        {
            Name = name;
            Level = level;
            Id = Guid.NewGuid();

            double levelMultiplier = 1 + (level - 1) * 0.20;

            HP = (int)(hp * levelMultiplier);
            Armor = (int)(armor * levelMultiplier);
            Strength = (int)(strenght * levelMultiplier);
        }

        /// <summary>
        /// Наносит монстру чистый урон (после вычета всех модификаторов брони) 
        /// и возвращает true, если монстр умер.
        /// </summary>
        public bool TakeDamage(int finalDamage)
        {
            if (finalDamage < 0)
                throw new ArgumentException("Урон не может быть отрицательным");

            HP -= finalDamage;

            if (HP <= 0)
            {
                HP = 0;
                return true; // Монстр погиб
            }
            return false; // Монстр жив
        }

        public void Heal(int heal)
        {
            if (heal < 0)
                throw new ArgumentException("Лечение не может быть отрицательным");

            HP += heal;
        }
    }
}
