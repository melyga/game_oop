namespace Game
{
    internal class Program
    {
        static void Main()
        {
            Hero hero = new Hero(
                name: "Gabriel",
                hp: 100,
                strength: 8,
                agility: 5,
                score: 0,
                maxHP: 150
                );
            DisplayHeroStats(hero);
            Console.WriteLine();

            Monster goblin = new(
                name: "Gobline",
                hp: 15,
                armor: 3
                );
            Console.WriteLine($"Из темноты выходит {goblin.Name} (Здоровье: {goblin.HP}, Броня: {goblin.Armor})");
            Console.WriteLine();

            Monster ork = new(
                name: "Ork",
                hp: 80,
                armor: 15
                );
            Console.WriteLine($"Из подземелья выходит {ork.Name} (Здоровье: {ork.HP}, Броня: {ork.Armor})");
            Console.WriteLine();

            hero.TakeDamage(30);
            DisplayHeroStats(hero);
            Console.WriteLine();

            hero.Heal(160);
            DisplayHeroStats(hero);
            Console.WriteLine();

            hero.TakeDamage(150);
            DisplayHeroStats(hero);
        }

        static void DisplayHeroStats(Hero hero)
        {
            Console.WriteLine($"Имя героя: {hero.Name}");
            Console.WriteLine($"Здоровье: {hero.HP}");
            Console.WriteLine($"Сила: {hero.Strength}");
            Console.WriteLine($"Ловкость: {hero.Agility}");
            Console.WriteLine($"Опыт: {hero.Score}");
            Console.WriteLine($"Максимальное здоровье: {hero.MaxHP}");
            string alive = hero.IsAlive ? "жив" : "мертв";
            Console.WriteLine($"Герой: {alive}");
        }
    }
}