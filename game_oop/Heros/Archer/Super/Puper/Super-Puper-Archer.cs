using Game.Equipment;
using Game.Monsters;

namespace Game.Heros.Archer.Super.Puper
{
    /// <summary>
    /// Не используется
    /// </summary>
/*    public class Super_Puper_Archer : Hero
    {
        public Super_Puper_Archer(string name)
            : base(name, hp: 140, maxHp: 140, power: 28, critDamage: 100, critRate: 65, armor: 8) { }

        public override int TakeDamage(int damage)
        {
            // 30% шанс полностью уклониться от атаки
            if (rand.NextDouble() < 0.30)
            {
                return 0;
            }
            return base.TakeDamage(damage);
        }

        public override int Attack(IEnemy enemy)
        {
            int totalDamage = 0;
            if (enemy is Monster monster)
            {
                totalDamage += monster.TakeDamage(CalculateCrit());
                if (monster.IsAlive)
                {
                    totalDamage += monster.TakeDamage((int)(CalculateCrit() * 0.7f));
                }

                if (!monster.IsAlive)
                {
                    AddExperience(monster.CalculateExpReward(Progress.Level));
                }
            }
            return totalDamage;
        }

        protected override HashSet<TypeEquipment> AllowedEquipment => new HashSet<TypeEquipment>
        {
            TypeEquipment.Helmet,
            TypeEquipment.Breastplate,
            TypeEquipment.Boots,
            TypeEquipment.Sword,
            TypeEquipment.Shield,
            TypeEquipment.Ring,
        };

        public override int Heal()
        {
            _hp = Math.Min(MaxHP, _hp + HealHP);
            return HealHP;
        }

        public override string ClassName => "Супер Пупер Лучник";
    }*/
}
