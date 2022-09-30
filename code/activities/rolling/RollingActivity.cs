/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using System;
using Sandbox;

namespace BonitoBlitz.Board;

public partial class RollingActivity : libblitz.Activity
{
	public const int MinNumber = 1;
	public const int MaxNumber = 10;
	public const float NumberSwitchTime = 0.05f;
	[Net] public int CurrentNumber { get; private set; }
	[Net] public TimeSince NumberSwitchProgress { get; private set; }
	public bool Rolling { get; private set; } = true;

	public override Type PawnType => typeof( BoardPawn );
	public override Type HudPanelType => null;

	public libblitz.Player Roller { get; private set; }
	public RollingActivity( libblitz.Player roller ) : base( null )
	{
		Players.Add( roller );
		Roller = roller;
		CurrentNumber = MinNumber;
		NumberSwitchProgress = 0.0f;
	}

	public RollingActivity() : base( null )
	{
	}

	public override void ActivityActive()
	{
		base.ActivityActive();

		if ( Host.IsServer )
		{
			var pawn = Roller.Pawn as Board.BoardPawn;

			var camera = new Board.FollowCamera( pawn )
			{
				PositionOffset = (pawn.Rotation.Forward * -(Vector3.One * 50)) + (Vector3.Up * 50)
			};

			//pawn.Camera = camera;

			pawn.SetAnimParameter( "holdtype", 5 );
		}
	}

	public override void ActivityDormant()
	{
		base.ActivityDormant();

		var pawn = Roller.Pawn as Board.BoardPawn;
		pawn.SetAnimParameter( "holdtype", 0 );
	}

	public override void Simulate( Client cl )
	{
		if ( Host.IsClient )
			return;

		if ( cl == Roller.Client )
		{
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

				// debug output
				DebugOverlay.ScreenText( $"CURRENT ROLL: {CurrentNumber}", Vector2.One * 300, 0, Color.Cyan );

				// complete roll logic
				if ( Input.Pressed( InputButton.Jump ) )
				{
					Log.Info( $"ROLLED {CurrentNumber}!!!" );
					DebugOverlay.ScreenText( $"FINAL ROLL: {CurrentNumber}", Vector2.One * 300, 1, Color.Cyan, 2.0f );
					Rolling = false;
					libblitz.Game.Current.SetActivityByType<BoardActivity>();
					(Roller.Pawn as Board.BoardPawn).StartTurn( CurrentNumber );
					Delete();
				}
			}
		}
	}

	public override void Initialize()
	{

	}
}