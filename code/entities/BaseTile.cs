/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

/// <summary>
/// Singular map tile entity; used to denote tile position
/// Requires number, all position and transforms etc. done in-engine
/// </summary>
public abstract partial class BaseTile : Entity
{
	[Property( Title = "Tile Number" )]
	[Net]
	public int TileNumber { get; set; }

	[Property( Title = "Next Tile" )]
	[Net]
	public int NextTile { get; set; }

	/// <summary>
	/// Called once when a player passes this tile at any point
	/// </summary>
	/// <param name="player">BoardPlayer</param>
	public abstract void OnPlayerPass( BoardPlayer player );

	/// <summary>
	/// Called once when a player finishes their turn on this tile
	/// </summary>
	/// <param name="player">BoardPlayer</param>
	public abstract void OnPlayerStand( BoardPlayer player );

	/// <summary>
	/// Called every player Simulate until tile returned
	/// </summary>
	/// <param name="player">BoardPlayer</param>
	/// <returns>Non-null only if tile is done processing</returns>
	public abstract int? Process( BoardPlayer player );

	/// <summary>
	/// Find BaseTile from tile number
	/// </summary>
	/// <param name="tileNumber">Tile number</param>
	/// <returns>BaseTile or null</returns>
	public static BaseTile FromTileNumber( int tileNumber )
	{
		foreach ( var entity in Entity.All )
		{
			if ( entity is BaseTile tile )
				if ( tile.TileNumber == tileNumber )
					return tile;
		}
		return null;
	}
}