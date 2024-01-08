using SimpleObjectLoader.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleObjectLoader.Builder
{
    internal class WallpaperBuilder(TextureConfig config) : AbstractBuilder(config, typeof(WallpaperBuilder))
    {
        protected override void Initialize()
        {
            throw new NotImplementedException();
        }

        protected override void Register()
        {
            throw new NotImplementedException();
        }
    }
}
