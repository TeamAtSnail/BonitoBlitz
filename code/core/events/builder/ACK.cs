/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0.Events;

public partial class Builder
{
	public static GameEvent Acknowledge( uint status, uint eventUid )
		=> new( ActionCode.ACK, status, eventUid );
}