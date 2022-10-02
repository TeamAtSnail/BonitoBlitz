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

	public Debug() { }
}