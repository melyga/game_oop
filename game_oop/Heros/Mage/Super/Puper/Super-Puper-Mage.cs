using Game.Monsters;

namespace Game.Heros.Mage.Super.Puper
{
    public class Super_Puper_Mage : Hero
    {
        // Переменная для хранения накопленного бонуса силы от оверхила
        private int _bonusPower = 0;

        public Super_Puper_Mage(string name)
            : base(name, hp: 130, maxHp: 130, power: 32, critDamage: 120, critRate: 60, armor: 6) { }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;
            if (enemy is Monster monster)
            {
                damage = CalculateCrit(monster.Armor) + _bonusPower;
                damage = monster.TakeDamage(damage);

                _bonusPower = 0;

                if (!monster.IsAlive)
                {
                    _bonusPower = 0; 

                    AddExperience(monster.CalculateExpReward(Progress.Level));
                }
            }
            return damage;
        }

        public override int Heal()
        {
            int potentialHp = _hp + HealHP;

            if (potentialHp > MaxHP && _bonusPower != 0)
            {
                // Считаем размер избыточного лечения (оверхил)
                int overflow = potentialHp - MaxHP;
                _hp = MaxHP;

                // Прибавляем весь избыток к бонусному урону
                _bonusPower += overflow;

                return HealHP;
            }

            _hp = potentialHp;
            return HealHP;
        }
        
        // В UI для прикола отображается есть ли прибавок силы
        public override string ClassName => _bonusPower > 0
            ? $"Супер Пупер Маг (+{_bonusPower} Силы)"
            : "Супер Пупер Маг";
    }
}
