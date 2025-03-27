using ExtremelySimpleLogger;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Data;
using MLEM.Data.Content;
using MLEM.Textures;
using SimpleObjectLoader.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TinyLife.World;

namespace SimpleObjectLoader.Utils
{
    public class TextureUtils
    {

        public static TextureConfig[] LoadGenericTextures(RuntimeTexturePacker texturePacker, RawContentManager content, TextureConfig[] textures)
        {
            return LoadTextures(GenericTextures(content), texturePacker, textures);
        }

        public static TextureConfig[] LoadWallPaperTextures(RuntimeTexturePacker texturePacker, RawContentManager content, TextureConfig[] textures)
        {
            return LoadTextures(WallPaperTextures(content), texturePacker, textures);
        }

        /// <summary>
        /// Generic texture loader
        /// </summary>
        /// <param name="loader">The function to load specific textures</param>
        /// <param name="texturePacker"></param>
        /// <param name="textures"></param>
        /// <returns></returns>
        private static TextureConfig[] LoadTextures(Func<TextureConfig, UniformTextureAtlas> loader,
            RuntimeTexturePacker texturePacker,
            TextureConfig[] textures)
        {
            foreach (var texture in textures)
            {
                try
                {
                    texturePacker.Add(
                        loader(texture),
                        r => texture.TextureRegions = r,
                        1,
                        true
                    );
                }
                catch (Exception ex)
                {
                    SimpleObjectLoader.Errors.Add($"Failed to load textures for {texture.Name}");
                    SimpleObjectLoader.Logger.Error(ex);
                }


            }

            return textures;
        }

        private static Func<TextureConfig, UniformTextureAtlas> WallPaperTextures(RawContentManager content)
        {
            return (texture) => WallMode.ApplyMasks(
                    content.Load<Texture2D>(texture.File),
                    texture.NrOfColumns,
                    texture.NrOfRows);
        }

        private static Func<TextureConfig, UniformTextureAtlas> GenericTextures(RawContentManager content)
        {
            return (texture) => new UniformTextureAtlas(
                        content.Load<Texture2D>(texture.File),
                        texture.NrOfColumns,
                        texture.NrOfRows
                        );
        }
    }
}
