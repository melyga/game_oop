using Game.Monsters;

namespace Game.Heros.Warrior.Super
{
    public class Super_Warrior : Hero
    {
        public Super_Warrior(string name)
            : base(name, hp: 160, maxHp: 160, power: 15, critDamage: 60, critRate: 15, armor: 12) { }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;
            if (enemy is Monster monster)
            {
                // Бонусный урон от потерянного здоровья
                int bonusDamage = (MaxHP - HP) / 4;
                damage = monster.TakeDamage(CalculateCrit() + bonusDamage);

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

        public override string ClassName => "Супер Воин";
    }
}
