using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class O2
    {
        public int value { get; set; }
        public string units { get; set; }
        public double min_threshold { get; set; }
        public double max_threshold { get; set; }

    }

    public class Co
    {
        public double value { get; set; }
        public string units { get; set; }
        public double max_threshold { get; set; }

    }

    public class H2s
    {
        public double value { get; set; }
        public string units { get; set; }
        public double max_threshold { get; set; }

    }

    public class Hcn
    {
        public int value { get; set; }
        public string units { get; set; }
        public double max_threshold { get; set; }

    }

    public class Lel
    {
        public double value { get; set; }
        public string units { get; set; }
        public double max_threshold { get; set; }

    }

    public class Particulate
    {
        public double value { get; set; }
        public string units { get; set; }
        public double max_threshold { get; set; }

    }

    public class Coords
    {
        public double latitude { get; set; }
        public double longitude { get; set; }

    }

    public class AerialThermalDetector
{
        public string deviceid { get; set; }
        public string type { get; set; }
        public string time { get; set; }
        public O2 o2 { get; set; }
        public Co co { get; set; }
        public H2s h2s { get; set; }
        public Hcn hcn { get; set; }
        public Lel lel { get; set; }
        public Particulate particulate { get; set; }
        public Coords coords { get; set; }

    }
/*
dictionarty retuyrndictory() 
{

    return Dictionary<string, Object>;
}*/