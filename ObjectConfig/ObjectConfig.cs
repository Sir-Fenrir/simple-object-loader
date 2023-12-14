using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleObjectLoader
{
    internal class ObjectConfig
    {
        public string ModId { get; set; }
        public string Type { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public List<string> Categories { get; set; }
        public string TexturePath { get; set; }

        public string Atlas { get; set; }

        public int[] Size { get; set; }

        public string[] ColorSchemes { get; set; }

        public int[] TableSpots { get; set; }

        public ActionSpotConfig[] ActionSpots { get; set; }

        public int[] ColorMap { get; set; }
    }
}
