using Sparks.Models.WorkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparks.Models.ViewModel
{
    public class VmWargame
    {
        public string Name { get; set; }
        public List<VmGeneral> Factions;
        public string Theme;
        public VmGeneral Setting;
        public string Twist;

        public VmWargame()
        {
            Factions = new List<VmGeneral>();
        }

    }
}