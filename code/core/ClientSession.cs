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
	/// Acknowledge event from server
	/// </summary>
	/// <param name="eventUid">Event UID</param>
	/// <param name="status">Event status code</param>
	public static void Acknowledge( uint eventUid, uint status )
	{
		ServerSession.HandleEventCcmdShim( (uint) GameEventAction.ACK, status, eventUid, 0 );
	}
}