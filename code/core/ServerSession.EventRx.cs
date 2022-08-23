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
	private readonly static List<KeyValuePair<GameEventAction, Func<GameEvent, Client, uint>>> foreverHandlers = new();
	private readonly static List<KeyValuePair<GameEventAction, Func<GameEvent, Client, uint>>> singleUseHandlers = new();

	private static void AddHandler(
		List<KeyValuePair<GameEventAction, Func<GameEvent, Client, uint>>> list,
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
			for ( int i = 0; i < list.Count; i++ )
				if ( list[i].Key == actionUid )
				{
					Log.Warning( $"Replacing handler for already handled action {actionUid}" );
					list.RemoveAt( i );
				}

		list.Add( new( actionUid, actionHandlerMethod ) );
	}

	/// <summary>
	/// Add new forever handler for event action
	/// </summary>
	/// <param name="actionUid">Event action UID / code</param>
	/// <param name="actionHandlerMethod">Event action handler method (input GameEvent & Client, output uint as status code)</param>
	/// <param name="replaceExistingAction">Whether or not existing handler should be replaced (if it exists)</param>
	public static void AddForeverHandler(
		GameEventAction actionUid,
		Func<GameEvent, Client, uint> actionHandlerMethod,
		bool replaceExistingAction = true
	) => AddHandler( foreverHandlers, actionUid, actionHandlerMethod, replaceExistingAction );

	/// <summary>
	/// Add new single use handler for event action
	/// </summary>
	/// <param name="actionUid">Event action UID / code</param>
	/// <param name="actionHandlerMethod">Event action handler method (input GameEvent & Client, output uint as status code)</param>
	/// <param name="replaceExistingAction">Whether or not existing handler should be replaced (if it exists)</param>
	public static void AddSingleUseHandler(
		GameEventAction actionUid,
		Func<GameEvent, Client, uint> actionHandlerMethod,
		bool replaceExistingAction = true
	) => AddHandler( singleUseHandlers, actionUid, actionHandlerMethod, replaceExistingAction );


	/// <summary>
	/// Event handler for client -> server requests
	/// </summary>
	/// <param name="evt">Event</param>
	/// <param name="client">Client</param>
	public static void HandleEvent( GameEvent evt, Client client )
	{
		foreach ( var handler in foreverHandlers )
			if ( (uint)handler.Key == evt.Action )
				if ( handler.Value( evt, client ) == 0 )
					RegisterEvent( evt );

		for ( int i = singleUseHandlers.Count - 1; i >= 0; i-- )
		{
			if ( (uint)singleUseHandlers[i].Key == evt.Action )
			{
				singleUseHandlers[i].Value( evt, client );
				singleUseHandlers.RemoveAt( i );
			}
		}
	}

	/// <summary>
	/// sbox doesn't support client -> server rpc at the moment
	/// use console commands instead!
	/// </summary>
	/// <param name="evt">Event</param>
	[ConCmd.Server]
	public static void HandleEventCcmdShim( uint action, uint var1, uint var2, uint var3 )
	{
		HandleEvent( new GameEvent( action, var1, var2, var3 ), ConsoleSystem.Caller );
	}
}