/*
 * BoardActivity.cs
 * Core activity for the game board experience
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using System;
using System.Collections.Generic;
using libblitz;

namespace BonitoBlitz.CoreActivities.Board;

public partial class BoardActivity : Activity
{
	public override Type PawnType => typeof( BoardPawn );

	public override Type HudPanelType => null;

	public BoardActivity( List<Player> players ) : base( players )
	{
	}

	public BoardActivity() : base( null )
	{
	}

	public override void Initialize()
	{
		if ( Sandbox.Host.IsClient )
			return;

		// Just create our pawns
		foreach ( var player in Players )
		{
			var pawn = new BoardPawn( player );
			player.Pawn = pawn;
		}
	}
}