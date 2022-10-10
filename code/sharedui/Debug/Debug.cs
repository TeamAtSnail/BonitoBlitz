/*
 * Debug.cs
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox.UI;

namespace BonitoBlitz.SharedUi;

[UseTemplate]
public partial class Debug : Panel
{
	public static libblitz.Game CurrentGame => Sandbox.Game.Current as libblitz.Game;
	public string GameInfo => $"Game {CurrentGame.Uid}/{CurrentGame.DisplayName}";
	public string GamePlayersInfo => $"Player count {CurrentGame.Players.Count}";
	public string ActivityInfo
	{
		get
		{
			if (CurrentGame == null)
				return "No game active";
			if (CurrentGame.Activities == null)
				return "Activities list broken";
			if (CurrentGame.Activity == null)
				return "No activity active";
			return CurrentGame.Activity.GetType().Name;
		}
	}

	public Debug() { }
}