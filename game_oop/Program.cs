using Game.Equipment;
using Game.Heros;
using Game.Heros.Archer;
using Game.Heros.Archer.Super.Puper;
using Game.Heros.Archer.Сlever;
using Game.Heros.Archer.Сlever.Storm;
using Game.Heros.Mage;
using Game.Heros.Mage.Dark;
using Game.Heros.Mage.Dark.Blood_Lich;
using Game.Heros.Mage.Super;
using Game.Heros.Mage.Super.Puper;
using Game.Heros.Warrior;
using Game.Heros.Warrior.Dread;
using Game.Heros.Warrior.Dread.Titan;
using Game.Heros.Warrior.Super;
using Game.Heros.Warrior.Super.Puper;
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

    public enum UiState
    {
        Battle,
        Stats,
        Inventory,
        Forge
    }

    public class MainGame
    {
        readonly List<string> logStatUp = new List<string>();

        // --- Пагинация инвентаря ---
        private int _inventoryPage = 0;
        private const int ItemsPerPage = 9;

        // --- Режим разборки предметов в инвентаре ---
        private bool _dismantleMode = false;
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
            UiState state = UiState.Battle;

            battle.OnEnemyDefeated += enemy =>
            {
/*                hero = CheckClassEvolution(hero);
                battle.ReplaceHero(hero);*/

                IEnemy newMonster = CreateMonster(hero.Progress.Level);
                battle.ReplaceMonster(newMonster);
            };

            while (hero.IsAlive)
            {
                switch (state)
                {
                    case UiState.Battle:
                        DrawUI(hero, battle.CurrentMonster, (List<string>)battle.CombatLog);

                        WaitForKey();
                        ConsoleKeyInfo battleKey = Console.ReadKey(true);

                        switch (battleKey.Key)
                        {
                            case ConsoleKey.Enter:
                                battle.ProcessAttack();
                                break;
                            case ConsoleKey.Spacebar:
                                hero.Heal();
                                break;
                            case ConsoleKey.Escape:
                                state = UiState.Stats;
                                break;
                            case ConsoleKey.Tab:
                                state = UiState.Inventory;
                                break;
                        }
                        break;

                    case UiState.Stats:
                        DrawStat(hero);

                        WaitForKey();
                        ConsoleKeyInfo statsKey = Console.ReadKey(true);

                        switch (statsKey.Key)
                        {
                            case ConsoleKey.Escape:
                                state = UiState.Battle;
                                break;
                            case ConsoleKey.Tab:
                                state = UiState.Inventory;
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
                        break;

                    case UiState.Inventory:
                        DrawInventory(hero);

                        WaitForKey();
                        ConsoleKeyInfo invKey = Console.ReadKey(true);

                        switch (invKey.Key)
                        {
                            case ConsoleKey.Escape:
                                state = UiState.Battle;
                                break;
                            case ConsoleKey.Tab:
                                state = UiState.Stats;
                                break;
                            case ConsoleKey.D0:
                            case ConsoleKey.NumPad0:
                            case ConsoleKey.LeftArrow:
                                // Страница назад
                                if (_inventoryPage > 0) _inventoryPage--;
                                break;
                            case ConsoleKey.RightArrow:
                                // Страница вперед
                                _inventoryPage++;
                                break;
                            case ConsoleKey.R:
                                // Переключение режима "надеть" / "разобрать"
                                _dismantleMode = !_dismantleMode;
                                break;
                            case ConsoleKey.F:
                                state = UiState.Forge;
                                break;
                            default:
                                HandleInventoryInput(hero, invKey.Key);
                                break;
                        }
                        break;

                    case UiState.Forge:
                        RunForge(hero);
                        state = UiState.Inventory;
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
            var rand = new Random();

            // Редкость определяется случайно при каждом спавне: обычно обычный монстр,
            // изредка элитный, очень редко - босс (с более сильными характеристиками и лучшим лутом).
            MonsterRarity rarity = Monster.RollRarity(rand);

            var availableTypes = new List<(int MinLevel, Func<int, MonsterRarity, IEnemy> Factory)>
            {
                (1, (level, r) => new Goblin(level, r)),
                (5, (level, r) => new Ogre(level, r)),
                (10, (level, r) => new Golem(level, r)),
                (25, (level, r) => new Dragon(level, r)),
            };

            var available = availableTypes.FindAll(t => t.MinLevel <= heroLevel);
            var selected = available[rand.Next(available.Count)];

            int monsterLevel = heroLevel + rand.Next(-3, 4);
            if (monsterLevel < 1) monsterLevel = 1;

            IEnemy monster = selected.Factory(monsterLevel, rarity);

            if (rarity == MonsterRarity.Boss && monster is Monster bossMonster)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("==================================================");
                Console.WriteLine($"   ВНИМАНИЕ! Появился могущественный БОСС: {bossMonster.Name}!");
                Console.WriteLine("==================================================");
                Console.ResetColor();
                Thread.Sleep(1200);
            }

            return monster;
        }

        /*private Hero CheckClassEvolution(Hero hero)
        {
            Hero newHero = hero;

            if (hero.Progress.Level >= 10 && (hero.GetType() == typeof(Warrior) || hero.GetType() == typeof(Archer) || hero.GetType() == typeof(Mage)))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("==================================================");
                Console.WriteLine($"      ВАШ ГЕРОЙ ДОСТИГ {hero.Progress.Level} УРОВНЯ! ВЫБЕРИТЕ ПУТЬ:  ");
                Console.WriteLine("==================================================");
                Console.ResetColor();

                if (hero is Warrior)
                {
                    Console.WriteLine(" 1: Супер Воин (Берсерк: Урон растет от ран)");
                    Console.WriteLine(" 2: Страж (Танк: Урон усиливается от брони)");
                    string choice = Console.ReadLine();
                    newHero = choice == "2" ? new Dread_Warrior(hero.Name) : new Super_Warrior(hero.Name);
                }
                else if (hero is Archer)
                {
                    Console.WriteLine(" 1: Супер Лучник (Снайпер: Двойной выстрел)");
                    Console.WriteLine(" 2: Ловчий (Пробитие: Крит полностью игнорирует броню)");
                    string choice = Console.ReadLine();
                    newHero = choice == "2" ? new Trapper_Archer(hero.Name) : new Super_Archer(hero.Name);
                }
                else if (hero is Mage)
                {
                    Console.WriteLine(" 1: Супер Маг (Чародей: Шанс дополнительной атаки)");
                    Console.WriteLine(" 2: Тёмный Маг (Кровавый: Тратит HP ради 100% крита)");
                    string choice = Console.ReadLine();
                    newHero = choice == "2" ? new Dark_Mage(hero.Name) : new Super_Mage(hero.Name);
                }
            }

            else if (hero.Progress.Level >= 20)
            {
                if (hero.GetType() == typeof(Super_Warrior)) newHero = new Super_Puper_Warrior(hero.Name);
                else if (hero.GetType() == typeof(Dread_Warrior)) newHero = new Titan_Warrior(hero.Name);

                else if (hero.GetType() == typeof(Super_Archer)) newHero = new Super_Puper_Archer(hero.Name);
                else if (hero.GetType() == typeof(Trapper_Archer)) newHero = new Storm_Archer(hero.Name);

                else if (hero.GetType() == typeof(Super_Mage)) newHero = new Super_Puper_Mage(hero.Name);
                else if (hero.GetType() == typeof(Dark_Mage)) newHero = new Blood_Lich_Mage(hero.Name);
            }

            if (ReferenceEquals(newHero, hero))
            {
                return hero;
            }

            // Сообщения и начисление очков только при РЕАЛЬНОЙ смене класса
            newHero.TransferProgressFrom(hero);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nВы успешно сменили класс на {newHero.ClassName}!");
            Console.WriteLine($"Вам начислено {newHero.Score} очков прокачки ({newHero.Progress.Level}). Распределите их в меню статистики [Escape].");
            Console.ResetColor();
            Console.WriteLine("\nНажмите Enter, чтобы продолжить...");
            Console.ReadLine();

            return newHero;
        }*/

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
            Console.WriteLine($" [Enter] - Атаковать врага  |  [SpaceBar] - Исцеление (+{hero.HealHP} HP) | [Escape] - Статистика героя | [Tab] - Инвентарь ");
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

            // Считаем бонусы с учетом новой структуры списков
            int totalBonusArmor = hero.equippedItems.Values.SelectMany(list => list).Sum(i => i.BonusArmor);
            int totalBonusPower = hero.equippedItems.Values.SelectMany(list => list).Sum(i => i.BonusPower);

            // Информация о герое
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"ГЕРОЙ: {hero.Name} | Уровень: {hero.Progress.Level} | Опыт: {hero.Progress.Exp}/{hero.Progress.ExpToNextLevel} | Класс: {hero.ClassName}");
            Console.WriteLine($"Здоровье: {hero.HP}/{hero.MaxHP} | Броня: {hero.Armor} + {totalBonusArmor} | Сила: {hero.Power} + {totalBonusPower}");

            if (hero.Score != 0)
            {
                Console.WriteLine($"Имеется очков прокачки {hero.Score}!");
            }

            Console.ResetColor();
            Console.WriteLine("Текущая экипировка:");

            // Вывод списка экипировки в левой части экрана с учетом списков предметов
            foreach (TypeEquipment slot in Enum.GetValues(typeof(TypeEquipment)))
            {
                if (hero.equippedItems.TryGetValue(slot, out List<IEquipment> items) && items.Count > 0)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        var item = items[i];
                        // Если предметов несколько (например, кольца), добавляем индекс [Ring 1]
                        string slotName = items.Count > 1 ? $"{slot} {i + 1}" : slot.ToString();
                        Console.WriteLine($"[{slotName}] {item.Name} | Ур: {item.RequiredLevel} | Редкость: {item.Rarity} | А: +{item.BonusPower} Б: +{item.BonusArmor}");
                    }
                }
                else
                {
                    Console.WriteLine($"[{slot}] <Пусто>");
                }
            }

            Console.ResetColor();
            Console.WriteLine("--------------------------------------------------");

            // Подсказки по управлению
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" [Escape] - Закрыть статистику героя  |  [Tab] - Открыть инвентарь ");
            if (hero.Score != 0)
            {
                Console.WriteLine("Выберите статистику для прокачки:");
                Console.WriteLine(" 1: Сила + 4        2: Броня + 2        3: Здоровье + 30");
                Console.WriteLine(" 4: Крит. урон + 5% 5: Крит. шанс + 1%  6: Лечение + 10");
                Console.Write("Ваш выбор (1-6): ");
            }

            // --- ОТРИСОВКА ГРАФИКИ И СПИСКА СПРАВА ---
            DrawHeroAndEquipList(hero);

            // --- ЖУРНАЛ СОБЫТИЙ (Остается внизу слева) ---
            Console.ResetColor();
            int leftX = 0;
            int rightX = Console.WindowWidth - 45; // Смещаем границу журнала влево, чтобы не затереть графику
            if (rightX < 30) rightX = 30;

            int top = Console.CursorTop;
            Console.SetCursorPosition(leftX, top);
            Console.Write("ЖУРНАЛ СОБЫТИЙ:");
            top++;

            int startIdx = Math.Max(0, logStatUp.Count - 5);
            int logLines = logStatUp.Count - startIdx;
            for (int i = 0; i < logLines; i++)
            {
                string logLine = (i < logLines) ? logStatUp[startIdx + i] : "";
                Console.SetCursorPosition(leftX, top + i);
                Console.Write(logLine.PadRight(rightX - leftX));
            }
        }

        private void DrawHeroAndEquipList(Hero hero)
        {
            // Определяем координаты правого нижнего угла консоли
            int startY = Console.WindowHeight - 15; // 15 строк вверх от низа экрана
            int startX = Console.WindowWidth - 50;  // 50 символов от правого края экрана

            if (startY < 0) startY = 2; // Защита от маленького экрана
            if (startX < 40) startX = 40;

            // Проверяем наличие предметов в списках
            bool hasHelmet = hero.equippedItems.TryGetValue(TypeEquipment.Helmet, out var helmets) && helmets.Count > 0;
            bool hasChest = hero.equippedItems.TryGetValue(TypeEquipment.Breastplate, out var chests) && chests.Count > 0;
            bool hasBoots = hero.equippedItems.TryGetValue(TypeEquipment.Boots, out var boots) && boots.Count > 0;
            bool hasShield = hero.equippedItems.TryGetValue(TypeEquipment.Shield, out var shields) && shields.Count > 0;

            // Проверяем все виды оружия для левой руки
            bool hasSword = hero.equippedItems.TryGetValue(TypeEquipment.Sword, out var swords) && swords.Count > 0;
            bool hasBow = hero.equippedItems.TryGetValue(TypeEquipment.Bow, out var bows) && bows.Count > 0;
            bool hasStaff = hero.equippedItems.TryGetValue(TypeEquipment.Staff, out var staffs) && staffs.Count > 0;

            // Текстовые названия для вывода справа
            string helmName = hasHelmet ? helmets[0].Name : "<Пусто>";
            string chestName = hasChest ? chests[0].Name : "<Пусто>";
            string bootsName = hasBoots ? boots[0].Name : "<Пусто>";

            // Логика определения названия оружия для правого текстового блока
            string weaponName = "<Пусто>";
            if (hasSword) weaponName = swords[0].Name;
            else if (hasBow) weaponName = bows[0].Name;
            else if (hasStaff) weaponName = staffs[0].Name;

            // Логика визуального отображения левой руки персонажа
            string leftHand = " ";
            if (hasSword) leftHand = "!"; // Меч
            else if (hasStaff) leftHand = "*"; // Посох мага
            else if (hasBow) leftHand = "}"; // Лук

            // Правая рука (Щит)
            string rightHand = hasShield ? "[#]" : " ";

            // Торс (Кираса)
            string body = hasChest ? "[X]" : " | ";

            // Рисуем человечка символами
            Console.ForegroundColor = ConsoleColor.Cyan;

            // Голова / Шлем
            Console.SetCursorPosition(startX, startY);
            Console.Write(hasHelmet ? "  [===]  " : "   ( )   ");

            // Шея и плечи
            Console.SetCursorPosition(startX, startY + 1);
            Console.Write("   /|\\   ");

            // Нагрудник (Кираса) и оружие/щит в руках
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write($" {leftHand} {body} {rightHand} ");

            // Пояс / Ноги
            Console.SetCursorPosition(startX, startY + 3);
            Console.Write("   / \\   ");

            // Сапоги
            Console.SetCursorPosition(startX, startY + 4);
            Console.Write(hasBoots ? " |] [|  " : "  || || ");

            // Рисуем текстовое описание предметов ПРАВЕЕ человечка
            int textX = startX + 14;
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.SetCursorPosition(textX, startY);
            Console.Write($"Шлем: {helmName}");

            Console.SetCursorPosition(textX, startY + 1);
            Console.Write($"Оружие: {weaponName}"); // Универсальная строка для любого оружия

            Console.SetCursorPosition(textX, startY + 2);
            Console.Write($"Броня: {chestName}");

            Console.SetCursorPosition(textX, startY + 3);
            Console.Write($"Щит: {(hasShield ? shields[0].Name : "<Пусто>")}");

            Console.SetCursorPosition(textX, startY + 4);
            Console.Write($"Обувь: {bootsName}");

            // Выводим кольца, если они есть
            if (hero.equippedItems.TryGetValue(TypeEquipment.Ring, out var rings) && rings.Count > 0)
            {
                for (int i = 0; i < rings.Count; i++)
                {
                    Console.SetCursorPosition(textX, startY + 6 + i);
                    Console.Write($"Кольцо {i + 1}: {rings[i].Name}");
                }
            }

            Console.ResetColor();
        }

        private void AddLogStat(string message)
        {
            logStatUp.Add(message);
        }

        private static void WaitForKey()
        {
            while (!Console.KeyAvailable)
            {
                Thread.Sleep(20);
            }
        }

        /// <summary>
        /// Окно инвентаря героя, открываемое по Tab. Позволяет посмотреть подобранные предметы
        /// и надеть их (с учетом требуемого уровня, ограничений по классу и количеству слотов).
        /// Как и DrawStat, использует DrawHeroAndEquipList для отрисовки текущей экипировки.
        /// </summary>
        public void DrawInventory(Hero hero)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"ГЕРОЙ: {hero.Name} | Уровень: {hero.Progress.Level} | Класс: {hero.ClassName}");
            Console.ResetColor();
            Console.WriteLine("--------------------------------------------------");

            // --- Материалы ---
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"МАТЕРИАЛЫ: Металл: {hero.Materials[MaterialType.Metal]} | Дерево: {hero.Materials[MaterialType.Wood]} | Нить: {hero.Materials[MaterialType.Thread]} | Маг. камень: {hero.Materials[MaterialType.MagicStone]}");
            Console.ResetColor();
            Console.WriteLine("--------------------------------------------------");

            // --- Пагинация: считаем страницы по ItemsPerPage предметов ---
            int totalPages = Math.Max(1, (int)Math.Ceiling(hero.Inventory.Count / (double)ItemsPerPage));
            if (_inventoryPage >= totalPages) _inventoryPage = totalPages - 1;
            if (_inventoryPage < 0) _inventoryPage = 0;

            int pageStart = _inventoryPage * ItemsPerPage;
            int pageCount = Math.Max(0, Math.Min(ItemsPerPage, hero.Inventory.Count - pageStart));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"ИНВЕНТАРЬ (Страница {_inventoryPage + 1}/{totalPages}, всего предметов: {hero.Inventory.Count}):");
            if (_dismantleMode)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("РЕЖИМ: РАЗБОРКА НА МАТЕРИАЛЫ (клавиша [R] - вернуться к режиму надевания)");
            }
            Console.ResetColor();

            if (hero.Inventory.Count == 0)
            {
                Console.WriteLine("  <пусто - убивайте монстров, чтобы находить экипировку>");
            }
            else
            {
                for (int i = 0; i < pageCount; i++)
                {
                    IEquipment item = hero.Inventory[pageStart + i];
                    bool canEquip = hero.CanEquip(item);

                    Console.ForegroundColor = GetRarityColor(item.Rarity);
                    string lockMark = canEquip ? "" : "  (нельзя надеть)";
                    string number = $"[{i + 1}]";
                    Console.WriteLine($" {number} {item.Name} | Тип: {item.TypeEquipment} | Ур: {item.RequiredLevel} | Редкость: {item.Rarity} | А: +{item.BonusPower} Б: +{item.BonusArmor}{lockMark}");
                }
                Console.ResetColor();
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Текущая экипировка:");

            // --- ОТРИСОВКА ГРАФИКИ ГЕРОЯ И СПИСКА ЭКИПИРОВКИ СПРАВА ---
            DrawHeroAndEquipList(hero);

            // Последние события (например, результат надевания предмета)
            if (logStatUp.Count > 0)
            {
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("ЖУРНАЛ СОБЫТИЙ:");
                int startIdx = Math.Max(0, logStatUp.Count - 3);
                for (int i = startIdx; i < logStatUp.Count; i++)
                {
                    Console.WriteLine(logStatUp[i]);
                }
            }

            int footerY = Math.Max(Console.CursorTop + 1, Console.WindowHeight - 4);
            if (footerY >= Console.WindowHeight) footerY = Console.WindowHeight - 1;
            if (footerY < 0) footerY = 0;

            Console.SetCursorPosition(0, footerY);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" [Escape] - Вернуться в бой   |   [Tab] - Статистика героя   |   [F] - Кузница ");
            if (hero.Inventory.Count > 0)
            {
                string modeHint = _dismantleMode
                    ? " [1-9] - Разобрать предмет под этим номером на материалы "
                    : " [1-9] - Надеть предмет из инвентаря под этим номером ";
                Console.WriteLine(modeHint);
                Console.WriteLine(" [R] - Переключить режим (надеть / разобрать) ");
            }
            if (totalPages > 1)
            {
                Console.WriteLine(" [0] или [←] - Страница назад   |   [→] - Страница вперед ");
            }
            Console.ResetColor();
        }

        private void HandleInventoryInput(Hero hero, ConsoleKey key)
        {
            int localIndex = key switch
            {
                ConsoleKey.D1 or ConsoleKey.NumPad1 => 0,
                ConsoleKey.D2 or ConsoleKey.NumPad2 => 1,
                ConsoleKey.D3 or ConsoleKey.NumPad3 => 2,
                ConsoleKey.D4 or ConsoleKey.NumPad4 => 3,
                ConsoleKey.D5 or ConsoleKey.NumPad5 => 4,
                ConsoleKey.D6 or ConsoleKey.NumPad6 => 5,
                ConsoleKey.D7 or ConsoleKey.NumPad7 => 6,
                ConsoleKey.D8 or ConsoleKey.NumPad8 => 7,
                ConsoleKey.D9 or ConsoleKey.NumPad9 => 8,
                _ => -1
            };

            if (localIndex < 0)
            {
                return;
            }

            int globalIndex = _inventoryPage * ItemsPerPage + localIndex;
            if (globalIndex < 0 || globalIndex >= hero.Inventory.Count)
            {
                return;
            }

            IEquipment item = hero.Inventory[globalIndex];

            if (_dismantleMode)
            {
                string itemName = item.Name;
                if (hero.DismantleItem(item, out var gained))
                {
                    string matText = string.Join(", ", gained.Select(m => $"{GetMaterialName(m.Key)}: +{m.Value}"));
                    AddLogStat($"Разобрано: {itemName} -> {matText}");
                }
            }
            else
            {
                bool couldEquip = hero.CanEquip(item);
                hero.Equip(item);

                AddLogStat(couldEquip
                    ? $"Экипировано: {item.Name}"
                    : $"Не удалось надеть {item.Name} (проверьте требуемый уровень и класс)");
            }
        }

        private static string GetMaterialName(MaterialType type) => type switch
        {
            MaterialType.Metal => "Металл",
            MaterialType.Wood => "Дерево",
            MaterialType.Thread => "Нить",
            MaterialType.MagicStone => "Маг. камень",
            _ => type.ToString()
        };

        /// <summary>
        /// Кузница: выбор предмета из инвентаря для ковки (улучшения качества) и запуск мини-игры.
        /// Ковка тратит материалы независимо от результата мини-игры; успех улучшает качество
        /// предмета на одну ступень (например, Обычный -> Качественный).
        /// </summary>
        private void RunForge(Hero hero)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==================== КУЗНИЦА ====================");
            Console.ResetColor();
            Console.WriteLine($"Материалы: Металл {hero.Materials[MaterialType.Metal]} | Дерево {hero.Materials[MaterialType.Wood]} | Нить {hero.Materials[MaterialType.Thread]} | Маг. камень {hero.Materials[MaterialType.MagicStone]}");
            Console.WriteLine("--------------------------------------------------");

            List<IEquipment> forgeable = hero.Inventory.ToList();

            if (forgeable.Count == 0)
            {
                Console.WriteLine("В инвентаре нет предметов для ковки.");
                Console.WriteLine("Нажмите любую клавишу, чтобы вернуться...");
                WaitForKey();
                Console.ReadKey(true);
                return;
            }

            Console.WriteLine("Выберите предмет для ковки (улучшение качества на 1 ступень):");
            int shown = Math.Min(9, forgeable.Count);
            for (int i = 0; i < shown; i++)
            {
                IEquipment item = forgeable[i];
                var cost = Dismantler.GetForgeCost(item);
                string costText = string.Join(", ", cost.Select(c => $"{GetMaterialName(c.Key)}: {c.Value}"));

                Console.ForegroundColor = GetRarityColor(item.Rarity);
                string maxedMark = item.TypeQualities == TypeQuality.Divine ? "  (максимум)" : "";
                Console.WriteLine($" [{i + 1}] {item.Name} | Качество: {item.TypeQualities}{maxedMark} | Стоимость: {costText}");
            }
            Console.ResetColor();
            Console.WriteLine(" [Escape] - Отмена");

            WaitForKey();
            ConsoleKeyInfo chooseKey = Console.ReadKey(true);
            if (chooseKey.Key == ConsoleKey.Escape)
            {
                return;
            }

            int idx = chooseKey.Key switch
            {
                ConsoleKey.D1 or ConsoleKey.NumPad1 => 0,
                ConsoleKey.D2 or ConsoleKey.NumPad2 => 1,
                ConsoleKey.D3 or ConsoleKey.NumPad3 => 2,
                ConsoleKey.D4 or ConsoleKey.NumPad4 => 3,
                ConsoleKey.D5 or ConsoleKey.NumPad5 => 4,
                ConsoleKey.D6 or ConsoleKey.NumPad6 => 5,
                ConsoleKey.D7 or ConsoleKey.NumPad7 => 6,
                ConsoleKey.D8 or ConsoleKey.NumPad8 => 7,
                ConsoleKey.D9 or ConsoleKey.NumPad9 => 8,
                _ => -1
            };

            if (idx < 0 || idx >= shown)
            {
                return;
            }

            IEquipment chosen = forgeable[idx];

            if (chosen.TypeQualities == TypeQuality.Divine)
            {
                Console.WriteLine("Этот предмет уже имеет максимальное (Божественное) качество!");
                Console.WriteLine("Нажмите любую клавишу...");
                WaitForKey();
                Console.ReadKey(true);
                return;
            }

            var forgeCost = Dismantler.GetForgeCost(chosen);
            if (!hero.TrySpendMaterials(forgeCost))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Недостаточно материалов для ковки этого предмета!");
                Console.ResetColor();
                Console.WriteLine("Нажмите любую клавишу...");
                WaitForKey();
                Console.ReadKey(true);
                return;
            }

            bool success = PlayForgeMinigame();

            if (success && chosen.TryUpgradeQuality())
            {
                AddLogStat($"Ковка успешна! {chosen.Name} улучшен до качества: {chosen.TypeQualities}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nУСПЕХ! Новое качество предмета: {chosen.TypeQualities}");
            }
            else
            {
                AddLogStat($"Ковка неудачна: {chosen.Name} не улучшен, материалы потрачены впустую");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nНЕУДАЧА! Материалы потрачены впустую, предмет не улучшен.");
            }

            Console.ResetColor();
            Console.WriteLine("Нажмите любую клавишу, чтобы вернуться в инвентарь...");
            WaitForKey();
            Console.ReadKey(true);
        }

        private bool PlayForgeMinigame()
        {
            const int barLength = 30;
            var rand = new Random();
            int targetLength = 4;
            int targetStart = rand.Next(2, barLength - targetLength - 2);
            int targetEnd = targetStart + targetLength;

            int position = 0;
            int direction = 1;
            bool hit = false;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==================== КОВКА ====================");
            Console.ResetColor();
            Console.WriteLine("Нажмите [SPACE], когда звездочка (*) окажется внутри зоны из символов [=]!");
            Console.WriteLine();

            int barRow = Console.CursorTop;
            bool finished = false;

            while (!finished)
            {
                char[] bar = new char[barLength];
                for (int i = 0; i < barLength; i++)
                {
                    bar[i] = (i >= targetStart && i < targetEnd) ? '=' : '-';
                }
                bar[Math.Clamp(position, 0, barLength - 1)] = '*';

                Console.SetCursorPosition(0, barRow);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[" + new string(bar) + "]   ");
                Console.ResetColor();

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo hitKey = Console.ReadKey(true);
                    if (hitKey.Key == ConsoleKey.Spacebar)
                    {
                        hit = position >= targetStart && position < targetEnd;
                        finished = true;
                        break;
                    }
                }

                Thread.Sleep(55); // скорость движения звездочки

                position += direction;
                if (position >= barLength - 1) direction = -1;
                if (position <= 0) direction = 1;
            }

            Console.WriteLine();
            return hit;
        }

        private static ConsoleColor GetRarityColor(EquipmentRarity rarity) => rarity switch
        {
            EquipmentRarity.Common => ConsoleColor.Gray,
            EquipmentRarity.Uncommon => ConsoleColor.White,
            EquipmentRarity.Rare => ConsoleColor.Cyan,
            EquipmentRarity.Epic => ConsoleColor.Magenta,
            EquipmentRarity.Legendary => ConsoleColor.Yellow,
            EquipmentRarity.Divine => ConsoleColor.Red,
            _ => ConsoleColor.Gray
        };
    }
}