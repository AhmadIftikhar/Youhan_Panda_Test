using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 namespace FireData
{
    public class Coords
    {
        public double latitude { get; set; }
        public double longitude { get; set; }

    }

    public class WindSpeed
    {
        public double value { get; set; }
        public string units { get; set; }

    }

    public class WindDirection
    {
        public double value { get; set; }
        public string units { get; set; }

    }

    public class Anemometer
    {
        public string type { get; set; }
        public string deviceid { get; set; }
        public string time { get; set; }
        public Coords coords { get; set; }
        public WindSpeed wind_speed { get; set; }
        public WindDirection wind_direction { get; set; }
    }
}