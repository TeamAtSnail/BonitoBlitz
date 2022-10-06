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

	[Net]
	public Entity Target { get; private set; } = null;

	[Net]
	public BoardCameraModeState State { get; private set; } = BoardCameraModeState.ABSOLUTE;

	[Net]
	public Vector3 FollowOffset { get; set; } = Vector3.Zero;

	[Net]
	public Rotation FollowRotationOffset { get; set; } = Rotation.Identity;

	/// <summary>
	/// Position offset to add after rotation is calculated
	/// </summary>
	[Net]
	public Vector3 FollowPostOffset { get; set; } = Vector3.Zero;

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
		State = BoardCameraModeState.FOLLOWING;
		FollowOffset = positionOffset ?? Vector3.Zero;
		FollowRotationOffset = rotationOffset ?? Rotation.Identity;
		FollowPostOffset = postOffset ?? Vector3.Zero;
		Target = target;
	}

	/// <summary>
	/// Use position / rotation of an entity
	/// </summary>
	/// <param name="target">Entity</param>
	public void SetReplicated( Entity target )
	{
		State = BoardCameraModeState.REPLICATE;
		Target = target;
	}

	public override void Update()
	{
		// Set viewer (?)
		Viewer = Local.Pawn;

		if ( State == BoardCameraModeState.ABSOLUTE )
		{
			return;
		}

		if ( State == BoardCameraModeState.REPLICATE )
		{
			Position = Target.Position;
			Rotation = Target.Rotation;
		}
		else if ( State == BoardCameraModeState.FOLLOWING )
		{
			Position = Target.Position + FollowOffset;
			Rotation = Rotation.LookAt( Position ) + FollowRotationOffset;
			Position += FollowPostOffset;
		}
	}
}