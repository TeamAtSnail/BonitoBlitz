using libblitz;

namespace BonitoBlitz.Activities.CoreBoard;

public class CoreBoardActivity : libblitz.Activity
{
	/* These constructors are required! */
	public CoreBoardActivity( ActivityDescription d ) : base( d ) { }
	public CoreBoardActivity() { }

	/// <summary>
	/// This is for data you want to pass to the next activity.
	/// Create an instance of this and provide it to PushActivity / PopActivity
	/// </summary>
	public class Result : libblitz.ActivityResult
	{
		/// <summary>
		/// Moves left
		/// </summary>
		public int Moves { get; set; }
	}

	/// <summary>
	/// This is for data being provided to this activity.
	/// Other activities can inherit from this Expectation to make passing data to this activity easier.
	/// </summary>
	public class Expectation : libblitz.ActivityResult
	{
		/// <summary>
		/// Moves left
		/// </summary>
		public int Moves { get; set; }
	}

	/// <summary>
	/// Called serverside when the activity is ready to start.
	/// </summary>
	/// <param name="result">Result of previous activity or null</param>
	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

	}
}
