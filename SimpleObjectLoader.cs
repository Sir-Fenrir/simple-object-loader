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
using TinyLife.Tools;

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
        ObjectConfigs = new ObjectConfigLoader().GetMods();
        LocalizationUtils.ReadLocalizationFiles(ObjectConfigs);
    }
    public override void AddGameContent(GameImpl game, ModInfo info)
    {
        Logger.Info("Adding game content");

        foreach (var item in ObjectConfigs)
        {
            new FurnitureBuilder(item).Build();
        }

    }

    public override IEnumerable<string> GetCustomFurnitureTextures(ModInfo info)
    {
        return ObjectConfigs.
            Select(config => $"{config.FilePath}\\{config.Atlas}");
    }

}