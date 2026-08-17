using Game.Monsters;

namespace Game.Heros.Mage.Super
{
    public class Super_Mage : Hero
    {
        public Super_Mage(string name)
            : base(name, hp: 100, maxHp: 100, power: 24, critDamage: 90, critRate: 50, armor: 4) { }

        public override int Attack(IEnemy enemy)
        {
            int totalDamage = 0;
            if (enemy is Monster monster)
            {
                // Первый выстрел (игнорирует броню)
                totalDamage += monster.TakeDamage(CalculateCrit(monster.Armor));

                // 30% шанс повторной бесплатной атаки
                if (monster.IsAlive && rand.NextDouble() < 0.30)
                {
                    totalDamage += monster.TakeDamage(CalculateCrit(monster.Armor) / 3);
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

        public override string ClassName => "Супер Маг";
    }
}
