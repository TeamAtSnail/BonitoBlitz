/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System.Collections.Generic;

/// <summary>
/// Data for a single server-side session
/// </summary>
public static partial class ServerSession
{
	private readonly static List<RegisteredGameEvent> events = new();

	/// <summary>
	/// Register event with server-side session
	/// </summary>
	/// <param name="evt">Event to register</param>
	/// <returns>RegisteredGameEvent</returns>
	public static RegisteredGameEvent RegisterEvent( GameEvent evt )
	{
		var registeredGameEvent = new RegisteredGameEvent(
			(uint)events.Count,
			evt
		);

		events.Add( registeredGameEvent );
		return registeredGameEvent;
	}
}