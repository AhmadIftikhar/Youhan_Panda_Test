﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FireDataAnalytics
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Coords
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class RealtimeAnalyticsToIdentifyStructures
    {
        public string type { get; set; }
        public string deviceid { get; set; }
        public string time { get; set; }
        public Coords coords { get; set; }
        public string object_type { get; set; }

    }
}
