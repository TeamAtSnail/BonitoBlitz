/*
 * StartClimbTile.cs
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.CoreActivities.Board;

[Library( "bb_board_tile_start_climb" ), HammerEntity]
[Title( "Start Climb Tile" ), Category( "Player" ), Icon( "place" )]
[Description( "Tile for players to start climbing on (end with another tile)" )]
public partial class StartClimbTile : BaseTile
{
	public override void StartMoveAnimation( libblitz.Player player, BaseTile end )
	{
		base.StartMoveAnimation( player, end );
		(player.Pawn as AnimatedEntity).SetAnimParameter( "b_swim", true );
	}
	public override void EndMoveAnimation( libblitz.Player player, BaseTile end )
	{
		base.EndMoveAnimation( player, end );
		(player.Pawn as AnimatedEntity).SetAnimParameter( "b_swim", false );
	}
}