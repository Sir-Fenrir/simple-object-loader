using SimpleObjectLoader.Utils;
using System.Collections.Generic;
using MLEM.Misc;

namespace SimpleObjectLoader.Config
{
    /// <summary>
    /// Configuration for the mod to load.
    /// </summary>
    public class ObjectConfig
    {

        public string FilePath {  get; set; }
        /// <summary>
        /// Unique name for the mod
        /// </summary>
        public string ModId { get; set; }
        public string Type { get; set; }

        /// <summary>
        /// The name of the item when registering it.
        /// NOTE: This is not the name used in the game,
        /// for that we need the localization files.
        /// </summary>
        public string Name { get; set; }

        public int Price { get; set; }

        /// <summary>
        /// The categories this object belongs to, if applicable.
        /// </summary>
        public string[] Categories { get; set; }

        /// <summary>
        /// For when you want to specify which tab the object appears in in the build menu.
        /// </summary>
        public string Tab {  get; set; }

        /// <summary>
        /// If this object requires a specific texture (like wallpapers or clothing), we need the name of the file.
        /// </summary>
        public string TextureFile { get; set; }

        /// <summary>
        /// When adding furniture, we need an atlas file describing the orientation of the texture file of the same name.
        /// </summary>
        public string Atlas { get; set; }

        /// <summary>
        /// If you're creating furniture, we need the size. It expects an array of two values, 
        /// the first being the x, the second being the y.
        /// </summary>
        public int[] Size { get; set; }

        /// <summary>
        /// The ColorSchemes you want to use for the furniture, can be multiple.
        /// For options, see <see cref="ColorSchemes"/>
        /// </summary>
        public string[] ColorSchemes { get; set; }

        /// <summary>
        /// In case you want to set a default rotation for the object, for options see <see cref="Direction2"/>
        /// </summary>
        public string DefaultRotation { get; set; }

        /// <summary>
        /// If you're creating a table, you need some spots to sit. Generally this can be the same as <see cref="Size"/>.
        /// </summary>
        public int[] TableSpots { get; set; }

        /// <summary>
        /// All the places a Tiny can interact with/from with the furniture.
        /// </summary>
        public ActionSpotConfig[] ActionSpots { get; set; }

        /// <summary>
        /// Set the colors for the different layers, if applicable.
        /// It is an array, with as many elements as there are layers in your textures. 
        /// The numbers in it correspond to the given <see cref="ColorSchemes"/>. 
        /// If you have three layers and two ColorSchemes, this could be [1, 0, 1], 
        /// with the 0 referencing the first ColorScheme, the 1 the second.
        /// </summary>
        public int[] ColorMap { get; set; }
    }
}
