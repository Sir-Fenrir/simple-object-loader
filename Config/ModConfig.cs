namespace SimpleObjectLoader.Config
{
    public class ModConfig : IConfig
    {
        public string ModId { get; set; }

        public string ModName { get; set; }

        public string Author { get; set; }

        public string FilePath { get; set; }

        public FurnitureConfig[] Furniture { get; set; } = [];

        public TextureConfig[] Clothes { get; set; } = [];

        public TextureConfig[] Wallpapers { get; set; } = [];

        public TextureConfig[] Tiles { get; set; } = [];
    }
}
