using System.Linq;
using libblitz;
using Sandbox;

namespace BonitoBlitz.Activities.CoreBoard;

public partial class DiceRollActivity : Activity
{
	/* These constructors are required! */
	public DiceRollActivity( ActivityDescription d ) : base( d ) { }
	public DiceRollActivity() { }

	public class Result : ActivityResult
	{
		public int Moves { get; set; }
	}

	[Net] private int CurrentRoll { get; set; } = 1;
	private const float Duration = 0.3f;
	private TimeUntil _timer;
	private GameMember _actor;

	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		_actor = Actors.First();

		// Make sure we use BoardPawn
		_actor.UseOrCreatePawn<BoardPawn>();

		// Set up timer
		_timer = Duration;

		// Make everyone look at the actor
		foreach ( var member in Members )
		{
			var camera = DynamicCamera.AddOrGet( member.Pawn, _actor.Pawn );
			camera.PrePositionOffset = (Vector3.Left * 200) + (Vector3.Up * 30);
		}
	}

	public override void ActivityClientStart()
	{
		base.ActivityClientStart();

		_actor = Actors.First();
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( _timer <= 0 )
		{
			_timer = Duration;

			CurrentRoll++;

			if ( CurrentRoll > 10 )
			{
				CurrentRoll = 1;
			}
		}

		DebugOverlay.Text( $"Roll: {CurrentRoll}, press Enter", _actor.EyePosition, Color.Green );

		if ( Host.IsClient )
		{
			return;
		}

		if ( cl == _actor.Client && Input.Pressed( InputButton.Chat ) )
		{
			// Put us into next activity
			LogInfo( $"Roll completed! Result = {CurrentRoll}" );
			Game.Current.PushActivity(
				CreateDescription().Transform<MoveHostActivity>(),
				new Result() { Moves = CurrentRoll } );
		}
	}
}
