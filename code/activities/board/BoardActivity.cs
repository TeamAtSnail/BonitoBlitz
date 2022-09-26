/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace BonitoBlitz.Board;
using System;
using System.Collections.Generic;
using Sandbox;

public partial class BoardActivity : libblitz.Activity<BoardPawn>
{
	[Net] public StartArea StartArea { get; private set; } = null;

	[Net] public MapCamera StartCamera { get; private set; } = null;

	public BoardActivity( IList<libblitz.Player> players ) : base( players )
	{
	}

	public BoardActivity() : base( null )
	{
	}

	public override void ClientInitialize() { }

	public override void Initialize()
	{
		/* Pre game checks */
		// Entity gathering
		foreach ( var entity in Entity.All )
		{
			// Make sure there's only 1 StartArea...
			// (also get the StartArea)
			if ( entity is StartArea startArea )
			{
				if ( StartArea != null )
					throw new Error.GameMapException( "Map has multiple StartAreas!" );
				StartArea = startArea;
			}

			// Make sure there's only 1 starting MapCamera...
			// (also get the MapCamera)
			if ( entity is MapCamera mapCamera && mapCamera.Name == "StartCamera" )
			{
				if ( StartCamera != null )
					throw new Error.GameMapException( "Map has multiple cameras called StartCamera!" );
				StartCamera = mapCamera;
			}
		}

		// Make sure we found a StartArea
		if ( Host.IsServer && StartArea == null )
			throw new Error.GameMapException( "Map has no StartArea!" );

		// Make sure there's a camera with the name StartCamera
		if ( Host.IsServer && StartCamera == null )
			throw new Error.GameMapException( "Map has no camera called StartCamera!" );

		// Prepare all players
		uint distance = 64;
		float delta = 360 / Players.Count * (MathF.PI / 180);

		for ( int i = 0; i < Players.Count; i++ )
		{
			libblitz.Player player = Players[i];
			var pawn = new BoardPawn( player )
			{
				Position = new( 0, 0, StartArea.Position.z )
				{
					x = StartArea.Position.x + MathF.Sin( delta * i ) * distance,
					y = StartArea.Position.y + MathF.Cos( delta * i ) * distance
				}
			};

			// Give pawn new camera
			var camera = new PointCamera();
			camera.SetToMapCamera( StartCamera );
			pawn.Camera = camera;

			// Set player pawn
			player.Pawn = pawn;
		}
	}
}