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

		// Add debug panel
		if ( Sandbox.Host.IsClient )
			RootPanel.AddChild<SharedUi.Debug>();
	}

	/// <summary>
	/// Should be called after the libblitz Game is ready to play
	/// (players should be added and ready)
	/// </summary>
	public void StartGame()
	{
		// Set game to initial status
		Status = libblitz.GameStatus.INTRODUCTION_NEEDED;

		// Create activity instances
		AddActivity( new CoreActivities.MainMenuActivity() );
		AddActivity( new CoreActivities.Board.BoardActivity() );

		// Set up the game
		// (placeholder): for now just go to the mainmenu no matter what
		SetActivityByType<CoreActivities.Board.BoardActivity>();
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

		var bot = new libblitz.Player
		{
			DisplayName = "Flanders",
			CanBeBot = true,
		};

		var bot2 = new libblitz.Player
		{
			DisplayName = "Bart",
			CanBeBot = true,
		};

		entrypoint.Players.Add( bot );
		entrypoint.Players.Add( bot2 );
	}

	[Sandbox.ConCmd.Server( "bb_start" )]
	public static void ForceStart()
	{
		var entrypoint = Sandbox.Game.Current as Entrypoint;
		entrypoint.StartGame();
	}

	[Sandbox.ConCmd.Server( "bb_save" )]
	public static void ForceSave()
	{
		var entrypoint = Sandbox.Game.Current as Entrypoint;
		entrypoint.Save();
		Log.Info( $"> {entrypoint.Uid}" );
	}

	[Sandbox.ConCmd.Server( "bb_load" )]
	public static void ForceLoad( string uid )
	{
		var entrypoint = Sandbox.Game.Current as Entrypoint;

		entrypoint.Load( Guid.Parse( uid ) );

		// Create activity instances
		entrypoint.AddActivity( new CoreActivities.MainMenuActivity() );
		entrypoint.AddActivity( new CoreActivities.Board.BoardActivity() );

		// Set up the game
		// (placeholder): for now just go to the mainmenu no matter what
		entrypoint.SetActivityByType<CoreActivities.Board.BoardActivity>();
	}
}