/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;

/// <summary>
/// Data for a single client-side session
/// </summary>
public static partial class ClientSession
{
	/// <summary>
	/// Send game event to server
	/// </summary>
	/// <param name="evt">Event to send</param>
	public static void SendEvent( GameEvent evt )
	{
		ServerSession.HandleEventCcmdShim( evt.Action, evt.Var1, evt.Var2, evt.Var3 );
	}
}