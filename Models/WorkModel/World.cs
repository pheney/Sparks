using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System.Linq;

namespace Sparks.Models.WorkModel
{
    public static class World
    {
        public static VmWorld AnyWorld()
        {
            var vm = new VmWorld();

            vm.Star = World.Star();
            vm.Star.Description = vm.Star.Name;
            vm.Star.Name = Jabberwock.AnyWord();

            vm.Planet = World.Planet();
            vm.Planet.Description = vm.Planet.Name;
            vm.Planet.Name = Jabberwock.AnyWord();

            vm.Climate = World.Climate();
            vm.Ecosystem = World.Ecosystem();

            vm.Waterbody = World.Waterbody();
            vm.Waterbody.Description = vm.Waterbody.Name;
            vm.Waterbody.Name = Jabberwock.AnyWord();

            vm.Landmass = World.Landmass();
            vm.Landmass.Description = vm.Landmass.Name;
            vm.Landmass.Name = Jabberwock.AnyWord();
            
            return vm;
        }

        public static VmGeneral Ecology()
        {
            var vm = new VmGeneral();
            VmGeneral climate, ecosystem;

            climate = World.Climate();
            ecosystem = World.Ecosystem();
            string[] conjunctions =
            {
                "is home to",
                "hosts",
                "features",
                "plays host to",
                "features"
            };
            vm.Name = "This " + climate.Name + " climate "
                + conjunctions[Utilities.random.Next(conjunctions.Length)]
                + " " + ecosystem.NamePrep.ToLower() + " " + ecosystem.Name + " ecosystem.";
            return vm;
        }

        #region Helpers

        public static VmGeneral Star()
        {
            var db = new SparksDbContext();
            return db.WorldStar.SelectRandom().ToViewModel();
        }

        public static VmGeneral Planet()
        {
            var db = new SparksDbContext();
            return db.WorldPlanet.SelectRandom().ToViewModel();
        }

        public static VmGeneral Waterbody()
        {
            var db = new SparksDbContext();
            return db.WorldWaterBody.SelectRandom().ToViewModel();
        }

        public static VmGeneral Landmass()
        {
            var db = new SparksDbContext();
            return db.WorldLandMass.SelectRandom().ToViewModel();
        }

        public static VmGeneral Climate()
        {
            var db = new SparksDbContext();
            return db.WorldClimate.SelectRandom().ToViewModel();
        }

        public static VmGeneral Ecosystem()
        {
            var db = new SparksDbContext();
            return db.WorldEcosystem.SelectRandom().ToViewModel();
        }

        #endregion
    }

}