/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0.Events;

/// <summary>
/// Registry of all known events
/// </summary>
public enum ActionCode : uint
{
	INVALID = 0,
	ACK,
	UPDATE,

	/* Game config */
	PLAYER_DATA_INIT, // (player idx)
	PLAYER_STATE, // (player idx)

	/* Game actions */
	/* Camera */
	CAMERA_SET_ANG, // (pitch, yaw, roll)
	CAMERA_SET_POS, // (x, y, z)
	CAMERA_SET_FOV, // (fov,,)

	/* Currencies */
	CURRENCY_SET, // (player idx, currency, value)

	/* Board */
	BOARD_PLAYER_ATTEMPT_MOVE_TILE, // (player idx, tile id)
	BOARD_PLAYER_CONFIRM_MOVE_TILE, // (player idx, tile id)
	BOARD_PLAYER_REQUEST_CLIENT_OPTION, // (player idx)
	BOARD_PLAYER_REPLY_CLIENT_OPTION // (tile id)
}
