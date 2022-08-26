/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;
using System.Linq;
using System.Collections.Generic;

public static class SessionNetworking
{
	private static readonly List<Session> networkedSessions = new();

	public static void AddSession( Session session ) => networkedSessions.Add( session );

	[ConCmd.Server]
	private static void ServerOnReceiveEvent( GameEvent @event )
	{
		foreach ( var session in networkedSessions )
		{
			session.ServerOnReceiveEvent( @event, ConsoleSystem.Caller );
		}
	}

	[ClientRpc]
	private static void ClientOnReceiveEvent( RegisteredGameEvent @event )
	{
		foreach ( var session in networkedSessions )
		{
			session.ClientOnReceiveEvent( @event );
		}
	}

	public static void SendToServer( GameEvent @event )
	{
		ServerOnReceiveEvent( @event );
	}

	public static void SendToClient( Client client, RegisteredGameEvent @event )
	{
		ClientOnReceiveEvent( To.Single( client ), @event );
	}
}