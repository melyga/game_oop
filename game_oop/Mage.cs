namespace Game
{
    public class Mage : Hero
    {
        /// <summary>
        /// Создает героя с направлением в Мага. 
        /// Уникальность класса заключается в высоком уроне от магических атак и возможность игнорировать броню противника
        /// </summary>
        public Mage(string name)
        : base(name, hp : 80, strength : 18, agility : 8, score : 0, maxHP : 120, armor: 3)
        { }

        public override int Attack(Monster monster)
        {
            int damage = CalculateDamage();

            int finalDamage = CalculateFinalDamage(damage, monster); // Броня будет полностью проигнорирована

            if (monster.TakeDamage(finalDamage))
            {
                AwardExperience(monster);
            }

            return finalDamage;
        }

        protected override int CalculateFinalDamage(int rawDamage, Monster monster)
        {
            return rawDamage;
        }

        public override string ClassName => "Маг";
    }
}
