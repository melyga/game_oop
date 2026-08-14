namespace Game
{
    /// <summary>
    /// Интерфейс, описывающий любого врага (монстра).
    /// </summary>
    public interface IEnemy
    {
        string Name { get; }
        int Health { get; }
        int Armor { get; }
        int Strength { get; }          // сила врага
        int Level { get; }             // уровень врага
        bool IsAlive { get; }
        int ExpReward { get; }         // опыт, получаемый за убийство

        /// <summary>
        /// Наносит урон врагу. Возвращает true, если враг убит.
        /// </summary>
        bool TakeDamage(int damage);
    }
}