namespace Game.Heros
{
    public abstract class Hero : IEnemy
    {
        public string Name { get; private set; }

        public int Armor => _armor;
        private int _armor;

        public int Power => _power;
        private int _power;

        public int HP => _hp;
        protected int _hp;

        public int MaxHP { get; private set; }
        public int HealHP { get; private set; } = 20;

        protected float _critDamage;
        protected float _critRate;

        public bool IsAlive => HP > 0;

        public int Score { get; private set; } = 1;

        public LevelProgress Progress { get; private set; } = new LevelProgress();

        public Guid Id => Guid.NewGuid();

        protected Random rand = new Random();

        private const int HpStep = 20;
        private const int HealHPStep = 10;
        private const int PowerStep = 3;
        private const int ArmorStep = 2;
        private const float CritDamageStep = 5f;
        private const float CritRateStep = 1f;

        public Hero(string name, int hp, int maxHp, int armor,
            int power, float critDamage, float critRate)
        {
            Name = name;
            _hp = hp;
            MaxHP = maxHp;
            _armor = armor;
            _power = power;
            _critDamage = critDamage;
            _critRate = critRate;
        }

        public abstract int Attack(IEnemy enemy);

        public abstract int Heal();

        public virtual int TakeDamage(int damage)
        {
            int realDamage = damage - Armor;
            if (realDamage <= 0)
                realDamage = 1;

            _hp -= realDamage;
            return realDamage;
        }

        protected int CalculateCrit(int armor = 0)
        {
            if (rand.NextDouble() * 100 < _critRate)
                return (int)(Power * (_critDamage / 100f)) + armor;
            return Power + armor;
        }

        public void AddExperience(int exp)
        {
            int levelsGained = Progress.AddExp(exp);
            if (levelsGained > 0)
            {
                Score += levelsGained * 3;
            }
        }

        public bool TryUpgradeStat(StatType stat)
        {
            if (Score <= 0) return false;

            switch (stat)
            {
                case StatType.Power:
                    _power += PowerStep;
                    break;
                case StatType.Armor:
                    _armor += ArmorStep;
                    break;
                case StatType.MaxHp:
                    MaxHP += HpStep;
                    _hp += HpStep;
                    break;
                case StatType.CritDamage:
                    _critDamage += CritDamageStep;
                    break;
                case StatType.CritRate:
                    _critRate = Math.Min(100f, _critRate + CritRateStep);
                    break;
                case StatType.HealHP:
                    HealHP += HealHPStep;
                    break;
                default:
                    return false;
            }

            Score--;
            return true;
        }
        public void TransferProgressFrom(Hero oldHero)
        {
            this.Progress = oldHero.Progress;

            this.Score = oldHero.Progress.Level * 3;

            this._hp = this.MaxHP;
        }

        public abstract string ClassName { get; }
    }

    public enum StatType
    {
        Power,
        Armor,
        MaxHp,
        CritDamage,
        CritRate,
        HealHP
    }
}