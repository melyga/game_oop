namespace Game.Equipment
{
    public class Boots : FeminineEquipment
    {
        public Boots(string baseName, TypeQuality quality, int level, int attack, int defense, int hpRegen = 0, int manaRegen = 0)
            : base(baseName, TypeEquipment.Boots, quality, level, attack, defense, hpRegen, manaRegen)
        {
        }
    }

    public class RecruitBoots : Boots
    {
        public RecruitBoots(TypeQuality quality = TypeQuality.Default)
            : base("Кожаные обувки рекрута", quality, 1, 0, 4, 0, 0) { }
    }

    public class KnightBoots : Boots
    {
        public KnightBoots(TypeQuality quality = TypeQuality.Default)
            : base("Рыцарские сапоги", quality, 10, 4, 18, 3, 0) { }
    }

    public class TitanBoots : Boots
    {
        public TitanBoots(TypeQuality quality = TypeQuality.Default)
            : base("Литые сапоги Титана", quality, 20, 10, 45, 10, 0) { }
    }

    public class HunterBoots : Boots
    {
        public HunterBoots(TypeQuality quality = TypeQuality.Default)
            : base("Охотничьи сапоги", quality, 1, 2, 3, 0, 0) { }
    }

    public class RangerBoots : Boots
    {
        public RangerBoots(TypeQuality quality = TypeQuality.Default)
            : base("Бесшумные сапоги следопыта", quality, 10, 10, 12, 3, 0) { }
    }

    public class StormBoots : Boots
    {
        public StormBoots(TypeQuality quality = TypeQuality.Default)
            : base("Сапоги Штормового Ветра", quality, 20, 25, 28, 7, 0) { }
    }

    public class ApprenticeBoots : Boots
    {
        public ApprenticeBoots(TypeQuality quality = TypeQuality.Default)
            : base("Сандалии ученика", quality, 1, 2, 2, 1, 3) { }
    }

    public class SorcererBoots : Boots
    {
        public SorcererBoots(TypeQuality quality = TypeQuality.Default)
            : base("Сапоги чародея", quality, 10, 12, 8, 2, 10) { }
    }

    public class LichBoots : Boots
    {
        public LichBoots(TypeQuality quality = TypeQuality.Default)
            : base("Поступь Кровавого Лича", quality, 20, 28, 18, 6, 20) { }
    }

    public class DragonSlayerBoots : Boots
    {
        public DragonSlayerBoots(TypeQuality quality = TypeQuality.Default)
            : base("Сапоги Драконоборца", quality, 25, 35, 75, 20, 15) { }
    }
}