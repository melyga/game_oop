using Game.Monsters;

namespace Game.Heros.Archer
{
    public class Super_Archer : Hero
    {
        public Super_Archer(string name)
            : base(name, hp: 120, maxHp: 120, power: 22, critDamage: 85, critRate: 55, armor: 6) { }

        public override int Attack(IEnemy enemy)
        {
            int totalDamage = 0;
            if (enemy is Monster monster)
            {
                // Выстрел 1
                totalDamage += monster.TakeDamage(CalculateCrit());
                // Выстрел 2 (50% мощности)
                if (monster.IsAlive)
                {
                    totalDamage += monster.TakeDamage(CalculateCrit() / 2);
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

        public override string ClassName => "Супер Лучник";
    }
}
