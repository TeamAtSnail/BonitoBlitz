/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0.Events;

public partial class Builder
{
	public partial class PlayerData
	{
		public static GameEvent Init( uint playerIndex, long playerId )
			=> new( ActionCode.PLAYER_DATA_INIT, playerIndex, playerId );
	}
}