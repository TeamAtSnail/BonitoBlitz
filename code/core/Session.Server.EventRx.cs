/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System;
using System.Collections.Generic;
using Sandbox;

/// <summary>
/// Data for a single server-side session
/// </summary>
public static partial class ServerSession
{
	private readonly static List<KeyValuePair<GameEventAction, Func<GameEvent, Client, uint>>> handlers = new();

	/// <summary>
	/// Add new handler for event action
	/// </summary>
	/// <param name="actionUid">Event action UID / code</param>
	/// <param name="actionHandlerMethod">Event action handler method (input GameEvent & Client, output uint as status code)</param>
	/// <param name="replaceExistingAction">Whether or not existing handler should be replaced (if it exists)</param>
	public static void AddHandler(
		GameEventAction actionUid,
		Func<GameEvent, Client, uint> actionHandlerMethod,
		bool replaceExistingAction = true
	)
	{
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

	/// <summary>
	/// Event handler for client -> server requests
	/// </summary>
	/// <param name="evt">Event</param>
	/// <param name="client">Client</param>
	public static void HandleEvent( GameEvent evt, Client client )
	{
		foreach ( var handler in handlers )
			if ( (uint)handler.Key == evt.Action )
				if ( handler.Value( evt, client ) == 0 )
					RegisterEvent( evt );
	}

	/// <summary>
	/// sbox doesn't support client -> server rpc at the moment
	/// use console commands instead!
	/// </summary>
	/// <param name="evt">Event</param>
	[ConCmd.Server]
	public static void HandleEventCcmdShim( GameEvent evt )
	{
		HandleEvent( evt, ConsoleSystem.Caller );
	}
}