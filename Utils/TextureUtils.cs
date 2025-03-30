using Microsoft.Xna.Framework.Graphics;
using MLEM.Textures;
using SimpleObjectLoader.Config;
using System;
using TinyLife.World;

namespace SimpleObjectLoader.Utils
{
    public class TextureUtils
    {

        public static TextureConfig[] LoadGenericTextures(TextureConfig[] textures)
        {
            return LoadTextures(GenericTextures(), textures);
        }

        public static TextureConfig[] LoadWallPaperTextures(TextureConfig[] textures)
        {
            return LoadTextures(WallPaperTextures(), textures);
        }

        /// <summary>
        /// Generic texture loader
        /// </summary>
        /// <param name="loader">The function to load specific textures</param>
        /// <param name="texturePacker"></param>
        /// <param name="textures"></param>
        /// <returns></returns>
        private static TextureConfig[] LoadTextures(Func<TextureConfig, UniformTextureAtlas> loader,
            TextureConfig[] textures)
        {
            foreach (var texture in textures)
            {
                SOL.Try(
                        () => SOL.TexturePacker.Add(
                            loader(texture),
                            r => texture.TextureRegions = r,
                            1,
                            true
                        ),
                        $"Failed to load textures for {texture.Name}"
                    );
            }

            return textures;
        }

        private static Func<TextureConfig, UniformTextureAtlas> WallPaperTextures()
        {
            return (texture) => WallMode.ApplyMasks(
                    SOL.Content.Load<Texture2D>(texture.File),
                    texture.NrOfColumns,
                    texture.NrOfRows
                    );
        }

        private static Func<TextureConfig, UniformTextureAtlas> GenericTextures()
        {
            return (texture) => new UniformTextureAtlas(
                        SOL.Content.Load<Texture2D>(texture.File),
                        texture.NrOfColumns,
                        texture.NrOfRows
                        );
        }
    }
}
