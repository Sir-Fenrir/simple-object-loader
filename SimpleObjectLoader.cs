using ExtremelySimpleLogger;
using MLEM.Data;
using MLEM.Data.Content;
using MLEM.Ui.Elements;
using MLEM.Ui;
using SimpleObjectLoader.Builder;
using SimpleObjectLoader.Config;
using SimpleObjectLoader.Utils;
using System.Collections.Generic;
using System.Linq;
using TinyLife;
using TinyLife.Mods;
using MLEM.Formatting;
using MLEM.Font;
using MLEM.Extended.Font;
using TinyLife.Objects;
using System;
using Microsoft.Xna.Framework;
using TinyLife.Uis;

namespace SimpleObjectLoader;

public class SimpleObjectLoader : Mod
{

    // the logger that we can use to log info about this mod
    public static Logger Logger { get; private set; }

    // visual data about this mod
    public override string Name => "Simple Object Loader";

    public override string Version => "1.0.0";

    public override string Description => "A way to add custom objects without programming, made by Sir-Fenrir";
    public override string IssueTrackerUrl => "https://github.com/Sir-Fenrir/simple-object-loader/issues";
    public override string TestedVersionRange => "[0.47.2]";

    public static readonly List<string> Errors = [];

    private List<ModConfig> modConfigs;


    public override void Initialize(Logger logger, RawContentManager content, RuntimeTexturePacker texturePacker, ModInfo info)
    {
        SimpleObjectLoader.Logger = logger;
        modConfigs = new ModConfigLoader().GetMods();
        LocalizationUtils.ReadLocalizationFiles(modConfigs);
        TextureUtils.LoadGenericTextures(texturePacker, content, modConfigs.SelectMany(m => m.Clothes).ToArray());
        TextureUtils.LoadGenericTextures(texturePacker, content, modConfigs.SelectMany(m => m.Tiles).ToArray());
        TextureUtils.LoadWallPaperTextures(texturePacker, content, modConfigs.SelectMany(m => m.Wallpapers).ToArray());
    }

    /// <summary>
    /// Helper method to load items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="modItems">The array of items to load</param>
    /// <param name="builder">A lambda to produce the correct builder</param>
    /// <param name="modName">The name of the mod we're currently loading, for logging purposes</param>
    private void LoadItems<T>(T[] modItems, Action<T> builder, string modName) where T : Named
    {
        foreach (T modItem in modItems)
        {
            try
            {
                builder(modItem);
            } catch (Exception ex) {
                if(ex.InnerException != null) ex = ex.InnerException;
                Errors.Add($"Failed to load {modItem.Name} from <i>{modName}</i> with message <c Red>{ex.Message}</c>");
                Logger.Error(ex);
            }
        }
            
    }
    public override void AddGameContent(GameImpl game, ModInfo info)
    {
        Logger.Info("Adding game content");

        foreach (var mod in modConfigs)
        {
            LoadItems<FurnitureConfig>(mod.Furniture, (furniture) => new FurnitureBuilder(furniture).Build(), mod.ModName);
            LoadItems<TextureConfig>(mod.Clothes, (clothes) => new ClothingBuilder(clothes).Build(), mod.ModName);
            LoadItems<TextureConfig>(mod.Wallpapers, (wallpaper) => new WallpaperBuilder(wallpaper).Build(), mod.ModName);
            LoadItems<TextureConfig>(mod.Tiles, (tiles) => new TileBuilder(tiles).Build(), mod.ModName);
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
            OnPressed = element => GameImpl.Instance.UiSystem.Add("Loaded mods", ModInfoBox()),
            PositionOffset = new Vector2(0, 1)
        });

    }

    /// <summary>
    /// Helper method for building the loaded mod list.
    /// </summary>
    /// <returns>An Element containing all the info about the loaded mods.</returns>
    private Element ModInfoBox()
    {
        var cGroup = new CoveringGroup();
        var infoBox = new Panel(Anchor.Center, new Vector2(200, 1), Vector2.Zero, setHeightBasedOnChildren: true);
        cGroup.AddChild(infoBox);

        infoBox.AddChild(new Paragraph(Anchor.AutoCenter, 1, "<b>Loaded mods:</b>"));
        foreach (var mod in modConfigs)
        {
            infoBox.AddChild(new Paragraph(Anchor.AutoLeft, 1, $"<i>{mod.ModName}</i> by {mod.Author}"));
        }

        if (Errors.Count > 0)
        {
            infoBox.AddChild(new Paragraph(Anchor.AutoCenter, 1, "<c Red>Errors:</c>"));

            foreach (var error in Errors)
            {
                infoBox.AddChild(new Paragraph(Anchor.AutoCenter, 1, error));
            }
        }

        return cGroup;
    }


}