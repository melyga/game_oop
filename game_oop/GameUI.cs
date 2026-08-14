namespace Game
{
    public static class GameUI
    {
        public static void DrawUI(Hero hero, IEnemy monster, List<string> combatLog, int totalKills, List<string> killStats)
        {
            Console.Clear();

            // Информация о герое
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"ГЕРОЙ: {hero.Name} | Уровень: {hero.Level} | Опыт: {hero.Score}/{hero.ExpToNextLevel} | Класс: {hero.ClassName} | Шанс побега: {hero.EscapeChance:F1}%");
            Console.Write($"Здоровье: {hero.Hp}/{hero.MaxHp} | Броня: {hero.Armor} | Сила: {hero.Strength}");
            if (hero is Warrior warrior)
                Console.Write($" | Ярость: {warrior.Rage}/100");
            Console.WriteLine();
            Console.WriteLine($"Убито монстров: {totalKills}");
            Console.ResetColor();
            Console.WriteLine("--------------------------------------------------");

            // Информация о монстре
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ВРАГ:  {monster.Name} | Уровень: {monster.Level}");
            Console.WriteLine($"Здоровье: {monster.Health} | Броня: {monster.Armor} | Сила: {monster.Strength}");
            Console.ResetColor();
            Console.WriteLine("==================================================");

            // Подсказки по управлению
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" [Enter] - Атаковать врага  |  [Пробел] - Исцеление (+25 HP)  |  [Esc] - Попытаться сбежать");
            Console.ResetColor();
            Console.WriteLine("==================================================");

            // --- Двуколоночный вывод журнала и статистики ---
            int leftX = 0;
            int rightX = Console.WindowWidth - 30; // отступ справа для статистики
            if (rightX < 40) rightX = 40; // минимальная ширина

            int top = Console.CursorTop; // текущая строка после разделителя

            // Заголовки
            Console.SetCursorPosition(leftX, top);
            Console.Write("ЖУРНАЛ СОБЫТИЙ:");
            Console.SetCursorPosition(rightX, top);
            Console.Write("СТАТИСТИКА УБИЙСТВ:");
            top++;

            // Подготовка строк журнала (последние 10)
            int startIdx = Math.Max(0, combatLog.Count - 10);
            int logLines = combatLog.Count - startIdx;
            int statLines = killStats.Count;
            int maxLines = Math.Max(logLines, statLines);

            for (int i = 0; i < maxLines; i++)
            {
                string logLine = (i < logLines) ? combatLog[startIdx + i] : "";
                string statLine = (i < statLines) ? killStats[i] : "";

                Console.SetCursorPosition(leftX, top + i);
                Console.Write(logLine.PadRight(rightX - leftX)); // заполняем пробелами до правой колонки
                Console.SetCursorPosition(rightX, top + i);
                Console.Write(statLine);
            }

            // Курсор остаётся внизу, следующая итерация цикла очистит экран заново.
        }
    }
}