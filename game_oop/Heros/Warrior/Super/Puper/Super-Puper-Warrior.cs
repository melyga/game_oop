using Game.Equipment;
using Game.Monsters;

namespace Game.Heros.Warrior.Super.Puper
{
    /// <summary>
    /// Не используется
    /// </summary>
   /* public class Super_Puper_Warrior : Hero
    {
        public Super_Puper_Warrior(string name)
            : base(name, hp: 220, maxHp: 220, power: 22, critDamage: 75, critRate: 20, armor: 18) { }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;
            if (enemy is Monster monster)
            {
                // Увеличенный бонус от потерянного HP
                int bonusDamage = (MaxHP - HP) / 2;
                damage = monster.TakeDamage(CalculateCrit() + bonusDamage);

                // Вампиризм: исцеляет 20% от нанесенного урона
                int vampHeal = (int)(damage * 0.20f);
                _hp = Math.Min(MaxHP, _hp + vampHeal);

                if (!monster.IsAlive)
                {
                    AddExperience(monster.CalculateExpReward(Progress.Level));
                }
            }
            return damage;
        }

        public override int Heal()
        {
            _hp = Math.Min(MaxHP, _hp + HealHP);
            return HealHP;
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

        public override string ClassName => "Супер Пупер Воин";
    }*/
}
