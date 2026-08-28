namespace Game
{
    public interface IEnemy
    {
        string Name { get; }
        int HP { get; }
        int Armor { get; }
        int Power { get; }
        Guid Id { get; }

        bool IsAlive { get; }

        int Attack(IEnemy enemy);

        int TakeDamage(int damage);
    }
}
