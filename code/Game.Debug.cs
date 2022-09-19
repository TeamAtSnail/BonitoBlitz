/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public partial class Gamemode0 : libgm0.Game
{
	[ConCmd.Admin( "gm0_setmapcamera" )]
	public static void CSvSetMapCamera( string name = "mc_overview" )
	{
		BroadcastCamera( name );
	}

	[ConCmd.Admin( "gm0_followme" )]
	public static void CSvFollowMe()
	{
		foreach ( var client in Client.All )
		{
			if ( client.Pawn is BoardPawn player )
			{
				var camera = new FollowCamera( ConsoleSystem.Caller.Pawn )
				{
					FixedRotation = (Game.Current as Gamemode0).StartCamera.Rotation,
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
			if ( client.Pawn is BoardPawn player )
			{
				player.StartTurn();
			}
		}
	}

	[ConCmd.Admin( "gm0_runstart" )]
	public static void CSvRunStartCode() => (Game.Current as gm0.Gamemode0).OnAllPlayersConnected();
}