using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sparks.Models.WorkModel
{
    public static class Mechanics
    {
        public static VmMechanics AnyMechanics()
        {
            var db = new SparksDbContext();
            var vm = new VmMechanics();
            vm.Primary = Mechanics.General();
            int chances = 4;

            while (vm.IsEmpty)
            {
                if (Utilities.OneChanceIn(chances))
                {
                    do
                    {
                        vm.Secondary = Mechanics.General();
                    } while (vm.Primary.Equals(vm.Secondary));
                }
                if (Utilities.OneChanceIn(chances)) vm.Movement = Mechanics.Movement();
                if (Utilities.OneChanceIn(chances)) vm.Conflict = Mechanics.ContestResolution();
                if (Utilities.OneChanceIn(chances)) vm.Win = Mechanics.Win();
                if (Utilities.OneChanceIn(chances)) vm.Coop = Mechanics.Coop();
                if (Utilities.OneChanceIn(chances)) vm.Arcade = Mechanics.ClassicArcade();
            }
            return vm;
        }

        public static VmMechanics AllMechanics()
        {
            var db = new SparksDbContext();
            var vm = new VmMechanics();

            vm.Primary = Mechanics.General();

            do
            {
                vm.Secondary = Mechanics.General();
            } while (vm.Primary.Equals(vm.Secondary));

            vm.Movement = Mechanics.Movement();
            vm.Conflict = Mechanics.ContestResolution();
            vm.Win = Mechanics.Win();
            vm.Coop = Mechanics.Coop();
            vm.Arcade = Mechanics.ClassicArcade();

            return vm;
        }

        #region Helpers -- db accessors

        public static VmGeneral ClassicArcade()
        {
            var db = new SparksDbContext();
            return db.GameMechanicsArcade.SelectRandom().ToViewModel();
        }

        public static VmGeneral ContestResolution()
        {
            var db = new SparksDbContext();
            return db.GameMechanicsContest.SelectRandom().ToViewModel();
        }

        public static VmGeneral Movement()
        {
            var db = new SparksDbContext();
            return db.GameMechanicsMovement.SelectRandom().ToViewModel();
        }

        public static VmGeneral General()
        {
            var db = new SparksDbContext();
            return db.GameMechanicsPrimary.SelectRandom().ToViewModel();
        }

        public static VmGeneral Win()
        {
            var db = new SparksDbContext();
            return db.GameMechanicsWinCondition.SelectRandom().ToViewModel();
        }

        public static VmGeneral Coop()
        {
            var db = new SparksDbContext();
            return db.GameMechanicsCooperation.SelectRandom().ToViewModel();
        }

        #endregion
    }

}