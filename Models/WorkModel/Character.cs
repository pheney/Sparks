using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Sparks.Models.WorkModel
{
    public static class Character
    {
        public static VmCharacter AnyCharacter()
        {
            var vm = new VmCharacter();

            vm.Name = Jabberwock.AnyWord();
            vm.Title = WorkModel.Title.AnyTitle();
            vm.Actor = WorkModel.Species.AnySpeciesForNpc(1);
            vm.Actor.Name = vm.Name;
            if (Utilities.RandomBool()) vm.Weapon = WorkModel.Weapon.AnyWeapon();
            else vm.Weapon = WorkModel.Weapon.AnyItem();

            vm.Vehicle = WorkModel.Vehicle.AnyVehicle();
            vm.Society = WorkModel.Society.AnySociety(1);
            
            vm.Vehicle.Size = WorkModel.Vehicle.Size(WorkModel.Vehicle.GeneralSize.Personal);
            vm.World = WorkModel.World.AnyWorld();
            vm.Mechanics = WorkModel.Mechanics.AnyMechanics();

            return vm;
        }
    }

}