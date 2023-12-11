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

public class SimpleObjectLoader : Mod {

    // the logger that we can use to log info about this mod
    public static Logger Logger { get; private set; }

    // visual data about this mod
    public override string Name => "Simple Object Loader";
    public override string Description => "A way to add custom objects without programming";
    public override string IssueTrackerUrl => "https://github.com/Sir-Fenrir/simple-object-loader/issues";
    public override string TestedVersionRange => "[0.38.0]";

    private List<ObjectConfig> _objects;


    public override void Initialize(Logger logger, RawContentManager content, RuntimeTexturePacker texturePacker, ModInfo info) {
        SimpleObjectLoader.Logger = logger;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        string json = File.ReadAllText("table.json");
        var config = JsonSerializer.Deserialize<ObjectConfig>(json, options);

        _objects = new List<ObjectConfig>();

        _objects.Add(config);
    }

    public override void AddGameContent(GameImpl game, ModInfo info) {

        var baseType = typeof(Furniture);
        var assembly = typeof(Furniture).Assembly;
        var furnitureTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));

        var types = furnitureTypes.ToDictionary(
            type => type.Name,
            type => type);

        var categories = typeof(ObjectCategory).GetFields();
      

        foreach (var item in _objects)
        {
            Logger.Info($"ModId: {item.TexturePath}");
            Logger.Info($"Type: {item.Type}");
            Logger.Info($"Name: {item.Name}");
            Logger.Info($"TexturePath: {item.TexturePath}");
            
            // Aggregating the Category types
            var output = categories.Where(category => item.Categories.Contains(category.Name))
                .Select(category => (ObjectCategory)category.GetValue(null))
                .Aggregate((x, y) => x |= y);


            // Atlas file needs to match this mod right now
            FurnitureType.Register(new FurnitureType.TypeSettings($"SimpleObjectLoader.{item.ModId}.{item.Name}", new Point(1, 1), output, 150, ColorScheme.SimpleWood)
            {
                // specify the type that should be constructed when this furniture type is placed
                // if this is not specified, the  Furniture class is used, which is used for furniture without special animations or data
                // specifying icons for custom clothes and furniture is optional, but using the mod's icon helps users recognize a mod's features
                Icon = this.Icon,
                // allow chairs and plates to be slotted into and onto the table
                ObjectSpots = ObjectSpot.TableSpots(new Point(1, 1)).ToArray()
            });

        

        }



        // adding a custom furniture item
        //FurnitureType.Register(new FurnitureType.TypeSettings("SimpleObjectLoader.CustomTable", new Point(1, 1), ObjectCategory.Table , 150, //ColorScheme.SimpleWood) {
            // specify the type that should be constructed when this furniture type is placed
            // if this is not specified, the  Furniture class is used, which is used for furniture without special animations or data
            // specifying icons for custom clothes and furniture is optional, but using the mod's icon helps users recognize a mod's features
            //Icon = this.Icon,
            // allow chairs and plates to be slotted into and onto the table
            //ObjectSpots = ObjectSpot.TableSpots(new Point(1, 1)).ToArray()
        //});
    }

    public override IEnumerable<string> GetCustomFurnitureTextures(ModInfo info) {
        // tell the game about our custom furniture texture
        // this needs to be a path to a data texture atlas, relative to our "Content" directory
        // the texture atlas combines the png texture and the .atlas information
        // see https://mlem.ellpeck.de/api/MLEM.Data.DataTextureAtlas.html for more info
        yield return "CustomFurniture";
    }

}