using System.Linq;
using libblitz;
using Sandbox;
using Game = libblitz.Game;

namespace BonitoBlitz.Activities;

public partial class PlayerTurnActivity : libblitz.Activity
{
	/* These constructors are required! */
	public PlayerTurnActivity( ActivityDescription d ) : base( d ) { }
	public PlayerTurnActivity() { }

	public class Result : libblitz.ActivityResult
	{
		public int Roll;
	}

	private GameMember _actor;

	[Net] private int Roll { get; set; } = 0;
	private TimeUntil _timer;

	public override void ActivityClientStart()
	{
		base.ActivityClientStart();

		_actor = Actors.First();
	}

	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		_timer = 2;

		_actor = Actors.First();
	}

	public override void Simulate( Client cl )
	{
		if ( Host.IsClient )
			return;

		if ( cl == _actor.Client && Input.Pressed( InputButton.Chat ) )
		{
			var desc = CreateDescription().Transform( "PlayerMoveActivity" );
			Game.Current.PushActivity( desc, new Result() { Roll = Roll } );
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
