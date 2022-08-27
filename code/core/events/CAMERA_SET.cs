/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;

public partial class GameEventCreator
{
	public partial class Camera
	{
		public static GameEvent SetCameraAngle( Angles angles )
			=> new( (uint)GameEventAction.CAMERA_SET_ANG, angles.pitch, angles.yaw, angles.roll );
		public static GameEvent SetCameraAngle( float pitch, float yaw, float roll )
			=> new( (uint)GameEventAction.CAMERA_SET_ANG, pitch, yaw, roll );
		public static GameEvent SetCameraPos( Vector3 position )
			=> new( (uint)GameEventAction.CAMERA_SET_POS, position.x, position.y, position.z );
		public static GameEvent SetCameraPos( float x, float y, float z )
			=> new( (uint)GameEventAction.CAMERA_SET_POS, x, y, z );
		public static GameEvent SetCameraFOV( uint fov )
			=> new( (uint)GameEventAction.CAMERA_SET_FOV, fov );
	}
}