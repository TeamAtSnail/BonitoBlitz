/*
 * part of the gm0 (w.i.p name) gamemode
 * library used across the board gamemode & minigames
 * - last updated indev:3
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace libgm0;
using System;
using System.Collections.Generic;
using Sandbox;

public partial class Game : Sandbox.Game
{
	[Net]
	public Guid Uid { get; set; }
	public List<KnownPlayer> KnownPlayers { get; set; }
	public StoredGameData Stored { get; set; }

	/// <summary>
	/// True if all players have connected
	/// </summary>
	[Net]
	public bool GameReady { get; set; }

	/// <summary>
	/// Load ongoing game from disk
	/// </summary>
	/// <param name="uid">Uid</param>
	public Game( Guid uid )
	{
		KnownPlayers = new();
		Stored = new();
		GameReady = false;

		if ( IsClient )
			return;

		if ( uid == Guid.Empty )
		{
			// Init new Uid
			Uid = Guid.NewGuid();
			Log.Info( $"Using new UID {Uid}" );
		}
		else
		{
			Uid = uid;
			Log.Info( $"Using provided UID {Uid}" );
		}

		SetUpStorage();

		if ( IsServer && uid != Guid.Empty )
		{
			if ( SaveFs.FileExists( "data" ) )
			{
				Load();
			}
			else
			{
				throw new NoGameFoundException( $"Game {Uid} not found!" );
			}
		}
	}

	/// <summary>
	/// Create new game
	/// </summary>
	public Game() : this( Guid.Empty )
	{
	}
}