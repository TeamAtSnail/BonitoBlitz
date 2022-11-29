using System.Linq;
using Sandbox;

namespace BonitoBlitz.Activities;

public partial class DynamicCamera : CameraMode
{
	[Net] public Entity Target { get; set; }
	[Net] public bool UseTargetEyePosition { get; set; } = true;
	[Net] public Vector3 PrePositionOffset { get; set; }
	[Net] public Vector3 PostPositionOffset { get; set; }
	[Net] public Rotation RotationOffset { get; set; }
	public const float LerpSpeed = 5.0f;

	public DynamicCamera( Entity target ) => Target = target;

	public DynamicCamera() => Host.AssertClient();

	public override void Update()
	{
		var targetPos = UseTargetEyePosition ? Target.EyePosition : Target.Position;
		Position = Position.LerpTo( targetPos + PrePositionOffset, LerpSpeed * Time.Delta );
		Rotation = Rotation.Lerp(
			Rotation,
			Rotation.LookAt( targetPos - Position, Vector3.Up ) + RotationOffset,
			LerpSpeed * Time.Delta );
		Position += PostPositionOffset;
	}

	public void UseOffsetToEntity( Entity entity ) => PrePositionOffset = Entity.Position - Target.Position;

	public static bool CheckExisting( Entity entity, Entity target ) => entity.Components.GetAll<DynamicCamera>()
		.Any( component => component.Target == target );

	public static DynamicCamera AddOrGet( Entity entity, Entity target )
	{
		var existing = entity.Components.Get<DynamicCamera>();
		if ( existing != null )
		{
			existing.Target = target;
			return existing;
		}
		else
		{
			entity.Components.RemoveAny<CameraMode>();
			return entity.Components.Add( new DynamicCamera( target ) );
		}
	}
}
