using Game.Heros;

namespace Game.Monsters
{
    public class Dragon : Monster
    {
        public Dragon(int level, MonsterRarity rarity = MonsterRarity.Boss)
            : base("Dragon", level, baseHP: 150, basePower: 22, baseArmor: 12, rarity) { }

        public override int Attack(IEnemy enemy)
        {
            return enemy is Hero hero ? hero.TakeDamage(Power) : 0;
        }
    }
}