/*
 * part of the gm0 (w.i.p name) gamemode
 * library used across the board gamemode & minigames
 * - last updated indev:3
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace libgm0;
using Sandbox;

public partial class ArbCamera : CameraMode
{
	[Net]
	public Vector3? NetPosition { get; set; }
	[Net]
	public Rotation? NetRotation { get; set; }
	[Net]
	public float? NetFov { get; set; }

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