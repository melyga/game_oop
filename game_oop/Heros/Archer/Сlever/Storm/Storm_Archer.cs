using Game.Monsters;

namespace Game.Heros.Archer.Сlever.Storm
{
    public class Storm_Archer : Hero
    {
        public Storm_Archer(string name)
            : base(name, hp: 130, maxHp: 130, power: 18, critDamage: 70, critRate: 45, armor: 8) { }

        public override int Attack(IEnemy enemy)
        {
            int totalDamage = 0;
            if (enemy is Monster monster)
            {
                // Случайное число стрел за ход: 1-4
                int arrowCount = rand.Next(1, 5);

                for (int i = 0; i < arrowCount; i++)
                {
                    if (!monster.IsAlive) break;
                    totalDamage += monster.TakeDamage(CalculateCrit());
                }

                if (!monster.IsAlive)
                {
                    AddExperience(monster.CalculateExpReward(Progress.Level));
                }
            }
            return totalDamage;
        }

        public override int Heal()
        {
            _hp = Math.Min(MaxHP, _hp + HealHP);
            return HealHP;
        }

        public override string ClassName => "Штормовой Лучник";
    }
}
