using libblitz;
using Sandbox;
using Game = libblitz.Game;

namespace BonitoBlitz.Activities.Introduction;

public class TutorialActivity : libblitz.Activity
{
	public override void ActivityStart( ActivityResult result )
	{
		Log.Info( "tutorial time..." );
		Log.Info( "hellooooooooooo new york!!!!" );

		foreach ( var actor in Actors )
		{
			var pawn = actor.GetOrCreatePawn<BoardPawn>( new object[] { actor } );
			actor.UsePawn( pawn );
		}
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( Input.Pressed( InputButton.Chat ) )
		{
			Game.Current.PushActivity<ActivityResult>( CreateDescription().Transform( "TestActivity" ), null );
		}
	}

	public TutorialActivity( ActivityDescription d ) : base( d ) { }
}
