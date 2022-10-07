/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using System.Linq;
using System;
using Sandbox;
using SandboxEditor;

namespace BonitoBlitz;

[Library( "bb_cfg" ), HammerEntity]
[Title( "Game Configuration" ), Category( "Gameplay" ), Icon( "place" )]
[Description( "Game configuration. Needs to be set per-map" )]
public partial class GameConfiguration : Entity
{
	/// <summary>
	/// Confirm configuration
	/// </summary>
	public override void Spawn()
	{
		if ( InitialTileName == null )
			throw new ArgumentNullException( "InitialTileName", "No initial tile specified in BoardConfiguration!" );

		if ( InitialCameraName == null )
			throw new ArgumentNullException( "InitialCameraName", "No initial camera specified in BoardConfiguration!" );
	}

	[Property( Title = "Initial Tile" ), FGDType( "target_destination" )]
	[Net]
	public string InitialTileName { get; set; } = null;
	public CoreActivities.Board.BaseTile InitialTile => CoreActivities.Board.BaseTile.FromTileName( InitialTileName );

	[Property( Title = "Initial Camera" ), FGDType( "target_destination" )]
	[Net]
	public string InitialCameraName { get; set; } = null;
	public CoreActivities.Board.BoardCamera InitialCamera
		=> All.OfType<CoreActivities.Board.BoardCamera>().Where( ( camera ) => camera.Name == InitialCameraName ).Single();
}