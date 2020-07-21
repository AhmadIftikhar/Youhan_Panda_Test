using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Geometry
    {
        public string type { get; set; }
        public List<List<double>> coordinates { get; set; }

    }

public class Root
{
    public string deviceid { get; set; }
    public string type { get; set; }
    public string time { get; set; }
    public string name { get; set; }
    public int average { get; set; }
    public Geometry geometry { get; set; }

}

public class RoadClosure
{
        public string deviceid { get; set; }
        public string type { get; set; }
        public string alert_type { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string street { get; set; }
        public Geometry geometry { get; set; }

    }


public class TrafficConditions
{
    public string deviceid { get; set; }
    public string type { get; set; }
    public string time { get; set; }
    public string name { get; set; }
    public int average { get; set; }
    public Geometry geometry { get; set; }

}

