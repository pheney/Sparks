using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparks.Models.WorkModel
{
    public static class Species
    {
        public static VmGeneral AnySpeciesForNpc(float chance = 0.5f)
        {
            var db = new SparksDbContext();
            var species = new VmSpecies();

            species.Name = Jabberwock.AnyWord();
            if (Utilities.random.NextDouble() < chance) species.Activity = db.CharacterActivity.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < chance) species.Appearance = db.CharacterAppearance.SelectRandom().ToViewModel();
            if (Utilities.random.NextDouble() < chance) species.Personality = db.CharacterPersonality.SelectRandom().ToViewModel();
            species.Base = AnyCreature();
            if (Utilities.random.NextDouble() < chance) species.Template = AnyTemplate(species.Base.ID);

            var vm = new VmGeneral();
            vm.Name = species.Name;
            vm.Description = species.Category;
            vm.GameEffect = species.Description;

            return vm;
        }

        public static VmGeneral AnySpecies()
        {
            var db = new SparksDbContext();
            var creature = Species.AnyCreature();
            var template = Species.AnyTemplate(creature.ID);

            string n = template.Name.ToProper() + " " + creature.Name.ToProper();

            int[] tauricSpeciesIDs = new int[] { 23, 90, 112 };
            if (tauricSpeciesIDs.Contains(template.ID))
            {
                n += " " + template.Description.ToLower(); ;
            }

            var vm = new VmGeneral();
            vm.Name = Jabberwock.AnyWord();
            vm.Description = n;
            return vm;
        }

        //  No dependancies
        public static VmGeneral AnyTemplate(int speciesID)
        { 
            var db = new SparksDbContext();
            VmGeneral vm;   //  for the template
            
            //  ensure template and species don't match, e.g., no "alien aliens"
            string speciesName = db.Species.Where(n => n.ID == speciesID).First().Name;
            vm = db.SpeciesTemplate.Where(x=>!x.Name.Substring(0,4).Equals(speciesName.Substring(0,4))).SelectRandom().ToViewModel();

            switch (vm.ID)
            {
                case 23: // Chimeric
                         //  A MULTI-HEADED animal where each head is from a different animal. 
                         //  Sometimes also used to describe any animal COMPRISED OF PARTS from various other animals,
                         //  e.g., a lion with a goat head and eagle wings and an elephant's trunk.

                    List<string> parts = new List<string>();
                    bool valid = false;
                    int chimericContributors = Utilities.random.Next(3) + 1;
                    do
                    {
                        string part = AnyAnimal().Name;
                        if (!parts.Contains(part))
                        {
                            parts.Add(part);
                        }
                        valid = parts.Count == chimericContributors;
                    } while (!valid);

                    vm.Description = "with parts of";
                    foreach (string h in parts)
                    {
                        vm.Description += " " + h + ",";
                    }
                    vm.Description = vm.Description.Substring(0, vm.Description.Length - 1);
                    break;
                case 90: // Orthric
                         //  Possessing the body of one creature and the head of another, e.g., a minotaur.
                    var orthric = AnyAnimal().Name;
                    vm.Description = "with the head of " + orthric.ToPrep() + " " + orthric;
                    break;
                case 112: // Tauric
                          //  Possessing the upper torso of a human (or other sentient species) 
                          //  and the bottom half of an animal, e.g., a centaur.
                    var tauric = AnyAnimal().Name;
                    vm.Description = "with the body of " + tauric.ToPrep() + " " + tauric;
                    break;
                default:
                    break;
            }

            return vm;
        }

        //  Calls AnyAnimal()
        public static VmGeneral AnyCreature()
        {
            var db = new SparksDbContext();
            var vm = db.Species.SelectRandom().ToViewModel();
            VmGeneral sub = null;
            
            //  select a sub-species or animal type depending on the species
            switch (vm.ID)
            {
                case 1: // Human
                case 2: // Humanoid
                case 3: // Alien
                    break;
                case 4: // Animal
                    sub = AnyAnimal();
                    break;
                case 5: // Purpose-built Robot
                case 6: // General-purpose Robot
                    break;
                case 7: // Evolved Animal
                    sub = AnyAnimal();
                    break;
                case 8: // Machine Intelligence
                    break;
                case 9: // Insectoid
                    sub = db.AnimalInsect.SelectRandom().ToViewModel();
                    break;
                case 10: // Android
                case 11: // Monster
                case 12: // Horror
                case 13: // Abomination
                case 14: // Planeswalker
                case 15: // Spirit
                case 16: // Crystaloid
                case 17: // Mutant
                    break;
                case 18: // Reptilian
                    if (Utilities.RandomBool())
                    {
                        sub = db.AnimalLizard.SelectRandom().ToViewModel();
                    } else
                    {
                        sub = db.AnimalAmphibian.SelectRandom().ToViewModel();
                    }
                    break;
                case 19: // Shadow
                    break;
                case 20: // Avian
                    sub = db.AnimalBirdOfPrey.SelectRandom().ToViewModel();
                    break;
                case 21: // Ichthian
                    sub = db.AnimalShark.SelectRandom().ToViewModel();
                    break;
                case 22: // Ursan
                    sub = db.AnimalBear.SelectRandom().ToViewModel();
                    break;
                case 23: // Sentient
                    break;
                case 24: // Humanoid Animal
                    sub = AnyAnimal();
                    break;
                default:
                    break;
            }

            if (sub != null)
            {
                vm.Name = sub.Name + " (" + vm.Name + ")";
            }
            return vm;
        }

        //  no dependancies
        public static VmGeneral AnyAnimal()
        {
            var db = new SparksDbContext();
            var vm = new VmGeneral();
            int id = db.AnimalType.SelectRandom().ID;

            switch (id)
            {
                case 1: // diet
                    vm = db.AnimalDiet.SelectRandom().ToViewModel();
                    break;
                case 2: // turtle
                    vm = db.AnimalTurtle.SelectRandom().ToViewModel();
                    break;
                case 3: // lizard
                    vm = db.AnimalLizard.SelectRandom().ToViewModel();
                    break;
                case 4: // invertebrate
                    vm = db.AnimalInvertebrate.SelectRandom().ToViewModel();
                    break;
                case 5: // mammal
                    vm = db.AnimalMammal.SelectRandom().ToViewModel();
                    break;
                case 6: // dinosaur
                    vm = db.AnimalDinosaur.SelectRandom().ToViewModel();
                    break;
                case 7: // mythical
                    vm = db.AnimalMythical.SelectRandom().ToViewModel();
                    break;
                case 8: // undead
                    vm = db.AnimalUndead.SelectRandom().ToViewModel();
                    break;
                case 9: // amphibian
                    vm = db.AnimalAmphibian.SelectRandom().ToViewModel();
                    break;
                case 10: // bear
                    vm = db.AnimalBear.SelectRandom().ToViewModel();
                    break;
                case 11: // bird of prey
                    vm = db.AnimalBirdOfPrey.SelectRandom().ToViewModel();
                    break;
                case 12: // dog
                    vm = db.AnimalDog.SelectRandom().ToViewModel();
                    break;
                case 13: // cat
                    vm = db.AnimalCat.SelectRandom().ToViewModel();
                    break;
                case 14: // insect
                    vm = db.AnimalInsect.SelectRandom().ToViewModel();
                    break;
                case 15: // shark
                    vm = db.AnimalShark.SelectRandom().ToViewModel();
                    break;
                case 16: // snake
                    vm = db.AnimalSnake.SelectRandom().ToViewModel();
                    break;
                default:
                    break;
            }

            return vm;
        }
    }
}