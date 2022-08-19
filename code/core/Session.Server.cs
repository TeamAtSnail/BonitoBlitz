/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System.Collections.Generic;
using Sandbox;

/// <summary>
/// Data for a single player stored server-side
/// Should be used by UID so the player data can move between clients freely
/// </summary>
public struct ServerSessionPlayer
{
	public ServerSessionPlayer( uint uid, Client client )
	{
		this.Uid = uid;
		this.Client = client;
	}

	public readonly uint Uid;
	public Client Client;
	public uint? LastAcknowledgedEvent = null; // Last event index to be acknowledged
}

/// <summary>
/// Data for a single server-side session
/// </summary>
public static class ServerSession
{
	private readonly static List<RegisteredGameEvent> events = new();

	private readonly static List<ServerSessionPlayer> players = new();

	/// <summary>
	/// Get a ServerSessionPlayer from its client
	/// </summary>
	/// <param name="client">Client to compare</param>
	/// <returns>ServerSessionPlayer or null</returns>
	public static ServerSessionPlayer? GetServerSessionPlayerByClient( Client client )
	{
		foreach ( var player in players )
			if ( player.Client == client )
				return player;
		return null;
	}

	/// <summary>
	/// Get a ServerSessionPlayer from its entity
	/// </summary>
	/// <param name="entity">Entity to compare</param>
	/// <returns>ServerSessionPlayer or null</returns>
	public static ServerSessionPlayer? GetServerSessionPlayerByEntity( Entity entity )
	{
		foreach ( var player in players )
			if ( player.Client.Pawn == entity )
				return player;
		return null;
	}

	/// <summary>
	/// Get a ServerSessionPlayer from its UID
	/// </summary>
	/// <param name="uid">UID to compare</param>
	/// <returns>ServerSessionPlayer or null</returns>
	public static ServerSessionPlayer? GetServerSessionPlayerByUid( uint uid )
	{
		foreach ( var player in players )
			if ( player.Uid == uid )
				return player;
		return null;
	}

	/// <summary>
	/// Keep the session players list in check
	/// todo: write something better for this
	/// </summary>
	public static void ReconfigureSessionPlayers()
	{
		// todo: this should allow player pop in
		foreach ( var client in Client.All )
			if ( players.Exists( player => player.Client == client ) )
			{
				Log.Info( $"ServerSessionPlayer for {client.Name} already exists!" );
			}
			else
			{
				// make new ServerSessionPlayer
				players.Add( new ServerSessionPlayer(
					(uint)players.Count, // todo: make this better, this isn't a good way to set uid and will break if player removed
					client
				) );
			}
	}

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

	[ConCmd.Server]
	public static void ClientAcknowledgementToServer( uint eventUid, uint status )
	{
		var maybeSessionPlayer = GetServerSessionPlayerByEntity( ConsoleSystem.Caller.Pawn );
		if ( maybeSessionPlayer == null )
		{
			Log.Error( $"Received event acknowledgement from unknown client (event {eventUid}, status {status})" );
			return;
		}

		var sessionPlayer = maybeSessionPlayer.Value;
		sessionPlayer.LastAcknowledgedEvent = eventUid;
		Log.Info( $"{sessionPlayer.Client.Name} acknowledged event {eventUid}!" ); // todo: remove this
	}
}