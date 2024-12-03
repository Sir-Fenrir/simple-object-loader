using ExtremelySimpleLogger;
using Microsoft.Xna.Framework;
using MLEM.Maths;
using MLEM.Misc;
using SimpleObjectLoader.Config;
using SimpleObjectLoader.Utils;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HarmonyLib;
using TinyLife.Objects;
using TinyLife.Tools;
using TinyLife.Utilities;

namespace SimpleObjectLoader.Builder
{
    /// <summary>
    /// Dynamically builds a piece of furniture based on the properties filled in the JSON file.
    /// </summary>
    [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
    internal class FurnitureBuilder(FurnitureConfig config) : AbstractBuilder(config, typeof(FurnitureBuilder))
    {
        private readonly FurnitureConfig _config = config;
        private FurnitureType.TypeSettings _typeSettings;

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
            ObjectUtils.Types.Do(_type => SimpleObjectLoader.Logger.Info(_type.Key));

            // Set special ConstructedType, if any
            if (ObjectUtils.Types.TryGetValue(_config.Type.ToLower(), out var type))
            {
                SimpleObjectLoader.Logger.Info(type.Name);
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

        [HandlerFor("WaterRating")]
        public void WaterRating()
        {
            _typeSettings.WaterRating = (float)_config.WaterRating;
        }

        [HandlerFor("ElectricityRating")]
        public void ElectricityRating()
        {
            _typeSettings.ElectricityRating = (float)_config.ElectricityRating;
        }

        [HandlerFor("EfficiencyModifier")]
        public void EfficiencyModifier()
        {
            _typeSettings.EfficiencyModifier = (float)_config.EfficiencyModifier;
        }

        [HandlerFor("DecorativeRating")]
        public void DecorativeRating()
        {
            _typeSettings.DecorativeRating = _ => (float)_config.DecorativeRating;
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
                        new Vector2((float)spot.VectorX, (float)spot.VectorY),
                        (float)spot.YOffset,
                        ObjectUtils.Directions[spot.Direction.ToLower()])
                    {
                        DrawLayer = f => (float)spot.DrawLayer
                    })
                .ToArray();
        }

        /// <summary>
        /// Executed if the <see cref="FurnitureConfig.TableSpots"/> property is not null.
        /// </summary>
        [HandlerFor("TableSpots")]
        public void TableSpots()
        {
            var point = new Point(
                _config.TableSpots.Size[0],
                _config.TableSpots.Size[1]);

            if (_config.TableSpots.Height != null && _config.TableSpots.DrawLayer != null)
            {
                _typeSettings.ObjectSpots =
                [
                    .. ObjectSpot.TableSpots(
                        point,
                        (float)_config.TableSpots.Height,
                        (float)_config.TableSpots.DrawLayer
                    ),
                ];
            }
            else
            {
                _typeSettings.ObjectSpots = [.. ObjectSpot.TableSpots(point)];
            }
        }

        [HandlerFor("PicnicTableSpots")]
        public void PicnicTableSpots()
        {
            var point = new Point(
                _config.PicnicTableSpots.Size[0],
                _config.PicnicTableSpots.Size[1]);

            if (_config.PicnicTableSpots.DrawLayer != null)
            {
                _typeSettings.ObjectSpots =
                [
                    .. ObjectSpot.PicnicTableSpots(
                        point,
                        (float)_config.PicnicTableSpots.DrawLayer
                    ),
                ];
            }
            else
            {
                _typeSettings.ObjectSpots = [.. ObjectSpot.PicnicTableSpots(point)];
            }
        }

        [HandlerFor("SingleShelfSpots")]
        public void SingleShelfSpots()
        {
            if (_config.SingleShelfSpots.Height != null && _config.SingleShelfSpots.DrawLayer != null)
            {
                _typeSettings.ObjectSpots =
                [
                    .. ObjectSpot.SingleShelfSpots(
                        (float)_config.SingleShelfSpots.Height,
                        (float)_config.SingleShelfSpots.DrawLayer
                    ),
                ];
            }
            else
            {
                _typeSettings.ObjectSpots = [.. ObjectSpot.SingleShelfSpots()];
            }
        }

        [HandlerFor("DeskSpots")]
        public void DeskSpots()
        {
            if (_config.DeskSpots.YOffset != null && _config.DeskSpots.DrawLayer != null)
            {
                _typeSettings.ObjectSpots =
                [
                    .. ObjectSpot.DeskSpots(
                        (float)_config.DeskSpots.YOffset,
                        _config.DeskSpots.Chairs,
                        (float)_config.DeskSpots.DrawLayer
                    ),
                ];
            }
            else
            {
                _typeSettings.ObjectSpots = [.. ObjectSpot.DeskSpots(chairs: _config.DeskSpots.Chairs)];
            }
        }

        /// <summary>
        /// Executed if the <see cref="FurnitureConfig.Counter"/> property is not null.
        /// </summary>
        [HandlerFor("CounterSpots")]
        public void CounterSpots()
        {
            if (_config.CounterSpots.YOffset != null && _config.CounterSpots.DrawLayer != null)
            {
                _typeSettings.ObjectSpots =
                [
                    .. ObjectSpot.CounterSpots(
                        _config.CounterSpots.Stove,
                        (float)_config.CounterSpots.YOffset,
                        (float)_config.CounterSpots.DrawLayer
                    ),
                ];
            }
            else
            {
                _typeSettings.ObjectSpots = [.. ObjectSpot.CounterSpots(stove: _config.CounterSpots.Stove)];
            }
        }

        [HandlerFor("BarSpots")]
        public void BarSpots()
        {
            if (_config.BarSpots.YOffset != null && _config.BarSpots.DrawLayer != null)
            {
                _typeSettings.ObjectSpots =
                [
                    .. ObjectSpot.BarSpots(
                        (float)_config.BarSpots.YOffset,
                        (float)_config.BarSpots.DrawLayer
                    ),
                ];
            }
            else
            {
                _typeSettings.ObjectSpots = [.. ObjectSpot.BarSpots()];
            }
        }

        [HandlerFor("TreeSpots")]
        public void TreeSpots()
        {
            if (_config.TreeSpots.YOffset != null && _config.TreeSpots.DrawLayer != null)
            {
                _typeSettings.ObjectSpots =
                [
                    .. ObjectSpot.TreeSpots(
                        (float)_config.TreeSpots.YOffset,
                        (float)_config.TreeSpots.DrawLayer
                    ),
                ];
            }
            else
            {
                _typeSettings.ObjectSpots = [.. ObjectSpot.TreeSpots()];
            }
        }
    }
}