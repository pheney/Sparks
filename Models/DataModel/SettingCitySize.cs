using Sparks.Models.ViewModel;
using Sparks.Models.WorkModel;

namespace Sparks.Models.DataModel
{
    public class Setting_CitySize : BaseModel 
    {
        public int MinPopulation { get; set; }
        public int MaxPopulation { get; set; }
        public string PopulationUnits { get; set; }

        public override VmGeneral ToViewModel()
        {
            return new VmGeneral(ID, Name, Utilities.random.Next(MinPopulation, MaxPopulation) + " " + PopulationUnits);
        }
    }
}