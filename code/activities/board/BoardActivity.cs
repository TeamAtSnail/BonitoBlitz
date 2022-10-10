/*
 * BoardActivity.cs
 * Core activity for the game board experience
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using libblitz;

namespace BonitoBlitz.CoreActivities.Board;

public partial class BoardActivity : Activity
{
	public override Type PawnType => typeof( BoardPawn );

	public override Type HudPanelType => null;

	[Sandbox.Net]
	public Player CurrentPlayer { get; private set; } = null;

	public static int TurnsLeft { get => Game.Current.TurnsLeft; set => Game.Current.TurnsLeft = value; }

	public BoardActivity( List<Player> players ) : base( players )
	{
	}

	public BoardActivity() : base( null )
	{
	}

	/// <summary>
	/// Get the player whose turn should be next
	/// </summary>
	/// <param name="currentPlayerUid">UID of the player currently having their turn</param>
	/// <returns>Player up next (if none found the first player in line is returned)</returns>
	public Player GetNextInLine( Guid currentPlayerUid )
	{
		if ( Players.Count == 1 )
			return Players[0]; // Just return only player if needed

		Player currentPlayer = null;
		foreach ( var player in Players )
		{
			if ( player.Uid != currentPlayerUid )
				continue;
			currentPlayer = player;
			break;
		}

		Player result = null;
		foreach ( var player in Players )
		{
			if ( player.TurnOrderIndex <= currentPlayer.TurnOrderIndex )
				continue;

			if ( result == null || player.TurnOrderIndex <= result.TurnOrderIndex )
			{
				result = player;
			}
		}

		if ( result != null && result.Uid == currentPlayerUid )
		{
			// Same player was found, no player comes after
			Log.Info( $"( result != null && result.Uid == currentPlayerUid )" );
			return GetFirstInLine(); // Just return first player
		}

		if ( result == null )
			return GetFirstInLine(); // Just return first player

		return result;
	}

	/// <summary>
	/// Get first player in line
	/// </summary>
	/// <returns>Player (or null)</returns>
	public Player GetFirstInLine()
	{
		Player result = null;
		foreach ( var player in Players )
		{
			if ( result == null || player.TurnOrderIndex <= result.TurnOrderIndex )
			{
				result = player;
			}
		}
		return result;
	}

	/// <summary>
	/// This being called means we are either:
	/// 1. just starting the game for the first time
	/// 2. starting a new game turn
	/// 3. moving to the next players turn
	/// </summary>
	public override void ActivityActive( string previous, string result )
	{
		base.ActivityActive( previous, result );

		switch ( Game.Current.Status )
		{
			case GameStatus.NONE:
				throw new InvalidOperationException( "Tried to proceed with game that hasn't started." );

			case GameStatus.INTRODUCTION_NEEDED:
				// (placeholder): just set the game status to in progress and restart activity
				Log.Info( "INTRO SHOULD START RIGHT NOW!!!" );

				// (sorta placeholder): set map camera to (single) board camera on map
				var camera = All.OfType<BoardCamera>().Single();
				foreach ( var player in Players )
				{
					(player.Pawn as BoardPawn).Camera.SetReplicated( camera );
				}

				// set all player starting tiles to correct tile
				foreach ( var player in Players )
				{
					player.SavedTileName = GameConfiguration.Instance.InitialTileName;
					player.Pawn.Position = GameConfiguration.Instance.InitialTile.Position;
				}

				if ( Sandbox.Host.IsClient )
					return;

				TurnsLeft = 15;
				Game.Current.Status = GameStatus.IN_PROGRESS;
				Game.Current.SetActivityByType<BoardActivity>();
				break;

			case GameStatus.IN_PROGRESS:
				if ( Sandbox.Host.IsClient )
					return;

				var first = GetFirstInLine();

				// Make sure we have a current player
				if ( CurrentPlayer == null )
					CurrentPlayer = first;
				else
					CurrentPlayer = GetNextInLine( CurrentPlayer.Uid );

				// Check for new turn
				if ( CurrentPlayer == first )
				{
					// First player of this turn
					TurnsLeft--;
					Log.Info( $"New turn... {TurnsLeft}" );
				}
				else
				{
					Log.Info( $"Continuing turn... player: {CurrentPlayer}" );
				}

				// Check for game complete / game in-progress
				if ( TurnsLeft <= 0 )
				{
					// (placeholder): game complete logic
					Game.Current.Status = GameStatus.COMPLETED;
				}
				else
				{
					// (placeholder): game turn logic
					Game.Current.SetActivity( new PlayerTurnActivity( CurrentPlayer ) );
				}
				break;
		}
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