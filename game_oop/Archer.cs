namespace Game
{
    public class Archer : Hero
    {
        /// <summary>
        /// Создает героя с направлением в Лучника.
        /// Уникальность класса - высокий шанс критического удара, высокий урон от критического удара и возможность проигнорировать часть брони противника.
        /// </summary>
        public Archer(string name, float critDamage = 75, float critRate = 20)
        : base(name, hp: 100, strength: 22, agility: 15, score: 0, maxHP: 150, critDamage, critRate, armor: 5)
        { }

        public override int Attack(Monster monster)
        {
            int rawDamage = CalculateDamage(); // Базовый урон + крит
            int finalDamage = CalculateFinalDamage(rawDamage, monster); // С учетом 50% брони

            if (monster.TakeDamage(finalDamage))
            {
                AwardExperience(monster);
            }

            return finalDamage;
        }

        protected override int CalculateFinalDamage(int rawDamage, Monster monster)
        {
            int effectiveArmor = monster.Armor / 2;
            int finalDamage = rawDamage - effectiveArmor;

            return finalDamage < 0 ? 0 : finalDamage;
        }

        public override string ClassName => "Лучник";
    }
}
