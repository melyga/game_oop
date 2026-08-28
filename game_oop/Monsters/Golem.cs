using Game.Heros;

namespace Game.Monsters
{
    public class Golem : Monster
    {
        public Golem(int level, MonsterRarity rarity = MonsterRarity.Elite)
            : base("Golem", level, baseHP: 70, basePower: 12, baseArmor: 7, rarity) { }

        public override int Attack(IEnemy enemy)
        {
            return enemy is Hero hero ? hero.TakeDamage(Power) : 0;
        }
    }
}