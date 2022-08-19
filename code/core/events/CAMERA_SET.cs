/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;

public partial class GameEventCreator
{
	public static GameEvent SetCameraAngle( uint x, uint y, uint z )
	{
		return new GameEvent( (uint)GameEventAction.CAMERA_SET_ANGLE, x, y, z );
	}
	public static GameEvent SetCameraPos( uint x, uint y, uint z )
	{
		return new GameEvent( (uint)GameEventAction.CAMERA_SET_POS, x, y, z );
	}
	public static GameEvent SetCameraFOV( uint fov )
	{
		return new GameEvent( (uint)GameEventAction.CAMERA_SET_FOV, fov );
	}
}