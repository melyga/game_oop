using System;

namespace Game
{
    public class Berserker : Hero
    {
        private float _bonusMultiplierCap = 2.0f; // макс. множитель (от 1 до 2)
        private bool _berserkActive = false;

        public Berserker(string name)
            : base(name, hp: 180, strength: 14, agility: 8, score: 0, maxHP: 180,
                   critDamage: 30, critRate: 15, armor: 5)
        { }

        public override int Attack(IEnemy enemy)
        {
            float healthPercent = (float)Hp / MaxHp;
            float bonus = 1f + (1f - healthPercent) * (_bonusMultiplierCap - 1f);
            if (healthPercent < 0.2f && Level >= 5)
            {
                bonus *= 2f;
                Battle.AddLog($"{Name} впадает в боевой транс! Урон удваивается!");
            }
            int rawDamage = (int)(CalculateDamage() * bonus);
            int finalDamage = CalculateFinalDamage(rawDamage, enemy);
            bool killed = enemy.TakeDamage(finalDamage);
            if (killed) AwardExperience(enemy);
            Battle.AddLog($"{Name} атакует с множителем {bonus:F2}, урон {finalDamage}.");
            return finalDamage;
        }

        protected override void LevelUp()
        {
            Battle.AddLog($"Поздравляем! {Name} достиг {Level} уровня!");
            MaxHp += 25;
            Hp = MaxHp;
            Strength += 4;
            Armor += 1;

            if (Level % 2 == 0)
            {
                _bonusMultiplierCap += 0.1f;
                Battle.AddLog($"Максимальный бонус урона увеличен до {_bonusMultiplierCap:F1}.");
            }
            if (Level % 3 == 0)
            {
                CritDamage += 5;
                Battle.AddLog($"Критический урон увеличен до {CritDamage}%.");
            }
            if (Level == 5)
            {
                Battle.AddLog("Изучен Боевой транс! При HP < 20% урон удваивается.");
            }
            if (Level == 10)
            {
                _bonusMultiplierCap = 2.5f;
                Battle.AddLog("Максимальный бонус урона теперь 2.5!");
            }
        }

        public override string ClassName => "Берсерк";
    }
}