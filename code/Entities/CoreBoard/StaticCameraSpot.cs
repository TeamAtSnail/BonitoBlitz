using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.Entities.CoreBoard;

[Library( "bb_static_camera_spot" ), HammerEntity]
[Title( "Static Camera" ), Category( "Map" ), Icon( "camera" )]
[Description( "Camera info for a static camera" )]
[EditorModel( "models/editor/camera.vmdl" )]
public partial class StaticCameraSpot : Entity
{
	[Property( Title = "Field Of View" )]
	[Net]
	public float FieldOfView { get; set; } = 89.0f;
}
