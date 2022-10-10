/*
 * BaseTile.cs
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using System;
using System.Linq;
using Sandbox;

namespace BonitoBlitz.CoreActivities.Board;

/// <summary>
/// Singular map tile entity; used to denote tile position
/// Requires number, all position and transforms etc. done in-engine
/// </summary>
public abstract partial class BaseTile : Sandbox.Entity
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

	/// <summary>
	/// Called once when a player passes this tile at any point
	/// </summary>
	/// <param name="player">libblitz.Player</param>
	public virtual void OnPlayerPass( libblitz.Player player ) { }
	/// <summary>
	/// Called once when a player finishes their turn on this tile
	/// </summary>
	/// <param name="player">libblitz.Player</param>
	public virtual void OnPlayerStand( libblitz.Player player ) { }
	/// <summary>
	/// Called every player Simulate until tile returned
	/// </summary>
	/// <param name="player">libblitz.Player</param>
	/// <returns>Non-null only if tile is done processing</returns>
	public virtual string Process( libblitz.Player player ) => NextTile;

	private Rotation EndRotation;

	/// <summary>
	/// Called when starting move animation 
	/// </summary>
	/// <param name="player">Moving player</param>
	/// <param name="end">End tile of animation</param>
	public virtual void StartMoveAnimation( libblitz.Player player, BaseTile end )
	{
		if ( Host.IsClient )
			return;

		if ( player.Pawn is BoardPawn pawn )
		{
			// Calculate delta
			Vector3 AnimationDelta = end.Position - Position;

			// Set walking animation
			pawn.SetAnimParameter( "move_x", AnimationDelta.x );
			pawn.SetAnimParameter( "move_y", AnimationDelta.y );
			pawn.SetAnimParameter( "move_z", AnimationDelta.z );

			// Calculate end rotation
			EndRotation = Rotation.LookAt( end.Position - pawn.Position, Vector3.Up );
			pawn.EyeRotation = EndRotation;
			pawn.Rotation = EndRotation.Angles().WithPitch( 0 ).ToRotation();
		}
	}

	/// <summary>
	/// Update player move animation
	/// Called in Simulate / FrameSimulate
	/// </summary>
	/// <param name="player">Moving player</param>
	/// <param name="end">End tile of animation</param>
	/// <param name="progress">0-1 float progress of animation</param>
	public virtual void UpdateMoveAnimation( libblitz.Player player, BaseTile end, float progress )
	{
		if ( end == null )
			return;

		Vector3 delta = end.Position - Position;
		player.Pawn.Position = Position + (delta * progress);

		Vector3 offset = Sandbox.Host.IsClient ? Vector3.Zero : Vector3.Up * 20;
		Color color = Sandbox.Host.IsClient ? Color.Orange : Color.Blue;
		DebugOverlay.Text( $"* This *", Position + offset, color );
		DebugOverlay.Text( $"> Next {end.Name} <", end.Position + offset, color );
	}

	/// <summary>
	/// Called when ending move animation
	/// </summary>
	/// <param name="player">Moving player</param>
	/// <param name="end">End tile of animation</param>
	public virtual void EndMoveAnimation( libblitz.Player player, BaseTile end )
	{
		if ( player.Pawn is BoardPawn pawn )
		{
			pawn.SetAnimParameter( "move_x", 0 );
			pawn.SetAnimParameter( "move_y", 0 );
			pawn.SetAnimParameter( "move_z", 0 );
			pawn.Position = end.Position;
		}
	}

	/// <summary>
	/// Find BaseTile from tile name
	/// </summary>
	/// <param name="tileName">Tile name</param>
	/// <returns>BaseTile or null</returns>
	public static BaseTile FromTileName( string tileName )
	{
		foreach ( var tile in Entity.All.OfType<BaseTile>() )
		{
			if ( tile.Name.ToLower() == tileName.ToLower() )
				return tile;
		}
		return null;
	}
}