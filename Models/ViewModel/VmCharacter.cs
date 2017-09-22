using Sparks.Models.WorkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparks.Models.ViewModel
{
    public class VmCharacter
    {
        public string Name { get; set; }
        public VmTitle Title;
        public VmGeneral Actor;
        public VmWeapon Weapon;
        public VmVehicle Vehicle;
        public VmWorld World;
        public VmSociety Society;
        public VmMechanics Mechanics;
    }
}