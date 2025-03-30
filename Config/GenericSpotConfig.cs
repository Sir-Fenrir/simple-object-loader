namespace SimpleObjectLoader.Config;

/// <summary>
/// Base class for ObjectSpots configurations.
/// This one is enough for:
/// BarSpots
/// TreeSpots
/// </summary>
public class GenericSpotConfig
{
    public float? YOffset { get; set; }

    public float? DrawLayer { get; set; }
}

/// <summary>
/// As I understand for now, these properties are necessary 
/// for determining the location of the Tiny relative to the 
/// object while interacting with it.
/// </summary>
public class ActionSpotConfig : GenericSpotConfig
{
    public float? VectorX { get; set; }

    public float? VectorY { get; set; }

    public string Direction { get; set; }

}

public class CounterSpotsConfig : GenericSpotConfig
{
    public bool Stove { get; set; } = false;
}

public class DeskSpotsConfig : GenericSpotConfig
{
    public bool Chairs { get; set; } = true;
}

public class PicnicTableSpotsConfig : GenericSpotConfig
{
    public int[] Size { get; set; }
}

public class SingleShelfSpotsConfig : GenericSpotConfig
{
    public float? Height { get; set; }
}

public class TableSpotsConfig : GenericSpotConfig
{
    /// <summary>
    /// If you're creating a table, you need some spots to sit. Generally this can be the same as <see cref="FurnitureConfig.Size"/>.
    /// </summary>
    public int[] Size { get; set; }

    public float? Height { get; set; }
}
