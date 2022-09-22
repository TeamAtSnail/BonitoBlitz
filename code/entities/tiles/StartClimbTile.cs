/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;
using SandboxEditor;

[Library( "gm0_tile_start_climb" ), HammerEntity]
[Title( "Start Climb Tile" ), Category( "Player" ), Icon( "place" )]
[Description( "Tile for players to start climbing on (end with End Climb Tile)" )]
public partial class StartClimbTile : BaseTile
{
	public override void StartMoveAnimation( BoardPawn pawn, BaseTile endTile )
	{
		base.StartMoveAnimation( pawn, endTile );

		pawn.SetAnimParameter( "b_swim", true );
	}

	public override void EndMoveAnimation( BoardPawn pawn )
	{
		base.EndMoveAnimation( pawn );

		pawn.SetAnimParameter( "b_swim", false );
	}

	public override void OnPlayerStand( BoardPawn player ) => player.Data.Coins += 3;

	public override void OnPlayerPass( BoardPawn player ) { }

	public override string Process( BoardPawn player ) => NextTile;
}