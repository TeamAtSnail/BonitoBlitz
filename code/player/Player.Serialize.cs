/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sandbox;

public partial class Player
{
	private long provisionalPlayerId = 0;

	[JsonInclude]
	public long PlayerId { get => Client.PlayerId; set => provisionalPlayerId = value; }

	/// <summary>
	/// Save data for player stored as JSON
	/// </summary>
	public string SaveData => JsonSerializer.Serialize( this, new JsonSerializerOptions { IncludeFields = true } );

	/// <summary>
	/// Create new Player from save data JSON
	/// </summary>
	/// <param name="data">Save data</param>
	/// <returns>Player</returns>
	public static Player FromSaveData( string data ) => JsonSerializer.Deserialize<Player>( data, new JsonSerializerOptions { IncludeFields = true } );
}