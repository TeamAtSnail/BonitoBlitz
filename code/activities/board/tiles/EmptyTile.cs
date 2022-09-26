/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace BonitoBlitz.Board;
using Sandbox;
using SandboxEditor;

[Library( "gm0_tile_empty" ), HammerEntity]
[Title( "Empty Tile" ), Category( "Player" ), Icon( "place" )]
[Description( "Tile for players to stand on" )]
public class EmptyTile : BaseTile
{
	public override void OnPlayerStand( BoardPawn player ) { }

	public override void OnPlayerPass( BoardPawn player ) { }

	public override string Process( BoardPawn player ) => NextTile;
}