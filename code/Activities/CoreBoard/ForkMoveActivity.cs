using System.Linq;
using BonitoBlitz.Entities.CoreBoard;
using libblitz;
using Sandbox;

namespace BonitoBlitz.Activities.CoreBoard;

public partial class ForkMoveActivity : libblitz.Activity
{
	/* These constructors are required! */
	public ForkMoveActivity( ActivityDescription d ) : base( d ) { }

	public ForkMoveActivity() { }

	/// <summary>
	/// This is for data you want to pass to the next activity.
	/// Create an instance of this and provide it to PushActivity / PopActivity
	/// </summary>
	public class Result : MoveControllerActivity.Expectation
	{
	}

	[Net] public BaseTile NextTile { get; set; }

	private ForkTile _currentTile;

	private GameMember _actor;

	private int _moves;

	/// <summary>
	/// Called serverside when the activity is ready to start.
	/// </summary>
	/// <param name="result">Result of previous activity or null</param>
	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		_actor = Actors.First();

		if ( result is not IActivityTile.Result activityResult )
		{
			return;
		}

		_moves = activityResult.Moves;

		_currentTile = (ForkTile)activityResult.Tile;
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
			NextTile = BaseTile.FromName( _currentTile.NextTileOne );
		}
		else if ( Input.Pressed( InputButton.Slot2 ) )
		{
			NextTile = BaseTile.FromName( _currentTile.NextTileTwo );
		}
		else if ( Input.Pressed( InputButton.Slot3 ) )
		{
			NextTile = BaseTile.FromName( _currentTile.NextTileThree );
		}

		if ( Input.Pressed( InputButton.Chat ) )
		{
			_actor.CurrentTile = NextTile;
			Game.Current.PopActivity( new Result() { Moves = _moves - 1 } ); // todo: this sucks
		}
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		if ( NextTile == null )
		{
			return;
		}

		DebugOverlay.ScreenText(
			$"CURRENTLY SELECTED TILE: {NextTile.Name}", new Vector2( 300, 300 ), 0, Color.Cyan );
	}
}
