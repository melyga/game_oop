namespace Game
{
    internal class Program
    {
        private static List<string> combatLog = new List<string>();
        private static Random Rand = new Random();

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Hero hero;

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
            Console.Write("Ваш выбор (1-4): ");
            string choise = Console.ReadLine();

            switch (choise)
            {
                case "1": hero = new Warrior(name); break;
                case "2": hero = new Archer(name); break;
                case "3": hero = new Mage(name); break;
                case "4": hero = new Bandit(name); break;
                default:
                    Console.WriteLine("Неверный выбор. По умолчанию выбран Воин.");
                    hero = new Warrior(name);
                    Thread.Sleep(1000);
                    break;
            }

            // Создаем первого монстра
            Monster currentMonster = CreateMonster(hero.Level);
            AddLog($"Из темноты появляется {currentMonster.Name}!");

            bool escapeFailedInThisBattle = false;

            // Первичная отрисовка экрана
            DrawUI(hero, currentMonster);

            while (hero.IsAlive)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    bool actionTaken = false;

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Enter:
                            // Ход игрока
                            int damage = hero.Attack(currentMonster);
                            AddLog($"{hero.Name} наносит {damage} урона по {currentMonster.Name}.");

                            if (!currentMonster.IsAlive)
                            {
                                AddLog($"{currentMonster.Name} повержен!");

                                escapeFailedInThisBattle = false;

                                // Создание нового монстра
                                currentMonster = CreateMonster(hero.Level);
                                AddLog($"Новый враг: {currentMonster.Name} [{currentMonster.Level} ур.] приближается!");
                            }
                            else
                            {
                                // Ответный ход монстра, если он выжил
                                AddLog($"{currentMonster.Name} атакует в ответ!");
                                hero.TakeDamage(currentMonster);
                            }
                            actionTaken = true;
                            break;

                        case ConsoleKey.Spacebar:
                            hero.Heal(25);
                            AddLog($"{hero.Name} восстанавливает 25 HP.");

                            // Монстр все равно бьет, пока игрок лечится
                            AddLog($"{currentMonster.Name} воспользовался моментом и атаковал!");
                            hero.TakeDamage(currentMonster);

                            actionTaken = true;
                            break;

                        case ConsoleKey.Escape:
                            if (escapeFailedInThisBattle)
                            {
                                AddLog("Путь к отступлению отрезан! Вы не можете сбежать!");
                            }
                            else
                            {
                                double currentEscapeChance = hero.EscapeChance;
                                AddLog($"Попытка побега (Шанс: {currentEscapeChance:F1}%)...");

                                if (Rand.NextDouble() * 100 < currentEscapeChance)
                                {
                                    AddLog("Успех! Вы успешно убежали от монстра!");

                                    escapeFailedInThisBattle = false;

                                    currentMonster = CreateMonster(hero.Level);
                                    AddLog($"Вы забрели в другую комнату. Там вас ждет {currentMonster.Name}!");
                                }
                                else
                                {
                                    AddLog("Провал! Не удалось убежать!");

                                    escapeFailedInThisBattle = true;

                                    AddLog($"{currentMonster.Name} перехватывает вас, зажимает в угол и бьет в спину!");
                                    hero.TakeDamage(currentMonster);
                                }
                            }
                            actionTaken = true;
                            break;

                        default:
                            break;
                    }

                    // Перерисовываем интерфейс только если было совершено действие
                    if (actionTaken)
                    {
                        DrawUI(hero, currentMonster);
                    }
                }

                Thread.Sleep(20);
            }

            // Экран поражения
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("==================================================");
            Console.WriteLine("                ИГРА ОКОНЧЕНА                     ");
            Console.WriteLine("==================================================");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"{hero.Name} пал в бою на {hero.Level} уровне.");
            Console.ReadLine();
        }

        /// <summary>
        /// Генерация монстра с жестким отсечением уровня < 1 и шансами 75%/25%
        /// </summary>
        static Monster CreateMonster(int heroLevel)
        {
            Random rand = new Random();
            int monsterLevel;

            int chance = rand.Next(1, 101);

            if (chance <= 75) // 75% шанс на диапазон +-5
            {
                int offset = rand.Next(-5, 5);
                monsterLevel = heroLevel + offset;
            }
            else // 25% шанс на диапазон от 5 до 10 в любую сторону
            {
                bool isStronger = rand.Next(0, 2) == 1;
                int offset = isStronger ? rand.Next(5, 8) : rand.Next(-7, -5);
                monsterLevel = heroLevel + offset;
            }

            // Уровень монстра не может быть меньше 1
            if (monsterLevel < 1)
            {
                monsterLevel = 1;
            }

            // Шаблоны имен для разнообразия в зависимости от уровня
            string monsterName = "Гоблин";
            if (monsterLevel > heroLevel + 5) monsterName = "Элитный Гоблин-Вожак";
            else if (monsterLevel < heroLevel - 5) monsterName = "Слабый Гоблин-раб";

            return new Monster(monsterName, hp: 35, armor: 5, strenght: 15, level: monsterLevel);
        }

        /// <summary>
        /// Отрисовка строгого текстового интерфейса (HUD)
        /// </summary>
        static void DrawUI(Hero hero, Monster monster)
        {
            Console.Clear();

            // СТАТУС ГЕРОЯ
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"ГЕРОЙ: {hero.Name,-15} | Уровень: {hero.Level,-3} | Опыт: {hero.Score}/{hero.ExpToNextLevel} | Класс: {hero.ClassName} | Шанс побега: {hero.EscapeChance:F1}");
            Console.Write($"Здоровье: {hero.HP}/{hero.MaxHP,-5} | Броня: {hero.Armor,-3} | Сила: {hero.Strength}");
            if (hero is Warrior warrior)
            {
                Console.Write($" | Ярость: {warrior.Rage}/100");
            }
            Console.WriteLine();

            Console.ResetColor();
            Console.WriteLine("--------------------------------------------------");

            // СТАТУС МОНСТРА
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ВРАГ:  {monster.Name,-15} | Уровень: {monster.Level,-3}");
            Console.WriteLine($"Здоровье: {monster.HP,-7} | Броня: {monster.Armor}");
            Console.ResetColor();
            Console.WriteLine("==================================================");

            // ИНСТРУКЦИЯ
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" [Enter] - Атаковать врага  |  [Пробел] - Исцеление (+15 HP)");
            Console.ResetColor();
            Console.WriteLine("==================================================");

            // ЖУРНАЛ БОЯ (Выводим последние 5 событий)
            Console.WriteLine("ЖУРНАЛ СОБЫТИЙ:");
            int startIdx = Math.Max(0, combatLog.Count - 10);
            for (int i = startIdx; i < combatLog.Count; i++)
            {
                Console.WriteLine($" {combatLog[i]}");
            }
        }

        // Помощник для добавления записей в лог боя
        static void AddLog(string message)
        {
            combatLog.Add(message);
        }
    }
}