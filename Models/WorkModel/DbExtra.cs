using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparks.Models.WorkModel
{
    public static class DbExtra
    {
        //  Sends a list of tables to display
        //  Tables object is a serialized list of VmGeneral objects
        //  VmGeneral .name is the title name
        //      name formatting: replace '_' with ', '
        //      name formatting: replace camel case with spaces, e.g., 'myTable' -> 'my table'
        //      name formatting: Proper case
        //  VmGeneral .ID is a unique ID generated at runtime and maintained on the server
        public static List<VmGeneral> TableNames()
        {
            var db = new SparksDbContext();

            var choices = new List<VmGeneral>();
            choices.Add(new VmGeneral(1, "Weapon Mod, Aesthetic", ""));
            choices.Add(new VmGeneral(2, "World, Setting Adjective", ""));
            choices.Add(new VmGeneral(3, "Setting, Location", ""));
            choices.Add(new VmGeneral(4, "Item, Miscellaneous", ""));
            choices.Add(new VmGeneral(5, "Character, Appearance", ""));
            choices.Add(new VmGeneral(6, "Character, Personality", ""));

            return choices;
        }
    }
}