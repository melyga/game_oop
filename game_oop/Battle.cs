using Game.Heros;
using Game.Monsters;

namespace Game
{
    public class Battle
    {
        private readonly Hero _hero;
        private IEnemy _currentMonster;

        private static readonly List<string> _combatLog = new List<string>();

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
                OnEnemyDefeated?.Invoke(_currentMonster);
            }
            else
            {
                AddLog($"{_currentMonster.Name} атакует в ответ!");
                _currentMonster.Attack(_hero);
            }
        }

        public void ReplaceMonster(IEnemy newMonster)
        {
            if (newMonster is Monster monster)
            {
                _currentMonster = newMonster;
                AddLog($"Новый враг: {newMonster.Name} [{monster.Level} ур.] приближается!");
            }
        }

        public static void AddLog(string message) => _combatLog.Add(message);
    }
}
