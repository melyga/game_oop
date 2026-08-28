using Game.Equipment;
using Game.Monsters;

namespace Game.Heros.Warrior
{
    public class Warrior : Hero
    {
        public Warrior(string name)
            : base(name, hp: 120, maxHp: 120, power: 10,
                   critDamage: 50, critRate: 10, armor: 8)
        { }

        public override int Attack(IEnemy enemy)
        {
            int damage = 0;

            if (enemy is Monster monster)
            {
                int bonusPower = equippedItems.Values.SelectMany(list => list).Sum(item => item.BonusPower);
                damage = monster.TakeDamage(CalculateCrit() + bonusPower);
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

        public override string ClassName => "Воин";
    }
}