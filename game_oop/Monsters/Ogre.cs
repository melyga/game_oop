using Game.Heros;

namespace Game.Monsters
{
    public class Ogre : Monster
    {
        public Ogre(int level, MonsterRarity rarity = MonsterRarity.Normal)
            : base("Ogre", level, baseHP: 35, basePower: 9, baseArmor: 3, rarity) { }

        public override int Attack(IEnemy enemy)
        {
            return enemy is Hero hero ? hero.TakeDamage(Power) : 0;
        }

        protected override string BuildBossName(string baseName) => "Вождь огров";
    }
}