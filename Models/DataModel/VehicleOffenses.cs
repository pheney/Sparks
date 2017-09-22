using Sparks.Models.ViewModel;

namespace Sparks.Models.DataModel
{
    public class Vehicle_Offenses : BaseModel
    {
        public int Quantity { get; set; }

        public override VmGeneral ToViewModel()
        {
            var vm = new VmGeneral(ID, Name, Description);
            vm.GameEffect = Quantity.ToString();
            return vm;
        }
    }
}