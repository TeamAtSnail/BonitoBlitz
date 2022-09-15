/*
 * part of the gm0 (w.i.p name) gamemode
 * library used across the board gamemode & minigames
 * - last updated indev:3
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace libgm0;
using Sandbox;

public partial class Game
{
	public virtual void LibSimulate( Client cl ) { }
	public virtual void LibKnownPlayerJoined( Client cl, KnownPlayer knownPlayer ) { }

	/// <summary>
	/// Simulate -> LibSimulate
	/// </summary>
	/// <param name="cl">Client</param>
	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( GameReady )
			LibSimulate( cl );
	}

	/// <summary>
	/// ClientJoined -> LibKnownPlayerJoined
	/// </summary>
	/// <param name="cl">Client</param>
	public override void ClientJoined( Client cl )
	{
		base.ClientJoined( cl );
		int readyPlayers = 0;
		// For each known player...
		KnownPlayers.ForEach( knownPlayer =>
		{
			// Check if that player was played by this client
			foreach ( var playedBy in knownPlayer.Data.PlayedBy )
			{
				if ( cl.PlayerId == playedBy )
					LibKnownPlayerJoined( cl, knownPlayer );
			}
			if ( knownPlayer.Player != null )
				readyPlayers++;
		} );

		if ( readyPlayers >= KnownPlayers.Count )
		{
			GameReady = true;
			Log.Info( "Game now ready!" );
		}
	}
}