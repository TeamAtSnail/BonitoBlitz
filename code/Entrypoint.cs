/*
 * Game.cs - entrypoint and initialization
 * Initializes the gamemode and figures out where to go from the save on disk
 * (alternatively creates that save on disk)
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using BonitoBlitz.Activities.Introduction;
using libblitz;

namespace BonitoBlitz;

public partial class Entrypoint : libblitz.Game
{
	public Entrypoint()
	{
		// Log some game info in console
		Log.Info( $"BonitoBlitz - development version (https://github.com/lotuspar/BonitoBlitz)" );
		Log.Info(
			$"Running {(Sandbox.Host.IsClient ? "clientside" : "serverside")} on {DateTime.Now.ToShortDateString()}" );
	}

	/// <summary>
	/// Should be called after the libblitz Game is ready to play
	/// (players should be added and ready)
	/// </summary>
	public void StartGame()
	{
		var description = new ActivityDescription() { Name = "TutorialActivity", Actors = Members, Members = Members };
		var activity = description.CreateInstance<TutorialActivity>();
		PushActivity<ActivityResult>( activity, null );
	}

	[Sandbox.ConCmd.Server( "bb_popl" )]
	public static void ForcePopulate()
	{
		if ( Sandbox.Game.Current is not Entrypoint entrypoint )
		{
			Log.Error( "entrypoint NULL!" );
			return;
		}

		foreach ( var client in Sandbox.Client.All )
		{
			var member = new libblitz.GameMember
			{
				DisplayName = client.Name, ClientIds = new List<long>() { client.PlayerId }
			};
			Log.Info( $"adding {member}" );
			entrypoint.Members.Add( member );
			member.UpdateCurrentClient();
		}
	}

	[Sandbox.ConCmd.Server( "bb_start" )]
	public static void ForceStart()
	{
		var entrypoint = Sandbox.Game.Current as Entrypoint;
		entrypoint?.StartGame();
	}
}
