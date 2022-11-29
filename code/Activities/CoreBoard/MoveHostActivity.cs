using System;
using System.Linq;
using BonitoBlitz.Entities.CoreBoard;
using libblitz;
using Sandbox;

namespace BonitoBlitz.Activities.CoreBoard;

/// <summary>
/// Pushes different activities onto the stack based on required movement type
/// </summary>
public partial class MoveHostActivity : Activity
{
	/* These constructors are required! */
	public MoveHostActivity( ActivityDescription d ) : base( d ) { }
	public MoveHostActivity() { }

	public class Result : ActivityResult
	{
		public int Moves { get; set; }
		public string TileName { get; set; }

		public BaseTile Tile => BaseTile.FromName( TileName );
	}

	/// <summary>
	/// This is what we expect to come back from the next activity.
	/// We give it an amount of moves (Result.Moves), it does as many as it can,
	/// and it gives us the amount of moves it performed and the amount it was
	/// expected to perform.
	/// </summary>
	public class Expectation : ActivityResult
	{
		public int MovesExpected { get; set; }
		public int MovesPerformed { get; set; }
	}

	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		int? moves = result switch
		{
			DiceRollActivity.Result rollResult => rollResult.Moves,
			Expectation expectationResult => expectationResult.MovesExpected - expectationResult.MovesPerformed,
			_ => null
		};

		if ( moves == null )
		{
			throw new UnexpectedActivityResultException(
				"MoveHostActivity requires a DiceRollActivity.Result or MoveHostActivity.Expectation" );
		}

		var actor = Actors.First();

		// Make sure we use BoardPawn
		actor.UseOrCreatePawn<BoardPawn>();

		// Make everyone look at the actor
		foreach ( var member in Members )
		{
			var camera = DynamicCamera.AddOrGet( member.Pawn, actor.Pawn );
			camera.PrePositionOffset = (Vector3.Left * 180) + (Vector3.Up * 60);
		}

		// Do movement logic!
		if ( moves <= 0 )
		{
			LogInfo( "MoveHostActivity done, no more moves left" );
			Game.Current.PopActivity<ActivityResult>( null );

			// todo: should this be BEFORE the PopActivity?
			if ( actor.CurrentTile is IActionTile at )
			{
				at.OnStand( actor );
			}
		}
		else
		{
			var currentTile = actor.CurrentTile;
			switch ( currentTile )
			{
				case IStaticTile:
					// Use BatchActivity
					LogInfo( "-> BatchActivity" );
					Game.Current.PushActivity( CreateDescription().Transform<Movement.BatchActivity>(),
						new Result() { Moves = moves.Value } );
					return;
				case IActivityTile at:
					// Use tile activity
					LogInfo( $"-> (activity tile) {at.ActivityName}" );
					Game.Current.PushActivity( CreateDescription().Transform( at.ActivityName ),
						new Result { Moves = moves.Value, TileName = currentTile.Name } );
					return;
				default:
					LogErr( "Current tile can't be handled by MoveHostActivity!" );
					return;
			}
		}
	}
}
