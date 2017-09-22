namespace Sparks.Models.ViewModel
{
    public class VmTheme
    {
        public VmGeneral ArtisticConcept, LiterarySetting, EvocativeMood;
        public string Description
        {
            get
            {
                return EvocativeMood.Name
                  + " " + LiterarySetting.Name
                  + " story about " + ArtisticConcept.Name + ".";
            }
        }
    }
}