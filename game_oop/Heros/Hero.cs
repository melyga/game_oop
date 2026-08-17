namespace Game.Heros;
public abstract class Hero : IEnemy
{
    public string Name { get; private set; }

    public int Armor => _armor;
    private int _armor;

    public int Power => _power;
    private int _power;

    public int HP => _hp;
    protected int _hp;

    protected float _critDamage;
    protected float _critRate;

    public bool IsAlive => HP > 0;

    public LevelProgress Progress { get; private set; } = new LevelProgress();

    public Guid Id => Guid.NewGuid();

    protected Random rand = new Random();

    public Hero(string name, int hp, int armor,
        int power, float critDamage, float critRate)
    { 
        Name = name;
        _hp = hp;
        _armor = armor;
        _power = power;
        _critDamage = critDamage;
        _critRate = critRate;
    }

    public abstract int Attack(IEnemy enemy);

    public abstract int Heal();

    public virtual int TakeDamage(int damage)
    {
        int realDamage = damage - Armor;
        if (realDamage <= 0)
            realDamage = 1;

        _hp -= realDamage;
        return realDamage;
    }
}