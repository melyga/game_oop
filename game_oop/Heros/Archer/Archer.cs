using Game.Equipment;
using Game.Monsters;

namespace Game.Heros.Archer
{
    public class Archer : Hero
    {
        public Archer(string name)
            : base(name, hp: 100, maxHp: 100, power: 18,
                   critDamage: 70, critRate: 50, armor: 5) 
        {
            equippedItems[TypeEquipment.Bow].Add(new HuntingBow(TypeQuality.Divine));
        }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;

            if (enemy is Monster monster)
            {
                damage = monster.TakeDamage(CalculateCrit());
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
            TypeEquipment.Ring,
            TypeEquipment.Bow,
        };

        public override string ClassName => "Лучник";
    }
}