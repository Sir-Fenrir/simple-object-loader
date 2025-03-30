using HarmonyLib;
using SimpleObjectLoader.Config;
using System;

namespace SimpleObjectLoader.Utils
{
    /// <summary>
    /// Helper class to hide some of the syntax for loading mod items.
    /// </summary>
    internal class LoaderUtils
    {

        /// <summary>
        /// Helper method to load items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modItems">The array of items to load</param>
        /// <param name="builder">A lambda to produce the correct builder</param>
        /// <param name="modName">The name of the mod we're currently loading, for logging purposes</param>
        public static void LoadItems<T>(T[] modItems, Action<T> builder, string modName) where T : Named
        {
            modItems.Do(
                (modItem) =>
                    SOL.Try(
                            () => builder(modItem),
                            $"Failed to load {modItem.Name} from <i>{modName}</i>"
                        )
                );
        }

    }
}
