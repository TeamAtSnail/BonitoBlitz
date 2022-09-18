/*
 * part of the gm0 (w.i.p name) gamemode
 * library used across the board gamemode & minigames
 * - last updated indev:4
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public partial class FollowCamera : CameraMode
{
	[Net]
	public Vector3? PositionOffset { get; set; }
	[Net]
	public float? Fov { get; set; }
	[Net]
	public Rotation? FixedRotation { get; set; }
	[Net]
	public Entity Target { get; set; }
	public FollowCamera( Entity target )
	{
		Target = target;
	}
	public FollowCamera() { }

	public override void Update()
	{
		Position = Target.Position + (PositionOffset ?? Vector3.Zero);
		Rotation = FixedRotation ?? Rotation.LookAt( Position );
		// Set FOV to current server camera FOV (or 80)
		FieldOfView = Fov ?? 80;
		// Set viewer (?)
		Viewer = Local.Pawn;
	}
}