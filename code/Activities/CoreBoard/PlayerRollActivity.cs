using System.Linq;
using BonitoBlitz.Activities.CoreBoard;
using libblitz;
using Sandbox;
using Game = libblitz.Game;

namespace BonitoBlitz.Activities.CoreBoard;

public partial class PlayerRollActivity : libblitz.Activity
{
	/* These constructors are required! */
	public PlayerRollActivity( ActivityDescription d ) : base( d ) { }
	public PlayerRollActivity() { }

	public class Result : MoveControllerActivity.Expectation
	{
	}

	[Net] private int Roll { get; set; } = 0;
	private TimeUntil _timer;
	private GameMember _actor;

	public override void ActivityClientStart()
	{
		base.ActivityClientStart();

		_actor = Actors.First();
	}

	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		_timer = 0.3f;

		_actor = Actors.First();
	}

	public override void Simulate( Client cl )
	{
		if ( Host.IsClient )
		{
			return;
		}

		if ( cl == _actor.Client && Input.Pressed( InputButton.Chat ) )
		{
			var desc = CreateDescription().Transform<MoveControllerActivity>();
			Game.Current.PushActivity( desc, new Result() { Moves = Roll } );
		}

		if ( !(_timer <= 0) )
		{
			return;
		}

		_timer = 0.3f;
		Roll++;
		if ( Roll > 10 )
			Roll = 0;
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		if ( _actor == null )
		{
			return;
		}

		DebugOverlay.ScreenText( $"roll active: {Roll}", Vector2.One * 150 );
	}
}
