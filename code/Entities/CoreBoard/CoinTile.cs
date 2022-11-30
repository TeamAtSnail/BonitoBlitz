using BonitoBlitz.Activities;
using libblitz;
using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.Entities.CoreBoard;

[Library( "bb_tile_coin" ), HammerEntity]
[Title( "Coin Tile" ), Category( "Tile" ), Icon( "place" )]
[Description( "Tile that gives out coins when standing on it" )]
public partial class CoinTile : WalkTile, IStaticTile, IBasicAnimationTile, IActionTile
{
	public void OnPass( GameMember member )
	{
	}

	public void OnStand( GameMember member )
	{
		member.Coins += 3;
	}
}
