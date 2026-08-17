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
        readonly List<string> logStatUp = new List<string>();
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
            bool seeStats = false;

            battle.OnEnemyDefeated += enemy =>
            {
                IEnemy newMonster = CreateMonster(hero.Progress.Level);
                battle.ReplaceMonster(newMonster);
            };

            while (hero.IsAlive)
            {
                if (!seeStats)
                {
                    DrawUI(hero, battle.CurrentMonster, (List<string>)battle.CombatLog);

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
                        case ConsoleKey.Escape:
                            seeStats = true;
                            break;
                    }
                }
                else
                {
                    DrawStat(hero);

                    while (!Console.KeyAvailable)
                    {
                        Thread.Sleep(20);
                    }

                    ConsoleKeyInfo key = Console.ReadKey(true);

                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            seeStats = false;
                            break;

                        case ConsoleKey.D1:
                            AddLogStat(hero.TryUpgradeStat(StatType.Power)
                                 ? "Сила успешно увеличена!"
                                 : "Недостаточно очков для прокачки!");
                            break;
                        case ConsoleKey.D2:
                            AddLogStat(hero.TryUpgradeStat(StatType.Armor)
                                 ? "Защита успешно увеличена!"
                                 : "Недостаточно очков для прокачки!");
                            break;
                        case ConsoleKey.D3:
                            AddLogStat(hero.TryUpgradeStat(StatType.MaxHp)
                                 ? "Здоровье успешно увеличено!"
                                 : "Недостаточно очков для прокачки!");
                            break;
                        case ConsoleKey.D4:
                            AddLogStat(hero.TryUpgradeStat(StatType.CritDamage)
                                 ? "Крит. урон успешно увеличен!"
                                 : "Недостаточно очков для прокачки!");
                            break;
                        case ConsoleKey.D5:
                            AddLogStat(hero.TryUpgradeStat(StatType.CritRate)
                                 ? "Крит. шанс успешно увеличен!"
                                 : "Недостаточно очков для прокачки!");
                            break;
                        case ConsoleKey.D6:
                            AddLogStat(hero.TryUpgradeStat(StatType.HealHP)
                                 ? "Лечение успешно увеличено!"
                                 : "Недостаточно очков для прокачки!");
                            break;
                    }
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
                (5, level => new Ogre(level)),
                (10, level => new Golem(level)),
                (25, level => new Dragon(level)),
            };

            var available = availableTypes.FindAll(t => t.MinLevel <= heroLevel);
            var rand = new Random();
            var selected = available[rand.Next(available.Count)];

            int monsterLevel = heroLevel + rand.Next(-3, 4);
            if (monsterLevel < 1) monsterLevel = 1;

            return selected.Factory(monsterLevel);
        }

        public static void DrawUI(Hero hero, IEnemy enemy, List<string> combatLog)
        {
            Console.Clear();

            // Информация о герое
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"ГЕРОЙ: {hero.Name} | Класс: {hero.ClassName}");
            Console.WriteLine($"Здоровье: {hero.HP}/{hero.MaxHP}");
            Console.ResetColor();
            Console.WriteLine("--------------------------------------------------");

            // Информация о монстре
            if (enemy is Monster monster)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ВРАГ:  {monster.Name} | Уровень: {monster.Level}");
                Console.WriteLine($"Здоровье: {monster.HP} | Броня: {monster.Armor} | Сила: {monster.Power}");
                Console.ResetColor();
                Console.WriteLine("==================================================");
            }

            // Подсказки по управлению
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($" [Enter] - Атаковать врага  |  [SpaceBar] - Исцеление (+{hero.HealHP} HP) | [Escape] - Посмотреть статистику героя ");
            Console.ResetColor();
            Console.WriteLine("==================================================");

            int leftX = 0;
            int rightX = Console.WindowWidth - 30; // отступ справа для статистики
            if (rightX < 40) rightX = 40; // минимальная ширина

            int top = Console.CursorTop; // текущая строка после разделителя

            // Заголовки
            Console.SetCursorPosition(leftX, top);
            Console.Write("ЖУРНАЛ СОБЫТИЙ:");
            top++;

            // Подготовка строк журнала (последние 10)
            int startIdx = Math.Max(0, combatLog.Count - 10);
            int logLines = combatLog.Count - startIdx;

            for (int i = 0; i < logLines; i++)
            {
                string logLine = (i < logLines) ? combatLog[startIdx + i] : "";

                Console.SetCursorPosition(leftX, top + i);
                Console.Write(logLine.PadRight(rightX - leftX)); // заполняем пробелами до правой колонки
                Console.SetCursorPosition(rightX, top + i);
            }
        }

        public void DrawStat(Hero hero)
        {
            Console.Clear();

            // Информация о герое
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"ГЕРОЙ: {hero.Name} | Уровень: {hero.Progress.Level} | Опыт: {hero.Progress.Exp}/{hero.Progress.ExpToNextLevel} | Класс: {hero.ClassName}");
            Console.WriteLine($"Здоровье: {hero.HP}/{hero.MaxHP} | Броня: {hero.Armor} | Сила: {hero.Power}");
            if (hero.Score != 0)
            {
                Console.WriteLine($"Имеется очков прокачки {hero.Score}!");
            }
            Console.ResetColor();
            Console.WriteLine("--------------------------------------------------");
            // Подсказки по управлению
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" [Escape] - Закрыть статистику героя ");
            if (hero.Score != 0)
            {
                Console.WriteLine("Выберите статистику для прокачки:");
                Console.WriteLine(" 1: Сила + 4");
                Console.WriteLine(" 2: Броня + 2");
                Console.WriteLine(" 3: Здоровье + 30");
                Console.WriteLine(" 4: Крит. удар + 5%");
                Console.WriteLine(" 5: Крит. шанс + 1%");
                Console.WriteLine(" 6: Лечение + 10");
                Console.Write("Ваш выбор (1-6): ");
            }

            int leftX = 0;
            int rightX = Console.WindowWidth - 30; // отступ справа для статистики
            if (rightX < 40) rightX = 40; // минимальная ширина

            int top = Console.CursorTop; // текущая строка после разделителя

            // Заголовки
            Console.SetCursorPosition(leftX, top);
            Console.Write("ЖУРНАЛ СОБЫТИЙ:");
            top++;

            int startIdx = Math.Max(0, logStatUp.Count - 10);
            int logLines = logStatUp.Count - startIdx;
            for (int i = 0; i < logLines; i++)
            {
                string logLine = (i < logLines) ? logStatUp[startIdx + i] : "";

                Console.SetCursorPosition(leftX, top + i);
                Console.Write(logLine.PadRight(rightX - leftX)); // заполняем пробелами до правой колонки
                Console.SetCursorPosition(rightX, top + i);
            }
        }

        private void AddLogStat(string message)
        {
            logStatUp.Add(message);
        }
    }
}