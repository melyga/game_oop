namespace Game.Monsters
{
    public class Goblin : Monster
    {
        public Goblin(int level) : base("Goblin", level, 25, 8, 5)
        {
        }

        public override int Attack(IEnemy enemy)
        {
            return 0;
        }
    }
}
