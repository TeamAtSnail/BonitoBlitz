/*
 * PlayerTurnActivity.cs
 * Activity for a players turn
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using System;
using libblitz;

namespace BonitoBlitz.CoreActivities.Board;

public partial class PlayerTurnActivity : Activity
{
	public override Type PawnType => typeof( BoardPawn );

	public override Type HudPanelType => typeof( PlayerTurnPanel );

	public PlayerTurnPanel Panel;

	[Sandbox.Net]
	public Player Player { get; private set; }

	public PlayerTurnActivity( Player player ) : base( null )
	{
		Players.Clear();
		Players.Add( player );
		Player = player;
	}

	public PlayerTurnActivity() : base( null ) { }

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

		if ( Sandbox.Input.Pressed( Sandbox.InputButton.Jump ) )
		{
			Game.Current.SetActivity( new PlayerRollActivity( Player ) );
		}
	}

	public override string ActivityDormant()
	{
		base.ActivityDormant();

		Game.Current.RemovePanelByType( HudPanelType );

		if ( Sandbox.Host.IsServer )
			Delete();

		return null;
	}

	public override void Initialize()
	{
		if ( Sandbox.Host.IsClient )
			Panel = new();
	}
}