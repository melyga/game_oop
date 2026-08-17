using Game.Heros;
using Game.Monsters;

namespace Game
{
    public class Battle
    {
        private readonly Hero _hero;
        private IEnemy _currentMonster;

        public event Action<IEnemy> OnEnemyDefeated;

        public Battle(Hero hero, IEnemy initialMonster)
        {
            _hero = hero;
            _currentMonster = initialMonster;
            Console.WriteLine($"Из темноты появляется {_currentMonster.Name}!");
        }

        public void ProcessAttack()
        {
            int damage = _hero.Attack(_currentMonster);
            Console.WriteLine($"{_hero.Name} наносит {damage} урона по {_currentMonster.Name}.");

            if (!_currentMonster.IsAlive)
            {
                Console.WriteLine($"{_currentMonster.Name} повержен!");
                OnEnemyDefeated?.Invoke(_currentMonster);
            }
            else
            {
                Console.WriteLine($"{_currentMonster.Name} атакует в ответ!");
                _hero.TakeDamage(_currentMonster.Power);
            }
        }

        public void ReplaceMonster(IEnemy newMonster)
        {
            if (newMonster is Monster monster)
            {
                _currentMonster = newMonster;
                Console.WriteLine($"Новый враг: {newMonster.Name} [{monster.Level} ур.] приближается!");
            }
        }
    }
}
