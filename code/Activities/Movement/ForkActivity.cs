using System;
using System.Linq;
using BonitoBlitz.Activities.CoreBoard;
using BonitoBlitz.Entities.CoreBoard;
using libblitz;
using Sandbox;

namespace BonitoBlitz.Activities.Movement;

public partial class ForkActivity : Activity
{
	/* These constructors are required! */
	public ForkActivity( ActivityDescription d ) : base( d ) { }
	public ForkActivity() { }

	[Net] public BaseTile NextTile { get; set; }
	[Net] public ForkTile CurrentTile { get; set; }

	private int _movesExpected;
	private GameMember _actor;

	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		_actor = Actors.First();

		// Make sure we use BoardPawn
		_actor.UseOrCreatePawn<BoardPawn>();

		// Make everyone look at the actor
		foreach ( var member in Members )
		{
			var camera = CameraComponent.AddNewOrGet( member.Pawn );
			camera.LookAtEntity( _actor.Pawn );
			camera.PrePositionOffset = (Vector3.Left * 180) + (Vector3.Up * 60);
		}

		if ( result is not MoveHostActivity.Result moveHostResult )
		{
			throw new UnexpectedActivityResultException( "ForkActivity requires a MoveHostActivity.Result" );
		}

		if ( moveHostResult.Tile is not ForkTile forkTile )
		{
			throw new Exception( "Provided tile in ActivityResult not a ForkTile" );
		}

		// Set actor current tile to provided one (just in case the game restarts or something)
		_actor.CurrentTile = moveHostResult.Tile;

		// Set expected moves
		_movesExpected = moveHostResult.Moves;

		// Set current tile
		CurrentTile = forkTile;

		// Set initial tile option
		NextTile = CurrentTile.NextTileOne;
	}

	public override void ActivityClientStart()
	{
		base.ActivityClientStart();

		_actor = Actors.First();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( Host.IsClient )
		{
			return;
		}

		if ( cl != _actor.Client )
		{
			return;
		}

		if ( Input.Pressed( InputButton.Slot1 ) )
		{
			NextTile = BaseTile.FromName( CurrentTile.NextTileOneName );
		}
		else if ( Input.Pressed( InputButton.Slot2 ) )
		{
			NextTile = BaseTile.FromName( CurrentTile.NextTileTwoName );
		}
		else if ( Input.Pressed( InputButton.Slot3 ) )
		{
			NextTile = BaseTile.FromName( CurrentTile.NextTileThreeName );
		}

		if ( Input.Pressed( InputButton.Chat ) )
		{
			_actor.CurrentTile = NextTile;
			Game.Current.PopActivity( new MoveHostActivity.Expectation()
			{
				MovesExpected = _movesExpected, MovesPerformed = 1,
			} );
		}
	}

	private float _rotation = 0.0f;

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		if ( NextTile != null )
		{
			DebugOverlay.Circle( NextTile.Position, Rotation.Identity, _rotation, Color.Green );
			DebugOverlay.Text( NextTile.Name, NextTile.Position, Color.Cyan );
		}
	}
}
