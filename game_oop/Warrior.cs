namespace Game
{
    public class Warrior : Hero
    {
        public int Rage { get; private set; } = 0;
        public const int MaxRage = 100;

        /// <summary>
        /// Создает героя с направлением в Воина. 
        /// Уникальность класса является высокий запас здоровья, высокая броня и возможность накопления ярости для мощного удара.
        /// </summary>
        public Warrior(string name) 
            : base(name, hp: 120, strength: 15, agility: 5, score: 0, maxHP: 175, armor: 10)
        { }

        public override int Attack(Monster monster)
        {
            int damage = CalculateDamage();

            if (Rage >= 50)
            {
                Rage -= 50;
                int powerfulDamage = CalculateDamage() * 2; // Мощный удар за 50 ярости
                monster.TakeDamage(powerfulDamage);
                return powerfulDamage;
            }

            if (monster.TakeDamage(damage))
            {
                AwardExperience(monster);
            }

            return damage;
        }
        public new void TakeDamage(Monster monster)
        {
            base.TakeDamage(monster);

            // 25% шанс контратаковать при получении урона
            if (Rand.Next(0, 100) < 25)
            {
                int counterDamage = Strength / 2;
                monster.TakeDamage(counterDamage);
            }
            Rage = Math.Min(MaxRage, Rage + (monster.Strength / 2)); // Получаем ярость в зависимости от потерянного здоровья
        }

        public override string ClassName => "Воин";
    }
}
