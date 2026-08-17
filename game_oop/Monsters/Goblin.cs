using Game.Heros;

namespace Game.Monsters
{
    public class Goblin : Monster
    {
        public Goblin(int level, MonsterRarity rarity = MonsterRarity.Normal)
            : base("Goblin", level, baseHP: 15, basePower: 5, baseArmor: 1, rarity) { }

        public override int Attack(IEnemy enemy)
        {
            return enemy is Hero hero ? hero.TakeDamage(Power) : 0;
        }
    }
}