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
	private struct ClientQueue
	{
		public readonly Client Client;
		public readonly List<RegisteredGameEvent> Queue;
		public uint? LastAcknowledgedEvent;
		private bool _Active;
		public bool Active => _Active;

		public ClientQueue(
			Client client,
			uint? lastAcknowledgedEvent = null )
		{
			Client = client;
			Queue = new();
			LastAcknowledgedEvent = lastAcknowledgedEvent;
			_Active = false;
		}

		public void SendLastEvent()
		{
			if ( Queue.Count == 0 )
				return;

			_Active = true;
			SessionNetworking.InternalSendToClient( Client, Queue.Last() );
		}

		public void RemoveLastEvent()
		{
			if ( Queue.Count == 0 )
			{
				Log.Warning( "Tried to remove event from empty ClientQueue" );
				return;
			}

			Queue.RemoveAt( Queue.Count - 1 );

			if ( Queue.Count == 0 )
			{
				_Active = false;
			}
		}
	}

	private static readonly List<Session> networkedSessions = new();
	private static readonly List<ClientQueue> clientQueues = new();

	/// <summary>
	/// Get client queue from client (or create one if it doesn't exist)
	/// </summary>
	/// <param name="client">Client</param>
	/// <returns>ClientQueue?</returns>
	private static ClientQueue GetClientQueue( Client client )
	{
		ClientQueue? queue = null;
		queue = clientQueues.Where( ( @queue ) => @queue.Client == client ).SingleOrDefault( null );
		if ( queue != null )
			return queue.Value;

		queue = new ClientQueue( client );
		clientQueues.Add( queue.Value );
		return queue.Value;
	}

	public static void AddSession( Session session )
	{
		networkedSessions.Add( session );
		if ( !Host.IsServer )
		{
			session.AddForeverHandler( new SessionEventHandler<SessionIncomingMessage>(
				GameEventAction.INVALID, "_Session.Networking_ACK_TX",
				( SessionIncomingMessage message ) =>
				{
					if ( message.RegistryIndex == null )
						return 1;

					SendToServer( GameEventCreator.Acknowledge( 0, message.RegistryIndex.Value ) );
					return 0;
				}
			) );
			return;
		}

		session.AddForeverHandler( new SessionEventHandler<SessionIncomingMessage>(
			GameEventAction.ACK, "_Session.Networking_ACK_RX",
			( SessionIncomingMessage message ) =>
			{
				var queue = GetClientQueue( message.Client );
				queue.LastAcknowledgedEvent = message.RegistryIndex;
				queue.RemoveLastEvent();
				queue.SendLastEvent();
				return 0;
			}
		) );

		session.AddForeverHandler( new SessionEventHandler<SessionIncomingMessage>(
			GameEventAction.UPDATE, "_Session.Networking_UPDATE",
			( SessionIncomingMessage message ) =>
			{
				var queue = GetClientQueue( message.Client );
				if ( queue.Active )
				{
					Log.Warning( "Client asked for update from active queue" );
					return 1;
				}
				queue.SendLastEvent();
				return 0;
			}
		) );
	}

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

	private static void InternalSendToClient( Client client, RegisteredGameEvent @event )
	{
		ClientOnReceiveEvent( To.Single( client ), @event );
	}

	public static void SendToAllClients( RegisteredGameEvent @event )
	{
		foreach ( var client in Client.All )
		{
			SendToClient( client, @event );
		}
	}

	public static void SendToClient( Client client, RegisteredGameEvent @event )
	{
		var queue = GetClientQueue( client );
		queue.Queue.Add( @event );
		if ( !queue.Active )
		{
			queue.SendLastEvent();
		}
	}
}