using BonitoBlitz.Activities;
using libblitz;
using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.Entities.CoreBoard;

[Library( "bb_tile_walk" ), HammerEntity]
[Title( "Walk Tile" ), Category( "Tile" ), Icon( "place" )]
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

		var animationHelper = new CitizenAnimationHelper( pawn );
		animationHelper.WithVelocity( delta );
		animationHelper.WithWishVelocity( delta );
		animationHelper.WithLookAt( to.Position );
		animationHelper.IsWeaponLowered = true;
	}

	public void EndAnimation( GameMember member, Entity to )
	{
	}
}
