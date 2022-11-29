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

		if ( result is not MoveHostActivity.Result moveHostResult )
		{
			throw new UnexpectedActivityResultException( "ForkActivity requires a MoveHostActivity.Result" );
		}

		if ( moveHostResult.Tile is not ForkTile forkTile )
		{
			throw new Exception( "Provided tile in ActivityResult not a ForkTile" );
		}

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
}
