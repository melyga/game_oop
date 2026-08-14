namespace Game
{
    /// <summary>
    /// Менеджер для подсчёта убийств по типам монстров.
    /// </summary>
    public class AchievementManager
    {
        private readonly Dictionary<Type, int> _killCount = new();
        private Hero _hero;

        public AchievementManager(Hero hero)
        { 
            _hero = hero;
        }

        public List<string> GetKillStatsLines()
        {
            var lines = new List<string>();
            foreach (var kvp in _killCount)
            {
                if (kvp.Key == typeof(Total)) continue; // пропускаем общий счётчик
                lines.Add($"{kvp.Key.Name}: {kvp.Value}");
            }
            lines.Sort(); // сортировка по имени
            return lines;
        }

        /// <summary>
        /// Увеличивает счётчик для указанного типа монстра и для общего счётчика.
        /// </summary>
        public void OnEnemyDefeated(IEnemy enemy)
        {
            Type type = enemy.GetType();
            if (!_killCount.ContainsKey(type))
                _killCount[type] = 0;
            _killCount[type]++;

            // Общий счётчик (для достижений типа "убить N любых врагов")
            if (!_killCount.ContainsKey(typeof(Total)))
                _killCount[typeof(Total)] = 0;
            _killCount[typeof(Total)]++;

            _hero.CheckAchievements(this);
        }

        /// <summary>
        /// Возвращает количество убитых монстров указанного типа.
        /// </summary>
        public int GetKillCount(Type monsterType)
            => _killCount.GetValueOrDefault(monsterType, 0);

        /// <summary>
        /// Возвращает общее количество убитых монстров.
        /// </summary>
        public int GetTotalKills()
            => _killCount.GetValueOrDefault(typeof(Total), 0);

        // Вспомогательный класс для общего счётчика
        private class Total { }
    }
}