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
		if ( Host.IsClient && handler.Event.Action != Events.ActionCode.ACK )
			CoreNetworking.SendToServer( Events.Builder.Acknowledge( handler.RegistryIndex.Value, statusCode ) );
	}
	protected override void PostHandleSingleUseEvent( SessionIncomingMessage handler, uint statusCode )
	{
		if ( Host.IsClient && handler.Event.Action != Events.ActionCode.ACK )
			CoreNetworking.SendToServer( Events.Builder.Acknowledge( handler.RegistryIndex.Value, statusCode ) );
	}

	public void ServerOnReceiveEvent( Events.GameEvent evt, Client client )
	{
		HandleEvent( new SessionIncomingMessage( evt, client ) );
	}

	public void ClientOnReceiveEvent( IndexedGameEvent evt )
	{
		HandleEvent( new SessionIncomingMessage( evt.Event, registryIndex: evt.Index ) );
	}
}