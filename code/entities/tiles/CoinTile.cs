/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;
using SandboxEditor;

[Library( "gm0_coin_tile" ), HammerEntity]
[Title( "Coin Tile" ), Category( "Player" ), Icon( "place" )]
[Description( "Tile for players to stand on" )]
public class CoinTile : BaseTile
{
	public override void OnPlayerDestination( BoardPlayer player )
	{
		player.Coins += 3;
	}

	public override void OnPlayerEnter( BoardPlayer player )
	{ }
}