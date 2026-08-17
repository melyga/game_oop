using Game.Monsters;

namespace Game.Heros.Archer.Сlever
{
    public class Trapper_Archer : Hero
    {
        public Trapper_Archer(string name)
            : base(name, hp: 120, maxHp: 120, power: 20, critDamage: 80, critRate: 60, armor: 7) { }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;
            if (enemy is Monster monster)
            {
                // При крите выстрел пробивает броню полностью
                bool isCrit = rand.NextDouble() * 100 < _critRate;
                int rawPower = isCrit ? (int)(Power * (_critDamage / 100f)) + monster.Armor : Power;

                damage = monster.TakeDamage(rawPower);

                if (!monster.IsAlive)
                {
                    AddExperience(monster.CalculateExpReward(Progress.Level));
                }
            }
            return damage;
        }

        public override int Heal()
        {
            _hp = Math.Min(MaxHP, _hp + HealHP);
            return HealHP;
        }

        public override string ClassName => "Ловчий";
    }
}
