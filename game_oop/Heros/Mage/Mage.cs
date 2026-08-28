using Game.Equipment;
using Game.Monsters;

namespace Game.Heros.Mage
{
    public class Mage : Hero
    {
        public Mage(string name)
            : base(name, hp: 80, maxHp: 80, power: 18,
                   critDamage: 75, critRate: 40, armor: 2)
        { }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;

            if (enemy is Monster monster)
            {
                damage = monster.TakeDamage(CalculateCrit(monster.Armor)); // Отправляет броню чтобы ее компенсировать в уроне так как маг игнорирует всю броню
                if (!monster.IsAlive)
                {
                    HandleMonsterDefeat(monster);
                }
            }

            return damage;
        }

        public override int Heal()
        {
            _hp += HealHP;

            if (_hp > MaxHP)
            {
                _hp = MaxHP;
            }

            return HealHP;
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

        public override string ClassName => "Маг";
    }
}