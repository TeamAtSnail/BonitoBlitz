/*
 * BoardPawn.cs
 * Core pawn for the game board experience
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox;

namespace BonitoBlitz.CoreActivities.Board;

public partial class BoardPawn : AnimatedEntity
{
	[Net] public libblitz.Player Player { get; private set; }

	public BoardPawn( libblitz.Player player ) : base()
	{
		Player = player;
	}
}