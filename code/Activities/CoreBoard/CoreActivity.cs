using System.Linq;
using libblitz;

namespace BonitoBlitz.Activities.CoreBoard;

/// <summary>
/// Core activity: moves to the next activity required
/// </summary>
public class CoreActivity : Activity
{
	/* These constructors are required! */
	public CoreActivity( ActivityDescription d ) : base( d ) { }
	public CoreActivity() { }

	private GameMember Next( GameMember current )
	{
		if ( current == null )
			return null;

		// we want to get the lowest index ABOVE the provided member
		// if none found then return null
		var memberOrderIndex = int.MaxValue;
		GameMember result = null;

		foreach ( var member in Members )
		{
			if ( member.TurnOrderIndex <= current.TurnOrderIndex )
				continue;

			if ( member.TurnOrderIndex > memberOrderIndex )
				continue;

			result = member;
			memberOrderIndex = member.TurnOrderIndex;
		}

		return result;
	}

	public GameMember First()
	{
		var memberOrderIndex = int.MaxValue;
		GameMember result = null;

		foreach ( var member in Members )
		{
			if ( member.TurnOrderIndex >= memberOrderIndex )
			{
				continue;
			}

			memberOrderIndex = member.TurnOrderIndex;
			result = member;
		}

		return result;
	}

	public override void ActivityStart( ActivityResult result )
	{
		base.ActivityStart( result );

		// Find the member that should have their turn
		var member = Game.Current.LastTurnPlayer == null ? First() : Next( Game.Current.LastTurnPlayer );
		Game.Current.LastTurnPlayer = member;

		// Print the name of it
		LogInfo( $"New turn player (order idx {member.TurnOrderIndex}): {member}/{member.DisplayName}" );

		// Put us into DiceRollActivity
		var description = CreateDescription().Transform<DiceRollActivity>();
		description.Actors = new[] { member };
		Game.Current.PushActivity<ActivityResult>( description, null );
	}
}
