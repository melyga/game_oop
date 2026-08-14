namespace Game
{
    public class Paladin : Hero
    {
        private float _lifeSteal = 0.25f;
        private bool _holyLightUsed = false;

        public Paladin(string name)
            : base(name, hp: 150, strength: 12, agility: 6, score: 0, maxHP: 200,
                   critDamage: 25, critRate: 10, armor: 8)
        { }

        public override int Attack(IEnemy enemy)
        {
            int rawDamage = CalculateDamage();
            int finalDamage = CalculateFinalDamage(rawDamage, enemy);

            int heal = (int)(finalDamage * _lifeSteal);
            if (heal > 0) Heal(heal);

            if (!_holyLightUsed && Hp < MaxHp * 0.3)
            {
                Heal((int)(MaxHp * 0.3));
                _holyLightUsed = true;
                Battle.AddLog($"{Name} использует Священный свет!");
            }

            bool killed = enemy.TakeDamage(finalDamage);
            if (killed)
            {
                AwardExperience(enemy);
                _holyLightUsed = false; // сброс для нового боя
            }
            return finalDamage;
        }

        protected override void LevelUp()
        {
            Battle.AddLog($"Поздравляем! {Name} достиг {Level} уровня!");
            MaxHp += 20;
            Hp = MaxHp;
            Strength += 3;
            Armor += 2;

            if (Level % 2 == 0)
            {
                Armor += 1;
                Battle.AddLog($"Броня увеличена до {Armor}.");
            }
            if (Level % 3 == 0)
            {
                MaxHp += 5;
                Battle.AddLog($"Макс. здоровье увеличено до {MaxHp}.");
            }
            _lifeSteal = Math.Min(_lifeSteal + 0.02f, 0.5f);
            Battle.AddLog($"Вампиризм теперь {_lifeSteal * 100}%.");

            if (Level == 5)
            {
                _holyLightUsed = false;
                Battle.AddLog("Изучен Священный свет! Один раз за бой восстанавливает 30% HP.");
            }
            if (Level == 10)
            {
                _lifeSteal = Math.Min(_lifeSteal + 0.05f, 0.5f);
                Battle.AddLog($"Вампиризм усилен до {_lifeSteal * 100}%.");
            }
        }

        public override string ClassName => "Паладин";
    }
}