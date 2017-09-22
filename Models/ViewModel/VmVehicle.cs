using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparks.Models.ViewModel
{
    public class VmVehicle
    {
        public VmGeneral BaseVehicle, Armor, Cargo, Characteristic, 
            Condition, Defenses, Material, Movement, Offenses,
            Power, Range, Speed, Technology, Size;
                

        public string Aesthetic
        {
            get
            {
                string w = "";
                if (Condition != null) w += Condition.Name + " ";
                if (Characteristic != null) w += Characteristic.Name + " ";
                if (Material != null) w += Material.Name;
                return w;
            }
        }

        public string Name
        {
            get
            {
                string w = "";
                if (Condition != null) w += Condition.Name + " ";
                if (Technology != null) w += Technology.Name + " ";
                if (Characteristic != null) w += Characteristic.Name + " ";
                if (Power != null) w += Power.Name + " powered ";
                if (Material != null) w += Material.Name + " ";
                if (Movement != null) w += Movement.Name + " ";
                w += BaseVehicle.Name; 
                if (Speed != null) w += ", " + Speed.Name + " top speed";
                if (Range != null) w += ", " + Range.Name + " range";
                if (Cargo != null) w += ", " + Cargo.Name + " cargo capacity";
                if (Offenses != null) w += ", " + Offenses.Name + " offenses";
                if (Defenses != null) w += ", " + Defenses.Name + " defenses";
                if (Armor != null) w += ", " + Armor.Name + " armor";

                return w;
            }
        }

        public string NameAsCombat
        {
            get
            {
                string w = "";
                
                if (Movement != null) w += Movement.Name + " ";
                w += BaseVehicle.Name;
                if (Offenses != null) w += ", " + Offenses.Name + " offenses";
                if (Defenses != null) w += ", " + Defenses.Name + " defenses";
                if (Armor != null) w += ", " + Armor.Name + " armor";

                return w;
            }
        }

        public string NameAsUtility
        {
            get
            {
                string w = "";

                if (Technology != null) w += Technology.Name + " ";
                if (Characteristic != null) w += Characteristic.Name + " ";
                if (Power != null) w += Power.Name + " powered ";
                if (Material != null) w += Material.Name + " ";
                if (Movement != null) w += Movement.Name + " ";
                w += BaseVehicle.Name;
                if (Cargo != null) w += ", " + Cargo.Name + " cargo capacity";
                if (Defenses != null) w += ", " + Defenses.Name + " defenses";
                if (Armor != null) w += ", " + Armor.Name + " armor";

                return w;
            }
        }

        public string NameAsTransport
        {
            get
            {
                string w = "";

                if (Technology != null) w += Technology.Name + " ";
                if (Power != null) w += Power.Name + " powered ";
                if (Movement != null) w += Movement.Name + " ";
                w += BaseVehicle.Name;
                if (Speed!= null) w += ", " + Speed.Name + " max speed";
                if (Range != null) w += ", " + Range.Name + " range";

                return w;
            }
        }
    }
}