/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System;
using Sandbox;

public partial class Gamemode0 : libgm0.Game
{
	[Net]
	private StartArea StartArea { get; set; } = null;

	[Net]
	private MapCamera StartCamera { get; set; } = null;

	public Gamemode0() : base( libgm0.GameData.LoadActiveGame() )
	{ }

	public override void PostLevelLoaded()
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
					throw new libgm0.error.GameMapException( "Map has multiple StartAreas!" );
				StartArea = startArea;
			}

			// Make sure there's only 1 starting MapCamera...
			// (also get the MapCamera)
			if ( entity is MapCamera mapCamera && mapCamera.Name == "StartCamera" )
			{
				if ( StartCamera != null )
					throw new libgm0.error.GameMapException( "Map has multiple cameras called StartCamera!" );
				StartCamera = mapCamera;
			}
		}

		// Make sure we found a StartArea
		if ( Host.IsServer && StartArea == null )
			throw new libgm0.error.GameMapException( "Map has no StartArea!" );

		// Make sure there's a camera with the name StartCamera
		if ( Host.IsServer && StartCamera == null )
			throw new libgm0.error.GameMapException( "Map has no camera called StartCamera!" );
	}

	public override libgm0.Pawn PlayerJoined( Client cl, libgm0.PlayerData playerData )
	{
		// set player
		var pawn = new BoardPawn( playerData );
		cl.Pawn = pawn;
		return pawn;
	}

	public override void OnAllPlayersConnected()
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

	public override void RenderHud()
	{
		base.RenderHud();

		int line = 0;
		foreach ( var client in Client.All )
		{
			if ( client.Pawn is BoardPawn player )
			{
				DebugOverlay.ScreenText( $"Client {client.Name}", Vector2.One * 30, line++, Color.Orange );
				DebugOverlay.ScreenText( $"   Coins {player.Data.Coins}", Vector2.One * 30, line++, Color.Cyan );
				DebugOverlay.ScreenText( $"   Stars {player.Data.Stars}", Vector2.One * 30, line++, Color.Cyan );
			}
		}
	}
}