/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;
using System.Linq;

public partial class Gamemode0
{
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