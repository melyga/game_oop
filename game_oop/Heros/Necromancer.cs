namespace Game
{
    public class Necromancer : Hero
    {
        public int Souls { get; private set; } = 0;
        private int _maxSouls = 5;
        private float _soulUseChance = 0.6f;
        private float _soulDamageMultiplier = 2.0f;

        public Necromancer(string name)
            : base(name, hp: 90, strength: 14, agility: 7, score: 0, maxHP: 130,
                   critDamage: 40, critRate: 10, armor: 2)
        { }

        public override int Attack(IEnemy enemy)
        {
            int rawDamage = CalculateDamage();
            int finalDamage = CalculateFinalDamage(rawDamage, enemy);

            bool useSoul = Souls > 0 && Rand.NextDouble() < _soulUseChance;
            if (useSoul)
            {
                Souls--;
                int bonusDamage = (int)(Strength * _soulDamageMultiplier);
                finalDamage += bonusDamage;
                Battle.AddLog($"{Name} использует душу, добавляя {bonusDamage} магического урона.");
            }

            bool killed = enemy.TakeDamage(finalDamage);
            if (killed)
            {
                AwardExperience(enemy);
                if (Level >= 5)
                {
                    int heal = (int)(MaxHp * 0.2);
                    Heal(heal);
                    Battle.AddLog($"{Name} пожирает душу, восстанавливая {heal} HP.");
                }
                if (Souls < _maxSouls)
                {
                    Souls++;
                    Battle.AddLog($"{Name} получает душу (всего {Souls}).");
                }
            }
            return finalDamage;
        }

        protected override void LevelUp()
        {
            Battle.AddLog($"Поздравляем! {Name} достиг {Level} уровня!");
            MaxHp += 15;
            Hp = MaxHp;
            Strength += 3;
            Agility += 1;

            if (Level % 2 == 0)
            {
                _maxSouls = Math.Min(_maxSouls + 1, 10);
                Battle.AddLog($"Максимум душ теперь {_maxSouls}.");
            }
            if (Level % 3 == 0)
            {
                _soulUseChance = Math.Min(_soulUseChance + 0.05f, 0.9f);
                Battle.AddLog($"Шанс использования души теперь {_soulUseChance * 100}%.");
            }
            if (Level == 5)
            {
                Battle.AddLog("Изучен Пожиратель душ! Убийство врага восстанавливает 20% HP.");
            }
            if (Level == 10)
            {
                _soulDamageMultiplier = 2.5f;
                Battle.AddLog("Урон от душ увеличен: теперь Strength * 2.5.");
            }
        }

        public override string ClassName => "Некромант";
    }
}