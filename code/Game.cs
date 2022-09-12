/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox;
namespace gm0;

public partial class Gamemode0 : Game
{
	public Gamemode0() { }

	public override void ClientJoined( Client cl )
	{
		base.ClientJoined( cl );

		// Create a pawn for this client to play with
		new BoardPlayer().SetClient( cl );
	}
}