using Sparks.Models.ViewModel;
using Sparks.Models.DataModel;
using System.Linq;

namespace Sparks.Models.WorkModel
{
    public static class Business
    {
        /// <summary>
        /// Returns a VmGeneral object containing a random business.
        /// Description contains and specific information from the database about
        /// that particular business. GameEffects contains the business category.
        /// </summary>
        public static VmGeneral AnyBusiness()
        {
            var db = new SparksDbContext();
            var business = db.Businesses.ElementAt(Utilities.random.Next(db.Businesses.Count()));
            var vm = business.ToViewModel();
            vm.GameEffect = db.BusinessType.Where(t => t.ID == business.TypeID).First().Name;
            return vm;
        }

        public static VmGeneral AnyBusinessType()
        {
            var db = new SparksDbContext();
            return db.BusinessType.ElementAt(Utilities.random.Next(db.BusinessType.Count())).ToViewModel();
        }
    }
}