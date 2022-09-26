/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;
using SandboxEditor;

[Library( "gm0_tile_option" ), HammerEntity]
[Title( "Option Tile" ), Category( "Player" ), Icon( "place" )]
[Description( "Tile for players to stand on" )]
public partial class OptionTile : BaseTile
{
	[Property( Title = "Secondary Next Tile Name" ), FGDType( "target_destination" )]
	[Net]
	public string SecondaryNextTile { get; set; }

	[Net]
	private string SelectedNextTile { get; set; }

	public override void OnPlayerStand( BoardPawn player )
	{ }

	public override void OnPlayerPass( BoardPawn player )
	{ }

	public override string Process( BoardPawn player )
	{
		DebugOverlay.ScreenText( $"next: {SelectedNextTile}", Vector2.One * 100, 0, Color.Red );

		if ( player.Client.IsBot )
			return (Rand.Int( 0, 1 ) == 0) ? NextTile : SecondaryNextTile;

		if ( Input.Pressed( InputButton.Left ) )
			SelectedNextTile = NextTile;

		if ( Input.Pressed( InputButton.Right ) )
			SelectedNextTile = SecondaryNextTile;

		if ( Input.Pressed( InputButton.Chat ) )
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