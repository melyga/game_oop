namespace Game.Equipment
{
    public class Bow : BaseEquipment
    {
        public Bow(string baseName, TypeQuality quality, int level, int attack, int defense = 0, int hpRegen = 0, int manaRegen = 0)
            : base(baseName, TypeEquipment.Bow, quality, level, attack, defense, hpRegen, manaRegen)
        {
        }
    }

    public class HuntingBow : Bow
    {
        public HuntingBow(TypeQuality quality = TypeQuality.Default)
            : base("Простой охотничий лук", quality, 1, 14, 0, 0, 0) { }
    }

    public class RangerBow : Bow
    {
        public RangerBow(TypeQuality quality = TypeQuality.Default)
            : base("Составной лук следопыта", quality, 10, 52, 2, 2, 0) { }
    }

    public class StormBow : Bow
    {
        public StormBow(TypeQuality quality = TypeQuality.Default)
            : base("Лук Штормового Ветра", quality, 20, 125, 5, 8, 0) { }
    }

    public class DragonSlayerBow : Bow
    {
        public DragonSlayerBow(TypeQuality quality = TypeQuality.Default)
            : base("Большой лук Драконоборца", quality, 25, 245, 20, 20, 0) { }
    }
}