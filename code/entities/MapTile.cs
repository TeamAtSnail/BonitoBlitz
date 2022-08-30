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
	/// Tile ID
	/// </summary>
	[Property( Title = "ID" )]
	public int Id { get; set; }

	/// <summary>
	/// Next tile option
	/// </summary>
	[Property( Title = "Next Tile Option 1" )]
	public int Option1 { get; set; }

	/// <summary>
	/// Next tile option
	/// </summary>
	[Property( Title = "Next Tile Option 2" )]
	public int Option2 { get; set; }
}