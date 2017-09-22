using Sparks.Models.WorkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparks.Models.ViewModel
{
    public class VmWorld
    {
        /// <summary>
        /// Star, Planet, Landmass and Waterbody "Name" fields will contain
        /// a Jabberwock word (literally the name of the object that inhabitants
        /// would call it, e.g., "Pleasant Bay"). The "Description" will contain 
        /// the actual type/name of the object, e.g., "Lake".
        /// </summary>
        public VmGeneral Star, Planet, Landmass, Waterbody, Climate, Ecosystem;
        
        public string Description
        {
            get
            {
                return Planet.Name + " is " + Planet.DescPrep + " " + Planet.Description.ToLower()
                    + " orbiting " + Star.Name + ", " + Star.DescPrep + " " + Star.Description + ". "
                    + "The " + Landmass.Description.ToLower() + ", " + Landmass.Name + ", meets with "
                    + Waterbody.Name + ", " + Waterbody.DescPrep + " " + Waterbody.Description.ToLower() + ". "
                    + "The dominant climate is " + Climate.Name.ToLower() + ", with " + Ecosystem.Name.ToLower()
                    + " ecosystems.";
            }
        }
    }
}