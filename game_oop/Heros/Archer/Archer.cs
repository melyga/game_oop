namespace Game.Heros.Archer
{
    public class Archer : Hero
    {
        public Archer(string name)
            : base(name, hp: 100, power: 18,
                   critDamage: 70, critRate: 50, armor: 5) 
        { }

        public override int Attack(IEnemy enemy)
        {
            return enemy.TakeDamage(Power);
        }

        public override int Heal()
        {
            if (_hp > 100)
            {
                _hp = 100;
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