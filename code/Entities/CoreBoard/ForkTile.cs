using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.Entities.CoreBoard;

[Library( "bb_tile_fork" ), HammerEntity]
[Title( "Fork Tile" ), Category( "Player" ), Icon( "place" )]
[Description( "Tile for players to walk over" )]
public partial class ForkTile : BaseTile, IActivityTile
{
	public string ActivityName { get; set; } = "ForkMoveActivity";

	[Property( Title = "Next Tile 1" ), FGDType( "target_destination" )]
	[Net]
	public string NextTileOne { get; set; }

	[Property( Title = "Next Tile 2" ), FGDType( "target_destination" )]
	[Net]
	public string NextTileTwo { get; set; }

	[Property( Title = "Next Tile 3" ), FGDType( "target_destination" )]
	[Net]
	public string NextTileThree { get; set; }
}
