using Game.Monsters;

namespace Game.Heros.Mage
{
    public class Mage : Hero
    {
        public Mage(string name)
            : base(name, hp: 80, maxHp: 80, power: 18,
                   critDamage: 75, critRate: 40, armor: 2)
        { }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;

            if (enemy is Monster monster)
            {
                damage = monster.TakeDamage(CalculateCrit(monster.Armor)); // Отправляет броню чтобы ее компенсировать в уроне так как маг игнорирует всю броню
                if (!monster.IsAlive)
                {
                    AddExperience(monster.CalculateExpReward(Progress.Level));
                }
            }

            return damage;
        }

        public override int Heal()
        {
            _hp += HealHP;

            if (_hp > MaxHP)
            {
                _hp = MaxHP;
            }

            return HealHP;
        }

        public override string ClassName => "Маг";
    }
}