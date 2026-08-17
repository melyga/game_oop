using Game.Monsters;

namespace Game.Heros.Warrior.Dread
{
    public class Dread_Warrior : Hero
    {
        public Dread_Warrior(string name)
            : base(name, hp: 180, maxHp: 180, power: 12, critDamage: 50, critRate: 10, armor: 16) { }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;
            if (enemy is Monster monster)
            {
                // Прибавляет броню к расчету базового урона
                int totalPower = CalculateCrit() + Armor;
                damage = monster.TakeDamage(totalPower);

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

        public override string ClassName => "Страж";
    }
}
