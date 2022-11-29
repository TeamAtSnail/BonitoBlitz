using BonitoBlitz.Entities.CoreBoard;
using Sandbox;

namespace BonitoBlitz.Activities;

public partial class StaticCamera : CameraMode
{
	[Net] private StaticCameraSpot StaticCameraSpot { get; set; }

	public StaticCamera( StaticCameraSpot staticCameraSpot ) => StaticCameraSpot = staticCameraSpot;

	public StaticCamera() => Host.AssertClient();

	public override void Update()
	{
		Position = StaticCameraSpot.Position;
		Rotation = StaticCameraSpot.Rotation;
		FieldOfView = StaticCameraSpot.FieldOfView;
	}
}
