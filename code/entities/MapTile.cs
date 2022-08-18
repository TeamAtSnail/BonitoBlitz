using Sandbox;
using SandboxEditor;

namespace gm0;

/// <summary>
/// Singular map tile entity; used to denote tile position
/// Requires name & next tile, position & transforms done in-engine
/// </summary>
[Library( "gm0_map_tile" ), HammerEntity]
[Title( "Map Tile" ), Category( "Player" ), Icon( "place" )]
public class MapTile : Entity {
    /// <summary>
    /// ID of map tile
    /// </summary>
    [Property( Title = "Map Tile Identifier" ) ]
    public string TileId { get; set; }

    /// <summary>
    /// ID of next map tile to move to
    /// </summary>
    [Property( Title = "Next Map Tile" ) ]
    public string NextTile { get; set; }
}