using Microsoft.Xna.Framework;
using MLEM.Textures;
using System;
using System.Collections.Generic;
using System.Drawing;
using Point = Microsoft.Xna.Framework.Point;

namespace SimpleObjectLoader.Config
{
    public class TextureConfig : IConfig
    {

        public string Name { get; set; }
        public string File { get; set; }

        /// <summary>
        /// Internal use only, to store a loaded texture.
        /// </summary>
        public Dictionary<Point, TextureRegion> TextureRegions { get; set; }

        public int Price { get; set; }

        public int NrOfColumns { get; set; }

        public int NrOfRows { get; set; }

        public int FirstRow { get; set; }

        public int FirstColumn { get; set; }
        public string Layer { get; set; }

        public string StylePreference { get; set; }

        public string[] ClothesIntention { get; set; }

        public string[] ColorSchemes { get; set; }
    }
}
