/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System.Text.Json;

public partial class BasePlayer
{
	public int TileNumber { get => Tile.TileNumber; }

	/// <summary>
	/// Save data for player stored as JSON
	/// </summary>
	public string SaveData => JsonSerializer.Serialize( new { Uid, Coins, Stars, PlayerId, TileNumber } );

	/// <summary>
	/// Create new Player from save data JSON
	/// </summary>
	/// <param name="data">Save data</param>
	/// <returns>Player</returns>
	public static T FromSaveData<T>( string data ) => JsonSerializer.Deserialize<T>( data );
}