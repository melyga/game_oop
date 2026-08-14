namespace Game
{
    public class Rogue : Hero
    {
        private bool _firstAttack = true;
        private float _firstStrikeMultiplier = 1.5f;
        private int _escapeBonus = 0;

        public Rogue(string name)
            : base(name, hp: 100, strength: 8, agility: 15, score: 0, maxHP: 150,
                   critDamage: 200, critRate: 60, armor: 7)
        { }

        public override int Attack(IEnemy enemy)
        {
            float damage = CalculateDamage();

            if (_firstAttack)
            {
                damage *= _firstStrikeMultiplier;
                _firstAttack = false;
                Battle.AddLog($"{Name} наносит первый удар с множителем {_firstStrikeMultiplier}!");
            }

            int finalDamage = (int)damage;
            bool killed = enemy.TakeDamage(finalDamage);
            if (killed)
            {
                AwardExperience(enemy);
                _firstAttack = true;
            }
            return finalDamage;
        }

        public new double EscapeChance
        {
            get
            {
                double baseChance = base.EscapeChance;
                return Math.Min(baseChance + _escapeBonus, 75.0);
            }
        }

        protected override void LevelUp()
        {
            Battle.AddLog($"Поздравляем! {Name} достиг {Level} уровня!");
            MaxHp += 15;
            Hp = MaxHp;
            Strength += 2;
            Agility += 3;
            CritDamage += 5;

            if (Level % 2 == 0)
            {
                CritRate += 2;
                Battle.AddLog($"Критический шанс увеличен до {CritRate}%.");
            }
            if (Level % 3 == 0)
            {
                _firstStrikeMultiplier += 0.05f;
                Battle.AddLog($"Множитель первого удара теперь {_firstStrikeMultiplier:F2}.");
            }
            if (Level == 5)
            {
                _escapeBonus = 10;
                Battle.AddLog("Изучена Теневая поступь! Шанс побега +10%.");
            }
            if (Level == 10)
            {
                _firstStrikeMultiplier = 2.0f;
                Battle.AddLog("Первый удар теперь наносит 200% урона!");
            }
        }

        public override string ClassName => "Разбойник";
    }
}