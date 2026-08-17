using Game.Monsters;

namespace Game.Heros.Mage.Dark
{
    public class Dark_Mage : Hero
    {
        public Dark_Mage(string name)
            : base(name, hp: 90, maxHp: 90, power: 28, critDamage: 100, critRate: 100, armor: 3) { }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;
            if (enemy is Monster monster)
            {
                // Тратит 5% от текущего HP на заклинение
                int hpCost = Math.Max(1, (int)(_hp * 0.05f));
                _hp -= hpCost;

                damage = monster.TakeDamage(CalculateCrit(monster.Armor));
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

        public override string ClassName => "Тёмный Маг";
    }
}
