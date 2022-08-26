/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
#nullable enable // ?
namespace gm0;
using System.Collections.Generic;

/// <summary>
/// Server-side GameEvent with index / history number
/// </summary>
public struct RegisteredGameEvent
{
	public RegisteredGameEvent( uint index, GameEvent @event )
	{
		Index = index;
		Event = @event;
	}
	public readonly uint Index;
	public readonly GameEvent Event;
}

/// <summary>
/// Server-side player data with index / history number
/// </summary>
public struct RegisteredPlayer
{
	public RegisteredPlayer( uint index, Sandbox.Client? activeClient = null )
	{
		Index = index;
		ActiveClient = activeClient;
	}
	public readonly uint Index;
	public readonly Sandbox.Client? ActiveClient;
}

public partial class Session
{
    private readonly List<RegisteredGameEvent> eventRegistry = new();
    private readonly List<RegisteredPlayer> playerRegistry = new();
	protected uint GetNextEventIndex() => (uint)eventRegistry.Count;
    protected uint GetNextPlayerIndex() => (uint)playerRegistry.Count;

	protected RegisteredGameEvent RegisterEvent( GameEvent @event ) {
		var registeredEvent = new RegisteredGameEvent(
			GetNextEventIndex(),
			@event
		);

		eventRegistry.Add( registeredEvent );
		return registeredEvent;
	}

    protected RegisteredPlayer RegisterPlayer( Sandbox.Client client ) {
		var registeredClient = new RegisteredPlayer(
			GetNextPlayerIndex(),
			client
		);

		playerRegistry.Add( registeredClient );
		return registeredClient;
	}

    public void SendToPlayer( RegisteredPlayer client, GameEvent @event ) {
        if (Sandbox.Host.IsServer)
		    SessionNetworking.SendToClient( client.ActiveClient, RegisterEvent( @event ) );
	}

    public void SendToPlayer( RegisteredPlayer client, RegisteredGameEvent @event ) {
        if (Sandbox.Host.IsServer)
		    SessionNetworking.SendToClient( client.ActiveClient, @event );
	}

    public void SendToAllPlayers( GameEvent @event ) {
        if (Sandbox.Host.IsServer)
		    SessionNetworking.SendToAllClients( RegisterEvent( @event ) );
	}

    public void SendToAllPlayers( RegisteredGameEvent @event ) {
        if (Sandbox.Host.IsServer)
		    SessionNetworking.SendToAllClients( @event );
	}
}