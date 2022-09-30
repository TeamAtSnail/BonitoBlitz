/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using System.Collections.Generic;
using Sandbox;

namespace BonitoBlitz;

public partial class BonitoBlitz
{

	[ConCmd.Admin( "gm0_becomeroller" )]
	public static void CSvBecomeRoller( string playerName )
	{
		foreach ( var player in (Game.Current as BonitoBlitz).Players )
		{
			if ( player.Client.Name == playerName )
			{
				Log.Info( $"CSvBecomeRoller creating activity for: {player}" );
				Current.SetActivity( new Board.RollingActivity( player ) );
				return;
			}
		}
		Log.Info( "Player not found" );
	}

	[ConCmd.Admin( "gm0_startallturns" )]
	public static void CSvStartAllTurns()
	{
		foreach ( var client in Client.All )
		{
			if ( client.Pawn is Board.BoardPawn player )
			{
				player.StartTurn(Rand.Int(0, 10));
			}
		}
	}
}