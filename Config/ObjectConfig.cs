using System.Collections.Generic;

namespace SimpleObjectLoader.Config
{
    internal class ObjectConfig
    {

        public string FilePath {  get; set; }
        public string ModId { get; set; }
        public string Type { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string[] Categories { get; set; }
        public string TexturePath { get; set; }

        public string Atlas { get; set; }

        public int[] Size { get; set; }

        public string[] ColorSchemes { get; set; }

        public int[] TableSpots { get; set; }

        public ActionSpotConfig[] ActionSpots { get; set; }

        public int[] ColorMap { get; set; }
    }
}
