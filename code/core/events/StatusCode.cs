/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0.Events;

public enum StatusCode : uint
{
	OK = 0,
	UNEXPECTED_PLAYER,
	UNEXPECTED_SENDER,
	NO_HANDLER,
	FAIL_GENERIC,
	FAIL_INVALID_ARGUMENT
}
