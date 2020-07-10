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

    public class BoundingVolume2
    {
        public List<double> box { get; set; }

    }

    public class Content2
    {
        public string uri { get; set; }

    }

    public class BoundingVolume3
    {
        public List<double> box { get; set; }

    }

    public class Content3
    {
        public string uri { get; set; }

    }

    public class BoundingVolume4
    {
        public List<double> box { get; set; }

    }

    public class Content4
    {
        public string uri { get; set; }

    }

    public class BoundingVolume5
    {
        public List<double> box { get; set; }

    }

    public class Content5
    {
        public string uri { get; set; }

    }

    public class Child4
    {
        public BoundingVolume5 boundingVolume { get; set; }
        public Content5 content { get; set; }
        public List<object> children { get; set; }
        public double geometricError { get; set; }
        public string refine { get; set; }

    }

    public class Child3
    {
        public BoundingVolume4 boundingVolume { get; set; }
        public Content4 content { get; set; }
        public List<Child4> children { get; set; }
        public double geometricError { get; set; }
        public string refine { get; set; }

    }

    public class Child2
    {
        public BoundingVolume3 boundingVolume { get; set; }
        public Content3 content { get; set; }
        public List<Child3> children { get; set; }
        public double geometricError { get; set; }
        public string refine { get; set; }

    }

    public class Child
    {
        public BoundingVolume2 boundingVolume { get; set; }
        public Content2 content { get; set; }
        public List<Child2> children { get; set; }
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