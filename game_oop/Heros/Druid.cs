using System;

namespace Game
{
    public class Druid : Hero
    {
        private bool _isBeastForm = false;
        private float _beastDamageBonus = 0.5f;
        private int _beastArmorBonus = 5;
        private bool _restorationUsed = false;

        public Druid(string name)
            : base(name, hp: 120, strength: 12, agility: 12, score: 0, maxHP: 150,
                   critDamage: 30, critRate: 12, armor: 4)
        { }

        public override int Attack(IEnemy enemy)
        {
            _isBeastForm = true;
            int rawDamage = CalculateDamage();
            rawDamage = (int)(rawDamage * (1 + _beastDamageBonus));
            int finalDamage = CalculateFinalDamage(rawDamage, enemy);
            bool killed = enemy.TakeDamage(finalDamage);
            if (killed)
            {
                AwardExperience(enemy);
                _restorationUsed = false; // сброс для следующего боя
            }

            Battle.AddLog($"{Name} атакует в звериной форме, урон {finalDamage}.");

            // Восстановление при возврате (если доступно и не использовано в этом бою)
            if (!_restorationUsed && Level >= 5)
            {
                Heal((int)(MaxHp * 0.1));
                _restorationUsed = true;
                Battle.AddLog($"{Name} восстанавливает 10% HP после возвращения.");
            }
            _isBeastForm = false;
            return finalDamage;
        }

        public override void TakeDamage(IEnemy enemy)
        {
            int currentArmor = Armor;
            if (_isBeastForm) currentArmor += _beastArmorBonus;
            int damage = enemy.Strength - currentArmor;
            if (damage <= 0) damage = 1;
            Hp -= damage;
            if (Hp < 0) Hp = 0;
            Battle.AddLog($"{Name} получает {damage} урона (броня {currentArmor}).");
        }

        protected override void LevelUp()
        {
            Battle.AddLog($"Поздравляем! {Name} достиг {Level} уровня!");
            MaxHp += 20;
            Hp = MaxHp;
            Strength += 2;
            Agility += 3;
            Armor += 1;

            if (Level % 2 == 0)
            {
                _beastDamageBonus += 0.05f;
                Battle.AddLog($"Бонус урона в звериной форме теперь {_beastDamageBonus * 100}%.");
            }
            if (Level % 3 == 0)
            {
                _beastArmorBonus += 2;
                Battle.AddLog($"Броня в звериной форме увеличена до {_beastArmorBonus}.");
            }
            if (Level == 5)
            {
                _restorationUsed = false;
                Battle.AddLog("Изучено Восстановление! При возврате в человеческую форму +10% HP.");
            }
            if (Level == 10)
            {
                _beastDamageBonus = 0.75f;
                _beastArmorBonus = 10;
                Battle.AddLog("Звериная форма усилена! +75% урона и +10 брони.");
            }
        }

        public override string ClassName => "Друид";
    }
}