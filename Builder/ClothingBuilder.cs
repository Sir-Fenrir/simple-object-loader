using Microsoft.Xna.Framework;
using SimpleObjectLoader.Config;
using SimpleObjectLoader.Utils;
using System;
using System.Linq;
using TinyLife.Objects;

namespace SimpleObjectLoader.Builder
{
    internal class ClothingBuilder(TextureConfig config) : AbstractBuilder(config, typeof(ClothingBuilder))
    {

        private readonly TextureConfig _config = config;

        private Clothes newItem;

        protected override void Initialize()
        {
            var intentions = ObjectUtils.ParseClothingIntentions(_config.ClothesIntention);

            newItem = new Clothes(
                $"SimpleObjectLoader.{_config.Name}", 
                Enum.Parse<ClothesLayer>(_config.Layer, true),
                _config.TextureRegions, 
                new Point(_config.FirstColumn, _config.FirstRow),
                _config.Price,
                intentions,
                Enum.Parse<StylePreference>(_config.StylePreference, true),
                ObjectUtils.ParseColorSchemes(_config.ColorSchemes)
            );
        }

        protected override void Register()
        {
            Clothes.Register(newItem);
        }
    }
}
