namespace Game.Equipment
{
    public class Ring : NeuterEquipment
    {
        public Ring(string baseName, TypeQuality quality, int level, int attack, int defense, int hpRegen = 0, int manaRegen = 0)
            : base(baseName, TypeEquipment.Ring, quality, level, attack, defense, hpRegen, manaRegen)
        {
        }
    }

    public class CopperRing : Ring
    {
        public CopperRing(TypeQuality quality = TypeQuality.Default)
            : base("Медное кольцо", quality, 1, 2, 2, 1, 1) { }
    }

    public class WarriorRing : Ring
    {
        public WarriorRing(TypeQuality quality = TypeQuality.Default)
            : base("Кольцо Ярости", quality, 10, 15, 10, 5, 0) { }
    }

    public class ArcherRing : Ring
    {
        public ArcherRing(TypeQuality quality = TypeQuality.Default)
            : base("Кольцо Точности", quality, 10, 20, 5, 3, 2) { }
    }

    public class MageRing : Ring
    {
        public MageRing(TypeQuality quality = TypeQuality.Default)
            : base("Кольцо Мудрости", quality, 10, 18, 3, 2, 20) { }
    }

    public class DragonSlayerRing : Ring
    {
        public DragonSlayerRing(TypeQuality quality = TypeQuality.Default)
            : base("Кольцо Драконоборца", quality, 25, 50, 40, 30, 30) { }
    }
}