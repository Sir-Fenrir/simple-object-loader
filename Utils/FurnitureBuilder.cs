using Microsoft.Xna.Framework;
using MLEM.Misc;
using SimpleObjectLoader.Config;
using System;
using System.Linq;
using TinyLife.Objects;
using TinyLife.Tools;

namespace SimpleObjectLoader.Utils
{
    /// <summary>
    /// Dynamically builds a piece of furniture based on the properties filled in the JSON file.
    /// </summary>
    internal class FurnitureBuilder(ObjectConfig config) : AbstractBuilder(config, typeof(FurnitureBuilder))
    {

        private FurnitureType.TypeSettings _typeSettings;

        /// <summary>
        /// Initialize the basics of a Furniture object.
        /// </summary>
        protected override void Initialize()
        {
            var combinedCategories = EnvironmentUtils.ParseCategories(config.Categories);

            var selectedColorSchemes = EnvironmentUtils.ParseColorSchemes(config.ColorSchemes);
            _typeSettings = new FurnitureType.TypeSettings($"SimpleObjectLoader.{config.ModId}.{config.Name}",
            new Point(_config.Size[0], _config.Size[1]),
                combinedCategories,
                _config.Price,
                selectedColorSchemes);

            // Set special ConstructedType, if any
            if (EnvironmentUtils.Types.TryGetValue(_config.Type, out var type))
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
        /// Executed if the <see cref="ObjectConfig.ActionSpots"/> property is not null.
        /// </summary>
        [HandlerFor("ActionSpots")]
        public void ActionSpots()
        {
            _typeSettings.ActionSpots = _config.ActionSpots
                    .Select(spot =>
                    new ActionSpot(
                        new Vector2(spot.VectorX, spot.VectorY),
                        spot.YOffset,
                        EnvironmentUtils.Directions[spot.Direction])
                    {
                        DrawLayer = f => spot.DrawLayer
                    })
                    .ToArray();
        }

        /// <summary>
        /// Executed if the <see cref="ObjectConfig.ColorMap"/> property is not null.
        /// </summary>
        [HandlerFor("ColorMap")]
        public void ColorMap()
        {
            _typeSettings.ColorMap = config.ColorMap;
        }

        /// <summary>
        /// Executed if the <see cref="ObjectConfig.TableSpots"/> property is not null.
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
        /// Executed if the <see cref="ObjectConfig.DefaultRotation"/> property is not null.
        /// </summary>
        [HandlerFor("DefaultRotation")]
        public void DefaultRotation()
        {
            _typeSettings.DefaultRotation = Enum.Parse<Direction2>(config.DefaultRotation, true);
        }

        /// <summary>
        /// Executed if the <see cref="ObjectConfig.Tab"/> property is not null.
        /// </summary>
        [HandlerFor("Tab")]
        public void Tab()
        {
            _typeSettings.Tab = Enum.Parse<FurnitureTool.Tab>(_config.Tab);
        }
    }
}
