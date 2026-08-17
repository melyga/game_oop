using Game.Monsters;

namespace Game.Heros.Archer
{
    public class Archer : Hero
    {
        public Archer(string name)
            : base(name, hp: 100, maxHp: 100, power: 18,
                   critDamage: 70, critRate: 50, armor: 5) 
        { }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;

            if (enemy is Monster monster)
            {
                damage = monster.TakeDamage(CalculateCrit());
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

        public override string ClassName => "Лучник";
    }
}