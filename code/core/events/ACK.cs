/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;

public partial class GameEventCreator
{
	public static GameEvent Acknowledge( uint status )
	{
		return new GameEvent( (uint)GameEventAction.ACK, status );
	}
}