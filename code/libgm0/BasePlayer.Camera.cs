/*
 * part of the gm0 (w.i.p name) gamemode
 * library used across the board gamemode & minigames
 * - last updated indev:3
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace libgm0;

public abstract partial class BasePlayer
{
	public ArbCamera Camera
	{
		get => Components.Get<ArbCamera>();
		set
		{
			if ( Camera == value ) return;

			Components.RemoveAny<ArbCamera>();
			Components.Add( value );
		}
	}
}