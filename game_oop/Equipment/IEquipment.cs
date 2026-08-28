namespace Game.Equipment
{
    public interface IEquipment
    {
        string Name { get; }
        int BonusPower { get; }
        int BonusArmor { get; }
        int BonusRegenHP { get; }
        int BonusRegenMana { get; }
        string QualityText { get; }
        TypeEquipment TypeEquipment { get; }
        TypeQuality TypeQualities { get; }
        int RequiredLevel { get; }
        EquipmentRarity Rarity { get; }
        bool TryUpgradeQuality();
    }

    public enum EquipmentRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Divine
    }

    public enum TypeEquipment
    {
        Helmet,
        Breastplate,
        Boots,
        Staff,
        Sword,
        Bow,
        Shield,
        Ring
    }

    public enum TypeQuality
    {
        Broken,
        Threadbare,
        Default,
        Qualitative,
        Divine
    }
}
