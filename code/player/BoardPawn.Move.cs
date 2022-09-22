/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public delegate void MovementCompleteEventHandler();

public partial class BoardPawn
{
	[Net]
	private TimeSince MoveAnimationProgress { get; set; }
	[Net]
	private float MoveAnimationLength { get; set; }
	[Net]
	private bool MoveAnimationActive { get; set; } = false;

	private bool HasActiveAnimation => MoveAnimationActive;

	public event MovementCompleteEventHandler MovementAnimationCompleteEvent;

	private void MoveSimulate( Client client )
	{
		if ( !MoveAnimationActive )
			return;

		if ( Tile != null )
		{
			Tile.UpdateMoveAnimation( this, MoveAnimationProgress / MoveAnimationLength );
		}

		// Check if animation is complete
		if ( MoveAnimationProgress < MoveAnimationLength )
			return;

		MoveAnimationActive = false;
		if ( Tile != null )
		{
			Tile.EndMoveAnimation( this );
		}
		MovementAnimationCompleteEvent?.Invoke();
	}

	private void MoveFrameSimulate( Client client )
	{
		if ( !MoveAnimationActive )
			return;

		if ( Tile != null )
		{
			Tile.UpdateMoveAnimation( this, MoveAnimationProgress / MoveAnimationLength );
		}
	}

	private void StartMoving( BaseTile end )
	{
		if ( MoveAnimationActive )
		{
			Log.Warning( "BeginMoveAnimation called while animation already active" );
		}

		if ( Tile == null )
		{
			Log.Error( "BeginMoveAnimation start is null!" );
			return;
		}

		if ( end == null )
		{
			Log.Error( "BeginMoveAnimation end is null!" );
			return;
		}

		MoveAnimationLength = (Tile == null) ? 0.0f : Tile.AnimationSpeed;
		MoveAnimationProgress = 0;

		if ( Tile != null )
		{
			Tile.StartMoveAnimation( this, end );
		}

		// Start animation
		MoveAnimationActive = true;
	}
}