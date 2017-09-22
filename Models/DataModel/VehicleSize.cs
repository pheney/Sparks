using Sparks.Models.ViewModel;
using Sparks.Models.WorkModel;

namespace Sparks.Models.DataModel
{
    public class Vehicle_Size : BaseModel
    {
        public int CrewMin { get; set; }
        public int CrewMax { get; set; }

        public override VmGeneral ToViewModel()
        {
            return new VmGeneral(ID, Name, (Utilities.random.Next(CrewMax - CrewMin) + CrewMin).ToString());
        }
    }
}