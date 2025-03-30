using ExtremelySimpleLogger;
using Microsoft.Xna.Framework;
using MLEM.Data;
using MLEM.Data.Content;
using MLEM.Ui;
using MLEM.Ui.Elements;
using SimpleObjectLoader.Builder;
using SimpleObjectLoader.Config;
using SimpleObjectLoader.Utils;
using System.Collections.Generic;
using System.Linq;
using TinyLife;
using TinyLife.Mods;

using static SimpleObjectLoader.Utils.LoaderUtils;

namespace SimpleObjectLoader;

public class SimpleObjectLoader : Mod
{

    // visual data about this mod
    public override string Name => "Simple Object Loader";

    public override string Version => "1.0.0";

    public override string Description => "A way to add custom objects without programming, made by Sir-Fenrir";
    public override string IssueTrackerUrl => "https://github.com/Sir-Fenrir/simple-object-loader/issues";
    public override string TestedVersionRange => "[0.47.2]";

    private List<ModConfig> modConfigs;


    public override void Initialize(Logger logger, RawContentManager content, RuntimeTexturePacker texturePacker, ModInfo info)
    {
        // Initialize the shared values
        SOL.Init(logger, content, texturePacker);

        // Find all compatible mods
        modConfigs = new ModConfigLoader().GetMods();

        // Read the localization files from the loaded mods and merge them with this mod, so they can be used by the game.
        LocalizationUtils.ReadLocalizationFiles(modConfigs);

        // Load all textures for clothes, tiles and wallpapers.
        // Textures for furniture work differently.
        // They're loaded using GetCustomFurnitureTextures.
        TextureUtils.LoadGenericTextures(modConfigs.SelectMany(m => m.Clothes).ToArray());
        TextureUtils.LoadGenericTextures(modConfigs.SelectMany(m => m.Tiles).ToArray());
        TextureUtils.LoadWallPaperTextures(modConfigs.SelectMany(m => m.Wallpapers).ToArray());
    }

    public override void AddGameContent(GameImpl game, ModInfo info)
    {
        SOL.Logger.Info("Adding game content");

        foreach (var mod in modConfigs)
        {
            LoadItems<FurnitureConfig>(
                mod.Furniture,
                (furniture) => new FurnitureBuilder(furniture).Build(),
                mod.ModName);
            LoadItems<TextureConfig>(
                mod.Clothes,
                (clothes) => new ClothingBuilder(clothes).Build(),
                mod.ModName);
            LoadItems<TextureConfig>(
                mod.Wallpapers,
                (wallpaper) => new WallpaperBuilder(wallpaper).Build(),
                mod.ModName);
            LoadItems<TextureConfig>(
                mod.Tiles,
                (tiles) => new TileBuilder(tiles).Build(),
                mod.ModName);
        }

    }

    public override IEnumerable<string> GetCustomFurnitureTextures(ModInfo info)
    {
        return modConfigs
            .SelectMany(config => config.Furniture)
            .Select(f => f.Atlas);
    }

    public override void PopulateOptions(Group group, ModInfo info)
    {
        group.AddChild(new Button(Anchor.AutoCenter, new Vector2(0.5F, 20), "Show loaded mods")
        {
            OnPressed = element => GameImpl.Instance.UiSystem.Add("Loaded mods", UiUtils.ModInfoBox(modConfigs)),
            PositionOffset = new Vector2(0, 1)
        });

    }

}