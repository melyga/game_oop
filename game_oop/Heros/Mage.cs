using System;

namespace Game
{
    public class Mage : Hero
    {
        private bool _shieldAvailable = true;
        private int _spellPowerBonus = 0;

        public Mage(string name)
            : base(name, hp: 80, strength: 18, agility: 8, score: 0, maxHP: 120,
                   critDamage: 60, critRate: 12, armor: 3)
        { }

        public override int Attack(IEnemy enemy)
        {
            int raw = CalculateDamage() + _spellPowerBonus;
            int final = CalculateFinalDamage(raw, enemy);
            bool killed = enemy.TakeDamage(final);
            if (killed)
            {
                AwardExperience(enemy);
                _shieldAvailable = true; // щит восстанавливается для нового боя
            }
            return final;
        }

        protected override int CalculateFinalDamage(int rawDamage, IEnemy enemy)
        {
            return rawDamage; // полностью игнорируем броню
        }

        public override void TakeDamage(IEnemy enemy)
        {
            int damage = enemy.Strength;
            if (_shieldAvailable)
            {
                damage /= 2;
                _shieldAvailable = false;
                Battle.AddLog($"{Name} активировал Магический щит, урон уменьшен вдвое!");
            }
            damage -= Armor;
            if (damage <= 0) damage = 1;
            Hp -= damage;
            if (Hp < 0) Hp = 0;
            Battle.AddLog($"{Name} получает {damage} урона.");
        }

        protected override void LevelUp()
        {
            Battle.AddLog($"Поздравляем! {Name} достиг {Level} уровня!");
            MaxHp += 10;
            Hp = MaxHp;
            Strength += 4;
            Agility += 1;

            if (Level % 2 == 0)
            {
                CritDamage += 2;
                Battle.AddLog($"Критический урон увеличен до {CritDamage}%.");
            }
            if (Level % 3 == 0)
            {
                _spellPowerBonus += 3;
                Battle.AddLog($"Сила заклинаний увеличена на {_spellPowerBonus} (всего +{_spellPowerBonus}).");
            }
            if (Level == 5)
            {
                _shieldAvailable = true;
                Battle.AddLog("Изучен Магический щит! Один раз в бою уменьшает урон вдвое.");
            }
            if (Level == 10)
            {
                Strength += 5;
                Battle.AddLog("Маг достигает нового уровня могущества! Сила +5.");
            }
        }

        public override string ClassName => "Маг";
    }
}