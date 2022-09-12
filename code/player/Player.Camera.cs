/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;

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