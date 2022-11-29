using System.Linq;
using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.Entities.CoreBoard;

[Library( "bb_config" ), HammerEntity]
[Title( "Map Configurator" ), Category( "Map" ), Icon( "data_object" )]
[Description( "Map configuration data" )]
public partial class Config : Entity
{
	[Property( Title = "Start Tile Name" ), FGDType( "target_destination" )]
	[Net]
	public string StartTileName { get; set; }

	public BaseTile StartTile => BaseTile.FromName( StartTileName );

	[Property( Title = "Main Camera Name" ), FGDType( "target_destination" )]
	[Net]
	public string MainCameraName { get; set; }

	public StaticCameraSpot MainCamera =>
		All.OfType<StaticCameraSpot>().SingleOrDefault( v => v.Name == MainCameraName );

	public static Config Current => All.OfType<Config>().SingleOrDefault();
}
