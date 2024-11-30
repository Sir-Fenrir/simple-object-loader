using ExtremelySimpleLogger;
using Microsoft.Xna.Framework;
using MLEM.Maths;
using MLEM.Misc;
using SimpleObjectLoader.Config;
using SimpleObjectLoader.Utils;
using System;
using System.Linq;
using TinyLife.Objects;
using TinyLife.Tools;
using TinyLife.Utilities;

namespace SimpleObjectLoader.Builder
{
    /// <summary>
    /// Dynamically builds a piece of furniture based on the properties filled in the JSON file.
    /// </summary>
    internal class FurnitureBuilder(FurnitureConfig config) : AbstractBuilder(config, typeof(FurnitureBuilder))
    {

        private FurnitureType.TypeSettings _typeSettings;

        private readonly FurnitureConfig _config = config;

        /// <summary>
        /// Initialize the basics of a Furniture object.
        /// </summary>
        protected override void Initialize()
        {
            var combinedCategories = ObjectUtils.ParseCategories(config.Categories);

            var selectedColorSchemes = ObjectUtils.ParseColorSchemes(config.ColorSchemes);
            _typeSettings = new FurnitureType.TypeSettings($"SimpleObjectLoader.{config.Name}",
            new Point(_config.Size[0], _config.Size[1]),
                combinedCategories,
                _config.Price,
                selectedColorSchemes);
        }

        [HandlerFor("Type")]
        public void Type()
        {
            // Set special ConstructedType, if any
            if (ObjectUtils.Types.TryGetValue(_config.Type.ToLower(), out var type))
            {
                _typeSettings.ConstructedType = type;
            }
        }

        /// <summary>
        /// Register the furniture with the game.
        /// </summary>
        protected override void Register()
        {
            FurnitureType.Register(_typeSettings);
        }

        /// <summary>
        /// Executed if the <see cref="FurnitureConfig.ActionSpots"/> property is not null.
        /// </summary>
        [HandlerFor("ActionSpots")]
        public void ActionSpots()
        {
            _typeSettings.ActionSpots = _config.ActionSpots
                    .Select(spot =>
                    new ActionSpot(
                        new Vector2(spot.VectorX, spot.VectorY),
                        spot.YOffset,
                        ObjectUtils.Directions[spot.Direction.ToLower()])
                    {
                        DrawLayer = f => spot.DrawLayer
                    })
                    .ToArray();
        }

        /// <summary>
        /// Executed if the <see cref="FurnitureConfig.ColorMap"/> property is not null.
        /// </summary>
        [HandlerFor("ColorMap")]
        public void ColorMap()
        {
            var colorSettings = new ColorSettings(ObjectUtils.ParseColorSchemes(_config.ColorSchemes))
            {
                Map = _config.ColorMap
            };
            _typeSettings.Colors = colorSettings;
        }

        /// <summary>
        /// Executed if the <see cref="FurnitureConfig.TableSpots"/> property is not null.
        /// </summary>
        [HandlerFor("TableSpots")]
        public void TableSpots()
        {
            _typeSettings.ObjectSpots =
            [
                .. ObjectSpot.TableSpots(
                                new Point(
                                    _config.TableSpots[0],
                                    _config.TableSpots[1])
                                ),
            ];
        }

        /// <summary>
        /// Executed if the <see cref="FurnitureConfig.DefaultRotation"/> property is not null.
        /// </summary>
        [HandlerFor("DefaultRotation")]
        public void DefaultRotation()
        {
            _typeSettings.DefaultRotation = Enum.Parse<Direction2>(config.DefaultRotation, true);
        }

        /// <summary>
        /// Executed if the <see cref="FurnitureConfig.Tab"/> property is not null.
        /// </summary>
        [HandlerFor("Tab")]
        public void Tab()
        {
            _typeSettings.Tab = Enum.Parse<FurnitureTool.Tab>(_config.Tab);
        }

        [HandlerFor("NeedModifier")]
        public void NeedModifier()
        {
            _typeSettings.RestoreNeedModifier = (float)_config.NeedModifier;
        }

        // DecorativeRating
        // WaterRating
        // ElectricityRating
        // EfficiencyModifier
    }
}
