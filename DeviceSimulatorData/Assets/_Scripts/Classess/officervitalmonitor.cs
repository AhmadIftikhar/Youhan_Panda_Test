using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Firedata
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Spo2
    {
        public int value { get; set; }
        public string unit { get; set; }
    }

    public class Pulse
    {
        public int value { get; set; }
        public string unit { get; set; }
    }

    public class Coords
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class officervitalmonitor
    {
        public string deviceid { get; set; }
        public string type { get; set; }
        public string time { get; set; }
        public Spo2 spo2 { get; set; }
        public Pulse pulse { get; set; }
        public Coords coords { get; set; }
        public bool sos { get; set; }
    }
}
