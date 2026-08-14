namespace Game
{
    public class Battle
    {
        private readonly Hero _hero;
        private IEnemy _currentMonster;
        private static readonly List<string> _combatLog = new List<string>();
        private bool _escapeFailed = false;
        private readonly Random _rand = new Random();

        public event Action<IEnemy> OnEnemyDefeated;

        public IEnemy CurrentMonster => _currentMonster;
        public IReadOnlyList<string> CombatLog => _combatLog;

        public Battle(Hero hero, IEnemy initialMonster)
        {
            _hero = hero;
            _currentMonster = initialMonster;
            AddLog($"Из темноты появляется {_currentMonster.Name}!");
        }

        public void ProcessAttack()
        {
            int damage = _hero.Attack(_currentMonster);
            AddLog($"{_hero.Name} наносит {damage} урона по {_currentMonster.Name}.");

            if (!_currentMonster.IsAlive)
            {
                AddLog($"{_currentMonster.Name} повержен!");
                _escapeFailed = false;
                OnEnemyDefeated?.Invoke(_currentMonster);
            }
            else
            {
                AddLog($"{_currentMonster.Name} атакует в ответ!");
                _hero.TakeDamage(_currentMonster);
            }
        }

        public void ProcessHeal()
        {
            _hero.Heal(10 + (_hero.Level * 5));
            AddLog($"{_hero.Name} восстанавливает {10 + (_hero.Level * 5)} HP.");

            AddLog($"{_currentMonster.Name} воспользовался моментом и атаковал!");
            _hero.TakeDamage(_currentMonster);
        }

        /// <summary>
        /// Попытка побега. Возвращает true, если побег удался.
        /// </summary>
        public bool ProcessEscape()
        {
            if (_escapeFailed)
            {
                AddLog("Путь к отступлению отрезан! Вы не можете сбежать!");
                return false;
            }

            double chance = _hero.EscapeChance;
            AddLog($"Попытка побега (Шанс: {chance:F1}%)...");

            if (_rand.NextDouble() * 100 < chance)
            {
                AddLog("Успех! Вы сбежали от монстра!");
                _escapeFailed = false;
                return true; // побег удался
            }
            else
            {
                AddLog("Провал! Не удалось убежать!");
                _escapeFailed = true;
                AddLog($"{_currentMonster.Name} наносит удар в спину!");
                _hero.TakeDamage(_currentMonster);
                return false;
            }
        }

        /// <summary>Заменяет текущего монстра новым.</summary>
        public void ReplaceMonster(IEnemy newMonster)
        {
            _currentMonster = newMonster;
            AddLog($"Новый враг: {newMonster.Name} [{newMonster.Level} ур.] приближается!");
        }

        public static void AddLog(string message) => _combatLog.Add(message);
    }
}