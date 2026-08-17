namespace Game
{
    public class LevelProgress
    {
        public int Level { get; private set; } = 1;

        public int Exp { get; private set; } = 0;

        public int ExpToNextLevel => (int)(50 * Math.Pow(Level, 1.5));
        private int Score = 0;

        /// <summary>
        /// Возвращает сколько уровней получили
        /// </summary>
        public int AddExp(int amount)
        {
            Exp += amount;
            Score = 0;

            bool leveledUp = false;
            while (Exp >= ExpToNextLevel)
            {
                Exp -= ExpToNextLevel;
                Level++;
                Score++;
            }
            return Score;
        }
    }
}
