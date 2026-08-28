namespace Game.Equipment
{
    public enum MaterialType
    {
        Metal,      // Металл
        Wood,       // Дерево
        Thread,     // Нить
        MagicStone  // Магический камень
    }

    public static class Dismantler
    {
        private static int GetBaseAmount(EquipmentRarity rarity) => rarity switch
        {
            EquipmentRarity.Common => 2,
            EquipmentRarity.Uncommon => 3,
            EquipmentRarity.Rare => 5,
            EquipmentRarity.Epic => 8,
            EquipmentRarity.Legendary => 12,
            EquipmentRarity.Divine => 18,
            _ => 1
        };

        private static Dictionary<MaterialType, double> GetWeights(TypeEquipment type) => type switch
        {
            TypeEquipment.Sword => new() { { MaterialType.Metal, 0.7 }, { MaterialType.Wood, 0.3 } },
            TypeEquipment.Shield => new() { { MaterialType.Metal, 0.6 }, { MaterialType.Wood, 0.4 } },
            TypeEquipment.Helmet => new() { { MaterialType.Metal, 0.7 }, { MaterialType.Thread, 0.3 } },
            TypeEquipment.Breastplate => new() { { MaterialType.Metal, 0.6 }, { MaterialType.Thread, 0.4 } },
            TypeEquipment.Boots => new() { { MaterialType.Thread, 0.6 }, { MaterialType.Metal, 0.4 } },
            TypeEquipment.Bow => new() { { MaterialType.Wood, 0.7 }, { MaterialType.Thread, 0.3 } },
            TypeEquipment.Staff => new() { { MaterialType.Wood, 0.5 }, { MaterialType.MagicStone, 0.5 } },
            TypeEquipment.Ring => new() { { MaterialType.MagicStone, 0.7 }, { MaterialType.Metal, 0.3 } },
            _ => new() { { MaterialType.Metal, 1.0 } }
        };

        private static double GetQualityMultiplier(TypeQuality quality) => quality switch
        {
            TypeQuality.Broken => 0.5,
            TypeQuality.Threadbare => 0.75,
            TypeQuality.Default => 1.0,
            TypeQuality.Qualitative => 1.25,
            TypeQuality.Divine => 1.5,
            _ => 1.0
        };

        public static Dictionary<MaterialType, int> Dismantle(IEquipment item)
        {
            var result = new Dictionary<MaterialType, int>();
            if (item == null) return result;

            int baseAmount = GetBaseAmount(item.Rarity);
            double qualityMult = GetQualityMultiplier(item.TypeQualities);

            foreach (var (material, weight) in GetWeights(item.TypeEquipment))
            {
                int amount = (int)Math.Round(baseAmount * weight * qualityMult);
                if (amount <= 0) amount = 1;
                result[material] = amount;
            }

            return result;
        }

        public static Dictionary<MaterialType, int> GetForgeCost(IEquipment item)
        {
            var result = new Dictionary<MaterialType, int>();
            if (item == null) return result;

            int baseAmount = Math.Max(1, GetBaseAmount(item.Rarity) / 2);

            foreach (var (material, weight) in GetWeights(item.TypeEquipment))
            {
                int amount = (int)Math.Round(baseAmount * weight);
                if (amount <= 0) amount = 1;
                result[material] = amount;
            }

            return result;
        }
    }
}
