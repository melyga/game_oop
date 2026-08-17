using Game.Heros;
using Game.Heros.Archer;
using Game.Heros.Mage;
using Game.Heros.Warrior;
using Game.Monsters;

namespace Game
{
    internal class Program
    {
        static void Main() 
        {
            MainGame game = new MainGame();
            game.Run();
        }
    }

    public class MainGame
    {
        public void Run() 
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("           ДОБРО ПОЖАЛОВАТЬ В CONSOLE RPG         ");
            Console.WriteLine("==================================================");

            Console.Write("Введите имя вашего героя: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) name = "Безымянный";

            Console.WriteLine("Выберите класс героя:");
            Console.WriteLine(" 1: Воин");
            Console.WriteLine(" 2: Лучник");
            Console.WriteLine(" 3: Маг");
            Console.Write("Ваш выбор (1-3): ");
            string choice = Console.ReadLine();

            Hero hero = choice switch
            {
                "1" => new Warrior(name),
                "2" => new Archer(name),
                "3" => new Mage(name),
                _ => new Warrior(name)
            };

            IEnemy initialMonster = CreateMonster(hero.Progress.Level);
            var battle = new Battle(hero, initialMonster);

            battle.OnEnemyDefeated += enemy =>
            {
                IEnemy newMonster = CreateMonster(hero.Progress.Level);
                battle.ReplaceMonster(newMonster);
            };

            while (hero.IsAlive)
            {
                while (!Console.KeyAvailable)
                {
                    Thread.Sleep(20);
                }

                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        battle.ProcessAttack();
                        break;
                    case ConsoleKey.Spacebar:
                        hero.Heal();
                        break;
                }

                if (!hero.IsAlive)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("==================================================");
                    Console.WriteLine("                ИГРА ОКОНЧЕНА                     ");
                    Console.WriteLine("==================================================");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"{hero.Name} пал в бою на {hero.Progress.Level} уровне.");
                    Console.ReadLine();
                    return;
                }
            }
        }

        static IEnemy CreateMonster(int heroLevel)
        {
            var availableTypes = new List<(int MinLevel, Func<int, IEnemy> Factory)>
            {
                (1, level => new Goblin(level)),
            };

            var available = availableTypes.FindAll(t => t.MinLevel <= heroLevel);
            var rand = new Random();
            var selected = available[rand.Next(available.Count)];

            int monsterLevel = heroLevel + rand.Next(-3, 4);
            if (monsterLevel < 1) monsterLevel = 1;

            return selected.Factory(monsterLevel);
        }
    }
}