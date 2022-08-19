/// <summary>
/// part of the gm0 (w.i.p name) gamemode
/// - lotuspar, 2022 (github.com/lotuspar)
/// </summary>
namespace gm0;
using System;
using System.Collections.Generic;
using Sandbox;

/// <summary>
/// Data for a single server-side session
/// </summary>
public static class ServerSession {
    /// <summary>
    /// Data for a single player stored server-side
    /// Should be used by UID so the player data can move between clients freely
    /// </summary>
    public struct ServerSessionPlayer {
        public ServerSessionPlayer(uint uid, Client client) {
			this.Uid = uid;
			this.Client = client;
		}

		public readonly uint Uid;
		public Client Client;
		public uint? LastAcknowledgedEvent = null; // Last event UID to be acknowledged
	}

	private readonly static List<GameEvent> events = new();

	private readonly static List<ServerSessionPlayer> players = new();

	/// <summary>
	/// Get a ServerSessionPlayer from its client
	/// </summary>
	/// <param name="client">Client to compare</param>
	/// <returns>ServerSessionPlayer or null</returns>
    public static ServerSessionPlayer? GetServerSessionPlayerByClient( Client client ) {
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
    public static ServerSessionPlayer? GetServerSessionPlayerByEntity( Entity entity ) {
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
    public static ServerSessionPlayer? GetServerSessionPlayerByUid( uint uid ) {
        foreach ( var player in players )
            if ( player.Uid == uid )
				return player;
		return null;
	}

    /// <summary>
    /// Keep the session players list in check
    /// todo: write something better for this
    /// </summary>
    public static void ReconfigureSessionPlayers() {
        // todo: this should allow player pop in
        foreach ( var client in Client.All )
            if ( players.Exists( player => player.Client == client ) ) {
				Log.Info( $"ServerSessionPlayer for {client.Name} already exists!" );
			} else {
				// make new ServerSessionPlayer
				players.Add( new ServerSessionPlayer(
                    (uint) players.Count, // todo: make this better, this isn't a good way to set uid and will break if player removed
                    client
				) );
			}
    }

	/// <summary>
	/// Broadcast game event to all players and if required save it to history (default on)
	/// </summary>
	/// <param name="evt">Event to broadcast</param>
    /// <param name="saveToHistory">Whether or not the event should be saved to history</param>   
	public static void BroadcastEvent( GameEvent evt, bool saveToHistory = true ) {
        if ( saveToHistory )
		    events.Add( evt );
		ClientSession.HandleEvent( To.Everyone, evt );
	}

    /// <summary>
    /// Add event to player event queue and if required save it to history (default off)
    /// </summary>
    /// <param name="evt">Event to add</param>
    /// <param name="sessionPlayer">Player with event queue</param>
    /// <param name="saveToHistory">Whether or not the event should be saved to history</param>   
    public static void AddToPlayerEventQueue( ServerSessionPlayer sessionPlayer, GameEvent evt, bool saveToHistory = false ) {
        if ( saveToHistory )
		    events.Add( evt );
        ClientSession.AddToQueue( To.Single( sessionPlayer.Client ), evt );
	}

    /// <summary>
    /// Make player run event queue
    /// </summary>
    /// <param name="sessionPlayer">Player with event queue</param>
    public static void RunPlayerEventQueue( ServerSessionPlayer sessionPlayer ) {
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

/// <summary>
/// Data for a single client-side session
/// </summary>
public static partial class ClientSession {
	private readonly static List<KeyValuePair<GameEventAction, Action<GameEvent>>> handlers = new();

	private readonly static List<GameEvent> queue = new();

	/// <summary>
	/// Acknowledge event from server
	/// </summary>
	/// <param name="eventUid">Event UID</param>
    /// <param name="status">Event status code</param>
	public static void Acknowledge( uint eventUid, uint status ) {
		ServerSession.ClientAcknowledgementToServer( eventUid, status );
	}

    /// <summary>
    /// Add new handler for event action
    /// </summary>
    /// <param name="actionUid">Event action UID / code</param>
    /// <param name="actionHandlerMethod">Event action handler method</param>
	/// <param name="checkForSameAction">Whether or not handler should be added if one exists for that action</param>
    public static void AddHandler( 
		GameEventAction actionUid, 
		Action<GameEvent> actionHandlerMethod, 
		bool checkForSameAction = true
	) {
        if ( actionHandlerMethod == null )
        {
			Log.Error( "Attempted to add null handler method" );
			return;
		}

		// Check for existing handler if required
		if ( checkForSameAction )
			foreach ( var handler in handlers )
            	if ( handler.Key == actionUid )
                {
					Log.Warning( $"Tried to add handler for already handled action {actionUid}" );
					return;
				}

		handlers.Add( new( actionUid, actionHandlerMethod ) );
	}

	[ClientRpc]
    public static void HandleEvent( GameEvent evt ) {
        foreach ( var handler in handlers )
            if ( (uint)handler.Key == evt.Action )
                handler.Value( evt );
	}

    [ClientRpc]
    public static void AddToQueue( GameEvent evt ) {
		queue.Add( evt );
	}

    [ClientRpc]
    public static void RunQueue() {
        foreach ( var evt in queue )
			HandleEvent( evt ); // maybe this shouldn't use the RPC function?
		queue.Clear(); // maybe this whole queue thing needs a mutex
	}
}