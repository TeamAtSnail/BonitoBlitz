using BonitoBlitz.Activities.Movement;
using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.Entities.CoreBoard;

[Library( "bb_tile_fork" ), HammerEntity]
[Title( "Fork Tile" ), Category( "Tile" ), Icon( "place" )]
[Description( "Tile for players to walk over" )]
public partial class ForkTile : BaseTile, IActivityTile
{
	public string ActivityName { get; set; } = typeof(ForkActivity).FullName;

	[Property( Title = "Next Tile 1" ), FGDType( "target_destination" )]
	[Net]
	public string NextTileOneName { get; set; }

	public BaseTile NextTileOne => FromName( NextTileOneName );

	[Property( Title = "Next Tile 2" ), FGDType( "target_destination" )]
	[Net]
	public string NextTileTwoName { get; set; }

	public BaseTile NextTileTwo => FromName( NextTileOneName );

	[Property( Title = "Next Tile 3" ), FGDType( "target_destination" )]
	[Net]
	public string NextTileThreeName { get; set; }

	public BaseTile NextTileThree => FromName( NextTileOneName );
}
