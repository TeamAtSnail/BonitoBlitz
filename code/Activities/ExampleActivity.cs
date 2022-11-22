using libblitz;
using Sandbox;
using Game = libblitz.Game;

namespace BonitoBlitz.Activities;

public class ExampleActivity : libblitz.Activity
{
	/* These constructors are required! */
	public ExampleActivity( ActivityDescription d ) : base( d ) { }
	public ExampleActivity() { }

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

		Log.Info( $"Hello from ActivityStart! I'm running on the {Host.Name}" );

		// Print out all our member names (spectators included)
		foreach ( var member in Members )
		{
			Log.Info( $"Member: {member}" );
		}

		// Print out all our actor names (spectators not included)
		foreach ( var actor in Actors )
		{
			Log.Info( $"Actor: {actor}" );
		}

		// We are done here! Move to the next activity...
		// To set up the next activity with the same players, just use a similar ActivityDescription
		var description = CreateDescription().Transform( "YourNextActivity" );

		// Push the next activity onto the Activity Stack
		//Game.Current.PushActivity( description, new Result() );
	}

	/// <summary>
	/// Called clientside when the activity is ready to start.
	/// The result of the previous activity isn't provided.
	/// </summary>
	public override void ActivityClientStart()
	{
		base.ActivityClientStart();

		Log.Info( $"Hello from ActivityClientStart! I'm running on the {Host.Name}" );
	}
}
