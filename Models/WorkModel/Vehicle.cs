using Newtonsoft.Json;
using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sparks.Models.WorkModel
{
    //  TODO
    //  Split the vehicle  page into 4 sections -- utility, setting, tactical, capitol and transport
    //  These divisions reflect the intended use of the vehicle within the game or story.
    //  Transport vehicles exist soley to move the players around, so they should display
    //  information regarding range, speed, etc
    //  Tactical vehicles exist for the players to fight. The game or story revolves largely
    //  around the vehicles, so they should display pertinatnt combat information and special
    //  abilities.
    //  Capitol vehicles are massive vehicles, usually warships of some sort. The story focuses
    //  heavily on the vehicles and whatever it is they do. The players are likely to have
    //  several to many of these ships at their command and it is possible there are no actual
    //  characters involved in the game/story at all.
    //  Setting vehicles are major backdrops for a game or story. The players are likely to
    //  interact indirectly (or not at all) with the vehicle.
    //  Utility vehicles are not major components of a story or game. They are there to
    //  provide the characters with a mix of transportation, cargo hauling, and if neccessary, 
    //  combat. The story does not focus on the vehicles beyond their visual design.
    public static class Vehicle
    {
        public enum GeneralSize { Any, Wearable, Ridable, Personal, Large, Huge, Colossal };

        //  returns a complete vehicle of the indicated type, ridable, personal, etc
        public static VmVehicle SpecificVehicle(GeneralSize vehicleSize)
        {
            switch (vehicleSize)
            {
                case GeneralSize.Wearable:
                    return WearableVehicle();
                case GeneralSize.Ridable:
                    return RidableVehicle();
                case GeneralSize.Personal:
                    return PersonalVehicle();
                case GeneralSize.Large:
                    return LargeVehicle();
                case GeneralSize.Huge:
                    return HugeVehicle();
                case GeneralSize.Colossal:
                    return ColossalVehicle();
                case GeneralSize.Any:
                default:
                    return AnyVehicle();
            }
        }

        //  Returns a completely random vehicle.
        //  The parameter attrCount indicates the number of optional 
        //  attributes (armor, condition, range, etc) to generate.
        public static VmVehicle AnyVehicle(int attrCount = 1)
        {
            var db = new SparksDbContext();
            var vm = new VmVehicle();
            vm.BaseVehicle = Purpose();
            if (Utilities.RandomBool()) vm.Condition = Vehicle.Condition();

            int[] attrIndicies = new int[attrCount];
            for (int i = 0; i < attrCount; i++)
            {
                int index = -1;
                do
                {
                    index = Utilities.random.Next(12);
                } while (attrIndicies.Contains(index));
                attrIndicies[i] = index;                
            }

            for (int i = 0; i < attrCount; i++)
            {
                switch (attrIndicies[i])
                {
                    case 0:
                        vm.Armor = Vehicle.Armor();
                        break;
                    case 1:
                        vm.Cargo = Vehicle.Cargo();
                        break;
                    case 2:
                        vm.Characteristic = Vehicle.Characteristic();
                        break;
                    case 3:
                        vm.Defenses = Vehicle.Defenses();
                        break;
                    case 4:
                        vm.Material = Vehicle.Material();
                        break;
                    case 5:
                        vm.Movement = Vehicle.Movement();
                        break;
                    case 6:
                        vm.Offenses = Vehicle.Offenses();
                        break;
                    case 7:
                        vm.Size = Vehicle.Size();
                        break;
                    case 8:
                        vm.Power = Vehicle.Power();
                        break;
                    case 9:
                        vm.Range = Vehicle.Range();
                        break;
                    case 10:
                        vm.Speed = Vehicle.Speed();
                        break;
                    case 11:
                        vm.Technology = Vehicle.Technology();
                        break;
                }
            }
            
            return vm;
        }

        /// <summary>
        /// Returns a completely random purpose-built vehicle, e.g., firetruck, police car.
        /// The parameter attrCount indicates the number of optional 
        /// attributes (armor, condition, range, etc) to generate.
        /// </summary>
        public static VmVehicle VehicleByPurpose()
        {
            var db = new SparksDbContext();
            var vm = new VmVehicle();
            vm.BaseVehicle = Purpose();

            if (Utilities.RandomBool()) vm.Characteristic = Vehicle.Characteristic();
            if (Utilities.RandomBool()) vm.Condition = Vehicle.Condition();

            return vm;
        }

        /// <summary>
        /// Returns a completely random purpose-built vehicle, 
        /// e.g., quadruped tank, hover racecar
        /// The parameter attrCount indicates the number of optional 
        /// attributes (armor, condition, range, etc) to generate.
        /// </summary>
        public static VmVehicle VehicleByMovement(int[] validOptions= null)
        {
            var db = new SparksDbContext();
            var vm = new VmVehicle();
            var vehicle = new VmGeneral();
            vm.BaseVehicle = vehicle;
            if (Utilities.RandomBool()) vm.Characteristic = Vehicle.Characteristic();
            if (Utilities.RandomBool()) vm.Condition = Vehicle.Condition();

            int selection;
            if (validOptions == null) selection = Utilities.random.Next(4);
            else selection = validOptions.SelectRandom();

            switch (selection)
            {
                case 0:
                    vm.Movement = MovementByLand();
                    vm.BaseVehicle.Name = "vehicle";
                    break;
                case 1:
                    vm.Movement = MovementByWater();
                    vm.BaseVehicle.Name = new string[] { "vessel", "watercraft", "ship","craft" }.SelectRandom();
                    break;
                case 2:
                    vm.Movement = MovementByAir();
                    vm.BaseVehicle.Name = Utilities.RandomBool() ? "aircraft" : "craft";
                    break;
                case 3:
                    vm.Movement = MovementBySpace();
                    vm.BaseVehicle.Name = "";
                    break;
            }

            return vm;
        }

        #region Vehicle compnents
        //////////////////////////
        //  Vehicle components  //
        //////////////////////////

        public static VmGeneral Technology()
        {
            var db = new SparksDbContext();
            var result = db.VehicleTechnology.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral Power()
        {
            var db = new SparksDbContext();
            var result = db.VehiclePower.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral Range()
        {
            var db = new SparksDbContext();
            var result = db.VehicleRange.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral Speed()
        {
            var db = new SparksDbContext();
            var result = db.VehicleSpeed.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }
        
        public static VmGeneral Armor()
        {
            var db = new SparksDbContext();
            var result = db.VehicleArmor.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral Defenses()
        {
            var db = new SparksDbContext();
            var result = db.VehicleDefenses.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral Offenses()
        {
            var db = new SparksDbContext();
            var result = db.VehicleOffenses.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral Cargo()
        {
            var db = new SparksDbContext();
            var result = db.VehicleCargoCapacity.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral Characteristic()
        {
            var db = new SparksDbContext();
            var result=db.VehicleCharacteristic.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral Purpose()
        {
            var db = new SparksDbContext();
            var vm = new VmGeneral();

            switch (Utilities.random.Next(2))
            {
                case 0:
                    vm = VehiclePurpose();
                    break;
                case 1:
                    vm = BaseVehicle();
                    break;
            }
            vm.Name = vm.Name.ToLower();
            return vm;
        }

        public static VmGeneral Movement()
        {
            var db = new SparksDbContext();
            var vm = db.VehicleMovement.SelectRandom().ToViewModel();

            switch (vm.ID)
            {
                case 1: // Airjet
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " fanjet";
                    break;
                case 2: // Airship
                    vm.Name = db.MovementAirshipType.SelectRandom().Name;
                    vm.Name += " airship";
                    break;
                case 3: // Flight
                    vm.Name = db.MovementFlightType.SelectRandom().Name;
                    break;
                case 4: // Burrow
                    break;
                case 5: // Hover
                    break;
                case 6: // Vectored thrust
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " vectored thrust";
                    break;
                case 7: // Legs
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " legged";
                    break;
                case 8: // Reverse articulated legs
                    vm.Name = "(reverse articulated) ";
                    vm.Name += db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " legged";
                    break;
                case 9: // Light-runner
                    break;
                case 10: // Rockets
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " rocket";
                    break;
                case 11: // Sail
                    vm.Name = db.MovementSailType.SelectRandom().Name + "-sails";
                    break;
                case 12: // Space
                    vm.Name = db.MovementSpaceType.SelectRandom().Name + " space";
                    break;
                case 13: // Teleports
                    break;
                case 14: // Tentacles
                    vm.Name = (Utilities.random.Next(10) + 3) + " tentacled";
                    break;
                case 15: // Track
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " tracked";
                    break;
                case 16: // Turbofans
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " turbofan";
                    break;
                case 17: // Surface-aquatic
                    vm.Name = db.MovementWaterType.SelectRandom().Name;
                    break;
                case 18: // Waterjet
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " waterjet";
                    break;
                case 19: // Wheels
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " wheeled";
                    break;
                case 20: // Off-road wheels
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " wheeled off-road";
                    break;
                case 21: // Rails
                    vm.Name += " system";
                    break;
                case 22: // Serpentine
                    break;
                case 23: // Sub-aquatic
                    vm.Name += " " + db.MovementWaterType.SelectRandom().Name;
                    break;
                case 24: // Half-n-half, e.g., Half-track
                    vm.Name = "half-tracked system (" + db.MovementMultiSystemType.SelectRandom().Name + " wheels";
                    vm.Name += " and " + db.MovementMultiSystemType.SelectRandom().Name + " tracks)";
                    break;
                default:
                    break;
            }
            vm.Name = vm.Name.ToLower();
            return vm;
        }

        public static VmGeneral Condition()
        {
            var db = new SparksDbContext();
            var result = db.VehicleCondition.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral Material()
        {
            var db = new SparksDbContext();
            var result= db.VehicleMaterial.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral VehiclePurpose()
        {
            var db = new SparksDbContext();
            var vm = db.VehiclePurpose.SelectRandom().ToViewModel();
            vm.Name = vm.Name.ToLower() + " vehicle";
            return vm;
        }

        public static VmGeneral BaseVehicle()
        {
            var db = new SparksDbContext();
            var result=db.BaseVehicle.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        #endregion
        #region Vehicles by movement zone (land, sea, air, space)

        public static VmGeneral MovementByWater()
        {
            var db = new SparksDbContext();
            int[] validIDs = { 5, 11, 14, 17, 18, 22, 23 };
            var vm = db.VehicleMovement.Where(x=>validIDs.Contains(x.ID)).SelectRandom().ToViewModel();

            switch (vm.ID)
            {
                case 5: // Hover
                    break;
                case 11: // Sail
                    vm.Name = db.MovementSailType.SelectRandom().Name + "-sails";
                    break;
                case 14: // Tentacles
                    vm.Name = (Utilities.random.Next(10) + 3) + " tentacled";
                    break;
                case 17: // Surface-aquatic
                    vm.Name = db.MovementWaterType.SelectRandom().Name;
                    break;
                case 18: // Waterjet
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " waterjet";
                    break;
                case 22: // Serpentine
                    break;
                case 23: // Sub-aquatic
                    vm.Name += " " + db.MovementWaterType.SelectRandom().Name;
                    break;
                default:
                    break;
            }
            vm.Name = vm.Name.ToLower();
            return vm;
        }

        public static VmGeneral MovementByAir()
        {
            var db = new SparksDbContext();
            int[] validIDs = { 1, 2, 3, 6, 16 };
            var vm = db.VehicleMovement.Where(x => validIDs.Contains(x.ID)).SelectRandom().ToViewModel();

            switch (vm.ID)
            {
                case 1: // Airjet
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " fanjet";
                    break;
                case 2: // Airship
                    vm.Name = db.MovementAirshipType.SelectRandom().Name;
                    vm.Name += " airship";
                    break;
                case 3: // Flight
                    vm.Name = db.MovementFlightType.SelectRandom().Name;
                    break;
                case 6: // Vectored thrust
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " vectored thrust";
                    break;
                case 16: // Turbofans
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " turbofan";
                    break;
                default:
                    break;
            }
            vm.Name = vm.Name.ToLower();
            return vm;
        }

        public static VmGeneral MovementBySpace() {

            var db = new SparksDbContext();
            int[] validIDs = { 10, 12, 13 };
            var vm = db.VehicleMovement.Where(x => validIDs.Contains(x.ID)).SelectRandom().ToViewModel();

            switch (vm.ID)
            {
                case 10: // Rockets
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name+"-rockets";
                    break;
                case 12: // Space
                    vm.Name = db.MovementSpaceType.SelectRandom().Name;
                    break;
                case 13: // Teleports
                    break;
                default:
                    break;
            }

            string[] astros = { "rocket", "star", "space", "interplanetary" };
            string[] vessel = { "ship", "cruiser", "vessel" };
            vm.Name += " " + astros.SelectRandom() + "-" + vessel.SelectRandom();
            vm.Name = vm.Name.ToLower();
            return vm;
        }

        public static VmGeneral MovementByLand()
        {
            var db = new SparksDbContext();
            int[] validIDs = { 4, 5, 7, 8, 14, 15, 19, 20, 21, 22, 24 };
            var vm = db.VehicleMovement.Where(x => validIDs.Contains(x.ID)).SelectRandom().ToViewModel();

            switch (vm.ID)
            {
                case 4: // Burrow
                    break;
                case 5: // Hover
                    break;
                case 7: // Legs
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " legged";
                    break;
                case 8: // Reverse articulated legs
                    vm.Name = "(reverse articulated) ";
                    vm.Name += db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " legged";
                    break;
                case 14: // Tentacles
                    vm.Name = (Utilities.random.Next(10) + 3) + " tentacled";
                    break;
                case 15: // Track
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " tracked";
                    break;
                case 19: // Wheels
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " wheeled";
                    break;
                case 20: // Off-road wheels
                    vm.Name = db.MovementMultiSystemType.SelectRandom().Name;
                    vm.Name += " wheeled off-road";
                    break;
                case 21: // Rails
                    vm.Name += " system";
                    break;
                case 22: // Serpentine
                    break;
                case 24: // Half-n-half, e.g., Half-track
                    vm.Name = "half-tracked system (" + db.MovementMultiSystemType.SelectRandom().Name + " wheels";
                    vm.Name += " and " + db.MovementMultiSystemType.SelectRandom().Name + " tracks)";
                    break;
                default:
                    break;
            }
            vm.Name = vm.Name.ToLower();
            return vm;
        }

        #endregion
        #region Sizes of vehicles

        //////////////////////////////////////////////
        //  Generate different types of vehicles    //
        //////////////////////////////////////////////

        //  Return random crew information based on the vehicle size parameter provided
        public static VmGeneral Size(GeneralSize personnel = GeneralSize.Any)
        {
            var db = new SparksDbContext();
            var vm = new VmGeneral();

            switch (personnel)
            {
                case GeneralSize.Wearable:
                    int[] wIDs = new int[] { 0 };
                    vm = db.VehicleSize.Where(n => wIDs.Contains(n.ID)).SelectRandom().ToViewModel();
                    break;
                case GeneralSize.Ridable:
                    int[] rIDs = new int[] { 1, 2 };
                    vm = db.VehicleSize.Where(n => rIDs.Contains(n.ID)).SelectRandom().ToViewModel();
                    break;
                case GeneralSize.Personal:
                    int[] pIDs = new int[] { 3, 4 };
                    vm = db.VehicleSize.Where(n => pIDs.Contains(n.ID)).SelectRandom().ToViewModel();
                    break;
                case GeneralSize.Large:
                    int[] lIDs = new int[] { 4, 5, 6 };
                    vm = db.VehicleSize.Where(n => lIDs.Contains(n.ID)).SelectRandom().ToViewModel();
                    break;
                case GeneralSize.Huge:
                    int[] hIDs = new int[] { 6, 7, 8 };
                    vm = db.VehicleSize.Where(n => hIDs.Contains(n.ID)).SelectRandom().ToViewModel();
                    break;
                case GeneralSize.Colossal:
                    int[] cIDs = new int[] { 8, 9, 10 };
                    vm = db.VehicleSize.Where(n => cIDs.Contains(n.ID)).SelectRandom().ToViewModel();
                    break;
                case GeneralSize.Any:
                default:
                    vm = db.VehicleSize.SelectRandom().ToViewModel();
                    break;
            }
            vm.Name = vm.Name.ToLower();
            return vm;
        }

        //  returns a complete vehicle description, tailored
        //  as a wearable vehicle, like a jetpack
        public static VmVehicle WearableVehicle(float attr = 1)
        {
            var db = new SparksDbContext();
            var vm = new VmVehicle();
            if (Utilities.random.NextDouble() < attr) vm.Armor = db.VehicleArmor.SelectRandom().ToViewModel();
            //  cargo: 'none', 'tiny', 'very small'
            if (Utilities.random.NextDouble() < attr) vm.Cargo = db.VehicleCargoCapacity.Where(n => n.ID <= 3).SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Characteristic = db.VehicleCharacteristic.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Condition = db.VehicleCondition.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Defenses = db.VehicleDefenses.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Material = db.VehicleMaterial.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Movement = Movement();
            if (Utilities.random.NextDouble() < attr) vm.Offenses = db.VehicleOffenses.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Size = Size(GeneralSize.Wearable);
            if (Utilities.random.NextDouble() < attr) vm.Power = db.VehiclePower.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Range = db.VehicleRange.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Speed = db.VehicleSpeed.Where(n => n.ID < 6).SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Technology = db.VehicleTechnology.SelectRandom().ToViewModel();
            vm.BaseVehicle = Purpose();

            return vm;
        }

        //  returns a complete vehicle description, tailored
        //  as a small lightweight vehicle the user rides,
        //  such as a motorcycle, bicycle, hanglider, etc
        public static VmVehicle RidableVehicle(float attr = 1)
        {
            var db = new SparksDbContext();
            var vm = new VmVehicle();
            if (Utilities.random.NextDouble() < attr) vm.Armor = db.VehicleArmor.SelectRandom().ToViewModel();
            //  cargo: 'none', 'tiny', 'very small', 'small'
            if (Utilities.random.NextDouble() < attr) vm.Cargo = db.VehicleCargoCapacity.Where(n => n.ID <= 4).SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Characteristic = db.VehicleCharacteristic.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Condition = db.VehicleCondition.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Defenses = db.VehicleDefenses.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Material = db.VehicleMaterial.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Movement = Movement();
            if (Utilities.random.NextDouble() < attr) vm.Offenses = db.VehicleOffenses.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Size = Size(GeneralSize.Ridable);
            if (Utilities.random.NextDouble() < attr) vm.Power = db.VehiclePower.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Range = db.VehicleRange.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Speed = db.VehicleSpeed.Where(n => n.ID < 7).SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Technology = db.VehicleTechnology.SelectRandom().ToViewModel();
            vm.BaseVehicle = Purpose();

            return vm;
        }

        //  returns a complete vehicle description, tailored
        //  as a small personal vechile, like a typical car,
        //  sedan, SUV, up to as large an RV
        public static VmVehicle PersonalVehicle(float attr = 1)
        {
            var db = new SparksDbContext();
            var vm = new VmVehicle();
            if (Utilities.random.NextDouble() < attr) vm.Armor = db.VehicleArmor.SelectRandom().ToViewModel();
            //  cargo: 'none', 'tiny', 'very small', 'small'
            if (Utilities.random.NextDouble() < attr) vm.Cargo = db.VehicleCargoCapacity.Where(n => n.ID <= 6).SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Characteristic = db.VehicleCharacteristic.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Condition = db.VehicleCondition.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Defenses = db.VehicleDefenses.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Material = db.VehicleMaterial.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Movement = Movement();
            if (Utilities.random.NextDouble() < attr) vm.Offenses = db.VehicleOffenses.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Size = Size(GeneralSize.Personal);
            if (Utilities.random.NextDouble() < attr) vm.Power = db.VehiclePower.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Range = db.VehicleRange.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Speed = db.VehicleSpeed.Where(n => n.ID < 7).SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Technology = db.VehicleTechnology.SelectRandom().ToViewModel();
            vm.BaseVehicle = Purpose();

            return vm;
        }

        //  returns a complete vehicle
        //  any "small" capital vehicle or "large" combat vehicle
        //  with a typical crew from 5-25
        public static VmVehicle LargeVehicle(float attr = 1)
        {
            var db = new SparksDbContext();
            var vm = new VmVehicle();
            if (Utilities.random.NextDouble() < attr) vm.Armor = db.VehicleArmor.SelectRandom().ToViewModel();
            //  tiny -> utility vehicle
            if (Utilities.random.NextDouble() < attr) vm.Cargo = db.VehicleCargoCapacity.Where(n => n.ID >= 1 && n.ID <= 7).SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Characteristic = db.VehicleCharacteristic.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Condition = db.VehicleCondition.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Defenses = db.VehicleDefenses.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Material = db.VehicleMaterial.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Movement = Movement();
            if (Utilities.random.NextDouble() < attr) vm.Offenses = db.VehicleOffenses.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Size = Size(GeneralSize.Large);
            if (Utilities.random.NextDouble() < attr) vm.Power = db.VehiclePower.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Range = db.VehicleRange.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Speed = db.VehicleSpeed.Where(n => n.ID > 2 && n.ID < 8).SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Technology = db.VehicleTechnology.SelectRandom().ToViewModel();
            vm.BaseVehicle = Purpose();

            return vm;
        }

        //  returns a complete vehicle
        //  any large vehicle such as a warship or cruiseliner
        public static VmVehicle HugeVehicle(float attr = 1)
        {
            var db = new SparksDbContext();
            var vm = new VmVehicle();
            if (Utilities.random.NextDouble() < attr) vm.Armor = db.VehicleArmor.SelectRandom().ToViewModel();
            //
            if (Utilities.random.NextDouble() < attr) vm.Cargo = db.VehicleCargoCapacity.Where(n => n.ID >= 8).SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Characteristic = db.VehicleCharacteristic.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Condition = db.VehicleCondition.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Defenses = db.VehicleDefenses.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Material = db.VehicleMaterial.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Movement = Movement();
            if (Utilities.random.NextDouble() < attr) vm.Offenses = db.VehicleOffenses.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Size = Size(GeneralSize.Huge);
            if (Utilities.random.NextDouble() < attr) vm.Power = db.VehiclePower.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Range = db.VehicleRange.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Speed = db.VehicleSpeed.Where(n => n.ID > 3 && n.ID < 9).SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Technology = db.VehicleTechnology.SelectRandom().ToViewModel();
            vm.BaseVehicle = Purpose();

            return vm;
        }

        //  returns a complete vehicle
        //  any large vehicle such as a warship or cruiseliner
        public static VmVehicle ColossalVehicle(float attr = 1)
        {
            var db = new SparksDbContext();
            var vm = new VmVehicle();
            if (Utilities.random.NextDouble() < attr) vm.Armor = db.VehicleArmor.SelectRandom().ToViewModel();
            //
            if (Utilities.random.NextDouble() < attr) vm.Cargo = db.VehicleCargoCapacity.Where(n => n.ID >= 8).SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Characteristic = db.VehicleCharacteristic.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Condition = db.VehicleCondition.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Defenses = db.VehicleDefenses.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Material = db.VehicleMaterial.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Movement = Movement();
            if (Utilities.random.NextDouble() < attr) vm.Offenses = db.VehicleOffenses.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Size = Size(GeneralSize.Colossal);
            if (Utilities.random.NextDouble() < attr) vm.Power = db.VehiclePower.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Range = db.VehicleRange.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Speed = db.VehicleSpeed.Where(n => n.ID > 2 && n.ID < 7).SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < attr) vm.Technology = db.VehicleTechnology.SelectRandom().ToViewModel();
            vm.BaseVehicle = Purpose();

            return vm;
        }

        #endregion
        #region Military vehicles

        //  returns a complete vehicle, such as a tank
        public static VmVehicle CombatVehicle(int attrCount = 1)
        {
            var db = new SparksDbContext();
            var vm = AnyVehicle(attrCount);
            vm.Armor = db.VehicleArmor.Where(n => n.Strength > 4).SelectRandom().ToViewModel();
            vm.Defenses = db.VehicleDefenses.SelectRandom().ToViewModel();
            vm.Offenses = db.VehicleOffenses.SelectRandom().ToViewModel();
            vm.BaseVehicle = db.VehicleMilitary.SelectRandom().ToViewModel();

            return vm;
        }
        
        public static VmVehicle Warship(int attrCount)
        {
            var db = new SparksDbContext();
            var vm = CombatVehicle(attrCount);
            vm.BaseVehicle = db.VehicleWarship.SelectRandom().ToViewModel();

            int count = int.Parse(vm.Offenses.GameEffect);
            if (count > 0)
            {
                List<string> weapons = Weapon.GenericCapitalWeaponGroup(count);
                StringBuilder list = new StringBuilder();
                for(int i=0;i<weapons.Count; i++)
                {
                    list.Append(weapons[i]);
                    if (i < weapons.Count - 1) list.Append(", ");
                }
                vm.Offenses.Description = list.ToString();
            }

            return vm;
        }

        #endregion
        #region Commerical Vehicles

        public static string AnyCommericalAircraft()
        {
            var db = new SparksDbContext();
            //  size and type
            string aircraft = "";
            if (Utilities.RandomBool())
            {
                aircraft = db.MovementFlightType.SelectRandom().Name + " aircraft";
            }else
            {
                aircraft = db.MovementAirshipType.SelectRandom().Name + " airship";
            }
            string aircraftSize = db.VehicleSize.Where(x => x.ID > 3).SelectRandom().Name;
            return aircraftSize + " " + aircraft;
        }

        public static string AnyCommericalNauticalShip()
        {
            var db = new SparksDbContext();
            //  size and type
            string movement;
            if (Utilities.RandomBool())
            {
                movement = db.MovementWaterType.SelectRandom().Name;
            }
            else
            {
                movement = db.MovementSailType.SelectRandom().Name;
            }
            string size = db.VehicleSize.Where(x => x.ID > 3).SelectRandom().Name;
            return size + " " + movement + " ship";
        }

        public static string AnyCommericalSpaceShip()
        {
            var db = new SparksDbContext();
            //  size and type
            string movement = db.MovementSpaceType.SelectRandom().Name;
            string size = db.VehicleSize.Where(x => x.ID > 3).SelectRandom().Name;
            return size + " " + movement + " " + (Utilities.RandomBool()?"space":"star")+ "ship";
        }
        #endregion
    }
}