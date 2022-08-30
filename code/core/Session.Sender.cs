/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
#nullable enable // ?
namespace gm0;

public partial class Session
{
	public void SendEventToClient( SessionClient client, IndexedGameEvent @event, bool shared = true )
	{
		// Handle event on client
		client.Queue.Add( @event );
		client.SendNextInQueue();

		// Handle event on server
		if ( shared )
		{
			HandleEvent( new SessionIncomingMessage(
				@event.Event,
				registryIndex: @event.Index,
				shared: true
			) );
		}
	}
	public void SendEventToClient( SessionClient client, Events.GameEvent @event, bool shared = true ) => SendEventToClient( client, RegisterEvent( @event ), shared );
	public void BroadcastEvent( IndexedGameEvent @event, bool shared = true )
	{
		// Handle event on clients
		foreach ( var client in clientRegistry )
		{
			Log.Info( $"Adding {@event.Index} (action {@event.Event.Action}) to queue" );
			client.Queue.Add( @event );
			client.SendNextInQueue();
		}

		// Handle event on server
		if ( shared )
		{
			HandleEvent( new SessionIncomingMessage(
				@event.Event,
				registryIndex: @event.Index,
				shared: true
			) );
		}
	}
	public void BroadcastEvent( Events.GameEvent @event, bool shared = true ) => BroadcastEvent( RegisterEvent( @event ), shared );

	public void SendEventToServer( Events.GameEvent @event, bool shared = true )
	{
		// Handle event on server
		CoreNetworking.SendToServer( @event );

		// Handle event on client
		if ( shared )
		{
			HandleEvent( new SessionIncomingMessage(
				@event,
				client: Sandbox.Game.Current.Client,
				shared: true
			) );
		}
	}
}