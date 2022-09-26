/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace BonitoBlitz;
using Sandbox;
using SandboxEditor;

/// <summary>
/// Spot for camera to be
/// </summary>
[Library( "gm0_map_camera" ), HammerEntity]
[Title( "Map Camera" ), Category( "Gameplay" ), Icon( "place" )]
[Description( "Camera spot" )]
[EditorModel( "models/editor/camera.vmdl" )]
public class MapCamera : Entity
{
	/// <summary>
	/// FOV of camera
	/// </summary>
	[Property( Title = "Camera FOV" )]
	public int CameraFieldOfView { get; set; } = 80;
}