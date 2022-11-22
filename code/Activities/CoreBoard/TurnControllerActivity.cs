using System;
using System.Linq;
using libblitz;
using Sandbox;
using Game = libblitz.Game;

namespace BonitoBlitz.Activities.CoreBoard;

public class TurnControllerActivity : libblitz.Activity
{
	/* These constructors are required! */
	public TurnControllerActivity( ActivityDescription d ) : base( d ) { }
	public TurnControllerActivity() { }

	/// <summary>
	/// This is for data you want to pass to the next activity.
	/// Create an instance of this and provide it to PushActivity / PopActivity
	/// </summary>
	public class Result : libblitz.ActivityResult
	{
		public int MyValue = 123;
	}

	/// <summary>
	/// Called serverside when the activity is ready to start.
	/// </summary>
	/// <param name="result">Result of previous activity or null</param>
	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		Log.Info( $"Hello from TurnControllerActivity.ActivityStart! I'm running on the {Host.Name}" );
		Log.Info( "Selecting random actor..." );

		var actor = Actors.OrderBy( qu => Guid.NewGuid() ).First();
		Log.Info( $"Selected actor {actor.Uid} / {actor.Client.Name}" );

		// We are done here! Move to the next activity...
		var description = CreateDescription().Transform( "PlayerTurnActivity" );

		// Push the next activity onto the Activity Stack
		Game.Current.PushActivity( description, new Result() );
	}
}
