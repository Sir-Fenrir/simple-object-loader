using SimpleObjectLoader.Config;
using SimpleObjectLoader.Utils;
using Microsoft.Xna.Framework;
using TinyLife.World;

namespace SimpleObjectLoader.Builder
{
    internal class WallpaperBuilder(TextureConfig config) : AbstractBuilder(config, typeof(WallpaperBuilder))
    {

        private readonly TextureConfig _config = config;

        protected override void Initialize()
        {
             // Not needed for wallpapers
        }

        protected override void Register()
        {
            Wallpaper.Register(
                $"SimpleObjectLoader.{_config.Name}",
                _config.Price,
                _config.TextureRegions,
                new Point(_config.FirstColumn, _config.FirstRow),
                ObjectUtils.ParseColorSchemes(_config.ColorSchemes)
                );
        }
    }
}
