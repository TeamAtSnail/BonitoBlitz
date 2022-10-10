/*
 * CoinTile.cs
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.CoreActivities.Board;

[Library( "bb_board_tile_coin" ), HammerEntity]
[Title( "Coin Tile" ), Category( "Player" ), Icon( "place" )]
[Description( "Tile for players to stand on" )]
public class CoinTile : BaseTile
{
	public override void OnPlayerStand( libblitz.Player player ) => player.Coins += 3;
}