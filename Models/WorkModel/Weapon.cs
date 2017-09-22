using Newtonsoft.Json;
using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparks.Models.WorkModel
{
    public static class Weapon
    {
        #region constants

        public const int MAGIC_WEAPON_MOD = 0;
        public const int TECH_WEAPON_MOD = 1;
        public const int NON_ENERGY_MOD = 2;
        public const int AUGMENT_WEAPON_MOD = 3;
        public const int AESTHETIC_WEAPON_MOD = 4;
        private const int SIZE_WEAPON_MOD = 5;
        private const int CLASSIFICATION_WEAPON_MOD = 6;
        private const int EMPTY_ENERGY_MOD = 7;

        private static List<int> DEFAULT_MELEE_MODS = new List<int>() { MAGIC_WEAPON_MOD, TECH_WEAPON_MOD, NON_ENERGY_MOD, AUGMENT_WEAPON_MOD, SIZE_WEAPON_MOD, AESTHETIC_WEAPON_MOD };
        private static List<int> DEFAULT_PRIMITIVE_RANGED_MODS = new List<int>() { MAGIC_WEAPON_MOD, TECH_WEAPON_MOD };
        private static List<int> DEFAULT_MODERN_RANGED_MODS = new List<int>() { MAGIC_WEAPON_MOD, TECH_WEAPON_MOD, NON_ENERGY_MOD, CLASSIFICATION_WEAPON_MOD };
        private static List<int> DEFAULT_SEIGE_MODS = new List<int>() { TECH_WEAPON_MOD, NON_ENERGY_MOD, CLASSIFICATION_WEAPON_MOD };
        private static List<int> DEFAULT_ITEM_MODS = new List<int>() { EMPTY_ENERGY_MOD, MAGIC_WEAPON_MOD, AESTHETIC_WEAPON_MOD };

        #endregion
        #region Weapons

        /// <summary>
        /// Returns a random weapon of the following type:
        ///     Meleen Weapon
        ///     Primitive Ranged
        ///     Modern Ranged
        ///     Magic Weapon
        /// </summary>
        /// <returns></returns>
        public static VmWeapon AnyWeapon(int[] weaponTypes = null, int[] modTypes = null)
        {
            var vm = new VmWeapon();

            int selection;
            if (weaponTypes == null) selection = Utilities.random.Next(5);
            else selection = weaponTypes.SelectRandom();

            switch (selection)
            {
                case 0:
                    vm = AnyMelee(modTypes);
                    break;
                case 1:
                    vm = AnyPrimativeRanged(modTypes);
                    break;
                case 2:
                    vm = AnyModernRanged(modTypes);
                    break;
                case 3:
                    vm = AnyMagicItem(modTypes);
                    break;
                case 4:
                    vm = AnySiege(modTypes);
                    break;
            }
            return vm;
        }

        /// <summary>
        /// Returns a weapon that containss everything *but* the weapon type and subtype.
        /// </summary>
        private static VmWeapon AnyGenericWeapon(List<int> modTypes)
        {
            var vm = new VmWeapon();

            //  Energy (Magic, tech, non-energy, and augments) are mutually exclusive.
            //  No weapon will ever have more than one.
            int[] energyTypes = { MAGIC_WEAPON_MOD, TECH_WEAPON_MOD, NON_ENERGY_MOD, AUGMENT_WEAPON_MOD, EMPTY_ENERGY_MOD };
            List<int> energyChoices = new List<int>();
            foreach (var t in modTypes) if (energyTypes.Contains(t)) energyChoices.Add(t);

            if (energyChoices.Count > 0)
            {
                int selection = energyChoices.SelectRandom();
                switch (selection)
                {
                    case MAGIC_WEAPON_MOD:
                        vm.Energy = Weapon.MagicEnergy();
                        break;
                    case TECH_WEAPON_MOD:
                        vm.Energy = Weapon.TechEnergy();
                        break;
                    case NON_ENERGY_MOD:
                        vm.Energy = Weapon.NonEnergy();
                        break;
                    case AUGMENT_WEAPON_MOD:
                        vm.Augment = Weapon.MagicAugment();
                        break;
                    case EMPTY_ENERGY_MOD:
                        //  do nothing
                        break;
                }
            }

            //  Check for size, aesthetic
            if (Utilities.RandomBool()
                && modTypes.Contains(SIZE_WEAPON_MOD))
                vm.Size = Weapon.Size();

            if ((Utilities.RandomBool() || energyChoices.Count == 0)
                && modTypes.Contains(AESTHETIC_WEAPON_MOD))
                vm.Aesthetic = Weapon.Aesthetic();

            if (Utilities.RandomBool()
                && modTypes.Contains(CLASSIFICATION_WEAPON_MOD))
                vm.Classification = Weapon.Classification();

            return vm;
        }

        /// <summary>
        /// Type: Melee
        /// Subtype: randomized
        /// Size: randomized, 50% chance, 100% chance if no Energy or Augment generated
        /// Mods: one of MagicEnergy, TechEnergy, NonEnergy, MagicAugment
        /// Returns: [size] [mod] [item]
        /// </summary>
        public static VmWeapon AnyMelee(int[] modTypes = null)
        {
            var db = new SparksDbContext();
            var vm = new VmWeapon();

            //  set up the default parameters since none were provided
            if (modTypes == null)
            {
                vm = AnyGenericWeapon(DEFAULT_MELEE_MODS);
            } else
            {
                List<int> requestedMods = modTypes.ToList();
                requestedMods.Add(SIZE_WEAPON_MOD);
                if (modTypes.Contains(MAGIC_WEAPON_MOD)) requestedMods.Add(AUGMENT_WEAPON_MOD);
                vm = AnyGenericWeapon(requestedMods);
            }

            //  determine specific weapon type and subtype
            vm.WeaponType = db.WeaponType.First(n => n.ID == 3).ToViewModel();
            vm.Subtype = db.WeaponMelee.SelectRandom().ToViewModel();
            
            return vm;
        }

        /// <summary>
        /// Type: PrimativeRanged
        /// Subtype: randomized
        /// MagicAugment: randomized
        /// Mods: MagicEnergy OR TechEnergy (never both)
        /// Returns: [augment] [energy] [item]
        /// </summary>
        /// <returns></returns>
        public static VmWeapon AnyPrimativeRanged(int[] modTypes = null)
        {
            var db = new SparksDbContext();
            var vm = new VmWeapon();

            //  set up the default parameters since none were provided
            if (modTypes == null)
            {
                vm = AnyGenericWeapon(DEFAULT_PRIMITIVE_RANGED_MODS);
            }
            else
            {
                List<int> requestedMods = modTypes.ToList();
                if (modTypes.Contains(MAGIC_WEAPON_MOD)) requestedMods.Add(AUGMENT_WEAPON_MOD);
                vm = AnyGenericWeapon(requestedMods);
            }
            
            vm.WeaponType = db.WeaponType.First(n => n.ID == 1).ToViewModel();
            vm.Subtype = db.WeaponPrimativeRanged.SelectRandom().ToViewModel();

            return vm;
        }

        /// <summary>
        /// Type: ModernRanged
        /// Subtype: randomized
        /// Classification: randomized
        /// Mods: MagicEnergy OR TechEnergy OR NonEnergy (never both)
        /// Returns: [classification] [energy] [item]
        /// </summary>
        /// <returns></returns>
        public static VmWeapon AnyModernRanged(int[] modTypes = null)
        {
            var db = new SparksDbContext();
            var vm = new VmWeapon();

            //  set up the default parameters since none were provided
            if (modTypes == null)
            {
                vm = AnyGenericWeapon(DEFAULT_MODERN_RANGED_MODS);
            }
            else
            {
                List<int> requestedMods = modTypes.ToList();
                requestedMods.Add(CLASSIFICATION_WEAPON_MOD);
                if (modTypes.Contains(MAGIC_WEAPON_MOD)) requestedMods.Add(AUGMENT_WEAPON_MOD);
                vm = AnyGenericWeapon(requestedMods);
            }
            
            vm.WeaponType = db.WeaponType.First(n => n.ID == 2).ToViewModel();
            vm.Subtype = db.WeaponModernRanged.SelectRandom().ToViewModel();

            return vm;
        }

        /// <summary>
        /// Type: Seige
        /// Subtype: randomized
        /// Classification: randomized
        /// Mods: TechEnergy OR NonEnergy (never both)
        /// Returns: [classification] [energy] [item]
        /// </summary>
        /// <returns></returns>
        public static VmWeapon AnySiege(int[] modTypes = null)
        {
            var db = new SparksDbContext();
            var vm = new VmWeapon();

            //  set up the default parameters since none were provided
            if (modTypes == null)
            {
                vm = AnyGenericWeapon(DEFAULT_SEIGE_MODS);
            }
            else
            {
                List<int> requestedMods = modTypes.ToList();
                requestedMods.Add(CLASSIFICATION_WEAPON_MOD);
                vm = AnyGenericWeapon(requestedMods);
            }
            
            vm.WeaponType = db.WeaponType.First(n => n.ID == 5).ToViewModel();
            vm.Subtype = db.WeaponSiege.SelectRandom().ToViewModel();
            
            return vm;
        }

        /// <summary>
        /// Returns VmGeneral. Capital weapon like "heavy particle weapon" or "light cannon"
        /// </summary>
        private static VmGeneral AnyCapitalWeapon()
        {
            var db = new SparksDbContext();

            string size = db.WeaponClass.Where(x => x.ID > 1 && x.ID < 6).SelectRandom().Name;
            string weapon = db.WeaponCapital.SelectRandom().Name;
            var vm = new VmGeneral();
            vm.Name = size + " " + weapon;
            return vm;
        }

        /// <summary>
        /// Type: MagicWeapon
        /// Subtype: randomized
        /// Mods: Aesthetic, MagicEnergy
        /// Returns: [aesthetic] [magicenergy] [item]
        /// </summary>
        /// <returns></returns>
        public static VmWeapon AnyMagicItem(int[] modTypes = null)
        {
            var db = new SparksDbContext();
            var vm = new VmWeapon();

            //  set up the default parameters since none were provided
            if (modTypes == null)
            {
                vm = AnyGenericWeapon(DEFAULT_ITEM_MODS);
            }
            else
            {
                List<int> requestedMods = modTypes.ToList();
                vm = AnyGenericWeapon(requestedMods);
            }
            
            vm.WeaponType = db.WeaponType.First(n => n.ID == 4).ToViewModel();
            vm.Subtype = db.WeaponMagic.SelectRandom().ToViewModel();

            return vm;
        }

        #endregion
        #region Helpers        
        
        public static VmGeneral Size()
        {
            var db = new SparksDbContext();
            return db.WeaponSize.SelectRandom().ToViewModel();
        }

        public static VmGeneral Classification()
        {
            var db = new SparksDbContext();
            return db.WeaponClass.SelectRandom().ToViewModel();
        }

        public static VmGeneral Aesthetic()
        {
            var db = new SparksDbContext();
            return db.WeaponModAesthetic.SelectRandom().ToViewModel();
        }

        public static VmGeneral MagicEnergy()
        {
            var db = new SparksDbContext();
            return db.WeaponModMagicEnergy.SelectRandom().ToViewModel();
        }

        public static VmGeneral TechEnergy()
        {
            var db = new SparksDbContext();
            return db.WeaponModTechEnergy.SelectRandom().ToViewModel();
        }

        public static VmGeneral NonEnergy()
        {
            var db = new SparksDbContext();
            return db.WeaponModNonEnergy.SelectRandom().ToViewModel();
        }

        public static VmGeneral MagicAugment()
        {
            var db = new SparksDbContext();
            return db.WeaponModMagicAugment.SelectRandom().ToViewModel();
        }

        #endregion
        #region Non-Weapon Items

        /// <summary>
        /// Returns items and tools
        /// </summary>
        public static VmWeapon AnyItem()
        {
            var db = new SparksDbContext();
            var vm = new VmWeapon();
            vm.WeaponType = new VmGeneral();
            vm.WeaponType.Name = "item";

            if (Utilities.RandomBool()) vm.Energy = Weapon.TechEnergy();

            //  define the actual object ('item' or 'tool')
            if (Utilities.RandomBool())
            {
                vm.Subtype = db.ItemMiscellaneous.SelectRandom().ToViewModel();
            } else
            {
                vm.Subtype = db.ItemTool.SelectRandom().ToViewModel();
            }

            if (Utilities.RandomBool())
            {
                vm.Aesthetic = db.WeaponModAesthetic.SelectRandom().ToViewModel();
            }

            return vm;
        }

        #endregion
        #region Capital Weapons

        /// <summary>
        /// Returns generic capital weapon descriptions, e.g., 
        /// "heavy lasers" or "light pulsers"
        /// </summary>
        public static string AnyGenericCapitalWeapon(int maxSize = 100)
        {
            var db = new SparksDbContext();
            string weaponName = db.WeaponCapital.SelectRandom().Name;

            if (Utilities.OneChanceIn(4))
            {
                var sizeList = db.GeneralSize.ToList();
                var selectedSize = sizeList.SelectRandom();
                while (selectedSize.Weight > maxSize) selectedSize = sizeList.SelectRandom();
                weaponName = selectedSize.Name + " " + weaponName;
            }

            if (Utilities.OneChanceIn(4))
            {
                var rangeList = db.GeneralRange.ToList();
                var selectedRange = rangeList.SelectRandom();
                while (selectedRange.Weight > maxSize) selectedRange = rangeList.SelectRandom();
                weaponName = selectedRange.Name + " " + weaponName;
            }

            return weaponName;
        }
        
        /// <summary>
        /// Returns a list of generic capital weapons, e.g.,
        /// many large cannon, few light pulsers, singular medium laser
        /// </summary>
        public static List<string> GenericCapitalWeaponGroup(int count)
        {
            List<string> weaponList = new List<string>();
            List<string> singles = new List<string>();
            List<string> some = new List<string>();
            List<string> many = new List<string>();

            var db = new SparksDbContext();
            var weapons = db.WeaponCapital.Count();
            var sizes = db.GeneralSize.Count();
            int maxCount = weapons * sizes;
            
            if (count > maxCount) count = maxCount;

            while (weaponList.Count < count)
            {
                string candidate = AnyGenericCapitalWeapon(count+3);
                if (!weaponList.Contains(candidate))
                {
                    weaponList.Add(candidate);
                    bool isCannon = candidate.Contains("cannon");

                    switch (Utilities.random.Next(3))
                    {
                        case 0:
                            some.Add(isCannon ? candidate : candidate + "s");
                            break;
                        case 1:
                            many.Add("many " + (isCannon ? candidate : candidate + "s"));
                            break;
                        case 2:
                            singles.Add("single " + candidate);
                            break;
                    }
                }
            }

            weaponList.Clear();
            weaponList.AddRange(singles);
            weaponList.AddRange(some);
            weaponList.AddRange(many);

            return weaponList;
        }

        #endregion
    }
}