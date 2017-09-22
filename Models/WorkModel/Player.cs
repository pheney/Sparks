using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System.Linq;

namespace Sparks.Models.WorkModel
{
    public static class Player
    {
        public static VmGeneral Agency ()
        {
            var vm = new VmGeneral();
        //  TODO
        //  templates:
        //  Each player controls 2 actors. Each actor is represented by a combination of tokens.
        //  [distribution] [collective] [composition]
        //  The actors may have different titles, roles or species.
        //  [uniformity]
        //  Abilities are determined by title, role or species.
        //  [abilities]
        //  Abilities can improved through purchases
        //  [mutation]

            return vm;
        }

        public static VmGeneral ActorAbilities()
        {
            var db = new SparksDbContext();
            return db.ActorAbilities.SelectRandom().ToViewModel();
        }

        public static VmGeneral ActorCollective()
        {
            var db = new SparksDbContext();
            return db.ActorCollective.SelectRandom().ToViewModel();
        }

        public static VmGeneral ActorComposition()
        {
            var db = new SparksDbContext();
            return db.ActorComposition.SelectRandom().ToViewModel();
        }

        public static VmGeneral ActorDistribution()
        {
            var db = new SparksDbContext();
            return db.ActorDistribution.SelectRandom().ToViewModel();
        }

        public static VmGeneral ActorMutation()
        {
            var db = new SparksDbContext();
            return db.ActorMutation.SelectRandom().ToViewModel();
        }

        public static VmGeneral ActorUniformity()
        {
            var db = new SparksDbContext();
            return db.ActorUniformity.SelectRandom().ToViewModel();
        }
    }

}