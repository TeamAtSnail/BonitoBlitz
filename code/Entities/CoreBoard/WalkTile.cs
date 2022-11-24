using BonitoBlitz.Activities;
using libblitz;
using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.Entities.CoreBoard;

[Library( "bb_tile_walk" ), HammerEntity]
[Title( "Walk Tile" ), Category( "Player" ), Icon( "place" )]
[Description( "Tile for players to walk over" )]
public partial class WalkTile : BaseTile, IStaticTile, IBasicAnimationTile
{
	[Property( Title = "Next Tile Name" ), FGDType( "target_destination" )]
	[Net]
	public string NextName { get; set; }

	[Property( Title = "Animation Length" )]
	public float AnimationLength { get; set; } = 0.7f;

	private Rotation _endRotation;

	public void StartAnimation( GameMember member, Entity to )
	{
		if ( member.Pawn is not BoardPawn pawn )
		{
			return;
		}

		// calculate delta...
		var animationDelta = to.Position - Position;

		// set walking animation
		pawn.SetAnimParameter( "move_x", animationDelta.x );
		pawn.SetAnimParameter( "move_y", animationDelta.y );
		pawn.SetAnimParameter( "move_z", animationDelta.z );

		// calculate end rotation...
		_endRotation = Rotation.LookAt( to.Position - pawn.Position, Vector3.Up );

		// set pawn rotation...
		pawn.EyeRotation = _endRotation;
		pawn.Rotation = _endRotation.Angles().WithPitch( 0 ).ToRotation();
	}

	public void UpdateAnimation( GameMember member, Entity to, float progress )
	{
		if ( member.Pawn is not BoardPawn pawn )
		{
			return;
		}

		// calculate position delta...
		var delta = to.Position - Position;

		// set new position
		pawn.Position = Position + (delta * progress);
	}

	public void EndAnimation( GameMember member, Entity to )
	{
		if ( member.Pawn is not BoardPawn pawn )
		{
			return;
		}
	}
}
