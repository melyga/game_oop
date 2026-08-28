namespace Game.Equipment
{
    public class Helmet : BaseEquipment
    {
        public Helmet(string baseName, TypeQuality quality, int level, int attack, int defense, int hpRegen = 0, int manaRegen = 0)
            : base(baseName, TypeEquipment.Helmet, quality, level, attack, defense, hpRegen, manaRegen)
        {
        }
    }

    public class RecruitHelmet : Helmet
    {
        public RecruitHelmet(TypeQuality quality = TypeQuality.Default)
            : base("Шлем рекрута", quality, 1, 1, 5, 0, 0) { }
    }

    public class KnightHelmet : Helmet
    {
        public KnightHelmet(TypeQuality quality = TypeQuality.Default)
            : base("Рыцарский шлем", quality, 10, 5, 22, 4, 0) { }
    }

    public class TitanHelmet : Helmet
    {
        public TitanHelmet(TypeQuality quality = TypeQuality.Default)
            : base("Шлем Титана", quality, 20, 12, 55, 12, 0) { }
    }

    public class HunterHood : Helmet
    {
        public HunterHood(TypeQuality quality = TypeQuality.Default)
            : base("Кожаный капюшон", quality, 1, 3, 3, 0, 0) { }
    }

    public class RangerHood : Helmet
    {
        public RangerHood(TypeQuality quality = TypeQuality.Default)
            : base("Капюшон следопыта", quality, 10, 12, 14, 3, 0) { }
    }

    public class StormHood : Helmet
    {
        public StormHood(TypeQuality quality = TypeQuality.Default)
            : base("Капюшон Бури", quality, 20, 28, 34, 8, 0) { }
    }

    public class ApprenticeHood : Helmet
    {
        public ApprenticeHood(TypeQuality quality = TypeQuality.Default)
            : base("Капюшон ученика", quality, 1, 4, 2, 1, 4) { }
    }

    public class MageCrown : Helmet
    {
        public MageCrown(TypeQuality quality = TypeQuality.Default)
            : base("Корона Чародея", quality, 10, 18, 10, 3, 12) { }
    }

    public class LichHood : Helmet
    {
        public LichHood(TypeQuality quality = TypeQuality.Default)
            : base("Капюшон Кровавого Лича", quality, 20, 35, 22, 8, 25) { }
    }

    public class DragonSlayerHelmet : Helmet
    {
        public DragonSlayerHelmet(TypeQuality quality = TypeQuality.Default)
            : base("Шлем Драконоборца из Драконьей Чешуи", quality, 25, 45, 95, 25, 20) { }
    }
}