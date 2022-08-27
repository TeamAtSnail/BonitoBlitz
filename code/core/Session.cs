/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public partial class Session
{
	protected override void PostHandleForeverEvent( SessionIncomingMessage handler, uint statusCode )
	{
		if ( Host.IsClient && handler.Event.Action != GameEventAction.ACK )
			CoreNetworking.SendToServer( GameEventCreator.Acknowledge( handler.RegistryIndex.Value, statusCode ) );
	}
	protected override void PostHandleSingleUseEvent( SessionIncomingMessage handler, uint statusCode )
	{
		if ( Host.IsClient && handler.Event.Action != GameEventAction.ACK )
			CoreNetworking.SendToServer( GameEventCreator.Acknowledge( handler.RegistryIndex.Value, statusCode ) );
	}

	public void ServerOnReceiveEvent( GameEvent evt, Client client )
	{
		HandleEvent( new SessionIncomingMessage( evt, client ) );
	}

	public void ClientOnReceiveEvent( RegisteredGameEvent evt )
	{
		HandleEvent( new SessionIncomingMessage( evt.Event, registryIndex: evt.Index ) );
	}

	public void SendEventToServer( GameEvent @event )
	{
		CoreNetworking.SendToServer( @event );
	}
}