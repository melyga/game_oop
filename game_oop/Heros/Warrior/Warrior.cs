namespace Game.Heros.Warrior
{
    public class Warrior : Hero
    {
        public Warrior(string name)
            : base(name, hp: 120, power: 10,
                   critDamage: 50, critRate: 10, armor: 8)
        { }

        public override int Attack(IEnemy enemy)
        {
            return enemy.TakeDamage(Power);
        }

        public override int Heal()
        {
            if (_hp > 120)
            {
                _hp = 120;
                return 0;
            }
            else
            {
                _hp += 20;
                return 20;
            }
        }
    }
}