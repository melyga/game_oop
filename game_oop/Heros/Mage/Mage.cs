namespace Game.Heros.Mage
{
    public class Mage : Hero
    {
        public Mage(string name)
            : base(name, hp: 80, power: 18,
                   critDamage: 75, critRate: 40, armor: 2)
        { }

        public override int Attack(IEnemy enemy)
        {
            return enemy.TakeDamage(Power);
        }

        public override int Heal()
        {
            if (_hp > 80)
            {
                _hp = 80;
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