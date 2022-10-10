/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox;

namespace BonitoBlitz.CoreActivities.Board;

public enum BoardCameraModeState
{
	NONE,
	ABSOLUTE,
	FOLLOWING,
	REPLICATE
}

public partial class BoardCameraMode : CameraMode
{
	public BoardCameraMode() { }

	[Net, Local]
	public Entity Target { get; private set; } = null;

	[Net, Local]
	public BoardCameraModeState State { get; private set; } = BoardCameraModeState.ABSOLUTE;

	[Net]
	public Vector3 FollowOffset { get; set; }

	[Net]
	public Rotation FollowRotationOffset { get; set; }

	public const float FollowSpeed = 5.0f;

	/// <summary>
	/// Position offset to add after rotation is calculated
	/// </summary>
	[Net]
	public Vector3 FollowPostOffset { get; set; }

	/// <summary>
	/// Set camera to absolute position / rotation
	/// </summary>
	/// <param name="position">Position</param>
	/// <param name="rotation">Rotation</param>
	public void SetAbsolute( Vector3 position, Rotation rotation )
	{
		State = BoardCameraModeState.ABSOLUTE;
		Position = position;
		Rotation = rotation;
	}

	/// <summary>
	/// Make the camera follow an entity
	/// </summary>
	/// <param name="target">Entity to follow</param>
	/// <param name="positionOffset">Initial position offset from entity position</param>
	/// <param name="rotationOffset">Rotation offset after looking at entity</param>
	/// <param name="postOffset">Position offset to add after rotation has been calculated</param>
	public void SetFollow( Entity target, Vector3? positionOffset = null, Rotation? rotationOffset = null, Vector3? postOffset = null )
	{
		Target = target;
		State = BoardCameraModeState.FOLLOWING;
		FollowOffset = positionOffset ?? Vector3.Zero;
		FollowRotationOffset = rotationOffset ?? new Rotation( 0, 0, 0, 0 );
		FollowPostOffset = postOffset ?? Vector3.Zero;
	}

	/// <summary>
	/// Use position / rotation of an entity
	/// </summary>
	/// <param name="target">Entity</param>
	public void SetReplicated( Entity target )
	{
		Target = target;
		State = BoardCameraModeState.REPLICATE;
	}

	public override void Update()
	{
		// Set viewer (?)
		Viewer = Local.Pawn;

		if ( State == BoardCameraModeState.ABSOLUTE )
		{
			return;
		}

		if ( Target != null && State == BoardCameraModeState.REPLICATE )
		{
			Position = Target.Position;
			Rotation = Target.Rotation;
		}
		else if ( Target != null && State == BoardCameraModeState.FOLLOWING )
		{
			Position = Vector3.Lerp( Position, Target.Position + FollowOffset, FollowSpeed * Time.Delta );
			Rotation = Rotation.Lerp( Rotation, Rotation.LookAt( Target.Position - Position ) + FollowRotationOffset, FollowSpeed * Time.Delta );
			Position += FollowPostOffset;
		}
	}
}