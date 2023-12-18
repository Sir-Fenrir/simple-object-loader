using HarmonyLib;
using Newtonsoft.Json.Linq;
using SimpleObjectLoader.Config;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TinyLife.Mods;

namespace SimpleObjectLoader.Utils
{
    /// <summary>
    /// Methods to easily process Localization files.
    /// </summary>
    internal class LocalizationUtils
    {

        private static readonly string LOCALIZATION_EXTENSION = "*.json";

        private static readonly string MOD_DLL = $"{typeof(SimpleObjectLoader).Name}.dll";

        /// <summary>
        /// Find all localization files for the mods in use and write them to the directory of this mod.
        /// </summary>
        /// <param name="configs">All loaded mods</param>
        public static void ReadLocalizationFiles(List<ObjectConfig> configs)
        {
            var result = FindModLocalizationFiles(configs);

            Dictionary<string, JObject> mergedJsonPerLanguage = ParseLocalizationFiles(result);

            if (mergedJsonPerLanguage.Count == 0)
            {
                mergedJsonPerLanguage["en.json"] = new JObject();
            }

            var outputPath = GetLocalizationDir();

            mergedJsonPerLanguage.Do(
                    file => File.WriteAllText(Path.Combine(outputPath, file.Key), file.Value.ToString())
                );

        }

        /// <summary>
        /// Find all localization files for the mods.
        /// </summary>
        private static Dictionary<string, List<FileInfo>> FindModLocalizationFiles(List<ObjectConfig> configs)
        {
            return configs.Select(config => $"{config.FilePath}\\Localization")
                .Where(Directory.Exists)
                .SelectMany(path => Directory.GetFiles(path, "*.json", SearchOption.AllDirectories))
                .Select(file => new FileInfo(file))
                .GroupBy(file => file.Name)
                .ToDictionary(group => group.Key, group => group.ToList());
        }

        /// <summary>
        /// Merge all the found localization files into one JSON file per language.
        /// </summary>
        private static Dictionary<string, JObject> ParseLocalizationFiles(Dictionary<string, List<FileInfo>> files)
        {
            Dictionary<string, JObject> mergedJsonPerLanguage = [];

            foreach (var item in files)
            {
                mergedJsonPerLanguage[item.Key] = item.Value
                    .Select(file => File.ReadAllText(file.FullName))
                    .Select(JObject.Parse)
                    .Aggregate((x, y) =>
                    {
                        x.Merge(y);
                        return x;
                    });
            }

            return mergedJsonPerLanguage;
        }

        /// <summary>
        /// Get the path to the Localization directory for this mod.
        /// </summary>
        private static string GetLocalizationDir()
        {
            var currentDir = Directory
                .GetFiles(ModLoader.GetModsFolder().FullName, MOD_DLL, SearchOption.AllDirectories)
                .Select(Path.GetDirectoryName)
                .First();

            var outputPath = Path.Combine(currentDir, "Content", "SimpleObjectLoader", "Localization");

            return outputPath;
        }
    }
}
