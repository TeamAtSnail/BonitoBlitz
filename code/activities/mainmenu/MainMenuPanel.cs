/*
 * MainMenuPanel.cs
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox.UI;
using libblitz;

namespace BonitoBlitz.CoreActivities;

public partial class MainMenuPanel : Panel
{
	public static MainMenuActivity Activity => Game.Current.Activity as MainMenuActivity;

	public MainMenuPanel()
	{
		StyleSheet.Load( "/activities/mainmenu/MainMenuPanel.scss" );
	}
}