using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Classess
{
	
        public class Coords
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
        }

        public class PeopleCounter
        {
            public string deviceid { get; set; }
            public string type { get; set; }
            public int count { get; set; }
            public string time { get; set; }
            public Coords coords { get; set; }
        }

}
