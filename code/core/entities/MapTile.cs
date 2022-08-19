/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;
using SandboxEditor;

/// <summary>
/// Singular map tile entity; used to denote tile position
/// Requires name and next tile, all position and transforms etc. done in-engine
/// </summary>
[Library( "gm0_map_tile" ), HammerEntity]
[Title( "Map Tile" ), Category( "Player" ), Icon( "place" )]
[Description( "Tile for players to stand on" )]
public class MapTile : Entity
{
	/// <summary>
	/// Names of possible tiles to move to
	/// </summary>
	[Property( Title = "Next Map Tile(s)" )]
	public TagList NextTileOptions { get; set; }
}