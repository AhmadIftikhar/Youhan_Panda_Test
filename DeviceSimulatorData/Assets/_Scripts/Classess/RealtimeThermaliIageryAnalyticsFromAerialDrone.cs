using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._RealtimeThermalAnalytics
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Coords
    {
        public double latitude { get; set; }
        public double longitude { get; set; }

    }

    public class ThermalThreshold
    {
        public double value { get; set; }
        public string units { get; set; }

    }

    public class Radius
    {
        public double value { get; set; }
        public string units { get; set; }

    }

    public class RealtimeThermaliIageryAnalyticsFromAerialDrone
    {
        public string type { get; set; }
        public string deviceid { get; set; }
        public string time { get; set; }
        public Coords coords { get; set; }
        public ThermalThreshold thermal_threshold { get; set; }
        public Radius radius { get; set; }

    }


}
