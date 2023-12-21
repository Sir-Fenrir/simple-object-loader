using Microsoft.Xna.Framework.Graphics;
using MLEM.Data;
using MLEM.Data.Content;
using MLEM.Textures;
using SimpleObjectLoader.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleObjectLoader.Utils
{
    public class TextureUtils
    {

        public static TextureConfig[] loadTextures(RuntimeTexturePacker texturePacker, RawContentManager content, TextureConfig[] textures)
        {
            foreach (var texture in textures) 
            {
                texturePacker.Add(new UniformTextureAtlas(content.Load<Texture2D>(texture.File), texture.NrOfColumns, texture.NrOfRows), r => texture.TextureRegions = r, 1, true);
            }

            return textures;
        }
    }
}
