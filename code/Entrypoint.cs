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

	private BaseTile _start;

	/// <summary>
	/// Should be called after the Game is ready to play
	/// (members should be added)
	/// </summary>
	private void StartGame()
	{
		_start = BaseTile.FromName( "start" );
		
		foreach ( var member in Members )
		{
			var pawn = member.GetOrCreatePawn<BoardPawn>( new object[] { member } );
			pawn.Position = _start.Position;
			pawn.GameMember.CurrentTile = _start;
			member.UsePawn( pawn );
		}

		var description = ActivityDescription.For<TurnProviderActivity>();
		description.Actors = Members;
		description.Members = Members;
		PushActivity<ActivityResult>( description.CreateInstance<TurnProviderActivity>(), null );
	}

	[Sandbox.ConCmd.Server( "bb_populate" )]
	public static void ForcePopulate()
	{
		if ( Sandbox.Game.Current is not Entrypoint entrypoint )
		{
			return;
		}

		foreach ( var client in Sandbox.Client.All )
		{
			var member = new GameMember { DisplayName = client.Name, ClientIds = new List<long>() { client.PlayerId } };
			Log.Info( $"New game member: {member}, name {member.DisplayName}" );
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

	[Sandbox.ConCmd.Server( "bb_forceroll" )]
	public static void Test0( int roll )
	{
		var entrypoint = Sandbox.Game.Current as Entrypoint;
		var desc = entrypoint.ActivityStack.Last().Activity.CreateDescription().Transform( "MoveControllerActivity" );
		entrypoint.PushActivity( desc,
			new PlayerRollActivity.Result() { Moves = roll } );
	}
}
