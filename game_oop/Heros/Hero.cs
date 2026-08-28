using Game.Equipment;
using Game.Monsters;

namespace Game.Heros
{
    public abstract class Hero : IEnemy
    {
        public string Name { get; private set; }

        public int Armor => _armor;
        private int _armor;

        public int Power => _power;
        private int _power;

        public int HP => _hp;
        protected int _hp;

        public int MaxHP { get; private set; }
        public int HealHP { get; private set; } = 20;

        protected float _critDamage;
        protected float _critRate;

        public bool IsAlive => HP > 0;

        public int Score { get; private set; } = 1;

        public LevelProgress Progress { get; private set; } = new LevelProgress();
        public Dictionary<TypeEquipment, List<IEquipment>> equippedItems { get; private set; }
            = new Dictionary<TypeEquipment, List<IEquipment>>();

        /// <summary>
        /// Предметы, которые герой подобрал (например, выпали с монстров), но еще не надел.
        /// Размер инвентаря ничем не ограничен.
        /// </summary>
        public List<IEquipment> Inventory { get; private set; } = new List<IEquipment>();

        /// <summary>
        /// Материалы, полученные при разборке экипировки. Тратятся в кузнице на ковку.
        /// </summary>
        public Dictionary<MaterialType, int> Materials { get; private set; } = new Dictionary<MaterialType, int>
        {
            { MaterialType.Metal, 0 },
            { MaterialType.Wood, 0 },
            { MaterialType.Thread, 0 },
            { MaterialType.MagicStone, 0 },
        };

        protected abstract HashSet<TypeEquipment> AllowedEquipment { get; }

        protected static readonly Dictionary<TypeEquipment, int> EquipmentLimits = new Dictionary<TypeEquipment, int>
        {
            { TypeEquipment.Helmet, 1 },
            { TypeEquipment.Breastplate, 1 },
            { TypeEquipment.Boots, 1 },
            { TypeEquipment.Sword, 1 },
            { TypeEquipment.Shield, 1 },
            { TypeEquipment.Bow, 1 },
            { TypeEquipment.Staff, 1 },
            { TypeEquipment.Ring, 2 },
        };

        public Guid Id => Guid.NewGuid();

        protected Random rand = new Random();

        private const int HpStep = 20;
        private const int HealHPStep = 10;
        private const int PowerStep = 3;
        private const int ArmorStep = 2;
        private const float CritDamageStep = 5f;
        private const float CritRateStep = 1f;

        public Hero(string name, int hp, int maxHp, int armor,
            int power, float critDamage, float critRate)
        {
            Name = name;
            _hp = hp;
            MaxHP = maxHp;
            _armor = armor;
            _power = power;
            _critDamage = critDamage;
            _critRate = critRate;

            foreach (var type in EquipmentLimits.Keys)
            {
                equippedItems[type] = new List<IEquipment>();
            }
        }

        public abstract int Attack(IEnemy enemy);

        public abstract int Heal();

        /// <summary>
        /// Проверяет, можно ли вообще надеть предмет (класс героя и требуемый уровень).
        /// Занятость слота больше не блокирует надевание - если все слоты этого типа заняты,
        /// Equip() автоматически снимет самый старый предмет этого типа, освобождая место.
        /// </summary>
        public bool CanEquip(IEquipment equipment)
        {
            if (!AllowedEquipment.Contains(equipment.TypeEquipment))
            {
                return false;
            }

            // Проверка на требуемый уровень предмета
            if (Progress.Level < equipment.RequiredLevel)
            {
                return false;
            }

            return EquipmentLimits.ContainsKey(equipment.TypeEquipment);
        }

        public void Equip(IEquipment equipment)
        {
            if (!AllowedEquipment.Contains(equipment.TypeEquipment))
            {
                Console.WriteLine($"{ClassName} не умеет использовать {equipment.Name}!");
                return;
            }

            if (Progress.Level < equipment.RequiredLevel)
            {
                Console.WriteLine($"{Name} еще недостаточно опытен для {equipment.Name}: требуется {equipment.RequiredLevel} уровень (сейчас {Progress.Level})!");
                return;
            }

            var currentType = equipment.TypeEquipment;
            if (!EquipmentLimits.ContainsKey(currentType))
            {
                return;
            }

            var slotItems = equippedItems[currentType];

            // Если все слоты этого типа заняты - меняем снаряжение: снимаем самый старый
            // надетый предмет этого типа обратно в инвентарь, освобождая место под новый.
            if (slotItems.Count >= EquipmentLimits[currentType])
            {
                IEquipment oldItem = slotItems[0];
                slotItems.RemoveAt(0);
                Inventory.Add(oldItem);
                Console.WriteLine($"{Name} снял {oldItem.Name}, чтобы освободить место");
            }

            slotItems.Add(equipment);
            Inventory.Remove(equipment);
            Console.WriteLine($"{Name} успешно экипировал {equipment.Name}");
        }

