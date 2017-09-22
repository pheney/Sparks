namespace Sparks.Models.ViewModel
{
    public class VmMechanics
    {
        public VmGeneral Arcade, Primary, Secondary, Movement, Conflict, Win, Coop;

        public bool IsEmpty
        {
            get
            {
                int count = 0;
                count += Arcade == null ? 0 : 1;
                count += Primary == null ? 0 : 1;
                count += Secondary == null ? 0 : 1;
                count += Movement == null ? 0 : 1;
                count += Conflict == null ? 0 : 1;
                count += Coop == null ? 0 : 1;
                return count < 2;
            }
        }
        public string Description
        {
            get
            {
                string result = "Find a theme that uses " + Primary.Name;
                if (Secondary != null) result += " and " + Secondary.Name + " as mechanics. ";
                if (Movement != null) result += "Use " + Movement.Name + " movement";
                if (Conflict == null) result += ".";
                else result += " and " + Conflict.Name + " conflict resolution mechanics. ";
                if (Win != null) result += "The win condition is " + Win.Name + ".";
                return result;
            }
        }

        public string Inspiration
        {
            get
            {
                string result = "";
                if (Arcade != null) result = "Use the " + Arcade.Description + " arcade game " + Arcade.Name + " as inspiration and build a game. ";
                result += "Use " + Primary.Name;
                if (Conflict == null) result += " game mechanic.";
                else result += " and " + Conflict.Name + " game mechanics.";
                
                return result;
            }
        }

        public string Easy
        {
            get
            {
                return "Use " + Primary.Name + " mechanic.";
            }
        }

    }
}