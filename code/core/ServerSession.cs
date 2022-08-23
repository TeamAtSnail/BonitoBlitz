/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System.Collections.Generic;
using Sandbox;

/// <summary>
/// Data for a single player stored server-side
/// Should be used by UID so the player data can move between clients freely
/// </summary>
public struct ServerSessionPlayer
{
	public ServerSessionPlayer( uint uid, Client client )
	{
		this.Uid = uid;
		this.Client = client;
	}

	public readonly uint Uid;
	public Client Client;
	public uint? LastAcknowledgedEvent = null; // Last event index to be acknowledged
}

/// <summary>
/// Data for a single server-side session
/// </summary>
public static partial class ServerSession
{
	private readonly static List<ServerSessionPlayer> players = new();

	/// <summary>
	/// Get a ServerSessionPlayer from its client
	/// </summary>
	/// <param name="client">Client to compare</param>
	/// <returns>ServerSessionPlayer or null</returns>
	public static ServerSessionPlayer? GetServerSessionPlayerByClient( Client client )
	{
		foreach ( var player in players )
			if ( player.Client == client )
				return player;
		return null;
	}

	/// <summary>
	/// Get a ServerSessionPlayer from its entity
	/// </summary>
	/// <param name="entity">Entity to compare</param>
	/// <returns>ServerSessionPlayer or null</returns>
	public static ServerSessionPlayer? GetServerSessionPlayerByEntity( Entity entity )
	{
		foreach ( var player in players )
			if ( player.Client.Pawn == entity )
				return player;
		return null;
	}

	/// <summary>
	/// Get a ServerSessionPlayer from its UID
	/// </summary>
	/// <param name="uid">UID to compare</param>
	/// <returns>ServerSessionPlayer or null</returns>
	public static ServerSessionPlayer? GetServerSessionPlayerByUid( uint uid )
	{
		foreach ( var player in players )
			if ( player.Uid == uid )
				return player;
		return null;
	}

	/// <summary>
	/// Keep the session players list in check
	/// todo: write something better for this
	/// </summary>
	public static void ReconfigureSessionPlayers()
	{
		// todo: this should allow player pop in
		foreach ( var client in Client.All )
			if ( players.Exists( player => player.Client == client ) )
			{
				Log.Info( $"ServerSessionPlayer for {client.Name} already exists!" );
			}
			else
			{
				// make new ServerSessionPlayer
				players.Add( new ServerSessionPlayer(
					(uint)players.Count, // todo: make this better, this isn't a good way to set uid and will break if player removed
					client
				) );
			}
	}
}