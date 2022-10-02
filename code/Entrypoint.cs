/*
 * Game.cs - entrypoint and initialization
 * Initializes the gamemode and figures out where to go from the save on disk
 * (alternatively creates that save on disk)
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using System;

namespace BonitoBlitz;

public partial class Entrypoint : libblitz.Game
{
	public Entrypoint()
	{
		// Log some game info in console
		Log.Info( $"BonitoBlitz - development version (https://github.com/lotuspar/BonitoBlitz)" );
		Log.Info( $"Running {(Sandbox.Host.IsClient ? "clientside" : "serverside")} on {DateTime.Now.ToShortDateString()}" );
	}

	/// <summary>
	/// Should be called after the libblitz Game is ready to play
	/// (players should be added and ready)
	/// </summary>
	public void StartGame()
	{
		// Create activity instances
		AddActivity( new CoreActivities.MainMenuActivity() );

		// Set up the game
		// (placeholder): for now just go to the mainmenu no matter what
		SetActivityByType<CoreActivities.MainMenuActivity>();
	}

	[Sandbox.ConCmd.Server( "bb_popl" )]
	public static void ForcePopulate()
	{
		var entrypoint = Sandbox.Game.Current as Entrypoint;
		foreach ( var client in Sandbox.Client.All )
		{
			var player = new libblitz.Player
			{
				DisplayName = client.Name
			};
			player.SetClient( client );
			entrypoint.Players.Add( player );
		}
	}


	[Sandbox.ConCmd.Server( "bb_start" )]
	public static void ForceStart()
	{
		var entrypoint = Sandbox.Game.Current as Entrypoint;
		entrypoint.StartGame();
	}
}