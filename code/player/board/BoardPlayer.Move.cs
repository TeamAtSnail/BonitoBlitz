/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public delegate void MovementCompleteEventHandler();

public partial class BoardPlayer
{
	[Net]
	private Vector3 AnimationStartPoint { get; set; }
	[Net]
	private Vector3 AnimationEndPoint { get; set; }

	[Net]
	private TimeSince MoveAnimationProgress { get; set; }
	[Net]
	private float MoveAnimationLength { get; set; }
	[Net]
	private bool MoveAnimationActive { get; set; }

	private bool HasActiveAnimation => MoveAnimationActive;

	private Vector3? preCalculatedDeltaMultiplier;

	public event MovementCompleteEventHandler MovementAnimationCompleteEvent;

	private Vector3 GetDeltaMultiplier()
	{
		if ( preCalculatedDeltaMultiplier == null )
			preCalculatedDeltaMultiplier = (AnimationEndPoint - AnimationStartPoint) / 1;
		return preCalculatedDeltaMultiplier.Value;
	}

	private void ResetDeltaMultiplier() => preCalculatedDeltaMultiplier = null;

	private void MoveSimulate( Client client )
	{
		if ( !MoveAnimationActive )
			return;

		float progressMultiplier = MoveAnimationProgress / MoveAnimationLength;
		Position = AnimationStartPoint + (GetDeltaMultiplier() * progressMultiplier);

		// Check if animation is complete
		if ( MoveAnimationProgress < MoveAnimationLength )
			return;

		MoveAnimationActive = false;
		ResetDeltaMultiplier();
		SetAnimParameter( "move_x", 0 );
		SetAnimParameter( "move_y", 0 );
		SetAnimParameter( "move_z", 0 );
		MovementAnimationCompleteEvent?.Invoke();
	}

	private void MoveFrameSimulate( Client client )
	{
		if ( !MoveAnimationActive )
			return;

		float progressMultiplier = MoveAnimationProgress / MoveAnimationLength;
		Position = AnimationStartPoint + (GetDeltaMultiplier() * progressMultiplier);
	}

	private void BeginMoveAnimation( Vector3 start, Vector3 end, float length )
	{
		if ( MoveAnimationActive )
		{
			Log.Warning( "BeginMoveAnimation called while animation already active" );
		}

		if ( start == null )
		{
			Log.Error( "BeginMoveAnimation start is null!" );
			return;
		}

		if ( end == null )
		{
			Log.Error( "BeginMoveAnimation end is null!" );
			return;
		}

		AnimationStartPoint = start;
		AnimationEndPoint = end;
		MoveAnimationLength = length;
		MoveAnimationProgress = 0;

		// Calculate delta
		Vector3 delta = AnimationEndPoint - AnimationStartPoint;

		// Set walking animation
		SetAnimParameter( "move_x", delta.x );
		SetAnimParameter( "move_y", delta.y );
		SetAnimParameter( "move_z", delta.z );

		// Start animation
		MoveAnimationActive = true;
	}
}