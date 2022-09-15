/*
 * part of the gm0 (w.i.p name) gamemode
 * library used across the board gamemode & minigames
 * - last updated indev:3
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace libgm0;
using Sandbox;

public abstract partial class BasePlayer
{
	public virtual void LibSimulate( Client cl ) { }
	public virtual void LibFrameSimulate( Client cl ) { }

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		//if ( !(Game.Current as libgm0.Game).GameReady )
		//return;

		LibSimulate( cl );
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		//if ( !(Game.Current as libgm0.Game).GameReady )
		//return;

		LibFrameSimulate( cl );
	}
}