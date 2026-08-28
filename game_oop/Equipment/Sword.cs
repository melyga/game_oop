namespace Game.Equipment
{
    public class Sword : BaseEquipment
    {
        public Sword(string baseName, TypeQuality quality, int level, int attack, int defense = 0, int hpRegen = 0, int manaRegen = 0)
            : base(baseName, TypeEquipment.Sword, quality, level, attack, defense, hpRegen, manaRegen)
        {
        }
    }

    public class TrainingSword : Sword
    {
        public TrainingSword(TypeQuality quality = TypeQuality.Default)
            : base("Ржавый меч", quality, 1, 12, 1, 0, 0) { }
    }

    public class KnightSword : Sword
    {
        public KnightSword(TypeQuality quality = TypeQuality.Default)
            : base("Рыцарский стальной меч", quality, 10, 45, 5, 3, 0) { }
    }

    public class TitanSword : Sword
    {
        public TitanSword(TypeQuality quality = TypeQuality.Default)
            : base("Двуручный меч Титана", quality, 20, 110, 15, 10, 0) { }
    }

    public class DragonSlayerSword : Sword
    {
        public DragonSlayerSword(TypeQuality quality = TypeQuality.Default)
            : base("Великий клинок Драконоборца", quality, 25, 220, 35, 25, 0) { }
    }
}