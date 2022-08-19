/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System;
using System.Collections.Generic;
using Sandbox;

/// <summary>
/// Data for a single client-side session
/// </summary>
public static partial class ClientSession {
	private readonly static List<KeyValuePair<GameEventAction, Func<GameEvent, uint>>> handlers = new();

	private readonly static List<RegisteredGameEvent> queue = new();

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
    /// <param name="actionHandlerMethod">Event action handler method (input GameEvent, output uint as status code)</param>
	/// <param name="replaceExistingAction">Whether or not existing handler should be replaced (if it exists)</param>
    public static void AddHandler( 
		GameEventAction actionUid, 
		Func<GameEvent, uint> actionHandlerMethod, 
		bool replaceExistingAction = true
	) {
        if ( actionHandlerMethod == null )
        {
			Log.Error( "Attempted to add null handler method" );
			return;
		}

		// Check for existing handler if required
		if ( replaceExistingAction )
			for ( int i = 0; i < handlers.Count; i++ )
				if ( handlers[i].Key == actionUid )
				{
					Log.Warning( $"Replacing handler for already handled action {actionUid}" );
					handlers.RemoveAt( i );
				}
				
		handlers.Add( new( actionUid, actionHandlerMethod ) );
	}

	[ClientRpc]
    public static void HandleEvent( RegisteredGameEvent evt ) {
        foreach ( var handler in handlers )
            if ( (uint)handler.Key == evt.Event.Action )
                Acknowledge( evt.Index, handler.Value( evt.Event ) );
	}

    [ClientRpc]
    public static void AddToQueue( RegisteredGameEvent evt ) {
		queue.Add( evt );
	}

    [ClientRpc]
    public static void RunQueue() {
        foreach ( var evt in queue )
			HandleEvent( evt ); // maybe this shouldn't use the RPC function?
		queue.Clear(); // maybe this whole queue thing needs a mutex
	}
}