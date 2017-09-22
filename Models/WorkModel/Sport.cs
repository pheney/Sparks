using Sparks.Models.ViewModel;
using Sparks.Models.DataModel;
using System.Linq;

namespace Sparks.Models.WorkModel
{
    public static class RealSport
    {
        /// <summary>
        /// Returns a VmGeneral object containing a random sport.
        /// Description contains and specific information from the database about
        /// that particular sport. GameEffects contains the sport category.
        /// </summary>
        public static VmGeneral AnySport()
        {
            var db = new SparksDbContext();
            var sport = db.Sports.SelectRandom();
            var vm = sport.ToViewModel();
            vm.GameEffect = db.SportTypes.Where(t => t.ID == sport.TypeID).First().Name;
            return vm;
        }

        /// <summary>
        /// Returns a VmGeneral object containing a random sport type,
        /// e.g., ball-in-hoop, archery, etc.
        /// Sport types are general categories of sports.
        /// </summary>
        public static VmGeneral AnySportType()
        {
            var db = new SparksDbContext();
            return db.SportTypes.SelectRandom().ToViewModel();
        }
    }
}