        /// <summary>
        /// Снимает предмет и возвращает его в инвентарь, освобождая слот для другой экипировки.
        /// </summary>
        public bool Unequip(IEquipment equipment)
        {
            if (equipment == null)
            {
                return false;
            }

            if (equippedItems.TryGetValue(equipment.TypeEquipment, out var items) && items.Remove(equipment))
            {
                Inventory.Add(equipment);
                Console.WriteLine($"{Name} снял {equipment.Name}");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Добавляет предмет в инвентарь героя (например, полученный в качестве добычи).
        /// </summary>
        public void AddToInventory(IEquipment equipment)
        {
            if (equipment != null)
            {
                Inventory.Add(equipment);
            }
        }

        /// <summary>
        /// Безвозвратно разбирает предмет из инвентаря на материалы (металл, дерево, нить, маг. камень).
        /// </summary>
        public bool DismantleItem(IEquipment item, out Dictionary<MaterialType, int> gained)
        {
            gained = null;

            if (item == null || !Inventory.Contains(item))
            {
                return false;
            }

            gained = Dismantler.Dismantle(item);
            foreach (var (material, amount) in gained)
            {
                Materials[material] = Materials.GetValueOrDefault(material) + amount;
            }

            Inventory.Remove(item);
            return true;
        }

        /// <summary>
        /// Проверяет, хватает ли материалов на указанную стоимость (например, для ковки).
        /// </summary>
        public bool HasEnoughMaterials(Dictionary<MaterialType, int> cost)
        {
            foreach (var (material, amount) in cost)
            {
                if (Materials.GetValueOrDefault(material) < amount)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Списывает материалы, если их достаточно. Возвращает false и ничего не тратит, если не хватает.
        /// </summary>
        public bool TrySpendMaterials(Dictionary<MaterialType, int> cost)
        {
            if (!HasEnoughMaterials(cost))
            {
                return false;
            }

            foreach (var (material, amount) in cost)
            {
                Materials[material] -= amount;
            }

            return true;
        }

        public virtual int TakeDamage(int damage)
        {
            int realDamage = damage - Armor;
            if (realDamage <= 0)
                realDamage = 1;

            _hp -= realDamage;
            return realDamage;
        }

        protected int CalculateCrit(int armor = 0)
        {
            if (rand.NextDouble() * 100 < _critRate)
                return (int)(Power * (_critDamage / 100f)) + armor;
            return Power + armor;
        }

        public void AddExperience(int exp)
        {
            int levelsGained = Progress.AddExp(exp);
            if (levelsGained > 0)
            {
                Score += levelsGained * 3;
            }
        }

        /// <summary>
        /// Общая логика для всех классов героев при убийстве монстра:
        /// начисление опыта и шанс выпадения экипировки (с учетом редкости монстра, включая боссов).
        /// </summary>
        protected void HandleMonsterDefeat(Monster monster)
        {
            AddExperience(monster.CalculateExpReward(Progress.Level));

            IEquipment loot = LootTable.TryDropLoot(monster, rand);
            if (loot != null)
            {
                AddToInventory(loot);
                Console.WriteLine($"{monster.Name} выронил: {loot.Name} (ур. {loot.RequiredLevel}, редкость: {loot.Rarity})");
            }
        }

        public bool TryUpgradeStat(StatType stat)
        {
            if (Score <= 0) return false;

            switch (stat)
            {
                case StatType.Power:
                    _power += PowerStep;
                    break;
                case StatType.Armor:
                    _armor += ArmorStep;
                    break;
                case StatType.MaxHp:
                    MaxHP += HpStep;
                    _hp += HpStep;
                    break;
                case StatType.CritDamage:
                    _critDamage += CritDamageStep;
                    break;
                case StatType.CritRate:
                    _critRate = Math.Min(100f, _critRate + CritRateStep);
                    break;
                case StatType.HealHP:
                    HealHP += HealHPStep;
                    break;
                default:
                    return false;
            }

            Score--;
            return true;
        }
        public void TransferProgressFrom(Hero oldHero)
        {
            this.Progress = oldHero.Progress;

            this.Score = oldHero.Progress.Level * 3;

            this._hp = this.MaxHP;
        }

        public abstract string ClassName { get; }
    }

    public enum StatType
    {
        Power,
        Armor,
        MaxHp,
        CritDamage,
        CritRate,
        HealHP
    }
}