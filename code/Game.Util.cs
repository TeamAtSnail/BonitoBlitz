/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;
using System;
using System.Linq;

public partial class Gamemode0
{
	/// <summary>
	/// Move all players to starting area in a circle
	/// </summary>
	public void MovePlayersToStart()
	{
		uint distance = 64;
		float delta = 360 / Data.Players.Count * (MathF.PI / 180);
		for ( int i = 0; i < Data.Players.Count; i++ )
		{
			if ( Data.Players[i].Pawn is BoardPawn player )
			{
				// Move player to their starting spot
				player.Position = new( 0, 0, StartArea.Position.z )
				{
					x = StartArea.Position.x + MathF.Sin( delta * i ) * distance,
					y = StartArea.Position.y + MathF.Cos( delta * i ) * distance
				};

				// Give player new camera
				var camera = new PointCamera();
				camera.SetToMapCamera( StartCamera );
				player.Camera = camera;
			}
		}
	}

	/// <summary>
	/// Change all player cameras to provided MapCamera
	/// </summary>
	/// <param name="mapCamera">MapCamera</param>
	static public void BroadcastCamera( MapCamera mapCamera )
	{
		foreach ( var client in Client.All )
		{
			if ( client.Pawn is BoardPawn player )
			{
				var camera = new PointCamera();
				camera.SetToMapCamera( mapCamera );
				player.Camera = camera;
			}
		}
	}

	/// <summary>
	/// Change all player cameras to provided MapCamera
	/// </summary>
	/// <param name="cameraName">MapCamera hammer name</param>
	static public void BroadcastCamera( string cameraName )
	{
		var entities = Entity.All.OfType<MapCamera>();
		var camera = entities.Where( ( camera ) => camera.Name == cameraName ).Single();
		BroadcastCamera( camera );
	}
}