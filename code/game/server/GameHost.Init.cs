/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System.Linq;
using Sandbox;

/// <summary>
/// Active game data
/// </summary>
public partial class GameHost
{
	/// <summary>
	/// Event handler for PLAYER_DATA_INIT
	/// </summary>
	/// <param name="message">Event message</param>
	/// <returns>Status code</returns>
	private uint HandlePlayerDataInit( SessionIncomingMessage message )
	{
		uint playerIdx = message.Event.Var1;
		long playerId = message.Event.LongVar2;

		// Get player by SteamID (playerId / LongVar2)
		Client client = Client.All.Where( ( client ) => client.PlayerId == playerId ).Single();

		// Register client with session
		var sessionClient = Gamemode0.Session.GetSessionClient( client );

		// Create new player entity
		var entity = new PlayerEntity( sessionClient );
		Players.Add( entity );
		Log.Info( $"New entity {entity} created for SteamID:{client.PlayerId}" );
		return 0;
	}
}