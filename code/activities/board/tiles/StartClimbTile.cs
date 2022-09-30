/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.Board;

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

	public override void OnPlayerStand( BoardPawn pawn ) => pawn.Player.Coins += 3;

	public override void OnPlayerPass( BoardPawn pawn ) { }

	public override string Process( BoardPawn pawn ) => NextTile;
}