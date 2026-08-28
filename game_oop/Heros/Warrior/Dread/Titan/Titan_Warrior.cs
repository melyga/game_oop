using Game.Equipment;
using Game.Monsters;

namespace Game.Heros.Warrior.Dread.Titan
{
    /// <summary>
    /// Не используется
    /// </summary>
    /*public class Titan_Warrior : Hero
    {
        public Titan_Warrior(string name)
            : base(name, hp: 250, maxHp: 250, power: 15, critDamage: 50, critRate: 10, armor: 25) { }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;
            if (enemy is Monster monster)
            {
                // Сильное масштабирование урона от брони
                int totalPower = CalculateCrit() + (Armor * 2);
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
            // Лечение усиливается показателем брони
            int totalHeal = HealHP + Armor;
            _hp = Math.Min(MaxHP, _hp + totalHeal);
            return totalHeal;
        }

        protected override HashSet<TypeEquipment> AllowedEquipment => new HashSet<TypeEquipment>
        {
            TypeEquipment.Helmet,
            TypeEquipment.Breastplate,
            TypeEquipment.Boots,
            TypeEquipment.Sword,
            TypeEquipment.Shield,
            TypeEquipment.Ring,
        };

        public override string ClassName => "Титанический Воин";
    }*/
}
