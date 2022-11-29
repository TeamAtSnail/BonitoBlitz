using System;
using System.Linq;
using Sandbox;

namespace BonitoBlitz.Activities;

public partial class CameraComponent : CameraMode
{
	public enum CameraType
	{
		LookAtEntity,
		LookFromEntity,
		LookAtPoint
	}

	[Net] private Entity TargetEntity { get; set; }
	[Net] private Vector3 TargetPoint { get; set; }
	[Net] private CameraType Type { get; set; }

	[Net] public bool UseTargetEyePosition { get; set; } = true;
	[Net] public Vector3 PrePositionOffset { get; set; }
	[Net] public Vector3 PostPositionOffset { get; set; }
	[Net] public Rotation RotationOffset { get; set; }

	private const float LerpSpeed = 5.0f;

	public override void Update()
	{
		switch ( Type )
		{
			case CameraType.LookAtEntity or CameraType.LookAtPoint:
				{
					var targetPosition = Type switch
					{
						CameraType.LookAtEntity => UseTargetEyePosition
							? TargetEntity.EyePosition
							: TargetEntity.Position,
						CameraType.LookAtPoint => TargetPoint,
						_ => throw new ArgumentOutOfRangeException()
					};

					Position = Position.LerpTo( targetPosition + PrePositionOffset, LerpSpeed * Time.Delta );
					Rotation = Rotation.Lerp(
						Rotation,
						Rotation.LookAt( targetPosition - Position, Vector3.Up ) * RotationOffset,
						LerpSpeed * Time.Delta );
					Position += PostPositionOffset;
					break;
				}
			case CameraType.LookFromEntity:
				Position = TargetEntity.Position;
				Rotation = TargetEntity.Rotation;
				break;
		}
	}

	private void ResetOffsets()
	{
		PrePositionOffset = Vector3.Zero;
		PostPositionOffset = Vector3.Zero;
		RotationOffset = Rotation.Identity;
	}

	public void LookAtEntity( Entity target )
	{
		ResetOffsets();
		Type = CameraType.LookAtEntity;
		TargetEntity = target;
	}

	public void LookAtPoint( Vector3 point )
	{
		ResetOffsets();
		Type = CameraType.LookAtPoint;
		TargetPoint = point;
	}

	public void LookFromEntity( Entity entity )
	{
		ResetOffsets();
		Type = CameraType.LookFromEntity;
		TargetEntity = entity;
	}

	public override void Activated()
	{
		base.Activated();

		Position = Map.Camera.Position;
		Rotation = Map.Camera.Rotation;
	}

	/// <summary>
	/// Attempt to add a new CameraComponent. If one exists it will be returned.
	/// </summary>
	/// <param name="entity">Entity</param>
	/// <returns></returns>
	public static CameraComponent AddNewOrGet( Entity entity )
	{
		var existing = entity.Components.Get<CameraComponent>();
		if ( existing != null )
		{
			return existing;
		}

		entity.Components.RemoveAny<CameraMode>();
		return entity.Components.Add( new CameraComponent() );
	}
}
