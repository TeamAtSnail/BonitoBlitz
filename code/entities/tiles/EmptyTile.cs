/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;
using SandboxEditor;

[Library( "gm0_tile_empty" ), HammerEntity]
[Title( "Empty Tile" ), Category( "Player" ), Icon( "place" )]
[Description( "Tile for players to stand on" )]
public class EmptyTile : BaseTile
{
	/// <summary>
	/// Should the tile count as a move?
	/// </summary>
	[Property( Title = "Is Real Tile" )]
	public new bool IsRealTile { get; set; } = false;

	public override void OnPlayerStand( BoardPawn player ) { }

	public override void OnPlayerPass( BoardPawn player ) { }

	public override string Process( BoardPawn player ) => NextTile;
}