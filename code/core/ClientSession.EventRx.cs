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
public static partial class ClientSession
{
	private readonly static List<KeyValuePair<GameEventAction, Func<GameEvent, uint>>> foreverHandlers = new();
	private readonly static List<KeyValuePair<GameEventAction, Func<GameEvent, uint>>> singleUseHandlers = new();
	private readonly static List<RegisteredGameEvent> queue = new();

	private static void AddHandler(
		List<KeyValuePair<GameEventAction, Func<GameEvent, uint>>> list,
		GameEventAction actionUid,
		Func<GameEvent, uint> actionHandlerMethod,
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
	/// <param name="actionHandlerMethod">Event action handler method (input GameEvent, output uint as status code)</param>
	/// <param name="replaceExistingAction">Whether or not existing handler should be replaced (if it exists)</param>
	public static void AddForeverHandler(
		GameEventAction actionUid,
		Func<GameEvent, uint> actionHandlerMethod,
		bool replaceExistingAction = true
	) => AddHandler( foreverHandlers, actionUid, actionHandlerMethod, replaceExistingAction );

	/// <summary>
	/// Add new single use handler for event action
	/// </summary>
	/// <param name="actionUid">Event action UID / code</param>
	/// <param name="actionHandlerMethod">Event action handler method (input GameEvent, output uint as status code)</param>
	/// <param name="replaceExistingAction">Whether or not existing handler should be replaced (if it exists)</param>
	public static void AddSingleUseHandler(
		GameEventAction actionUid,
		Func<GameEvent, uint> actionHandlerMethod,
		bool replaceExistingAction = true
	) => AddHandler( singleUseHandlers, actionUid, actionHandlerMethod, replaceExistingAction );

	[ClientRpc]
	public static void HandleEvent( RegisteredGameEvent evt )
	{
		foreach ( var handler in foreverHandlers )
			if ( (uint)handler.Key == evt.Event.Action )
				Acknowledge( evt.Index, handler.Value( evt.Event ) );

		for ( int i = singleUseHandlers.Count - 1; i >= 0; i-- )
		{
			if ( (uint)singleUseHandlers[i].Key == evt.Event.Action )
			{
				singleUseHandlers[i].Value( evt.Event );
				singleUseHandlers.RemoveAt( i );
			}
		}
	}

	[ClientRpc]
	public static void AddToQueue( RegisteredGameEvent evt )
	{
		queue.Add( evt );
	}

	[ClientRpc]
	public static void RunQueue()
	{
		foreach ( var evt in queue )
			HandleEvent( evt ); // maybe this shouldn't use the RPC function?
		queue.Clear(); // maybe this whole queue thing needs a mutex
	}
}