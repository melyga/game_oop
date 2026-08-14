namespace Game
{
    /// <summary>Гоблин – слабый враг, доступен с 1 уровня.</summary>
    public class Goblin : Monster
    {
        public Goblin(int level = 1) : base("Гоблин", 30, 2, 5, level) { }
    }

    /// <summary>Голем – среднебронированный враг, доступен с 3 уровня.</summary>
    public class Mech_Golem : Monster
    {
        public Mech_Golem(int level = 1) : base("Голем", 50, 10, 8, level) { }
    }

    /// <summary>Орк – сильный враг, появляется с 5 уровня героя.</summary>
    public class Orc : Monster
    {
        public Orc(int level) : base("Орк", 60, 6, 14, level) { }
    }

    /// <summary>Тролль – живучий враг, появляется с 8 уровня героя.</summary>
    public class Troll : Monster
    {
        public Troll(int level) : base("Тролль", 90, 8, 20, level) { }
    }

    /// <summary>Дракон – элитный враг, появляется с 10 уровня героя.</summary>
    public class Dragon : Monster
    {
        public Dragon(int level) : base("Дракон", 150, 15, 30, level) { }
    }
}