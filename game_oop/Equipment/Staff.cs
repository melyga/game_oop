namespace Game.Equipment
{
    public class Staff : BaseEquipment
    {
        public Staff(string baseName, TypeQuality quality, int level, int attack, int defense = 0, int hpRegen = 0, int manaRegen = 0)
            : base(baseName, TypeEquipment.Staff, quality, level, attack, defense, hpRegen, manaRegen)
        {
        }
    }

    public class ApprenticeStaff : Staff
    {
        public ApprenticeStaff(TypeQuality quality = TypeQuality.Default)
            : base("Деревянный посох ученика", quality, 1, 16, 0, 0, 5) { }
    }

    public class ElementalStaff : Staff
    {
        public ElementalStaff(TypeQuality quality = TypeQuality.Default)
            : base("Посох Стихий", quality, 10, 60, 3, 3, 15) { }
    }

    public class AbyssalStaff : Staff
    {
        public AbyssalStaff(TypeQuality quality = TypeQuality.Default)
            : base("Посох Бездны и Крови", quality, 20, 140, 8, 12, 35) { }
    }

    public class DragonSlayerStaff : Staff
    {
        public DragonSlayerStaff(TypeQuality quality = TypeQuality.Default)
            : base("Посох Драконьего Повелителя", quality, 25, 260, 25, 30, 60) { }
    }
}
