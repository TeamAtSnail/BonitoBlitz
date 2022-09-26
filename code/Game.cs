/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace BonitoBlitz;

public partial class BonitoBlitz : libblitz.Game
{
	public Board.BoardActivity BoardActivity;

	public override void Spawn()
	{
		base.Spawn();

		var player = new libblitz.Player
		{
			DisplayName = "Test",
			CanBeBot = false
		};

		Players.Add( player );

		BoardActivity = new( Players );
	}
}