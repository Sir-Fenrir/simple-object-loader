using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleObjectLoader.Config
{
    public class Named
    {

        /// <summary>
        /// The name of the item when registering it.
        /// NOTE: This is not the name used in the game,
        /// for that we need the localization files.
        /// This name is used to find the relevant entries in the localization files and the atlas file.
        /// </summary>
        public string Name { get; set; }

    }
}
