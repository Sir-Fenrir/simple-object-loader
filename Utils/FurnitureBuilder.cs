using ExtremelySimpleLogger;
using HarmonyLib;
using Microsoft.Xna.Framework;
using SimpleObjectLoader.Config;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinyLife.Objects;

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
            new Point(config.Size[0], config.Size[1]),
                combinedCategories,
                config.Price,
                selectedColorSchemes);

            // Set special ConstructedType, if any
            if (EnvironmentUtils.Types.TryGetValue(config.Type, out var type))
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
            _typeSettings.ActionSpots = config.ActionSpots
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
                                    config.TableSpots[0],
                                    config.TableSpots[1])
                                ),
            ];
        }
    }
}
