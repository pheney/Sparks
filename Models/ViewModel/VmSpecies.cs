using Sparks.Models.WorkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparks.Models.ViewModel
{
    public class VmSpecies
    {
        public VmGeneral Template, Base, Appearance, Personality, Activity;
        public string Name { get; set; }

        public string Category
        {
            get
            {
                string n = "";
                if (Template != null) n += Template.Name.ToProper() + " ";
                n += Base.Name.ToProper();
                if (Template != null)
                { 
                    int[] tauricSpeciesIDs = new int[] { 23, 90, 112 };
                    if (tauricSpeciesIDs.Contains(Template.ID))
                    {
                        n += " " + Template.Description.ToLower(); ;
                    }
                }
                return n;
            }
        }

        public string Description
        {
            get
            {
                string n = "";
                if (Appearance != null)
                {
                    n += " is " + Appearance.Name + (Utilities.RandomBool() ? " looking" : "");
                }
                if (Personality != null)
                {
                    if (Appearance != null) n += ", with ";
                    else n += " has ";
                    n += Personality.NamePrep + " " + Personality.Name + " personality";
                }
                if (Activity != null)
                {
                    if (Appearance != null || Personality != null) n += " and ";
                    n += "is engaged in " + Activity.Name + " something";
                }
                return this.Name + ", " + this.Category.ToPrep() + " " + this.Category.ToLower() + "," + n.ToLower() + ".";
            }
        }
    }
}