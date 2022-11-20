using libblitz;

namespace BonitoBlitz.Activities.Introduction;

public class TestActivity : libblitz.Activity
{
	public override void ActivityStart( ActivityResult result )
	{
		Log.Info( "test activity time..." );

		foreach ( var actor in Actors )
		{
			var pawn = actor.GetOrCreatePawn<BoardPawn>( new object[] { actor } );
			actor.UsePawn( pawn );
		}
	}

	public TestActivity( ActivityDescription d ) : base( d ) { }
}
