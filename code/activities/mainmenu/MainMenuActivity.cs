/*
 * MainMenuActivity.cs
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using System;
using System.Collections.Generic;
using libblitz;

namespace BonitoBlitz.CoreActivities;

public partial class MainMenuActivity : Activity
{
	public override Type PawnType => null;

	public override Type HudPanelType => typeof( MainMenuPanel );

	public MainMenuPanel Panel;

	public MainMenuActivity( List<Player> players ) : base( players )
	{
	}

	public MainMenuActivity() : base( null )
	{
	}

	public override void Initialize()
	{
		if ( Sandbox.Host.IsClient )
			Panel = new();
	}

	public override void ActivityActive( ActivityResult previousActivityResult )
	{
		base.ActivityActive( previousActivityResult );

		if ( Sandbox.Host.IsClient )
			Game.Current.SetPanel( Panel );
	}
}