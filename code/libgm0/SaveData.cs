/*
 * part of the gm0 (w.i.p name) gamemode
 * library used across the board gamemode & minigames
 * - last updated indev:3
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace libgm0;
using System.Collections.Generic;
using Sandbox;

public partial class SaveData
{
	public readonly string Name;
	private Dictionary<string, string> data = new();
	private readonly BaseFileSystem fss;

	public SaveData( string name )
	{
		Name = name;
		FileSystem.OrganizationData.CreateDirectory( StorageLocation );
		fss = FileSystem.OrganizationData.CreateSubSystem( StorageLocation );

		if ( !fss.FileExists( $"{Name}.dat" ) )
			return;

		fss.ReadAllText( $"{Name}.dat" );
	}

	public string this[string i]
	{
		get => data[i];
		set
		{
			data[i] = value;
		}
	}
}