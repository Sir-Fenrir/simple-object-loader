using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleObjectLoader.Config
{
    public class ModConfig : IConfig
    {
        public string ModId { get; set; }

        public string FilePath { get; set; }

        public FurnitureConfig[] Furniture { get; set; } = [];

        public TextureConfig[] Clothes { get; set; } = [];

        public TextureConfig[] Wallpapers { get; set; } = [];
    }
}
