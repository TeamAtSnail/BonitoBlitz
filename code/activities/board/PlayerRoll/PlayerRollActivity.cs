/*
 * PlayerRollActivity.cs
 * Activity for a players roll
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using System;
using libblitz;

namespace BonitoBlitz.CoreActivities.Board;

public partial class PlayerRollActivity : Activity
{
	public override Type PawnType => typeof( BoardPawn );
	public override Type HudPanelType => typeof( PlayerRollPanel );

	public PlayerRollPanel Panel;

	[Sandbox.Net]
	public Player Player { get; private set; }

	public const int MinNumber = 1;
	public const int MaxNumber = 10;
	public const float NumberSwitchTime = 0.05f;

	[Sandbox.Net]
	public int CurrentNumber { get; private set; }

	[Sandbox.Net]
	public Sandbox.TimeSince NumberSwitchProgress { get; private set; }

	public bool Rolling { get; private set; } = true;

	public PlayerRollActivity( Player player ) : base( null )
	{
		Players.Clear();
		Players.Add( player );
		Player = player;
	}

	public PlayerRollActivity() : base( null ) { }

	public override void ActivityActive( string previous, string result )
	{
		base.ActivityActive( previous, result );

		if ( Sandbox.Host.IsClient )
			Game.Current.SetPanel( Panel );

		Game.Current.CurrentTurnPlayer = Player.Uid;

		foreach ( var player in Game.Current.Players )
		{
			(player.Pawn as BoardPawn).Camera.SetFollow( Player.Pawn, new Vector3( 0, 200, 200 ) );
		}
	}

	public override void Simulate( Sandbox.Client client )
	{
		if ( Sandbox.Host.IsClient )
			return;

		if ( client != Player.Client )
			return;

		if ( Rolling )
		{
			// increment number logic
			if ( NumberSwitchProgress > NumberSwitchTime )
			{
				NumberSwitchProgress = 0;
				CurrentNumber++;
			}

			// number roll over logic
			if ( CurrentNumber > MaxNumber )
				CurrentNumber = MinNumber;

			// complete roll logic
			if ( Sandbox.Input.Pressed( Sandbox.InputButton.Jump ) )
			{
				DebugOverlay.ScreenText( $"FINAL ROLL: {CurrentNumber}", Vector2.One * 300, 1, Color.Cyan, 2.0f );
				Rolling = false;
				Game.Current.SetActivity( new PlayerMoveActivity( Player ) );
			}
		}
	}

	public override string ActivityDormant()
	{
		base.ActivityDormant();

		Game.Current.RemovePanelByType( HudPanelType );

		if ( Sandbox.Host.IsServer )
			Delete();

		return CurrentNumber.ToString();
	}

	public override void Initialize()
	{
		if ( Sandbox.Host.IsClient )
			Panel = new();
	}
}