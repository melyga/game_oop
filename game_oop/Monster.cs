namespace Game
{
    public class Monster
    {
        public string Name { get; private set; }
        public int HP { get; private set; }
        public int Armor { get; private set; }

        public bool IsAlive => HP > 0;

        public Monster(string name, int hp, int armor)
        {
            Name = name;
            HP = hp;
            Armor = armor;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new ArgumentException("Урон не может быть отрицательным");

            int real = damage - Armor;
            if (real < 0)
                real = 0;

            HP -= real;
            if (HP < 0)
                HP = 0;
        }

        public void Heal(int heal)
        {
            if (heal < 0)
                throw new ArgumentException("Лечение не может быть отрицательным");

            HP += heal;
        }
    }
}
