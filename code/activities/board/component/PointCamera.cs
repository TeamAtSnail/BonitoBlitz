/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace BonitoBlitz.Board;
using Sandbox;

public partial class PointCamera : CameraMode
{
	[Net] public Vector3? NetPosition { get; set; }
	[Net] public Rotation? NetRotation { get; set; }
	[Net] public float? NetFov { get; set; }
	public PointCamera() { }

	/// <summary>
	/// Set to map camera
	/// </summary>
	/// <param name="camera">MapCamera</param>
	public void SetToMapCamera( MapCamera camera )
	{
		NetFov = camera.CameraFieldOfView;
		NetRotation = camera.Transform.Rotation;
		NetPosition = camera.Transform.Position;
	}

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