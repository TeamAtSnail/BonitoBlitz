/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:3
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
	[Property( Title = "Next Tile Name" ), FGDType( "target_destination" )]
	[Net]
	public string NextTile { get; set; }

	/// <summary>
	/// Should the tile count as a move?
	/// </summary>
	[Property( Title = "Is Real Tile" )]
	public bool IsRealTile { get; set; } = true;

	/// <summary>
	/// How fast the animation to the next tile is
	/// </summary>
	[Property( Title = "Animation Speed" )]
	public float AnimationSpeed { get; set; } = 0.7f;

	[Net]
	private Vector3 animationDelta { get; set; } = Vector3.Zero;

	/// <summary>
	/// Called once when a player passes this tile at any point
	/// </summary>
	/// <param name="player">BoardPawn</param>
	public abstract void OnPlayerPass( BoardPawn player );

	/// <summary>
	/// Called once when a player finishes their turn on this tile
	/// </summary>
	/// <param name="player">BoardPawn</param>
	public abstract void OnPlayerStand( BoardPawn player );

	/// <summary>
	/// Called every player Simulate until tile returned
	/// </summary>
	/// <param name="player">BoardPawn</param>
	/// <returns>Non-null only if tile is done processing</returns>
	public abstract string Process( BoardPawn player );

	/// <summary>
	/// Called when starting move animation 
	/// </summary>
	/// <param name="pawn">Moving pawn</param>
	/// <param name="endTile">End tile of animation</param>
	public virtual void StartMoveAnimation( BoardPawn pawn, BaseTile endTile )
	{
		// Calculate delta
		animationDelta = endTile.Position - Position;

		// Set walking animation
		pawn.SetAnimParameter( "move_x", animationDelta.x );
		pawn.SetAnimParameter( "move_y", animationDelta.y );
		pawn.SetAnimParameter( "move_z", animationDelta.z );
	}

	/// <summary>
	/// Update player move animation
	/// Called in Simulate / FrameSimulate
	/// </summary>
	/// <param name="pawn">Moving pawn</param>
	/// <param name="progress">0-1 float progress of animation</param>
	public virtual void UpdateMoveAnimation( BoardPawn pawn, float progress )
	{
		pawn.Position = Position + (animationDelta * progress);
	}

	/// <summary>
	/// Called when ending move animation
	/// </summary>
	/// <param name="pawn">Moving pawn</param>
	public virtual void EndMoveAnimation( BoardPawn pawn )
	{
		pawn.SetAnimParameter( "move_x", 0 );
		pawn.SetAnimParameter( "move_y", 0 );
		pawn.SetAnimParameter( "move_z", 0 );
	}

	/// <summary>
	/// Find BaseTile from tile name
	/// </summary>
	/// <param name="tileName">Tile name</param>
	/// <returns>BaseTile or null</returns>
	public static BaseTile FromTileName( string tileName )
	{
		foreach ( var entity in Entity.All )
		{
			if ( entity is BaseTile tile )
			{
				if ( tile.Name.ToLower() == tileName.ToLower() )
					return tile;
			}
		}
		return null;
	}
}