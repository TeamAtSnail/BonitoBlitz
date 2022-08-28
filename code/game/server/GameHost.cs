/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System.Collections.Generic;
using Sandbox;

/// <summary>
/// Active game data
/// </summary>
public partial class GameHost
{
	public readonly List<PlayerEntity> Players = new();

	public GameHost()
	{
		if ( !Host.IsServer )
			return;

		Gamemode0.Session.AddForeverHandler( new(
			Events.ActionCode.PLAYER_DATA_INIT, "_GameState.PLAYER_DATA_INIT",
			this.HandlePlayerDataInit ) );
	}
}