/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;
using System.Collections.Generic;

public static partial class CoreNetworking
{
	private static readonly List<Session> networkedSessions = new();

	public static void AddSession( Session session )
	{
		if ( networkedSessions.Contains( session ) )
		{
			Log.Warning( "Tried to network already networked session!" );
			return;
		}

		networkedSessions.Add( session );
	}

	[ConCmd.Server]
	public static void ServerOnReceiveEvent( uint action, uint var1, uint var2, uint var3 )
	{
		foreach ( var session in networkedSessions )
		{
			// having to make a new GameEvent sucks but
			// todo / wait: implement ServerRpc when able to 
			session.ServerOnReceiveEvent( new Events.GameEvent( (Events.ActionCode)action, var1, var2, var3 ), ConsoleSystem.Caller );
		}
	}

	[ClientRpc]
	// can't use @event as parameter name in ClientRpc functions
	// https://github.com/Facepunch/sbox-issues/issues/2227
	public static void ClientOnReceiveEvent( RegisteredGameEvent _event )
	{
		foreach ( var session in networkedSessions )
		{
			session.ClientOnReceiveEvent( _event );
		}
	}

	public static void SendToServer( Events.GameEvent @event )
	{
		ServerOnReceiveEvent( (uint)@event.Action, @event.Var1, @event.Var2, @event.Var3 );
	}

	public static void SendToClient( Client client, RegisteredGameEvent @event )
	{
		ClientOnReceiveEvent( To.Single( client ), @event );
	}
}