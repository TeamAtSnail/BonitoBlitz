/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;

public partial class GameEventCreator
{
	public static GameEvent SetCameraAngle( Angles angles )
	{
		return new GameEvent( (uint)GameEventAction.CAMERA_SET_ANGLE, angles.pitch, angles.yaw, angles.roll );
	}
	public static GameEvent SetCameraAngle( float pitch, float yaw, float roll )
	{
		return new GameEvent( (uint)GameEventAction.CAMERA_SET_ANGLE, pitch, yaw, roll );
	}
	public static GameEvent SetCameraPos( Vector3 position )
	{
		return new GameEvent( (uint)GameEventAction.CAMERA_SET_POS, position.x, position.y, position.z );
	}
	public static GameEvent SetCameraPos( float x, float y, float z )
	{
		return new GameEvent( (uint)GameEventAction.CAMERA_SET_POS, x, y, z );
	}
	public static GameEvent SetCameraFOV( uint fov )
	{
		return new GameEvent( (uint)GameEventAction.CAMERA_SET_FOV, fov );
	}
}