/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
#nullable enable // ?
namespace gm0;
using System.Collections.Generic;
using Sandbox;

/// <summary>
/// Data for a single event handler (using the Session event system)
/// </summary>
/// <typeparam name="T">Function type</typeparam>
public struct SessionEventHandler<T>
{
	public readonly Events.ActionCode Action;
	public readonly string Id;
	public readonly System.Func<T, uint> Func;

	public SessionEventHandler( Events.ActionCode action, string id, System.Func<T, uint> func )
	{
		Action = action;
		Id = id;
		Func = func;
	}
}

public struct SessionIncomingMessage
{
	public readonly Events.GameEvent Event;
	public readonly Client? Client;
	public readonly uint? RegistryIndex;
	public readonly bool Shared;

	public SessionIncomingMessage( Events.GameEvent @event, Client? client = null, uint? registryIndex = null, bool shared = false )
	{
		Event = @event;
		Client = client;
		RegistryIndex = registryIndex;
		Shared = shared;
	}
}

public abstract class EventReceiver
{
	protected abstract void PostHandleForeverEvent( SessionIncomingMessage handler, uint statusCode );
	protected abstract void PostHandleSingleUseEvent( SessionIncomingMessage handler, uint statusCode );
}

public partial class Session : EventReceiver
{
	protected readonly List<SessionEventHandler<SessionIncomingMessage>> foreverHandlers = new();
	protected readonly List<SessionEventHandler<SessionIncomingMessage>> singleUseHandlers = new();

	private static void AddHandler(
		List<SessionEventHandler<SessionIncomingMessage>> list,
		SessionEventHandler<SessionIncomingMessage> handler
	)
	{
		// Check for existing handler if required
		for ( int i = list.Count - 1; i >= 0; i-- )
		{
			if ( list[i].Action == handler.Action
				&& list[i].Id == handler.Id )
			{
				Log.Warning( $"Replacing handler for already handled action {handler.Action}:{handler.Id}" );
				list.RemoveAt( i );
			}
		}

		list.Add( handler );
	}

	/// <summary>
	/// Add new forever handler for event action
	/// </summary>
	/// <param name="handler">Event handler to add</param>
	public void AddForeverHandler(
		SessionEventHandler<SessionIncomingMessage> handler
	) => AddHandler( foreverHandlers, handler );

	/// <summary>
	/// Add new single use handler for event action
	/// </summary>
	/// <param name="handler">Event handler to add</param>
	public void AddSingleUseHandler(
		SessionEventHandler<SessionIncomingMessage> handler
	) => AddHandler( singleUseHandlers, handler );

	/// <summary>
	/// Run all event handlers for incoming message
	/// </summary>
	/// <param name="message">Incoming message</param>
	protected void HandleEvent( SessionIncomingMessage message )
	{
		foreach ( var handler in foreverHandlers )
		{
			if ( handler.Action == message.Event.Action || handler.Action == Events.ActionCode.INVALID )
			{
				PostHandleForeverEvent( message, handler.Func( message ) );
			}
		}

		for ( int i = singleUseHandlers.Count - 1; i >= 0; i-- )
		{
			var handler = singleUseHandlers[i];
			if ( handler.Action == message.Event.Action || handler.Action == Events.ActionCode.INVALID )
			{
				PostHandleSingleUseEvent( message, handler.Func( message ) );
				singleUseHandlers.RemoveAt( i );
			}
		}
	}
}