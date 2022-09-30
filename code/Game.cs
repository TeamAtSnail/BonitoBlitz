/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using System;

namespace BonitoBlitz;

public partial class BonitoBlitz : libblitz.Game
{
	public int HudLine = 2;
	public DateTime date;

	public BonitoBlitz()
	{
		date = DateTime.Now;
		if ( Sandbox.Host.IsClient )
		{
			Log.Info( $"!!! This is a development version of BonitoBlitz. ({date.ToShortDateString()})" );
			Log.Info( "!!! https://github.com/lotuspar/BonitoBlitz" );
		}
	}

	public void PrintDebugArea( string text, Color color )
	{
		DebugOverlay.ScreenText(
			text,
			Vector2.One * 20,
			HudLine++,
			color );
	}

	public override void RenderHud()
	{
		HudLine = 0;

		PrintDebugArea(
			$"This is a development version of BonitoBlitz. ({date.ToShortDateString()})",
			Color.Magenta
		);

		PrintDebugArea( "https://github.com/lotuspar/BonitoBlitz", Color.Magenta );

		PrintDebugArea(
			$"Game UID: {this.Uid}",
			Color.White
		);

		PrintDebugArea(
			$"Game display name: {this.DisplayName}",
			Color.White
		);

		PrintDebugArea(
			$"Game player count: {this.Players.Count}",
			Color.White
		);

		for ( int i = 0; i < Players.Count; i++ )
		{
			libblitz.Player player = Players[i];
			PrintDebugArea(
				$"Player {i + 1}: {player.Uid}/{player.DisplayName}",
				Color.Orange
			);
			PrintDebugArea(
				$"  :: Coins {player.Coins}, Special {player.SpecialCoins}",
				Color.White
			);
			PrintDebugArea(
				$"  :: Client {player.Client.Name}",
				Color.White
			);
		}

		for ( int i = 0; i < Activities.Count; i++ )
		{
			libblitz.Activity activity = Activities[i];
			if ( activity == null )
				continue;
			PrintDebugArea(
				$"Activity {i + 1}: {activity.GetType().Name} {(activity == Activity ? "(<--)" : "")}",
				Color.Cyan
			);

			PrintDebugArea( $"  :: PawnType {activity.PawnType.Name}", Color.White );
		}
	}

	public override void Spawn()
	{
		base.Spawn();

		var player = new libblitz.Player
		{
			DisplayName = "Player",
			CanBeBot = false
		};
		player.PlayedBy.Add( 0 );

		Players.Add( player );

		var activity = new Board.BoardActivity( Players );
		SetActivity( activity );
	}

}