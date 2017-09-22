using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Sparks.Models.WorkModel
{
    public static class Game
    {
        public enum GameType { Any, Wargame, TurretDefense }

        public static VmDefenseGame AnyDefenseGame()
        {
            var vm = new VmDefenseGame();
            
            vm.Theme = Theme.AnyStoryDrivenTheme().Description;

            //  Defenders are [ species ]
            vm.Defenders = Species.AnySpecies();

            //  Towers are [ title | vehicle ] with [ weapon ]
            vm.Towers = new List<string>();
            for (int i = Utilities.random.Next(6, 10); i > 0; i--)
            {
                var tower = Title.AnyTitle().Name;
                tower += " with " + Weapon.AnyWeapon().Name;
                vm.Towers.Add(tower);
            }

            //  Attackers are [species]
            vm.Attackers = Species.AnySpecies();

            //  Creeps are [title | vehicle] with [ weapon ]
            vm.Creeps = new List<string>();
            for (int i = vm.Towers.Count + 3; i > 0; i--)
            {
                var creep = Title.AnyTitle().Name;
                creep += " with " + Weapon.AnyWeapon().Name;
                vm.Creeps.Add(creep);
            }

            vm.Location = Location.UniqueLocation();

            //  Board
            //  single path | multi path | open field
            switch (Utilities.random.Next(3))
            {
                case 0:
                    vm.Board = "single path";
                    break;
                case 1:
                    vm.Board = "multiple paths";
                    break;
                case 2:
                    vm.Board = "open field";
                    break;
            }

            //  game title
            vm.Name = vm.Defenders.Name + " vs " + vm.Attackers.Name;

            return vm;
        }
        
        public static VmWargame AnyWargame()
        {
            var db = new SparksDbContext();
            var vm = new VmWargame();

            var factionCount = Utilities.random.Next(4) + 2;

            var factionType = (FactionType)Utilities.random.Next(3);

            for (int i = factionCount; i > 0; i--)
            {
                vm.Factions.Add(AnyFaction(factionType));
            }

            vm.Theme = Theme.AnyStoryDrivenTheme().Description;
            vm.Setting = Location.UniqueLocation();

            //  TODO
            //  Some added condition, character or rule that disrupts the traditional
            //  idea of a wargame
            vm.Twist = LoremIpsum.Paragraph(25);

            return vm;
        }

        public enum FactionType { Title, Creature, Species, Vehicle }

        public static VmGeneral AnyFaction(FactionType factionType)
        {
            var vm = new VmGeneral();
            vm.Name = Jabberwock.AnyWord();
            string description = "";

            switch (factionType)
            {
                case FactionType.Title:
                    description = Title.AnyTitle().Name;
                    description += " with ";
                    description += Weapon.AnyWeapon().Name;
                    break;
                case FactionType.Creature:
                    description = Species.AnyCreature().Name;
                    description += " with ";
                    description += Weapon.AnyWeapon().Name;
                    break;
                case FactionType.Species:
                    description = Species.AnySpecies().Description;
                    description += " with ";
                    description += Weapon.AnyWeapon().Name;
                    break;
                case FactionType.Vehicle:
                    description += Vehicle.AnyVehicle().Name;
                    break;
            }
            vm.Description = description;
            return vm;
        }

        //  TODO
        //  Parse the remaining concept templates from Quick Notes and add generators
        //  to this application.

        /// <summary>
        /// Returns a list of idea components for a traditional game.
        /// 
        /// VmGeneral Commitement;
        /// List<VmGeneral> Components;
        /// VmTheme Theme;
        /// VmGeneral GameType;
        /// VmGeneral Setting;
        /// VmMechanics Mechanics;
        /// VmGeneral Player;
        /// VmWeapon Equipment;
        /// </summary>
        public static VmIdeaTraditional AnyTraditional()
        {
            var result = new VmIdeaTraditional();
            var db = new SparksDbContext();
            int chances = 4;

            while (result.IsEmpty)
            {
                //  commitment
                if (Utilities.OneChanceIn(chances))
                {
                    result.Commitment = db.GameCommitment.SelectRandom().ToViewModel();
                }

                //  components
                if (Utilities.OneChanceIn(chances))
                {
                    result.Components = new List<VmGeneral>();
                    var componentList = db.GameComponent.ToList();
                    for (int i = Utilities.random.Next(2) + 1; i > 0; i--)
                    {
                        result.Components.Add(componentList.SelectRandom().ToViewModel());
                    }
                }

                //  theme
                if (Utilities.OneChanceIn(chances)) result.Theme = Theme.AnyStoryDrivenTheme();

                //  gametype
                if (Utilities.OneChanceIn(chances)) result.GameType = Theme.GameplayCategory();

                //  location
                if (Utilities.OneChanceIn(chances)) result.Location = Location.UniqueLocation();

                //  mechanics
                if (Utilities.OneChanceIn(chances)) result.Mechanics = Mechanics.AnyMechanics();

                //  player & equipment
                if (Utilities.OneChanceIn(chances))
                {
                    result.Player = new VmGeneral();
                    VmVehicle vehicle;
                    switch (Utilities.random.Next(7))
                    {
                        case 0:
                        case 1:
                            //  archetype
                            var title = Title.AnyTitle();
                            result.Player.Name = Jabberwock.AnyWord() + ", the " + title.Description;
                            if (Utilities.RandomBool()) result.Equipment = Weapon.AnyItem();
                            break;
                        case 2:
                        case 3:
                            //  species
                            VmGeneral species = Species.AnySpecies();
                            result.Player.Name = Jabberwock.AnyWord() + ", " + species.DescPrep + " " + species.Description;
                            if (Utilities.RandomBool()) result.Equipment = Weapon.AnyItem();
                            break;
                        case 4:
                            //  vehicle by movement
                            vehicle = Vehicle.VehicleByMovement();
                            result.Player.Name = "(Vehicle) " +vehicle.Name;
                            break;
                        case 5:
                            //  combat vehicle
                            vehicle = Vehicle.CombatVehicle(2);
                            result.Player.Name = "(Combat Vehicle) " +vehicle.Aesthetic + " " + vehicle.BaseVehicle.Name;
                            break;
                        case 6:
                            //  warship
                            vehicle = Vehicle.Warship(2);
                            result.Player.Name = "(Warship) " + vehicle.NameAsCombat;
                            break;
                        default:
                            break;
                    }
                }
            }
            return result;
        }
        
        /// <summary>
        /// Returns a list of idea components for a videogame.
        /// </summary>
        public static VmIdeaTraditional AnyDigital()
        {
            var result = new VmIdeaTraditional();
            var db = new SparksDbContext();
            int chances = 3;
            var componentList = db.GameSystem.ToList();

            while (result.IsEmpty)
            {
                //  components (game feautres, e.g., campaign mode, character customization
                if (Utilities.OneChanceIn(chances))
                {
                    //  generate number from 0 to 3
                    for (int i = Utilities.random.Next(2) + 1; i > 0; i--)
                    {
                        if (result.Components == null) result.Components = new List<VmGeneral>();
                        var selected = componentList.SelectRandom();
                        result.Components.Add(selected.ToViewModel());
                        componentList.Remove(selected);
                    }
                }

                //  theme
                if (Utilities.OneChanceIn(chances)) result.Theme = Theme.AnyStoryDrivenTheme();

                //  gametype
                if (Utilities.OneChanceIn(chances))
                {
                    if (Utilities.RandomBool()) result.GameType = Theme.GameplayCategory();
                    else result.GameType = Theme.GameplaySubcategory();
                }

                //  location
                if (Utilities.OneChanceIn(chances)) result.Location = Location.UniqueLocation();
                
                //  player & equipment
                if (Utilities.OneChanceIn(chances))
                {
                    result.Player = new VmGeneral();
                    VmVehicle vehicle;
                    switch (Utilities.random.Next(7))
                    {
                        case 0:
                        case 1:
                            //  archetype
                            var title = Title.AnyTitle();
                            result.Player.Name = Jabberwock.AnyWord() + ", the " + title.Description;
                            if (Utilities.RandomBool()) result.Equipment = Weapon.AnyItem();
                            break;
                        case 2:
                        case 3:
                            //  species
                            VmGeneral species = Species.AnySpecies();
                            result.Player.Name = Jabberwock.AnyWord() + ", " + species.DescPrep + " " + species.Description;
                            if (Utilities.RandomBool()) result.Equipment = Weapon.AnyItem();
                            break;
                        case 4:
                            //  vehicle by movement
                            vehicle = Vehicle.VehicleByMovement();
                            result.Player.Name = "(Vehicle) " + vehicle.Name;
                            break;
                        case 5:
                            //  combat vehicle
                            vehicle = Vehicle.CombatVehicle(2);
                            result.Player.Name = "(Combat Vehicle) " + vehicle.Aesthetic + " " + vehicle.BaseVehicle.Name;
                            break;
                        case 6:
                            //  warship
                            vehicle = Vehicle.Warship(2);
                            result.Player.Name = "(Warship) " + vehicle.NameAsCombat;
                            break;
                        default:
                            break;
                    }
                }
            }
            return result;
        }
    }
}