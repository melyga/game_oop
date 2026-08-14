using System;

namespace Game
{
    public class Archer : Hero
    {
        private float _armorPenetration = 0.5f;
        private bool _firstShot = true;
        private int _bonusDamageFirstShot = 0; // дополнительный бонус на 5 уровне

        public Archer(string name)
            : base(name, hp: 100, strength: 18, agility: 15, score: 0, maxHP: 150,
                   critDamage: 70, critRate: 20, armor: 5) 
        { }

        public override int Attack(IEnemy enemy)
        {
            int raw = CalculateDamage();

            if (_firstShot)
            {
                // Бонус первого выстрела применяется сразу, а не накапливается
                raw += (int)(raw * 0.5f);
                raw += _bonusDamageFirstShot;  // дополнительный бонус с 5 уровня
                _firstShot = false;
                Battle.AddLog($"{Name} делает меткий выстрел! +50% урона и бонус {_bonusDamageFirstShot}.");
            }

            int final = CalculateFinalDamage(raw, enemy);
            bool killed = enemy.TakeDamage(final);
            if (killed)
            {
                AwardExperience(enemy);
                _firstShot = true; // сбрасываем для следующего боя
            }
            return final;
        }

        protected override int CalculateFinalDamage(int rawDamage, IEnemy enemy)
        {
            int effectiveArmor = (int)(enemy.Armor * (1 - _armorPenetration));
            int final = rawDamage - effectiveArmor;
            return final < 0 ? 0 : final;
        }

        protected override void LevelUp()
        {
            Battle.AddLog($"Поздравляем! {Name} достиг {Level} уровня!");
            MaxHp += 20;
            Hp = MaxHp;
            Strength += 3;
            Agility += 2;
            CritRate += 1;

            if (Level % 2 == 0)
            {
                _armorPenetration = Math.Min(_armorPenetration + 0.02f, 1.0f);
                Battle.AddLog($"Игнорирование брони увеличено до {_armorPenetration * 100}%.");
            }
            if (Level % 3 == 0)
            {
                CritDamage += 5;
                Battle.AddLog($"Критический урон увеличен до {CritDamage}%.");
            }
            // Особое умение на 5 уровне
            if (Level == 5)
            {
                _bonusDamageFirstShot = 20;
                Battle.AddLog("Изучен Меткий выстрел! Первый выстрел +20 урона.");
            }
            if (Level == 10)
            {
                Strength += 5;
                Battle.AddLog("Дальность увеличена! Сила +5.");
            }
        }

        public override string ClassName => "Лучник";
    }
}