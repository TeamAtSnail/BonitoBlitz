/*
 * part of the gm0 (w.i.p name) gamemode
 * library used across the board gamemode & minigames
 * - last updated indev:3
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace libgm0;
using Sandbox;
using System.Text.Json;

public partial class Game
{
	public struct KnownPlayer
	{
		public System.Guid Uid { get => Data.Uid; }
		public bool WasBot = false;

		public PlayerData Data = null;

		[System.Text.Json.Serialization.JsonIgnore]
		public BasePlayer Player = null;

		public KnownPlayer()
		{
			Data = new();
		}

		public KnownPlayer( PlayerData data )
		{
			Data = data;
		}
	}

	public struct StoredGameData
	{
		public string DisplayName;
	}

	public readonly static string StorageLocation = "libgm0/savedata";
	public BaseFileSystem SaveFs;
	public BaseFileSystem PlayersFs;

	private void SetUpStorage()
	{
		if ( Host.IsClient )
			return;

		// Set up file sub system
		FileSystem.OrganizationData.CreateDirectory( $"{StorageLocation}/{Uid}" );
		SaveFs = FileSystem.OrganizationData.CreateSubSystem( $"{StorageLocation}/{Uid}" );

		// Create structure
		SaveFs.CreateDirectory( "player" );
		PlayersFs = SaveFs.CreateSubSystem( "player" );
	}

	/// <summary>
	/// Load game state
	/// Should be used after moving to next gamemode
	/// </summary>
	public void Load()
	{
		if ( Host.IsClient )
			return;

		// Load StoredGameData
		Stored = SaveFs.ReadJson<StoredGameData>( "data" );

		// Load KnownPlayer data
		foreach ( var file in PlayersFs.FindFile( "", "*" ) )
		{
			KnownPlayers.Add( new KnownPlayer( PlayersFs.ReadJson<PlayerData>( file ) ) );
		}
	}

	/// <summary>
	/// Save game state
	/// Should be used before moving to next gamemode
	/// </summary>
	public void Save()
	{
		if ( Host.IsClient )
			return;

		// Save StoredGameData
		SaveFs.WriteAllText( "data", JsonSerializer.Serialize( Stored ) );

		// Save KnownPlayer data
		foreach ( var knownPlayer in KnownPlayers )
			PlayersFs.WriteAllText( knownPlayer.Uid.ToString(), JsonSerializer.Serialize( knownPlayer.Data ) );
	}
}