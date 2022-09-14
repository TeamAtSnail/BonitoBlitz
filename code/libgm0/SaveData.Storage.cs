/*
 * part of the gm0 (w.i.p name) gamemode
 * library used across the board gamemode & minigames
 * - last updated indev:3
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace libgm0;
using System.Collections.Generic;
using System.Text.Json;
using Sandbox;

public partial class SaveData
{
	public readonly static string StorageLocation = "libgm0/savedata";
	public readonly static int Version = 01;

	/// <summary>
	/// List of saves
	/// </summary>
	public static List<string> All
	{
		get
		{
			List<string> output = new();
			if ( !FileSystem.OrganizationData.DirectoryExists( StorageLocation ) )
				return output;
			foreach ( var file in FileSystem.OrganizationData.FindFile( StorageLocation, "*.dat" ) )
			{
				// we know the string has .dat at the end, so just remove by number
				output.Add( file[..4] );
			}
			return output;
		}
	}

	/// <summary>
	/// Save current save data to save name
	/// </summary>
	/// <param name="name">Name</param>
	public void SaveAt( string name )
	{
		data["version"] = Version.ToString();
		fss.WriteAllText( $"{name}.dat", JsonSerializer.Serialize( data ) );
	}

	/// <summary>
	/// Save current save data
	/// </summary>
	public void Save() => SaveAt( Name );

	/// <summary>
	/// Save current save data to save name
	/// </summary>
	/// <param name="name">Name</param>
	public void LoadAt( string name )
	{
		var contents = fss.ReadAllText( $"{name}.dat" );
		data = JsonSerializer.Deserialize<Dictionary<string, string>>( contents );
	}
}