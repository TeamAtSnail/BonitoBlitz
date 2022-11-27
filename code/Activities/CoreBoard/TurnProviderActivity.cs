using System;
using System.Linq;
using libblitz;

namespace BonitoBlitz.Activities.CoreBoard;

public class TurnProviderActivity : libblitz.Activity
{
	/* These constructors are required! */
	public TurnProviderActivity( ActivityDescription d ) : base( d ) { }
	public TurnProviderActivity() { }

	/// <summary>
	/// This is for data you want to pass to the next activity.
	/// Create an instance of this and provide it to PushActivity / PopActivity
	/// </summary>
	public class Result : libblitz.ActivityResult
	{
	}

	/// <summary>
	/// Called serverside when the activity is ready to start.
	/// </summary>
	/// <param name="result">Result of previous activity or null</param>
	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		Log.Info( "Selecting random actor..." );

		var actor = Actors.OrderBy( qu => Guid.NewGuid() ).First();
		Log.Info( $"Selected random actor {actor.Uid} / {actor.Client.Name}" );

		// We are done here! Move to the next activity...
		var description = CreateDescription().Transform<PlayerRollActivity>();

		// Push the next activity onto the Activity Stack
		Game.Current.PushActivity( description, new Result() );
	}
}
