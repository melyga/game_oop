namespace Game
{
    public class Bandit : Hero
    {
        private readonly HashSet<Guid> _attackedMonsterIds = new HashSet<Guid>();

        /// <summary>
        /// Создает героя с направлением в Разбойника. 
        /// Уникальность класса - при первом ударе по монстру урон проходит в 1.5 раза выше чем обычно, но при повторной атаке по тому же монстру урон будет обычным. Разбойник имеет малый разовый урон, но средний шанс критического удара и огромный урон от критического удара.
        /// </summary>
        public Bandit(string name, float critDamage = 200, float critRate = 75)
            : base(name, hp: 100, strength: 5, agility: 15, score: 0, maxHP: 150, critDamage, critRate, armor: 7)
        { }

        public override int Attack(Monster monster)
        {
            float damage = CalculateDamage();

            if (!_attackedMonsterIds.Contains(monster.Id))
            {
                damage *= 1.5f;
                _attackedMonsterIds.Add(monster.Id); // Запоминаем, что этот монстр уже получил удар
            }

            if (monster.TakeDamage((int)damage))
            {
                AwardExperience(monster);
                ForgetMonster(monster);
            }
            return (int)damage;
        }

        // Метод для очистки ID умершего монстра
        public void ForgetMonster(Monster monster)
        {
            _attackedMonsterIds.Remove(monster.Id);
        }

        public override string ClassName => "Разбойник";
    }
}
