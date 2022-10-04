/*
 * Glyph.cs
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 * Based on the sbox-speed-dial implementation
 * - https://github.com/Eagle-One-Development/sbox-speed-dial/blob/main/code/base/ui/glyph/InputGlyph.cs
 */
using System;
using Sandbox;
using Sandbox.UI;

namespace BonitoBlitz.SharedUi;

[UseTemplate]
public partial class Glyph : Panel
{
	public Image Image { get; private set; }
	public InputGlyphSize ButtonSize { get; private set; } = InputGlyphSize.Medium;
	public InputButton Button { get; private set; } = 0;
	public bool HasButton => Button != 0;

	public override void SetProperty( string name, string value )
	{
		base.SetProperty( name, value );

		if ( name == "input" )
			Button = Enum.Parse<InputButton>( value, true );

		if ( name == "size" )
			ButtonSize = Enum.Parse<InputGlyphSize>( value, true );
	}

	public override void Tick()
	{
		base.Tick();

		if ( !HasButton )
			return;
		Image.Texture = Input.GetGlyph( Button, ButtonSize, GlyphStyle.Knockout );
		Image.Style.Height = Image.ComputedStyle.FontSize ?? Image.Texture.Height;
		Image.Style.AspectRatio = (float)Image.Texture.Width / Image.Texture.Height;
	}
}