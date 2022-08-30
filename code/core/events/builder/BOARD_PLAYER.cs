/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0.Events;

public partial class Builder
{
	public partial class Board
	{
		public static GameEvent AttemptMoveTile( uint playerIndex, uint tileId )
			=> new( ActionCode.BOARD_PLAYER_ATTEMPT_MOVE_TILE, playerIndex, tileId );
		public static GameEvent ConfirmMoveTile( uint playerIndex, uint tileId )
			=> new( ActionCode.BOARD_PLAYER_CONFIRM_MOVE_TILE, playerIndex, tileId ); 
		public static GameEvent RequestClientOption( uint playerIndex )
			=> new( ActionCode.BOARD_PLAYER_REQUEST_CLIENT_OPTION, playerIndex );
		public static GameEvent ReplyClientOption( uint tileId )
			=> new( ActionCode.BOARD_PLAYER_REPLY_CLIENT_OPTION, tileId ); 
	}
}