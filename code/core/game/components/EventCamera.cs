/// <summary>
/// part of the gm0 (w.i.p name) gamemode
/// - lotuspar, 2022 (github.com/lotuspar)
/// 
/// Handles GameEventAction(s):
///     CAMERA_SET_ANGLE
///     CAMERA_SET_POS
///     CAMERA_SET_FOV
/// </summary>
namespace gm0;
using Sandbox;

class EventCamera : CameraMode {
	private Angles? serverCameraAngle = null;

	private Vector3? serverCameraPosition = null;

	private float? serverCameraFieldOfView = null;

	public EventCamera() 
	{
		/* Handle GameEventActions */
		Log.Info( "GameEventCamera constructor called!" );
		ClientSession.AddHandler( GameEventAction.CAMERA_SET_ANGLE, 
        ( GameEvent evt ) =>
		{
			// evt: var1=x, var2=y, var3=z
			serverCameraAngle = new Angles( evt.Var1, evt.Var2, evt.Var3 ); // todo: don't use new Angles here
		} );

        ClientSession.AddHandler( GameEventAction.CAMERA_SET_POS, 
        ( GameEvent evt ) =>
		{
			serverCameraPosition = new Vector3( evt.Var1, evt.Var2, evt.Var3 ); // todo: don't use new Vector here
		} );

        ClientSession.AddHandler( GameEventAction.CAMERA_SET_FOV, 
        ( GameEvent evt ) =>
		{
			// evt: var1=fov
			serverCameraFieldOfView = evt.Var1;
		} );
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