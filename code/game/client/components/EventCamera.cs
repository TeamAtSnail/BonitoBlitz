/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 * 
 * Handles GameEventAction(s):
 *     CAMERA_SET_ANGLE
 *     CAMERA_SET_POS
 *     CAMERA_SET_FOV
 */
namespace gm0;
using Sandbox;

class EventCamera : CameraMode
{
	private Angles? serverCameraAngle = null;

	private Vector3? serverCameraPosition = null;

	private float? serverCameraFieldOfView = null;

	public EventCamera()
	{
		/* Handle GameEventActions */
		Log.Info( "GameEventCamera constructor called!" );
		if ( Host.IsClient )
		{
			Gamemode0.Session.AddForeverHandler( new(
				Events.ActionCode.CAMERA_SET_ANG, "_EventCamera_CAMERA_SET_ANG",
				( SessionIncomingMessage message ) =>
				{
					// evt: var1=x, var2=y, var3=z
					Log.Info( $"{message.Event.Var1f}, {message.Event.Var2f}, {message.Event.Var3f}" );
					serverCameraAngle = new Angles( message.Event.Var1f, message.Event.Var2f, message.Event.Var3f ); // todo: don't use new Angles here
					return 0;
				} )
			);

			Gamemode0.Session.AddForeverHandler( new(
				Events.ActionCode.CAMERA_SET_POS, "_EventCamera_CAMERA_SET_POS",
				( SessionIncomingMessage message ) =>
				{
					serverCameraPosition = new Vector3( message.Event.Var1f, message.Event.Var2f, message.Event.Var3f ); // todo: don't use new Vector here
					return 0;
				} )
			);

			Gamemode0.Session.AddForeverHandler( new(
				Events.ActionCode.CAMERA_SET_POS, "_EventCamera_CAMERA_SET_POS",
				( SessionIncomingMessage message ) =>
				{
					serverCameraFieldOfView = message.Event.Var1;
					return 0;
				} )
			);
		}
	}

	public override void Update()
	{
		// Set position to current server camera position (or 0)
		Position = serverCameraPosition ?? Vector3.Zero;

		// Set angles to current server camera angles (if possible)
		Rotation = Rotation.From( serverCameraAngle.GetValueOrDefault() );

		// Set FOV to current server camera FOV (or 80)
		FieldOfView = serverCameraFieldOfView ?? 80;

		// Set viewer (?)
		Viewer = Local.Pawn;
	}
}