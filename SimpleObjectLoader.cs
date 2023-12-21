using ExtremelySimpleLogger;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Data;
using MLEM.Data.Content;
using MLEM.Textures;
using SimpleObjectLoader.Builder;
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
    public override string TestedVersionRange => "[0.38.3]";

    private List<ModConfig> modConfigs;


    public override void Initialize(Logger logger, RawContentManager content, RuntimeTexturePacker texturePacker, ModInfo info)
    {
        SimpleObjectLoader.Logger = logger;
        modConfigs = new ModConfigLoader().GetMods();
        LocalizationUtils.ReadLocalizationFiles(modConfigs);
        TextureUtils.loadTextures(texturePacker, content, modConfigs.SelectMany(m => m.Clothes).ToArray());

    }
    public override void AddGameContent(GameImpl game, ModInfo info)
    {
        Logger.Info("Adding game content");

        foreach (var mod in modConfigs)
        {
            foreach(var furniture in mod.Furniture)
            {
                new FurnitureBuilder(furniture).Build();
            }

            foreach(var clothes in mod.Clothes)
            {
                new ClothingBuilder(clothes).Build();
            }
            
        }

    }

    public override IEnumerable<string> GetCustomFurnitureTextures(ModInfo info)
    {
        return modConfigs
            .SelectMany(config => config.Furniture)
            .Select(f => f.Atlas);
    }

}