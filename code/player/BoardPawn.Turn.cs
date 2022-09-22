/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public partial class BoardPawn
{
	[Net]
	private BaseTile InternalTile { get; set; }

	public BaseTile Tile
	{
		get => InternalTile;
		set
		{
			InternalTile = value;
			Data.Extensions["TileName"] = (value == null) ? "null" : value.Name;
		}
	}

	/// <summary>
	/// Moves left
	/// </summary>
	[Net]
	private int Moves { get; set; }

	/// <summary>
	/// Tile to move to, should be set using BaseTile.Process
	/// </summary>
	[Net]
	private string NextTile { get; set; } = null;

	public bool HasMoves => Moves > 0;

	public void StartTurn()
	{
		Moves = Rand.Int( 1, 7 );
		DebugOverlay.ScreenText( $"Moving {Moves} space(s)!", Vector2.One * 150, 0, Color.Cyan, 4.0f );
	}

	private void HandleMovementComplete()
	{
		if ( !Host.IsServer )
			return;

		if ( NextTile != null )
		{
			Tile = BaseTile.FromTileName( NextTile );

			if ( Tile.IsRealTile )
				Moves--;
		}

		Position = Tile.Position;

		Tile.OnPlayerPass( this );

		if ( Moves == 0 )
			Tile.OnPlayerStand( this );
	}

	private void TurnSimulate( Client client )
	{
		if ( HasActiveAnimation )
			return; // Wait for animation to complete

		if ( HasMoves )
		{
			// Make sure we are on a BaseTile first
			if ( Tile == null )
				Tile = BaseTile.FromTileName( "start" );

			// Get next tile
			NextTile = Tile.Process( this );

			if ( NextTile == null )
				return; // If we don't have a next tile yet then wait

			// Start animation to next tile
			StartMoving( BaseTile.FromTileName( NextTile ) );
		}
	}
}