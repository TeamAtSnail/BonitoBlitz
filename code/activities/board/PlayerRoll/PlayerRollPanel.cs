/*
 * PlayerTurnPanel.cs
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox.UI;
using libblitz;

namespace BonitoBlitz.CoreActivities.Board;

[UseTemplate]
public partial class PlayerRollPanel : Panel
{
	public static PlayerRollActivity Activity => Game.Current.Activity as PlayerRollActivity;
	public string CurrentNumber => $"{Activity.CurrentNumber}";
	public PlayerRollPanel() { }
}