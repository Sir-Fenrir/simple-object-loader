using ExtremelySimpleLogger;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TinyLife.Mods;
using TinyLife.Objects;

namespace SimpleObjectLoader.Config
{
    /// <summary>
    /// Class for loading all simple mods in the Tiny Life mod directory.
    /// </summary>
    internal class ModConfigLoader
    {

        /// <summary>
        /// We're requiring the 'extension' .simple.json, .simple.json5 or .simple.jsonc, 
        /// to recognize JSON files meant for this modloader.
        /// </summary>
        private static readonly string[] CONFIG_EXTENSIONS = [".simple.json", ".simple.json5", ".simple.jsonc"];

        public static Logger Logger = SimpleObjectLoader.Logger;

        /// <summary>
        /// Search for all simple mods.
        /// </summary>
        /// <returns>A list of simple mod configurations</returns>
        public List<ModConfig> GetMods()
        {
            var modConfigs = Directory.EnumerateFiles(ModLoader.GetModsFolder().FullName, "*", SearchOption.AllDirectories)
                .Where(file => CONFIG_EXTENSIONS.Any(file.ToLower().EndsWith))
                .Select(MapFileToObject)
                .Where(config => config != null)
                .ToList();

            Logger.Info($"{modConfigs.Count} mods discovered");

            return modConfigs;
        }

        /// <summary>
        /// Parse a file into a list of Configs.
        /// 
        /// </summary>
        /// <param name="file">The file to read</param>
        /// <returns>All succesfully loaded configurations</returns>
        private ModConfig MapFileToObject(string file)
        {
            ModConfig mod = new();
            var json = File.ReadAllText(file);

            try
            {
                mod = JsonConvert.DeserializeObject<ModConfig>(json);
            }
            catch (Exception ex)
            {
                Logger.Warn($"Failed to load the {file}:", ex);
                SimpleObjectLoader.Errors.Add($"Failed to read the file {file}");
                return null;
            }

            mod.FilePath = Path.GetDirectoryName(file);

            mod.Furniture.Do(f =>
            {
                f.Atlas = $"{mod.FilePath}\\{f.Atlas}";
                f.Name = $"{mod.ModId}.{f.Name}";
            });

            mod.Clothes.Do(f =>
            {
                f.File = $"{mod.FilePath}\\{f.File}";
                f.Name = $"{mod.ModId}.{f.Name}";
            });

            mod.Wallpapers.Do(f =>
            {
                f.File = $"{mod.FilePath}\\{f.File}";
                f.Name = $"{mod.ModId}.{f.Name}";
            });

            mod.Tiles.Do(f =>
            {
                f.File = $"{mod.FilePath}\\{f.File}";
                f.Name = $"{mod.ModId}.{f.Name}";
            });

            return mod;
        }

    }
}
