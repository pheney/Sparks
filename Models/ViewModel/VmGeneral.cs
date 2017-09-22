using Sparks.Models.WorkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparks.Models.ViewModel
{
    /// <summary>
    /// A generic class for holding information about an object.
    /// The object is described by a Name, Description and GameEffect.
    /// 
    /// Name is a short description such as "Gun" or "Fusion"
    /// 
    /// Description is a long description such as the description of a Tundra.
    /// 
    /// GameEffect is a description of possible effects on game mechanics
    ///     that this object might have, e.g., for a Phase weapon this might
    ///     say "this weapon is not hindered or blocked by physical obstacles."
    /// </summary>
    public class VmGeneral
    {
        /// <summary>
        /// Unique db ID of this object.
        /// </summary>
        public int ID { get; set; }

        #region Public properties

        /// <summary>
        /// The name of the object described by this view model.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The long description / flavor-text of the object.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Suggestions for how this might affect game mechanics.
        /// </summary>
        public string GameEffect { get; set; }

        /// <summary>
        /// The appropriate preposition (a/an) for this object's name.
        /// </summary>
        public string NamePrep
        {
            get
            {
                return Name != null && Name.Length > 0 ? Name.ToPrep() : "";
            }
        }

        /// <summary>
        /// The appropriate preposition (a/an) for this object's description.
        /// </summary>
        public string DescPrep
        {
            get
            {
                return Description != null && Description.Length > 0 ? Description.ToPrep() : "";
            }
        }

        #endregion
        #region Constructors

        public VmGeneral() { }
        public VmGeneral(int ID, string name, string desc)
        {
            this.ID = ID;
            this.Name = name;// != null ? name.ToLower() : name;
            this.Description = desc;// != null ? desc.ToLower() : desc;
        }

        #endregion
    }
}