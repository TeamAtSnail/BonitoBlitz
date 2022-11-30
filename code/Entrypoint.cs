/*
 * Game.cs - entrypoint and initialization
 * Initializes the gamemode and figures out where to go from the save on disk
 * (alternatively creates that save on disk)
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */

global using Game = libblitz.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using BonitoBlitz.Activities;
using BonitoBlitz.Activities.CoreBoard;
using BonitoBlitz.Entities.CoreBoard;
using libblitz;

namespace BonitoBlitz;

public class Entrypoint : libblitz.Game
{
	public Entrypoint()
	{
		// Log some game info in console
		Log.Info( $"BonitoBlitz - development version (https://github.com/TeamAtSnail/BonitoBlitz)" );
		Log.Info(
			$"Running {(Sandbox.Host.IsClient ? "clientside" : "serverside")} on {DateTime.Now.ToShortDateString()}" );
	}

	/// <summary>
	/// Should be called after the Game is ready to play
	/// (members should be added)
	/// </summary>
	private void StartGame()
	{
		var description = ActivityDescription.For<CoreActivity>();
		description.Actors = Members;
		description.Members = Members;
		PushActivity<ActivityResult>( description.CreateInstance<CoreActivity>(), null );
	}

	[Sandbox.ConCmd.Server( "bb_populate" )]
	public static void ForcePopulate()
	{
		var entrypoint = Sandbox.Game.Current as Entrypoint;

		foreach ( var client in Sandbox.Client.All )
		{
			var member = new GameMember { DisplayName = client.Name, ClientIds = new List<long>() { client.PlayerId } };
			Log.Info( $"New game member: {member}, name {member.DisplayName}" );
			entrypoint!.Members.Add( member );
			member.UpdateCurrentClient();
		}
	}

	[Sandbox.ConCmd.Server( "bb_start" )]
	public static void ForceStart()
	{
		var entrypoint = Sandbox.Game.Current as Entrypoint;
		entrypoint?.StartGame();
	}

	[Sandbox.ConCmd.Server( "bb_save" )]
	public static void TestSave()
	{
		var entrypoint = Sandbox.Game.Current as Entrypoint;
		entrypoint!.SavePersistent( entrypoint.Uid );
		Log.Info( $"Saved, UID: {entrypoint.Uid}" );
		Log.Info( $"Use bb_load <uid> or bb_loadlast" );
	}

	[Sandbox.ConCmd.Server( "bb_loadlast" )]
	public static void TestLoadLast()
	{
		var entrypoint = Sandbox.Game.Current as Entrypoint;
		entrypoint!.LoadPersistent( entrypoint.Uid );
	}

	[Sandbox.ConCmd.Server( "bb_load" )]
	public static void TestLoad( string uidString )
	{
		var entrypoint = Sandbox.Game.Current as Entrypoint;
		if ( !Guid.TryParse( uidString, out var result ) )
		{
			Log.Info( "Invalid UID" );
		}

		entrypoint!.LoadPersistent( result );
	}
}
