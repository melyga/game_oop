namespace Game.Equipment
{
    public class Breastplate : FeminineEquipment
    {
        public Breastplate(string baseName, TypeQuality quality, int level, int attack, int defense, int hpRegen = 0, int manaRegen = 0)
            : base(baseName, TypeEquipment.Breastplate, quality, level, attack, defense, hpRegen, manaRegen)
        {
        }
    }

    public class IronBreastplate : Breastplate
    {
        public IronBreastplate(TypeQuality quality = TypeQuality.Default)
            : base("Железная кираса", quality, 1, 2, 10, 0, 0) { }
    }

    public class KnightBreastplate : Breastplate
    {
        public KnightBreastplate(TypeQuality quality = TypeQuality.Default)
            : base("Стальная рыцарская кираса", quality, 10, 8, 40, 6, 0) { }
    }

    public class TitanBreastplate : Breastplate
    {
        public TitanBreastplate(TypeQuality quality = TypeQuality.Default)
            : base("Тяжелая кираса Титана", quality, 20, 20, 100, 20, 0) { }
    }

    public class HunterJacket : Breastplate
    {
        public HunterJacket(TypeQuality quality = TypeQuality.Default)
            : base("Кожаная куртка", quality, 1, 4, 6, 0, 0) { }
    }

    public class RangerArmor : Breastplate
    {
        public RangerArmor(TypeQuality quality = TypeQuality.Default)
            : base("Усиленная броня следопыта", quality, 10, 18, 28, 5, 0) { }
    }

    public class StormArmor : Breastplate
    {
        public StormArmor(TypeQuality quality = TypeQuality.Default)
            : base("Легкая броня Шторма", quality, 20, 42, 65, 14, 0) { }
    }

    public class ApprenticeRobe : Breastplate
    {
        public ApprenticeRobe(TypeQuality quality = TypeQuality.Default)
            : base("Тканевая мантия ученика", quality, 1, 5, 4, 2, 8) { }
    }

    public class SorcererRobe : Breastplate
    {
        public SorcererRobe(TypeQuality quality = TypeQuality.Default)
            : base("Шелковая мантия чародея", quality, 10, 25, 18, 5, 20) { }
    }

    public class BloodLichRobe : Breastplate
    {
        public BloodLichRobe(TypeQuality quality = TypeQuality.Default)
            : base("Кровавая мантия Лича", quality, 20, 50, 40, 12, 40) { }
    }

    public class DragonSlayerBreastplate : Breastplate
    {
        public DragonSlayerBreastplate(TypeQuality quality = TypeQuality.Default)
            : base("Чешуйчатая кираса Драконоборца", quality, 25, 60, 160, 40, 30) { }
    }
}