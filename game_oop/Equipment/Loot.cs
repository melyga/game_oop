using Game.Monsters;

namespace Game.Equipment
{
    /// <summary>
    /// Отвечает за то, что и с каким шансом выпадает с монстров.
    /// Редкость монстра (Monster.Rarity) определяет диапазон редкости предметов (EquipmentRarity),
    /// которые могут выпасть, а также шанс того, что предмет вообще выпадет и насколько
    /// хорошим будет его качество (TypeQuality).
    /// </summary>
    public static class LootTable
    {
        // Реестр фабрик всей доступной в игре экипировки.
        // Редкость каждого предмета вычисляется автоматически (см. BaseEquipment.CalculateRarity),
        // поэтому здесь не нужно вручную дублировать привязку к редкости.
        private static readonly List<Func<TypeQuality, IEquipment>> _factories = new()
        {
            // Уровень 1
            q => new RecruitHelmet(q),
            q => new IronBreastplate(q),
            q => new RecruitBoots(q),
            q => new TrainingSword(q),
            q => new WoodenShield(q),
            q => new CopperRing(q),
            q => new HuntingBow(q),
            q => new ApprenticeStaff(q),
            q => new HunterHood(q),
            q => new HunterJacket(q),
            q => new HunterBoots(q),
            q => new ApprenticeHood(q),
            q => new ApprenticeBoots(q),
            q => new ApprenticeRobe(q),

            // Уровень 10
            q => new KnightHelmet(q),
            q => new KnightBreastplate(q),
            q => new KnightBoots(q),
            q => new KnightSword(q),
            q => new KnightShield(q),
            q => new WarriorRing(q),
            q => new ArcherRing(q),
            q => new MageRing(q),
            q => new RangerHood(q),
            q => new RangerArmor(q),
            q => new RangerBoots(q),
            q => new RangerBow(q),
            q => new SorcererBoots(q),
            q => new SorcererRobe(q),
            q => new MageCrown(q),
            q => new ElementalStaff(q),

            // Уровень 20
            q => new TitanHelmet(q),
            q => new TitanBreastplate(q),
            q => new TitanBoots(q),
            q => new TitanSword(q),
            q => new TitanShield(q),
            q => new StormHood(q),
            q => new StormArmor(q),
            q => new StormBoots(q),
            q => new StormBow(q),
            q => new LichHood(q),
            q => new BloodLichRobe(q),
            q => new LichBoots(q),
            q => new AbyssalStaff(q),

            // Уровень 25 (сет Драконоборца)
            q => new DragonSlayerHelmet(q),
            q => new DragonSlayerBreastplate(q),
            q => new DragonSlayerBoots(q),
            q => new DragonSlayerSword(q),
            q => new DragonSlayerShield(q),
            q => new DragonSlayerRing(q),
            q => new DragonSlayerBow(q),
            q => new DragonSlayerStaff(q),
        };

        /// <summary>
        /// Базовый шанс того, что монстр вообще что-то уронит.
        /// </summary>
        private static double GetDropChance(MonsterRarity monsterRarity) => monsterRarity switch
        {
            MonsterRarity.Normal => 0.10,
            MonsterRarity.Elite => 0.30,
            MonsterRarity.Boss => 0.85,
            _ => 0.05
        };

        /// <summary>
        /// Диапазон редкости предметов (EquipmentRarity), которые может уронить монстр такой редкости.
        /// Боссы гарантированно роняют лучший из существующих в игре шмот.
        /// </summary>
        private static (EquipmentRarity Min, EquipmentRarity Max) GetRarityRange(MonsterRarity monsterRarity) => monsterRarity switch
        {
            MonsterRarity.Normal => (EquipmentRarity.Common, EquipmentRarity.Common),
            MonsterRarity.Elite => (EquipmentRarity.Common, EquipmentRarity.Rare),
            MonsterRarity.Boss => (EquipmentRarity.Epic, EquipmentRarity.Divine),
            _ => (EquipmentRarity.Common, EquipmentRarity.Common)
        };

        /// <summary>
        /// Определяет, каким качеством (TypeQuality) будет обладать выпавший экземпляр предмета.
        /// Чем "круче" монстр, тем выше шанс хорошего качества.
        /// </summary>
        private static TypeQuality RollQuality(MonsterRarity monsterRarity, Random rand)
        {
            int bonus = monsterRarity switch
            {
                MonsterRarity.Elite => 15,
                MonsterRarity.Boss => 35,
                _ => 0
            };

            int roll = Math.Min(99, rand.Next(100) + bonus);

            if (roll < 10) return TypeQuality.Broken;
            if (roll < 35) return TypeQuality.Threadbare;
            if (roll < 70) return TypeQuality.Default;
            if (roll < 92) return TypeQuality.Qualitative;
            return TypeQuality.Divine;
        }

        /// <summary>
        /// Пытается сгенерировать выпадение экипировки с убитого монстра.
        /// Возвращает null, если монстру "не повезло" ничего уронить.
        /// </summary>
        public static IEquipment TryDropLoot(Monster monster, Random rand)
        {
            if (monster == null || rand == null)
                return null;

            if (rand.NextDouble() > GetDropChance(monster.Rarity))
                return null;

            var (min, max) = GetRarityRange(monster.Rarity);

            var candidates = new List<Func<TypeQuality, IEquipment>>();
            foreach (var factory in _factories)
            {
                // "Пробный" экземпляр нужен только чтобы узнать редкость предмета,
                // в бой/инвентарь он не попадает.
                IEquipment probe = factory(TypeQuality.Default);
                if (probe.Rarity >= min && probe.Rarity <= max)
                {
                    candidates.Add(factory);
                }
            }

            if (candidates.Count == 0)
            {
                candidates = _factories;
            }

            var chosenFactory = candidates[rand.Next(candidates.Count)];
            TypeQuality quality = RollQuality(monster.Rarity, rand);

            return chosenFactory(quality);
        }
    }
}
