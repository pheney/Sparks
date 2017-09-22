using Newtonsoft.Json;
using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Sparks.Models.WorkModel
{
    public static class Title
    {
        /// <summary>
        /// Returns one of the following archetypes:
        ///     Professional, Heroic,
        ///     Savage, Melee, Shooter, Magic, Political
        /// </summary>
        public static VmTitle AnyTitle(int[] validOptions= null)
        {
            var vm = new VmTitle();

            int selection;
            if (validOptions == null) selection = Utilities.random.Next(4);
            else selection = validOptions.SelectRandom();


            switch (selection)
            {
                case 0:
                    vm = AnyHeroic();
                    break;
                case 1:
                    vm = AnyMagic();
                    break;
                case 2:
                    vm = AnyMelee();
                    break;
                case 3:
                    vm = AnySavage();
                    break;
                case 4:
                    vm = AnyShooter();
                    break;
                case 5:
                    vm = AnyPolitical();
                    break;
                case 6:
                    vm = AnyProfession();
                    break;
                case 7:
                    vm = new VmTitle();
                    AgePersonalityExperience(vm);
                    vm.Title= AnyTechProfession();
                    break;
                case 8:
                    vm = new VmTitle();
                    AgePersonalityExperience(vm);
                    vm.Title = new VmGeneral();
                    vm.Title.Name = AnyScientist();
                    break;
                case 9:
                    vm = new VmTitle();
                    AgePersonalityExperience(vm);
                    vm.Title = new VmGeneral();
                    vm.Title.Name = AnyEngineerProfession();
                    break;
                case 10:
                    vm = new VmTitle();
                    AgePersonalityExperience(vm);
                    vm.Title = new VmGeneral();
                    vm.Title.Name = AnyMedicalProfession();
                    break;
            }
            return vm;
        }

        /// <summary>
        /// Returns a random profession by first choosing a profession type,
        /// then selecting a random profession from that type.
        /// returns: [age|personality|experience] [profession]
        /// </summary>
        public static VmTitle AnyProfession()
        {
            var db = new SparksDbContext();
            VmTitle vmTitle = new VmTitle();
            vmTitle.Title = new VmGeneral();

            AgePersonalityExperience(vmTitle);

            int[] ignoreIDs = {
                1,  //  technologist
                5,  //  scientist
                6,  //  medical
                11  //  engineer
            };

            //  determine the profession type
            do
            {
                vmTitle.ProfessionTypeID = db.ProfessionType.SelectRandom().ID;
            } while (ignoreIDs.Contains(vmTitle.ProfessionTypeID));

            //  flesh out the profession with additional details
            switch (vmTitle.ProfessionTypeID)
            {
                case 2: // Facility Staff
                    vmTitle.Title = AnyFacilityStaff();
                    break;
                case 3: // Nautical Crew
                    vmTitle.Title.Name = AnyNauticalCrew();
                    break;
                case 4: // Production Crew
                    //  theatre crew (production assistant)
                    vmTitle.Title = ProductionCrew();
                    break;
                case 7: // Professional
                case 8: // Trade
                case 9: // Art
                case 10: //  Religion
                    //  catch-all categories for one-off professions, artists, etc
                    vmTitle.Title = db.ProfessionMisc.Where(x => x.ProfessionTypeID == vmTitle.ProfessionTypeID).SelectRandom().ToViewModel();
                    break;
            }

            if (vmTitle.ProfessionTypeID < 7) return vmTitle;

            //  continue only if the profession type ID is 7 (Professional),
            //  8 (Trade), 9 (Artist) or 10 (Religion)

            switch (vmTitle.Title.ID)
            {
                case 9: // Aircraft pilot - professional
                    string aircraft = Vehicle.AnyCommericalAircraft();
                    vmTitle.Title.Name += " " + (Utilities.RandomBool() ? "on" : "of")
                        + " " + Utilities.ToPrep(aircraft)
                        + " " + aircraft;
                    break;
                case 14: // vehicle captain
                case 197: // vehicle captain
                    switch (Utilities.random.Next(3))
                    {
                        case 0: vmTitle.Title.Name = "Admiral"; break;
                        case 1: vmTitle.Title.Name = "Commodore"; break;
                        case 2: vmTitle.Title.Name = "Commander"; break;
                        case 3: vmTitle.Title.Name = "Captain"; break;
                    }
                    vmTitle.Title.Name += " " + (Utilities.RandomBool() ? Vehicle.AnyCommericalNauticalShip() : Vehicle.AnyCommericalSpaceShip());
                    break;
                case 18: // Artist
                         //  art type
                    break;
                case 23: // Author -- art
                         //  art type/subject
                    string subject = AnyAuthorSubject();
                    vmTitle.Title.Name += " of " + subject;
                    break;
                case 73: // Engineer -- professional
                    break;
                case 122: // Powerplant Technician
                case 123: // Maintenance engineering
                          //  ENERGY, SCIENCE, TECHNOLOGY
                    break;
                case 128: // Materials engineer -- profession
                          //  MATERIAL
                    if (Utilities.RandomBool())
                    {
                        vmTitle.Title.Name += " specializing in " + Vehicle.Material().Name + " materials";
                    }
                    break;
                case 129: // Mechanic -- trade
                          //  POWER, MOVEMENT, MECHANIC
                    vmTitle.Title.Name = AnyMechanicTrade() + " mechanic";
                    break;
                case 168: // Pilot
                          //  AIRCRAFT
                    vmTitle.Title.Name = Vehicle.AnyCommericalAircraft() + " pilot";
                    break;
                case 174: // Police officer -- trade
                          //  POLICE TYPE / AGENCY TYPE
                          //    TODO
                    break;
                case 175: // Power Distributor & Dispatcher -- trade
                          //  ENERGY TYPE
                          //    TODO
                    break;
                case 217: // Transport -- trade
                          //  TYPE
                          //    TODO
                    break;

            }
            return vmTitle;
        }
        
        public static string AnyMechanicTrade()
        {
            string specialty = "";
            switch (Utilities.random.Next(3))
            {
                case 0: //  power
                    specialty = Vehicle.Power().Name + " system";
                    break;
                case 1: //  movement systems
                    specialty = Vehicle.Movement().Name + " system";
                    break;
                case 2: //  
                    break;
            }
            return specialty;
        }

        public static string AnyAuthorSubject()
        {
            var theme = Theme.AnyStoryDrivenTheme();
            string subject = "";
            if (Utilities.RandomBool())
            {
                subject = theme.LiterarySetting.Name + " stories";
            }
            else
            {
                subject = theme.EvocativeMood.Name + " books";
            }
            return subject;
        }

        //  Add medical specialties to database table Data_Science_Medical
        //  doctor|nurse|etc
        public static string AnyMedicalProfession()
        {
            var db = new SparksDbContext();
            string profession = db.DataScienceMedical.SelectRandom().Name;
            if (Utilities.RandomBool())
            {
                //  add prefix like 'xeno'
                bool containsPhysician = profession.IndexOf("physician") > 0;
                if (containsPhysician)
                {
                    profession = profession.Replace("physician", "xeno-physician");
                }
                else
                {
                    profession = "xeno-" + profession;
                }
            }
            return profession;
        }

        private static bool EngineerProfessionNeedsDetails(string profession)
        {
            bool containsPower = profession.IndexOf("{power}") > -1;
            bool containsEnergy = profession.IndexOf("{energy}") > -1;
            bool containsMovement = profession.IndexOf("{movement}") > -1;
            bool containsFields = profession.IndexOf("{fields}") > -1;
            return containsPower || containsEnergy || containsMovement || containsFields;
        }

        public static string AnyEngineerProfession()
        {
            var db = new SparksDbContext();
            string profession = db.DataEngineer.SelectRandom().Name;
            if (!EngineerProfessionNeedsDetails(profession)) return profession +" engineer";

            bool containsPower = profession.IndexOf("{power}") > -1;
            bool containsEnergy = profession.IndexOf("{energy}") > -1;
            bool containsMovement = profession.IndexOf("{movement}") > -1;
            bool containsFields = profession.IndexOf("{fields}") > -1;

            if (containsPower)
            {
                string power = db.VehiclePower.SelectRandom().Name;
                profession = profession.Replace("{power}", power);
            }

            if (containsEnergy)
            {
                string energy = db.PowerSystemEnergy.SelectRandom().Name;
                profession = profession.Replace("{energy}", energy);
            }

            if (containsMovement)
            {
                string movement = db.VehicleMovement.SelectRandom().Name;
                profession = profession.Replace("{movement}", movement);
            }

            if (containsFields)
            {
                string fieldA = profession, fieldB = profession;
                while (EngineerProfessionNeedsDetails(fieldA))
                {
                    fieldA = db.DataEngineer.SelectRandom().Name;
                }
                while (fieldA.Equals(fieldB) || EngineerProfessionNeedsDetails(fieldB))
                {
                    fieldB = db.DataEngineer.SelectRandom().Name;
                }
                profession = profession.Replace("{fields}", fieldA + "-" + fieldB);                
            }
            return profession +" engineer";
        }

        //  [astronomy] scientist
        public static string AnyScientist()
        {
            var db = new SparksDbContext();

            //  generate field of study (the science)
            string field = "";
            if (Utilities.RandomBool())
            {
                field = db.DataScience.SelectRandom().ToViewModel().Name + "-";
            }
            field += db.DataScience.SelectRandom().ToViewModel().Name;

            //  generate the type of scientist
            string role = "";
            switch (Utilities.random.Next(5))
            {
                case 0: role = "{0} scientist"; break;
                case 1: role = "{0} researcher"; break;
                case 2: role = "{0} theoretician"; break;
                case 3: role = "{0} experimentalist"; break;
                case 4: role = "observational scientist of {0}"; break;
                  
            }
            return role.Replace("{0}", field);
        }

        // [navigator] on an [ancient warship]
        public static string AnyNauticalCrew()
        {
            string result;

            //  crew position name
            var crewPosition = NauticalCrew();

            //  generate ship type
            var ship = Vehicle.VehicleByPurpose();

            result = crewPosition.Name + " on " + Utilities.ToPrep(ship.Name) + " " + ship.Name;
            return result;
        }

        //  [janitor] at a [nuclear power plant]
        public static VmGeneral AnyFacilityStaff()
        {
            var result = new VmGeneral();

            //  generate staff role
            var staffRole = FacilityStaff();

            //  TODO: more types of "facilities" besides power plants
            var power = Vehicle.Power().Name;
            var facility = "";
            switch (Utilities.random.Next(4))
            {
                case 0:facility = "plant";break;
                case 1: facility = "facility";break;
                case 2:facility = "factory";break;
                case 3:facility = "refinery";break;
            }

            result.Name = staffRole.Name + " at " + Utilities.ToPrep(power) + " " + power + " " + facility;
            return result;
        }

        //  [age|personality|experience] [heroic title]
        public static VmTitle AnyHeroic()
        {
            var db = new SparksDbContext();
            var vm = new VmTitle();

            AgePersonalityExperience(vm);
            
            vm.Title = HeroicTitle();

            return vm;
        }

        //  [age|personality] [savage title]
        public static VmTitle AnySavage()
        {
            var db = new SparksDbContext();
            var vm = new VmTitle();

            AgeAndPersonality(vm);

            vm.Title = SavageTitle();

            return vm;
        }

        //  [age|personality|experience] [melee title]
        public static VmTitle AnyMelee()
        {
            var db = new SparksDbContext();
            var vm = new VmTitle();

            AgePersonalityExperience(vm);

            vm.Title = MeleeTitle();

            return vm;
        }

        //  [age|personality|experience] [shooter title]
        public static VmTitle AnyShooter()
        {
            var db = new SparksDbContext();
            var vm = new VmTitle();

            AgePersonalityExperience(vm);

            vm.Title = ShooterTitle();

            return vm;
        }

        //  [age|personality|magic speciality] [magic title]
        public static VmTitle AnyMagic()
        {
            var db = new SparksDbContext();
            var vm = new VmTitle();

            AgeAndPersonality(vm);

            vm.Title = MagicTitle();

            if (Utilities.OneChanceIn(3)) {
                //  specialist
                vm.Title.Name += " specializing in " + MagicEnergyMod().Name + " magic";
            }

            return vm;
        }

        //  [tech field] [tech profession]
        public static VmGeneral AnyTechProfession()
        {
            var db = new SparksDbContext();
            var vm = new VmGeneral();

            vm = TechField();
            vm.Name += " " + TechProfession().Name;
            return vm;
        }

        // [personality] [political title]
        public static VmTitle AnyPolitical()
        {
            var db = new SparksDbContext();
            var vm = new VmTitle();

            AgeAndPersonality(vm);

            vm.Title = PoliticalTitle();
            return vm;
        }

        #region Adjective helpers for titles

        /// <summary>
        /// 50% chance of generating an 'age'
        /// 50% change of generating a 'personality'
        /// Chances are individual and independant.
        /// </summary>
        private static VmTitle AgeAndPersonality(VmTitle source)
        {
            if (Utilities.RandomBool()) source.Age = Age();
            if (Utilities.RandomBool()) source.Personality = Personality();
            return source;
        }

        /// <summary>
        /// 100% chance of generating one of: age, personality OR experience.
        /// Never more than one.
        /// </summary>
        private static VmTitle AgePersonalityExperience (VmTitle source)
        {
            bool hasAge = false, hasPersonality = false, hasExperience = false;
            int count = Utilities.random.Next(3);
            while (count > 0)
            {
                if (!hasAge && Utilities.OneChanceIn(6))
                {
                    hasAge = true;
                    source.Age = Age();
                    count--;
                    continue;
                }
                if (!hasPersonality && Utilities.OneChanceIn(6))
                {
                    hasPersonality = true;
                    source.Personality = Personality();
                    count--;
                    continue;
                }
                if (!hasExperience && Utilities.OneChanceIn(6))
                {
                    hasExperience = true;
                    source.Experience = Experience();
                    count--;
                    continue;
                }
            }

            return source;
        }

        public static VmGeneral FacilityStaff()
        {
            var db = new SparksDbContext();
            var result = db.RoleFacilityStaff.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral NauticalCrew()
        {
            var db = new SparksDbContext();
            var result = db.RoleNauticalCrew.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral ProductionCrew()
        {
            var db = new SparksDbContext();
            var role = db.RoleProductionCrew.SelectRandom().ToViewModel();
            string production = "";
            switch (Utilities.random.Next(6))
            {
                case 0: production = "film"; break;
                case 1:production = "theater";break;
                case 2:production = "television";break;
                case 3:production = "music video";break;
                case 4:production = "pirate show";break;
                case 5:production = "internet show";break;
            }
            role.Name+=" on a " +production + " production";
            role.Name = role.Name.ToLower();
            return role;
        }

        public static VmGeneral PoliticalTitle()
        {
            var db = new SparksDbContext();
            var result = db.TitlePolitical.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral TechField()
        {
            var db = new SparksDbContext();
            var result = db.ProfessionTechnologyField.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral TechProfession()
        {
            var db = new SparksDbContext();
            var result = db.ProfessionTechnologist.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        //  Returns a random profession from the Misc Profession
        //  table. ProfessionTypeID is stored in the GameEffect
        //  field.
        public static VmGeneral MiscProfession()
        {
            var db = new SparksDbContext();
            var selection = db.ProfessionMisc.SelectRandom();
            var result = selection.ToViewModel();
            result.GameEffect = selection.ProfessionTypeID.ToString();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral HeroicTitle()
        {
            var db = new SparksDbContext();
            var result = db.TitleHeroic.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral SavageTitle()
        {
            var db = new SparksDbContext();
            var result = db.TitleSavage.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral MeleeTitle()
        {
            var db = new SparksDbContext();
            var result = db.TitleMelee.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral ShooterTitle()
        {
            var db = new SparksDbContext();
            var result = db.TitleShooter.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral MagicTitle()
        {
            var db = new SparksDbContext();
            var result = db.TitleMagic.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral MagicEnergyMod()
        {
            var db = new SparksDbContext();
            var result = db.WeaponModMagicEnergy.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral Age()
        {
            var db = new SparksDbContext();
            var result = db.CharacterAge.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral Personality()
        {
            var db = new SparksDbContext();
            var result = db.CharacterPersonality.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        public static VmGeneral Experience()
        {
            var db = new SparksDbContext();
            var result = db.CharacterExperience.SelectRandom().ToViewModel();
            result.Name = result.Name.ToLower();
            return result;
        }

        #endregion
    }
}