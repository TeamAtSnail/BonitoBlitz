/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.Board;

[Library( "gm0_tile_coin" ), HammerEntity]
[Title( "Coin Tile" ), Category( "Player" ), Icon( "place" )]
[Description( "Tile for players to stand on" )]
public class CoinTile : BaseTile
{
	public override void OnPlayerStand( BoardPawn pawn ) => pawn.Player.Coins += 3;

	public override void OnPlayerPass( BoardPawn pawn ) { }

	public override string Process( BoardPawn pawn ) => NextTile;
}