using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System.Linq;

namespace Sparks.Models.WorkModel
{
    //  TODO
    //  Finish quantifying the infrastructures. Add db tables to reflect them.
    //  Write methods to select them.
    //  Build the cshtml file to display a society.
    public static class Society
    {
        public static VmSociety AnySociety(float attr)
        {
            var db = new SparksDbContext();
            var vm = new VmSociety();

            vm.Name = Polyglot.AnyWord();

            vm.Energy = Society.Energy();
            vm.Ekistic = Society.Ekistic();
            vm.Culture = Society.Culture();
            vm.Descriptor = Society.Descriptor();
            vm.TechnologicalEra = Society.Technology();

            vm.Government = Society.Government();
            vm.CivilDefenseInfrastructure = Society.CivilDefenseInfrastructure();
            vm.CulturalInfrastructure = Society.CultureInfrastructure();
            vm.EconomicInfrastructure = Society.EconomicInfrastructure();
            vm.EducationInfrastructure = Society.EducationInfrastructure();
            vm.EmergencyServicesInfrastructure = Society.EmergencyServicesInfrastructure();
            vm.HealthCareInfrastructure = Society.HealthCareInfrastructure();
            vm.IndustrialInfrastructure = Society.IndustrialInfrastructure();
            vm.LawEnforcementInfrastructure = Society.LawEnforcementInfrastructure();
            vm.SocialInfrastructure = Society.SocialInfrastructure();

            return vm;
        }

        public static VmGeneral Energy()
        {
            var db = new SparksDbContext();
            return db.WorldCommonEnergy.SelectRandom().ToViewModel();
        }

        public static VmGeneral Ekistic()
        {
            var db = new SparksDbContext();
            return db.WorldEkistic.SelectRandom().ToViewModel();
        }

        public static VmGeneral Culture()
        {
            var db = new SparksDbContext();
            return db.WorldMacroCulture.SelectRandom().ToViewModel();
        }

        public static VmGeneral Descriptor()
        {
            var db = new SparksDbContext();
            return db.WorldSettingAdjective.SelectRandom().ToViewModel();
        }

        public static VmGeneral Technology()
        {
            var db = new SparksDbContext();
            return db.WorldTechnology.SelectRandom().ToViewModel();
        }

        public static VmGeneral Government()
        {
            var db = new SparksDbContext();
            return db.SocietyGovernment.SelectRandom().ToViewModel();
        }

        public static VmGeneral CivilDefenseInfrastructure()
        {
            var db = new SparksDbContext();
            return db.SocietyCivilDefenseInf.SelectRandom().ToViewModel();
        }

        public static VmGeneral CultureInfrastructure()
        {
            var db = new SparksDbContext();
            return db.SocietyCulturalInf.SelectRandom().ToViewModel();
        }

        public static VmGeneral EconomicInfrastructure()
        {
            var db = new SparksDbContext();
            return db.SocietyEconomicInf.SelectRandom().ToViewModel();
        }

        public static VmGeneral EducationInfrastructure()
        {
            var db = new SparksDbContext();
            return db.SocietyEducationInf.SelectRandom().ToViewModel();
        }

        public static VmGeneral EmergencyServicesInfrastructure()
        {
            var db = new SparksDbContext();
            return db.SocietyEmergencyServicesInf.SelectRandom().ToViewModel();
        }

        public static VmGeneral HealthCareInfrastructure()
        {
            var db = new SparksDbContext();
            return db.SocietyHealthCareInf.SelectRandom().ToViewModel();
        }

        public static VmGeneral IndustrialInfrastructure()
        {
            var db = new SparksDbContext();
            return db.SocietyIndustrialInf.SelectRandom().ToViewModel();
        }

        public static VmGeneral LawEnforcementInfrastructure()
        {
            var db = new SparksDbContext();
            return db.SocietyLawEnforcementInf.SelectRandom().ToViewModel();
        }

        public static VmGeneral SocialInfrastructure()
        {
            var db = new SparksDbContext();
            return db.SocietySocialInf.SelectRandom().ToViewModel();
        }

    }

}