using Game.Monsters;

namespace Game.Heros.Mage.Dark.Blood_Lich
{
    public class Blood_Lich_Mage : Hero
    {
        public Blood_Lich_Mage(string name)
            : base(name, hp: 110, maxHp: 110, power: 35, critDamage: 120, critRate: 100, armor: 5) { }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;
            if (enemy is Monster monster)
            {
                int hpCost = Math.Max(1, (int)(_hp * 0.10f));
                _hp -= hpCost;

                damage = monster.TakeDamage(CalculateCrit(monster.Armor));

                // Крадет 20% от нанесенного урона
                int lifeSteal = (int)(damage * 0.20f);
                _hp = Math.Min(MaxHP, _hp + lifeSteal);

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

        public override string ClassName => "Кровавый Лич";
    }
}
