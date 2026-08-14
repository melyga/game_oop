using System;

namespace Game
{
    public class Warrior : Hero
    {
        public int Rage { get; private set; } = 0;
        public const int MaxRage = 100;
        private int _damageReduction = 0;

        public Warrior(string name)
            : base(name, hp: 120, strength: 15, agility: 5, score: 0, maxHP: 175,
                   critDamage: 20, critRate: 5, armor: 10)
        { }

        public override int Attack(IEnemy enemy)
        {
            int damage = CalculateDamage();
            int rageCost = Level >= 10 ? 40 : 50;

            if (Rage >= rageCost)
            {
                Rage -= rageCost;
                int powerful = CalculateDamage() * 2;
                enemy.TakeDamage(powerful);
                Battle.AddLog($"{Name} использует Яростный удар! Урон {powerful}.");
                return powerful;
            }

            bool killed = enemy.TakeDamage(damage);
            if (killed) AwardExperience(enemy);
            return damage;
        }

        public override void TakeDamage(IEnemy enemy)
        {
            int reducedDamage = enemy.Strength - (enemy.Strength * _damageReduction / 100);
            int damage = reducedDamage - Armor;
            if (damage <= 0) damage = 1;
            Hp -= damage;
            if (Hp < 0) Hp = 0;
            Battle.AddLog($"{Name} получает {damage} урона (сопротивление {_damageReduction}%).");

            if (Rand.Next(100) < 25)
            {
                int counter = Strength / 2;
                enemy.TakeDamage(counter);
                Battle.AddLog($"{Name} контратакует на {counter} урона!");
            }

            Rage = Math.Min(MaxRage, Rage + enemy.Strength / 2);
        }

        protected override void LevelUp()
        {
            Battle.AddLog($"Поздравляем! {Name} достиг {Level} уровня!");
            MaxHp += 25;
            Hp = MaxHp;
            Strength += 5;
            Armor += 4;

            if (Level % 2 == 0)
            {
                _damageReduction += 1;
                Battle.AddLog($"Сопротивление урону увеличено до {_damageReduction}%.");
            }
            if (Level % 5 == 0)
            {
                Strength += 5;
                Battle.AddLog($"{Name} изучил Боевой клич нового уровня! Сила +5.");
            }
            if (Level == 10)
            {
                Battle.AddLog("Ярость теперь расходуется на 40 вместо 50!");
            }
        }

        public override string ClassName => "Воин";
    }
}