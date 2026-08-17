namespace Game
{
    public class LevelProgress
    {
        public int Level { get; private set; } = 1;

        public int Exp { get; private set; } = 0;

        public int ExpToNextLevel => (int)(50 * Math.Pow(Level, 1.5));

        public bool AddExp(int amount)
        {
            Exp += amount;

            bool leveledUp = false;
            while (Exp >= ExpToNextLevel)
            {
                Exp -= ExpToNextLevel;
                Level++;
                leveledUp = true;
            }
            return leveledUp;
        }
    }
}
