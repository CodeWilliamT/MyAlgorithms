using System.Configuration;

namespace LensDriverController.Dto
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class DataPoint
    {
        public double analogInput {get; set;}
        public double focalPower {get; set; }
    }
}
