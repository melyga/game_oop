namespace Game.Equipment
{
    public class Shield : BaseEquipment
    {
        public Shield(string baseName, TypeQuality quality, int level, int attack, int defense, int hpRegen = 0, int manaRegen = 0)
            : base(baseName, TypeEquipment.Shield, quality, level, attack, defense, hpRegen, manaRegen)
        {
        }
    }

    public class WoodenShield : Shield
    {
        public WoodenShield(TypeQuality quality = TypeQuality.Default)
            : base("Деревянный щит", quality, 1, 0, 6, 0, 0) { }
    }

    public class KnightShield : Shield
    {
        public KnightShield(TypeQuality quality = TypeQuality.Default)
            : base("Геральдический щит рыцаря", quality, 10, 3, 30, 5, 0) { }
    }

    public class TitanShield : Shield
    {
        public TitanShield(TypeQuality quality = TypeQuality.Default)
            : base("Несокрушимый оплот Титана", quality, 20, 10, 80, 15, 0) { }
    }

    public class DragonSlayerShield : Shield
    {
        public DragonSlayerShield(TypeQuality quality = TypeQuality.Default)
            : base("Щит из Чешуи Древнего Дракона", quality, 25, 25, 140, 35, 10) { }
    }
}