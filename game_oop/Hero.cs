namespace Game
{
    public class Hero
    {
        public string Name { get; private set; }
        public int HP { get; private set; }
        public int Strength { get; private set; }
        public int Agility { get; private set; }
        public int Score { get; private set; }
        public int MaxHP { get; private set; }

        public bool IsAlive => HP > 0;

        public Hero(string name, int hp, int strength, int agility, int score, int maxHP)
        {
            Name = name;
            HP = hp;
            Strength = strength;
            Agility = agility;
            Score = score;
            MaxHP = maxHP;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new ArgumentException("Урон не может быть отрицательным");

            Console.WriteLine($"Получен урон в {damage} единиц здоровья!");
            Console.WriteLine();
            HP -= damage;
            if (HP < 0)
                HP = 0;
        }

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
                Console.WriteLine($"Вы вылечились на {heal} единиц здоровья!");
                Console.WriteLine();
            }                
        }
    }
}
