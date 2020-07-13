// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using System.Collections.Generic;
namespace Test
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Asset
    {
        public string version { get; set; }
        public string gltfUpAxis { get; set; }

    }

    public class BoundingVolume
    {
        public List<double> box { get; set; }

    }

    public class Content
    {
        public string uri { get; set; }

    }

    public class Child
    {
        public BoundingVolume boundingVolume { get; set; }
        public Content content { get; set; }
        public List<Child> children { get; set; }
        public double geometricError { get; set; }
        public string refine { get; set; }

    }

    public class Root2
    {
        public BoundingVolume boundingVolume { get; set; }
        public List<double> transform { get; set; }
        public Content content { get; set; }
        public List<Child> children { get; set; }
        public double geometricError { get; set; }
        public string refine { get; set; }

    }

    public class Root
    {
        public Asset asset { get; set; }
        public Root2 root { get; set; }
        public double geometricError { get; set; }

    }






}