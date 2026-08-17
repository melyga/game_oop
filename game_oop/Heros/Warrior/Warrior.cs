using Game.Monsters;

namespace Game.Heros.Warrior
{
    public class Warrior : Hero
    {
        public Warrior(string name)
            : base(name, hp: 120, maxHp: 120, power: 10,
                   critDamage: 50, critRate: 10, armor: 8)
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

        public override string ClassName => "Воин";
    }
}