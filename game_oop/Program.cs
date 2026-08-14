namespace Game
{
    internal class Program
    {
        static void Main()
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
            Console.WriteLine(" 4: Разбойник");
            Console.WriteLine(" 5: Берсерк");
            Console.WriteLine(" 6: Друид");
            Console.WriteLine(" 7: Некромант");
            Console.WriteLine(" 8: Паладин");
            Console.Write("Ваш выбор (1-8): ");
            string choice = Console.ReadLine();

            Hero hero = choice switch
            {
                "1" => new Warrior(name),
                "2" => new Archer(name),
                "3" => new Mage(name),
                "4" => new Rogue(name),
                "5" => new Berserker(name),
                "6" => new Druid(name),
                "7" => new Necromancer(name),
                "8" => new Paladin(name),
                _ => new Warrior(name)
            };

            IEnemy initialMonster = CreateMonster(hero.Level);
            var battle = new Battle(hero, initialMonster);
            AchievementManager achievementManager = new AchievementManager(hero);

            battle.OnEnemyDefeated += enemy =>
            {
                IEnemy newMonster = CreateMonster(hero.Level);
                battle.ReplaceMonster(newMonster);
            };

            battle.OnEnemyDefeated += achievementManager.OnEnemyDefeated;

            while (hero.IsAlive)
            {
                int totalKills = achievementManager.GetTotalKills();
                List<string> killStats = achievementManager.GetKillStatsLines();
                GameUI.DrawUI(hero, battle.CurrentMonster, (List<string>)battle.CombatLog, totalKills, killStats);

                while (!Console.KeyAvailable)
                {
                    Thread.Sleep(20);
                }

                ConsoleKeyInfo key = Console.ReadKey(true);
                bool actionTaken = false;

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        battle.ProcessAttack();
                        actionTaken = true;
                        break;
                    case ConsoleKey.Spacebar:
                        battle.ProcessHeal();
                        actionTaken = true;
                        break;
                    case ConsoleKey.Escape:
                        bool escaped = battle.ProcessEscape();
                        if (escaped && hero.IsAlive)
                        {
                            IEnemy newMonster = CreateMonster(hero.Level);
                            battle.ReplaceMonster(newMonster);
                        }
                        actionTaken = true;
                        break;
                }

                if (actionTaken && !hero.IsAlive)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("==================================================");
                    Console.WriteLine("                ИГРА ОКОНЧЕНА                     ");
                    Console.WriteLine("==================================================");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"{hero.Name} пал в бою на {hero.Level} уровне.");
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
                (3, level => new Mech_Golem(level)),
                (5, level => new Orc(level)),
                (8, level => new Troll(level)),
                (10, level => new Dragon(level))
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