using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System.Linq;

namespace Sparks.Models.WorkModel
{
    public static class Location
    {
        public static VmGeneral UniqueLocation ()
        {
            var db = new SparksDbContext();
            var vm = new VmGeneral();
            var location = db.SettingLocation.SelectRandom();
            vm.Name = location.Name.ToLower(); ;
            string[] similar;
            string adjective;
            VmGeneral general;

            switch (location.ID)
            {
                case 9: // City
                        //  Other table: Use Settings_CitySize
                    general = Location.CitySize();
                    vm.Name = general.Name + " of ^" + Polyglot.AnyWord().ToProper().Insert(1,"^");
                    vm.Description = general.Description;
                    break;
                case 13: // Ship
                         // Other table: Use vehicle conditions
                         // Subselection: Cruise ship, Research Vessel, Laboratory ship, Prison ship, Container ship, Passenger liner, Cargo ship, Aggriculture ship, Colony ship, Warship, Plague ship, Destroyed ship, Derelict ship, Pirate ship, Alien ship
                    similar = location.Similar.Split(',');
                    vm.Name = Vehicle.Condition().Name + " " + similar.SelectRandom().Trim().ToLower();
                    break;
                case 14: // Crystal Planet
                         //  Other table: Use vehicle_materials	
                         //  Subselection: Crystal Planet, Metal Planet
                    vm.Name = Vehicle.Material().Name + " " + location.Name.ToLower() + " called \"^" + Polyglot.AnyWord().ToProper().Insert(1, "^") + "\"";
                    break;
                case 15: // Lighthouse
                         // Other table: Use Setting Adjective
                case 39: // Monument
                         // Other table: Use Setting Adjectives
                case 50: // Volcano
                         // Other table: Use Setting Adjectives
                case 52: // Tomb
                         // Other table: Use Setting Adjectives
                    vm.Name = location.Name.ToLower();
                    break;
                case 18: // World
                         // Other table: Use Species Template
                    vm.Name = Species.AnyCreature().Name.ToLower() + " world";
                    break;
                case 19: // Water
                         // Other table: Use Planet Water Body
                    vm.Name = World.Waterbody().Name.ToLower();
                    break;
                case 21: // Foreign City
                    general = Location.CitySize();
                    vm.Name = "foreign " + general.Name.ToLower() + " of \"^" + Polyglot.AnyWord().ToProper().Insert(1, "^") + "\"";
                    vm.Description = general.Description;
                    break;
                case 23: // Planet orbiting star
                         // Other table: Use Planet and Stars
                    general = World.Star();
                    vm.Name = World.Planet().Name + " orbiting " + general.Name.ToPrep() + " " + general.Name;
                    break;
                case 31: // Moving vehicle
                         // Other table: Use vehicle purposes
                    vm.Name = Vehicle.Purpose().Name + " enroute somewhere";
                    vm.Name = vm.Name.ToLower();
                    break;
                case 33: // Power reactor
                         // Other table: Use power types
                         // Subselection: Power plant, power system
                    similar = location.Similar.Split(',');
                    vm.Name = Society.Energy().Name + " " + similar.SelectRandom().Trim();
                    vm.Name = vm.Name.ToLower();
                    break;
                case 53: // Capitol
                         // Subselection: Hegemony, Planetary, National, Regional, State
                    similar = location.Similar.Split(',');
                    vm.Name = similar.SelectRandom().Trim() + " " + location.Name;
                    vm.Name = vm.Name.ToLower();
                    break;
                case 36: // Orbit around
                         // Subselection: planet, star, blackhole, space anomoly, galactic core
                case 42: // Remote 
                         // Subselection: Castle, Palace, Base, Fort, Outpost
                case 71: // world of
                         // Subselection: insanity, mirrors, puppets, fantasy, nightmares, fairytales                
                    similar = location.Similar.Split(',');
                    vm.Name = location.Name + " " + similar.SelectRandom().Trim();
                    vm.Name = vm.Name.ToLower();
                    break;
                case 37: // Observatory
                         // Subselection: Asteroid, Orbital, Planetary, Solar
                case 38: // Spacestation
                         // Subselection: deep-space, orbital
                case 55: // HQ
                         // Subselection: Superhero, super villian
                case 48: // Construction yard
                         // Subselection: starship, navy ship, military ship, exploration ship, transport ship
                case 57: // Bridge
                         // Subselection: Hyperspace, Truss, Suspension, Antigravity, space elevator
                    similar = location.Similar.Split(',');
                    vm.Name = similar.SelectRandom().Trim() + " " + location.Name;
                    vm.Name = vm.Name.ToLower();
                    break;
                case 46: // Ruins	Other table: Use setting adjected	Subselection: Ruins, Temple, Monument
                    similar = location.Similar.Split(',');
                    vm.Name = similar.SelectRandom().Trim();
                    vm.Name = vm.Name.ToLower();
                    break;
                case 47: // Science
                         // Other table: or "Research"	Subselection: Lab, Facility
                    similar = location.Similar.Split(',');
                    vm.Name = "research " + similar.SelectRandom().Trim();
                    vm.Name = vm.Name.ToLower();
                    break;
                case 58: // Water surface
                         // Other table: Use World_WaterBody'
                    adjective = World.Waterbody().Name;
                    vm.Name = "surface of " + adjective.ToPrep() + " " + adjective;
                    break;
                case 56: // Surface of a star	Other table: Use star types
                    adjective = World.Star().Name;
                    vm.Name = "surface of " + adjective.ToPrep() + " " + adjective;
                    break;
                case 66: // Topiary
                         // Other table: maze or labyrinth
                         // Subselection: Topiary, dungeon, ancient
                    similar = location.Similar.Split(',');
                    vm.Name = similar.SelectRandom().Trim() + " " + (Utilities.RandomBool() ? "maze" : "labyrinth");
                    vm.Name = vm.Name.ToLower();
                    break;
                case 67: // World
                         // Other table: use Magic_Energy
                         // Subselection: World, rift, zone
                    similar = location.Similar.Split(',');
                    vm.Name = similar.SelectRandom().Trim() + " of " + Weapon.MagicEnergy().Name.ToLower();
                    vm.Name = vm.Name.ToLower();
                    break;
                case 68: // Crater
                         // Setting Adjectives
                    vm.Name = location.Name;
                    break;
                case 69: // World of
                         // Other table: use Magic_Titles
                case 70: // world of
                         // Other table: use RangedTitles
                case 72: // world of
                         // Other table: use Animals
                case 73: // world of
                         // Other table: use Titles
                case 75: // world of 
                         // Other table: use Savages
                case 76: // world of 
                         // Other table: use heroic
                    string title = "";
                    switch (Utilities.random.Next(6))
                    {
                        case 0:
                            title = Title.AnyMagic().Name;
                            break;
                        case 1:
                            title = Title.AnyShooter().Name;
                            break;
                        case 2:
                            var s = Species.AnySpecies();
                            title = "^" + s.Name.FirstLetter() + "^" + s.Name.Substring(1) + " (" + s.Description + ")";
                            break;
                        case 3:
                            title = Title.AnyTitle().Name;
                            break;
                        case 4:
                            title = Title.AnySavage().Name;
                            break;
                        case 5:
                            title = Title.AnyHeroic().Name;
                            break;
                    }
                    vm.Name = "world of the " + title;
                    break;
                case 1: //  Abyss
                        //  Subselection: Abyss, Underworld, Hades, Hell, Outworld, Land of the Forgotten
                case 2: //  Tradeport
                        //  Subselection: Seaport, Airport, Heliport, Spaceport, Starport, Sub-port, Tele-portal
                case 3: //  Planet	
                        //  Subselection: Planet, planetoid, sub-planet, comet, rock, super-planet, mega-planet, moon, super-moon, asteroid
                case 4: //  War
                        //  Subselection: Battlefield, DMZ, war
                case 5: //  Playgrounc
                        //  Subselection: Playground, boardwalk, garden, park, campsight, beach
                case 6: //  Airplane
                        //  Subselection: Airplane, Boat, Landcrawler, Submarine, Digger
                case 7: //  Lair
                        //  Subselection: Lair, Underground mines, Catacomb, Cave, Crag, Ravine
                case 8: //  Fair
                        //  Subselection: Fair, Circus, Carnival
                case 12: // Temple
                         //  Subselection: Temple, Monastery, Church, Convent
                case 16: // Cyberspace
                         // Subselection: Cyberspace, deep space, low earth orbit, interstellar space, intergalactic space
                case 17: // Megastructure
                         // Subselection: Dam, Undersea tunnel, Super sky scraper, Artificial island, Super suspension bridge, super prison, capital building, Mine, Megafactory, Great Defensive Wall
                case 20: // Jail
                         // Subselection: County Jail, State Prison, Federal Penitentiary, Remote Maximum Security Prison, Small town jail
                case 22: // Freeway
                         // Subselection: Freeway, highway, county road, backroad, small street, back alley, interstate, toll road, main street
                case 29: // Condo
                         // Subselection: Condo, Adobe, Shack, Mansion, House
                case 30: // Haunted house
                         // Subselection: Haunted house, funeral home, graveyard, burial ground, cemetary, crypt, moseleum
                case 32: // Aquarium
                         // Subselection: Aquarium, Zoo, museum, aviary, terarium
                case 34: // Olympus
                         // Subselection: Olympus, Northpole, mountain, plateau
                case 35: // Collisium
                         // Subselection: Collisium, Ampitheater, Operahouse, Concert hall
                case 40: // Municiple
                         // Subselection: Police station, Fire station, Hospital, Library, Embassy, Clocktower, Town hall, Historical museum
                case 51: // Slum
                         // Subselection: Slum, Ghetto
                case 54: // Subway
                         // Subselection: Subway, monorail, skyrail
                case 74: // garden
                         // Subselection: null
                default:
                    vm.Name = location.Name.ToLower();
                    if (!string.IsNullOrWhiteSpace(location.Similar))
                    {
                        similar = location.Similar.Split(',');
                        if (similar.Length > 0)
                        {
                            vm.Name = similar.SelectRandom().Trim().ToLower();
                        }
                    }
                    break;
            }

            string adj1 = Location.Adjective().Name;
            string adj2;
            do
            {
                adj2 = Location.Adjective().Name;
            } while (adj1.Equals(adj2, System.StringComparison.CurrentCultureIgnoreCase));

            if (Utilities.RandomBool()) vm.Name = adj1 + " " + vm.Name;
            if (Utilities.RandomBool()) vm.Name = adj2 + " " + vm.Name;

            return vm;
        }

        public static VmGeneral Adjective ()
        {
            var db = new SparksDbContext();
            var result = db.WorldSettingAdjective.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral CitySize()
        {
            var db = new SparksDbContext();
            var result = db.SettingCitySize.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }
    }
}