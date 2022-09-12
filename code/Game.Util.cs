/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox;
using System.Linq;
namespace gm0;

public partial class Gamemode0 : Game
{
	/// <summary>
	/// Change all player cameras to provided MapCamera
	/// </summary>
	/// <param name="camera">MapCamera</param>
	static public void BroadcastCamera( MapCamera camera )
	{
		foreach ( var client in Client.All )
		{
			if ( client.Pawn is BasePlayer player )
			{
				player.Camera.SetToMapCamera( camera );
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