using Sparks.Models.WorkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparks.Models.ViewModel
{
    public class VmWeapon
    {
        public VmGeneral Size, Aesthetic, Augment, Classification,
            Energy, Subtype, WeaponType;
        
        public string Type { get
            {
                switch (WeaponType.Name.ToLower())
                {
                    case "magic": return WeaponType.Name + " item";
                    case "melee":   //  fall through
                    case "primitive ranged": // fall through
                    case "modern ranged": return WeaponType.Name + " weapon";
                    case "siege": return WeaponType.Name + " vehicle";
                    default: return "interesting item";
                }
            }
        }

        public string Carries
        {
            get
            {
                switch (WeaponType.Name.ToLower())
                {
                    case "melee": return "wields " + NamePrep;
                    case "magic": return "has "+NamePrep;
                    case "primitive ranged": // fall through
                    case "modern ranged": return "is armed with " + NamePrep;
                    case "siege": return "commands " + NamePrep;
                    default: return "carries " + NamePrep;
                }
            }
        }

        public string NamePrep
        {
            get
            {
                return Name.ToPrep();
            }
        }

        public string Name
        {
            get
            {
                string w = "";

                if (Size != null) w += Size.Name + ' ';
                if (Aesthetic != null) w += Aesthetic.Name + ' ';
                if (Augment != null) w += Augment.Name + ' ';
                if (Classification != null) w += Classification.Name + ' ';
                if (Energy != null) w += Energy.Name + ' ';
                if (Subtype != null) w += Subtype.Name;

                return w.ToLower();
            }
        }
    }
}