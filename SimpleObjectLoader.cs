using ExtremelySimpleLogger;
using Microsoft.Xna.Framework;
using MLEM.Data;
using MLEM.Data.Content;
using SimpleObjectLoader.Config;
using SimpleObjectLoader.Utils;
using System.Collections.Generic;
using System.Linq;
using TinyLife;
using TinyLife.Mods;
using TinyLife.Objects;

namespace SimpleObjectLoader;

public class SimpleObjectLoader : Mod
{

    // the logger that we can use to log info about this mod
    public static Logger Logger { get; private set; }

    // visual data about this mod
    public override string Name => "Simple Object Loader";
    public override string Description => "A way to add custom objects without programming";
    public override string IssueTrackerUrl => "https://github.com/Sir-Fenrir/simple-object-loader/issues";
    public override string TestedVersionRange => "[0.38.2]";

    private List<ObjectConfig> ObjectConfigs;


    public override void Initialize(Logger logger, RawContentManager content, RuntimeTexturePacker texturePacker, ModInfo info)
    {
        SimpleObjectLoader.Logger = logger;
        Logger.Info("Initialize");

        ObjectConfigs = new ObjectConfigLoader().GetMods();
    }
    public override void AddGameContent(GameImpl game, ModInfo info)
    {
        foreach (var item in ObjectConfigs)
        {
            var combinedCategories = EnvironmentUtils.ParseCategories(item.Categories);

            Logger.Info(combinedCategories.ToString());

            var selectedColorSchemes = EnvironmentUtils.ParseColorSchemes(item.ColorSchemes);

            Logger.Info(selectedColorSchemes.Length);

            var newFurniture =
                new FurnitureType.TypeSettings($"SimpleObjectLoader.{item.ModId}.{item.Name}",
                new Point(item.Size[0], item.Size[1]),
                combinedCategories,
                item.Price,
                selectedColorSchemes);

            if (item.ActionSpots != null)
            {
                Logger.Info(EnvironmentUtils.Directions[item.ActionSpots[0].Direction].Length);
                newFurniture.ActionSpots = item.ActionSpots
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

            if (item.ColorMap != null)
            {
                newFurniture.ColorMap = item.ColorMap;
            }

            // Set special ConstructedType, if any
            if (EnvironmentUtils.Types.TryGetValue(item.Type, out var type))
            {
                newFurniture.ConstructedType = type;
            }

            if (item.TableSpots?.Length > 0)
            {
                newFurniture.ObjectSpots = ObjectSpot.TableSpots(new Point(item.TableSpots[0], item.TableSpots[1])).ToArray();
            }

            FurnitureType.Register(newFurniture);

        }

    }

    public override IEnumerable<string> GetCustomFurnitureTextures(ModInfo info)
    {
        return ObjectConfigs.
            Select(config => $"{config.FilePath}\\{config.Atlas}");
    }

}