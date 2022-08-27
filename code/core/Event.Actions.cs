/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System;


/// <summary>
/// Registry of all known events
/// </summary>
public enum GameEventAction : uint
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
	CURRENCY_SET // (player idx, currency, value)
}

public enum EventStatusCode : uint
{
	OK = 0,
	UNEXPECTED_PLAYER = 1
}