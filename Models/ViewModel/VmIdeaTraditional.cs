using System.Collections.Generic;

namespace Sparks.Models.ViewModel
{
    public class VmIdeaTraditional
    {
        public VmGeneral Commitment;
        public List<VmGeneral> Components;
        public VmTheme Theme;
        public VmGeneral GameType;
        public VmGeneral Location;
        public VmMechanics Mechanics;
        public VmGeneral Player;
        public VmWeapon Equipment;
        public bool IsEmpty
        {
            get
            {
                int count = (Commitment != null ? 1 : 0)
                    + (Components != null ? 1 : 0)
                    + (Theme != null ? 1 : 0)
                    + (GameType != null ? 1 : 0)
                    + (Location != null ? 1 : 0)
                    + (Mechanics != null ? 1 : 0)
                    + (Player != null ? 1 : 0);
                return count < 2;
            }
        }
    }
}