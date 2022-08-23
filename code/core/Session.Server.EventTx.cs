/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

/// <summary>
/// Data for a single server-side session
/// </summary>
public static partial class ServerSession
{
	/// <summary>
	/// Broadcast game event to all players after registering it
	/// </summary>
	/// <param name="evt">Event to register and broadcast</param>
	public static void BroadcastEvent( GameEvent evt )
	{
		ClientSession.HandleEvent( To.Everyone, RegisterEvent( evt ) );
	}

	/// <summary>
	/// Broadcast registered game event to all players
	/// </summary>
	/// <param name="evt">Event to broadcast</param>
	public static void BroadcastEvent( RegisteredGameEvent evt )
	{
		ClientSession.HandleEvent( To.Everyone, evt );
	}

	/// <summary>
	/// Register and add event to player event queue
	/// </summary>
	/// <param name="evt">Event to register and add</param>
	/// <param name="sessionPlayer">Player with event queue</param>
	public static void AddToPlayerEventQueue( ServerSessionPlayer sessionPlayer, GameEvent evt )
	{
		ClientSession.AddToQueue( To.Single( sessionPlayer.Client ), RegisterEvent( evt ) );
	}

	/// <summary>
	/// Add registered event to player event queue
	/// </summary>
	/// <param name="evt">Event to add</param>
	/// <param name="sessionPlayer">Player with event queue</param>
	public static void AddToPlayerEventQueue( ServerSessionPlayer sessionPlayer, RegisteredGameEvent evt )
	{
		ClientSession.AddToQueue( To.Single( sessionPlayer.Client ), evt );
	}

	/// <summary>
	/// Make player run event queue
	/// </summary>
	/// <param name="sessionPlayer">Player with event queue</param>
	public static void RunPlayerEventQueue( ServerSessionPlayer sessionPlayer )
	{
		ClientSession.RunQueue( To.Single( sessionPlayer.Client ) );
	}
}