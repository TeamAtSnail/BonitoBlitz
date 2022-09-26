/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace BonitoBlitz;
using Sandbox;

public partial class BonitoBlitz
{
	[ConCmd.Admin( "gm0_followme" )]
	public static void CSvFollowMe()
	{
		foreach ( var client in Client.All )
		{
			if ( client.Pawn is Board.BoardPawn player )
			{
				var camera = new Board.FollowCamera( ConsoleSystem.Caller.Pawn )
				{
					FixedRotation = (Game.Current as BonitoBlitz).BoardActivity.StartCamera.Rotation,
					PositionOffset = new Vector3( 0, -200, 100 )
				};
				player.Camera = camera;
			}
		}
	}

	[ConCmd.Admin( "gm0_startallturns" )]
	public static void CSvStartAllTurns()
	{
		foreach ( var client in Client.All )
		{
			if ( client.Pawn is Board.BoardPawn player )
			{
				player.StartTurn();
			}
		}
	}
}