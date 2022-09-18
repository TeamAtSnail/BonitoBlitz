/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public partial class Gamemode0 : libgm0.Game
{

	public Gamemode0() : base( libgm0.GameData.LoadActiveGame() )
	{
	}

	public override libgm0.Pawn PlayerJoined( Client cl, libgm0.PlayerData playerData )
	{
		// set player
		var pawn = new BoardPawn( playerData );
		cl.Pawn = pawn;
		return pawn;
	}

	public override void OnAllPlayersConnected()
	{
		// todo: why isn't this working?
		BroadcastCamera( "mc_overview" );

		foreach ( var entity in Entity.All )
		{
			if ( entity is StartArea area )
			{
				MovePlayersToStartArea( area );
			}
		}
	}

	public override void RenderHud()
	{
		base.RenderHud();

		int line = 0;
		foreach ( var client in Client.All )
		{
			if ( client.Pawn is BoardPawn player )
			{
				DebugOverlay.ScreenText( $"Client {client.Name}", Vector2.One * 30, line++, Color.Orange );
				DebugOverlay.ScreenText( $"   Coins {player.Data.Coins}", Vector2.One * 30, line++, Color.Cyan );
				DebugOverlay.ScreenText( $"   Stars {player.Data.Stars}", Vector2.One * 30, line++, Color.Cyan );
			}
		}
	}
}