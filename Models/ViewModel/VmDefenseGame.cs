using Sparks.Models.WorkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparks.Models.ViewModel
{
    public class VmDefenseGame
    {
        public string Name { get; set; }
        public string Theme { get; set; }
        public VmGeneral Defenders;
        public VmGeneral Attackers;
        public List<string> Towers;
        public List<string> Creeps;
        public VmGeneral Location;
        public string Board;

        public VmDefenseGame ()
        {
            Towers = new List<string>();
            Creeps = new List<string>();
        }
        /*
        TODO
        Randomly assign these rolls to the towers:
        SLOW, POSION, ANTI-AIR, LIGHT, MEDIUM, HEAVY, SUPER, BEAM / MULTI-HIT,
        WARP / DELAY, FREEZE, SHIELD-KILLER, ANTI-FIRE / WATER / AIR / EARTH / ETC,
        INCOME GENERATOR, POWER-UP RANGE, POWER-UP DAMAGE, POWER-UP ROF
        */
    }
}