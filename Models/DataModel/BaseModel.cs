using Sparks.Models.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace Sparks.Models.DataModel
{
    public class BaseModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }

        public virtual VmGeneral ToViewModel()
        {
            return new VmGeneral(ID, Name, Description);
        }
    }
}