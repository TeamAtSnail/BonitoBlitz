/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public partial class ArbCamera : CameraMode
{
	[Net]
	private Vector3? NetPosition { get; set; }
	[Net]
	private Rotation? NetRotation { get; set; }
	[Net]
	private float? NetFov { get; set; }

	/// <summary>
	/// Use position, angles and FOV from a MapCamera
	/// </summary>
	/// <param name="camera">MapCamera</param>
	public void SetToMapCamera( MapCamera camera )
	{
		NetPosition = camera.Transform.Position;
		NetRotation = camera.Transform.Rotation;
		NetFov = camera.CameraFieldOfView;
	}

	public ArbCamera() { }

	public override void Update()
	{
		// Set position to current server camera position (or 0)
		Position = NetPosition ?? Vector3.Zero;
		// Set angles to current server camera angles (if possible)
		Rotation = NetRotation ?? Rotation.Identity;
		// Set FOV to current server camera FOV (or 80)
		FieldOfView = NetFov ?? 80;
		// Set viewer (?)
		Viewer = Local.Pawn;
	}
}