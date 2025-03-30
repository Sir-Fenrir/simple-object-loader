using Microsoft.Xna.Framework;
using SimpleObjectLoader.Config;
using SimpleObjectLoader.Utils;
using TinyLife.World;

namespace SimpleObjectLoader.Builder
{
    internal class TileBuilder(TextureConfig config) : AbstractBuilder(config, typeof(TileBuilder))
    {

        private readonly TextureConfig _config = config;

        protected override void Initialize()
        {
            // Not required for tiles
        }

        protected override void Register()
        {
            Tile.Register(
                $"SimpleObjectLoader.{_config.Name}",
                _config.Price,
                _config.TextureRegions,
                new Point(_config.FirstColumn, _config.FirstRow),
                ObjectUtils.ParseColorSchemes(_config.ColorSchemes)
                );
        }
    }
}
