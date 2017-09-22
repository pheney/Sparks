using Sparks.Models.WorkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparks.Models.ViewModel
{
    public class VmTitle
    {
        public VmGeneral Title, Experience, Age, Role, Personality;
        public int ProfessionTypeID;
        public string ProfessionType;
                
        public string Name
        {
            get
            {
                return (Personality != null ? Personality.Name.ToProper() + " " : "") + Description;
            }
        }

        public string Description
        {
            get
            {
                string w = "";

                if (Experience != null) w += Experience.Name + " ";
                if (Age != null) w += Age.Name + " ";
                w += Title.Name;
                if (Role != null) w += " " + Role.Name;

                return w.ToTitle();
            }
        }
    }
}