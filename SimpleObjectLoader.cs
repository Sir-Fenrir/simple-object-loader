using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using ExtremelySimpleLogger;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Data;
using MLEM.Data.Content;
using MLEM.Misc;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using TinyLife;
using TinyLife.Actions;
using TinyLife.Emotions;
using TinyLife.Mods;
using TinyLife.Objects;
using TinyLife.Utilities;
using TinyLife.World;
using Action = TinyLife.Actions.Action;

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

    private FileInfo[] _simpleMods;

    private List<ObjectConfig> _objects;


    public override void Initialize(Logger logger, RawContentManager content, RuntimeTexturePacker texturePacker, ModInfo info)
    {
        SimpleObjectLoader.Logger = logger;
        Logger.Info("Initialize");

        initMods();
    }
    public override void AddGameContent(GameImpl game, ModInfo info)
    {

        var baseType = typeof(Furniture);
        var assembly = typeof(Furniture).Assembly;
        var furnitureTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));

        var types = furnitureTypes.ToDictionary(
                type => type.Name,
                type => type
            );

        var categories = typeof(ObjectCategory).GetFields();
        var colorSchemes = typeof(ColorScheme).GetFields()
            .ToDictionary(
                scheme => scheme.Name,
                scheme => (ColorScheme)scheme.GetValue(null)
            );

        var directions = typeof(Direction2Helper).GetFields()
            .ToDictionary(
                scheme => scheme.Name,
                scheme => (Direction2[])scheme.GetValue(null)
            );

        foreach (var item in _objects)
        {
            // Aggregating the Category types
            var combinedCategories = categories.Where(category => item.Categories.Contains(category.Name))
                .Select(category => (ObjectCategory)category.GetValue(null))
                .Aggregate((x, y) => x |= y);

            var selectedColorSchemes = item.ColorSchemes
                .Select(scheme => colorSchemes[scheme]).ToArray();
            //.Aggregate((x, y) => { x.MergeWith(y); return x; });

            var newFurniture =
                new FurnitureType.TypeSettings($"SimpleObjectLoader.{item.ModId}.{item.Name}",
                new Point(item.Size[0], item.Size[1]),
                combinedCategories,
                item.Price,
                selectedColorSchemes);


            if (item.ActionSpots != null)
            {
                Logger.Info(item.ActionSpots[0].Direction);
                newFurniture.ActionSpots = item.ActionSpots
                    .Select(spot => 
                    new ActionSpot(
                        new Vector2(spot.VectorX, spot.VectorY), 
                        spot.YOffset, 
                        directions[spot.Direction])
                    {
                        DrawLayer = f => spot.DrawLayer
                    })
                    .ToArray();
            }

            if(item.ColorMap != null)
            {
                newFurniture.ColorMap = item.ColorMap;
            }

            // Set special ConstructedType, if any
            if (types.TryGetValue(item.Type, out var type))
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
        Logger.Info("GetCustomFurnitureTextures");
        // tell the game about our custom furniture texture
        // this needs to be a path to a data texture atlas, relative to our "Content" directory
        // the texture atlas combines the png texture and the .atlas information
        // see https://mlem.ellpeck.de/api/MLEM.Data.DataTextureAtlas.html for more info

        List<string> paths = [];

        for (int i = 0; i < _simpleMods.Length; i++)
        {
            paths.Add($"{_simpleMods[i].FullName.Replace(_simpleMods[i].Name, "")}\\{_objects[i].Atlas}");
        }


        return paths;
    }

    public void initMods()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var modsFolder = ModLoader.GetModsFolder();
        _simpleMods = modsFolder.EnumerateDirectories()
            .SelectMany(x => x.EnumerateFiles())
            .Where(x => x.Name.EndsWith(".simple.json"))
            .ToArray();

        _objects = _simpleMods
            .Select(x => File.ReadAllText(x.FullName))
            .Select(x => JsonSerializer.Deserialize<ObjectConfig>(x, options))
            .ToList();
    }

}