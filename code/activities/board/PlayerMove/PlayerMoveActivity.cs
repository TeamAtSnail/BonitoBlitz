/*
 * PlayerMoveActivity.cs
 * Activity for a players turn
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using System;
using libblitz;

namespace BonitoBlitz.CoreActivities.Board;

public partial class PlayerMoveActivity : Activity
{
	public override Type PawnType => typeof( BoardPawn );

	public override Type HudPanelType => null;

	[Sandbox.Net]
	public Player Player { get; private set; }

	[Sandbox.Net, Sandbox.Change]
	private string InternalTileName { get; set; } = null;
	void OnInternalTileNameChanged( string o, string n ) => InternalTile = null;
	private BaseTile InternalTile = null;
	public BaseTile Tile
	{
		get
		{
			if ( Sandbox.Host.IsServer && InternalTileName == null )
				InternalTileName = Player.SavedTileName;
			if ( InternalTile == null && Player != null && InternalTileName != null )
				InternalTile = BaseTile.FromTileName( InternalTileName );
			return InternalTile;
		}
		set
		{
			if ( Sandbox.Host.IsClient )
			{
				InternalTile = null;
				return;
			}

			InternalTile = value;
			InternalTileName = value?.Name;
			Player.SavedTileName = InternalTileName;
		}
	}

	[Sandbox.Net, Sandbox.Change]
	private string InternalNextTileName { get; set; } = null;
	public string NextTileName
	{
		get => InternalNextTileName;
		set
		{
			InternalNextTileName = value;
			InternalNextTile = null;
		}
	}
	void OnInternalNextTileNameChanged( string o, string n ) => InternalNextTile = null;
	private BaseTile InternalNextTile = null;
	public BaseTile NextTile
	{
		get
		{
			if ( InternalNextTile == null && InternalNextTileName != null )
				InternalNextTile = BaseTile.FromTileName( InternalNextTileName );
			return InternalNextTile;
		}
	}

	/// <summary>
	/// Moves left
	/// </summary>
	[Sandbox.Net]
	public int MovesLeft { get; private set; } = 999;

	/// <summary>
	/// Animation Time To Complete
	/// </summary>
	[Sandbox.Net]
	public Sandbox.TimeUntil AnimationTime { get; set; } = 0;

	[Sandbox.Net]
	public float AnimationLength { get; set; } = 0;

	public float AnimationProgress => 1 - (AnimationTime.Relative / AnimationLength);

	public PlayerMoveActivity( Player player ) : base( null )
	{
		Players.Clear();
		Players.Add( player );
		Player = player;
	}

	public PlayerMoveActivity() : base( null ) { }

	public override void ActivityActive( string previous, string result )
	{
		base.ActivityActive( previous, result );

		Game.Current.CurrentTurnPlayer = Player.Uid;

		if ( !int.TryParse( result, out int resultInt ) )
			throw new Exception( "PlayerMoveActivity requires turn count to be provided by previous activity" );

		MovesLeft = resultInt;
	}

	public override void Simulate( Sandbox.Client client )
	{
		if ( client != Player.Client )
			return;

		// Check for activity completion
		if ( MovesLeft == 0 )
		{
			// Activity complete!
			if ( Sandbox.Host.IsServer )
				Game.Current.SetActivityByType<BoardActivity>();
			return;
		}

		// Check for animation completion
		if ( AnimationTime && NextTile != null )
		{
			// Animation complete!
			// Clean up after previous animation and move to next one
			Tile.EndMoveAnimation( Player, NextTile );

			if ( NextTile != null )
			{
				Tile = NextTile;

				if ( Tile.IsRealTile )
					MovesLeft--;
			}

			NextTileName = null;
		}
		else if ( !AnimationTime )
		{
			// Animation incomplete!
			if ( Sandbox.Host.IsServer )
				Tile.UpdateMoveAnimation( Player, NextTile, AnimationProgress );
			return;
		}

		if ( MovesLeft != 0 )
		{
			if ( Sandbox.Host.IsServer )
				NextTileName = Tile.Process( Player );

			// Check if the next tile has been confirmed
			if ( Sandbox.Host.IsServer && NextTileName != null )
			{
				// Next tile received
				AnimationTime = Tile.AnimationSpeed;
				AnimationLength = Tile.AnimationSpeed;

				Tile.StartMoveAnimation( Player, NextTile );
			}
		}
	}

	public override void FrameSimulate( Sandbox.Client client )
	{
		if ( client != Player.Client )
			return;

		if ( MovesLeft == 0 )
		{
			// Activity complete!
			return;
		}

		if ( !AnimationTime )
		{
			// Animation incomplete!
			if ( Tile != null && NextTile != null )
				Tile.UpdateMoveAnimation( Player, NextTile, AnimationProgress );
		}
	}

	public override string ActivityDormant()
	{
		base.ActivityDormant();

		if ( Sandbox.Host.IsServer )
			Delete();

		return null;
	}

	public override void Initialize() { }
}