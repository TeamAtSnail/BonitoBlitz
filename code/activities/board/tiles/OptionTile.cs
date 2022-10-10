/*
 * OptionTile.cs
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.CoreActivities.Board;

[Library( "bb_board_tile_option" ), HammerEntity]
[Title( "Option Tile" ), Category( "Player" ), Icon( "place" )]
[Description( "Tile for players to stand on" )]
public partial class OptionTile : BaseTile
{
	[Property( Title = "Secondary Next Tile Name" ), FGDType( "target_destination" )]
	[Net] public string SecondaryNextTile { get; private set; }
	[Net] public string SelectedNextTile { get; private set; }
	public override string Process( libblitz.Player player )
	{
		DebugOverlay.ScreenText( $"next: {SelectedNextTile}", Vector2.One * 100, 0, Color.Red );
		if ( player.Client.IsBot )
			return (Rand.Int( 0, 1 ) == 0) ? NextTile : SecondaryNextTile;
		if ( Input.Pressed( InputButton.Left ) )
			SelectedNextTile = NextTile;
		if ( Input.Pressed( InputButton.Right ) )
			SelectedNextTile = SecondaryNextTile;
		if ( Input.Pressed( InputButton.Jump ) )
		{
			if ( SelectedNextTile == null )
			{
				DebugOverlay.ScreenText( "can't", Vector2.One * 100, 1, Color.Red, 5.0f );
			}
			else
			{
				return SelectedNextTile;
			}
		}
		return null;
	}
}