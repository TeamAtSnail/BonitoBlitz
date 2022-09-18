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
}