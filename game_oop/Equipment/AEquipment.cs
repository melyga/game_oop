namespace Game.Equipment
{
    public abstract class BaseEquipment : IEquipment
    {
        protected readonly int BaseAttack;
        protected readonly int BaseDefense;
        protected readonly int BaseRegenHP;
        protected readonly int BaseRegenMana;
        private readonly string _baseName;

        public string Name => $"{QualityText} {_baseName}";
        public TypeEquipment TypeEquipment { get; private set; }
        public TypeQuality TypeQualities { get; private set; }
        public int RequiredLevel { get; private set; }
        public EquipmentRarity Rarity { get; private set; }

        protected BaseEquipment(string baseName, TypeEquipment type, TypeQuality quality, int level, int attack, int defense, int hpRegen, int manaRegen)
        {
            _baseName = baseName;
            TypeEquipment = type;
            TypeQualities = quality;
            RequiredLevel = level;
            BaseAttack = attack;
            BaseDefense = defense;
            BaseRegenHP = hpRegen;
            BaseRegenMana = manaRegen;
            Rarity = CalculateRarity(level);
        }
        private static EquipmentRarity CalculateRarity(int level) => level switch
        {
            <= 1 => EquipmentRarity.Common,
            <= 5 => EquipmentRarity.Uncommon,
            <= 10 => EquipmentRarity.Rare,
            <= 20 => EquipmentRarity.Epic,
            <= 25 => EquipmentRarity.Legendary,
            _ => EquipmentRarity.Divine
        };

        public int BonusPower => (int)(BaseAttack * GetQualityMultiplier(TypeQualities));
        public int BonusArmor => (int)(BaseDefense * GetQualityMultiplier(TypeQualities));


        public int BonusRegenHP => (int)(BaseRegenHP * GetQualityMultiplier(TypeQualities));
        public int BonusRegenMana => (int)(BaseRegenMana * GetQualityMultiplier(TypeQualities));

        private static double GetQualityMultiplier(TypeQuality quality) => quality switch
        {
            TypeQuality.Broken => 0.1,
            TypeQuality.Threadbare => 0.7,
            TypeQuality.Default => 1.0,
            TypeQuality.Qualitative => 1.4,
            TypeQuality.Divine => 2.0,
            _ => 1.0
        };

        public virtual string QualityText => TypeQualities switch
        {
            TypeQuality.Broken => "Сломанный",
            TypeQuality.Threadbare => "Изношенный",
            TypeQuality.Default => "Обычный",
            TypeQuality.Qualitative => "Качественный",
            TypeQuality.Divine => "Божественный",
            _ => "Неизвестный"
        };

        public bool TryUpgradeQuality()
        {
            if (TypeQualities == TypeQuality.Divine)
                return false;

            TypeQualities = TypeQualities + 1;
            return true;
        }
    }

    // Для предметов женского рода
    public abstract class FeminineEquipment : BaseEquipment
    {
        protected FeminineEquipment(string baseName, TypeEquipment type, TypeQuality quality, int level, int attack, int defense, int hpRegen, int manaRegen)
            : base(baseName, type, quality, level, attack, defense, hpRegen, manaRegen) { }

        public override string QualityText => TypeQualities switch
        {
            TypeQuality.Broken => "Сломанная",
            TypeQuality.Threadbare => "Изношенная",
            TypeQuality.Default => "Обычная",
            TypeQuality.Qualitative => "Качественная",
            TypeQuality.Divine => "Божественная",
            _ => "Неизвестная"
        };
    }

    // Для предметов среднего рода
    public abstract class NeuterEquipment : BaseEquipment
    {
        protected NeuterEquipment(string baseName, TypeEquipment type, TypeQuality quality, int level, int attack, int defense, int hpRegen, int manaRegen)
            : base(baseName, type, quality, level, attack, defense, hpRegen, manaRegen) { }

        public override string QualityText => TypeQualities switch
        {
            TypeQuality.Broken => "Сломанное",
            TypeQuality.Threadbare => "Изношенное",
            TypeQuality.Default => "Обычное",
            TypeQuality.Qualitative => "Качественное",
            TypeQuality.Divine => "Божественное",
            _ => "Неизвестное"
        };
    }
}
