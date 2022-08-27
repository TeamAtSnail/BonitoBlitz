/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;

public partial class GameEventCreator
{
	public partial class PlayerData
	{
		public static GameEvent Init( uint playerIndex )
			=> new( (uint)GameEventAction.PLAYER_DATA_INIT, playerIndex, 0, 0 );
	}
}